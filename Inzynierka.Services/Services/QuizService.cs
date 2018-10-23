using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inzynierka.Data.DbModels;
using Inzynierka.Data.Dtos;
using Inzynierka.Data.HelpModels;
using Inzynierka.Data.ViewModels;
using Inzynierka.Repository.Interfaces;
using Inzynierka.Services.Interfaces;

namespace Inzynierka.Services.Services
{
    public class QuizService:IQuizService
    {
        private readonly IRepository<Quiz> _quizRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Rate> _rateRepository;
        private readonly IRepository<Question> _questionRepository;
        private readonly IMapper _mapper;
        private readonly Dictionary<string, double> QuizTypes = new Dictionary<string, double>()
        {
            { "sound", 1 },
            { "chord", 1.5 },

        };
        private readonly Dictionary<string, double> TimeLimits = new Dictionary<string, double>()
        {
            { "sound", 10 },
            { "chord", 12.5 },
        };

        public QuizService(IRepository<Question> questionRepository,
            IRepository<Rate> rateRepository, IRepository<User> userRepository, IRepository<Quiz> quizRepository, 
            IMapper mapper, IConfigurationManager configurationManager)
        {
            _userRepository = userRepository;
            _quizRepository = quizRepository;
            _rateRepository = rateRepository;
            _mapper = mapper;
            _questionRepository = questionRepository;
        }

        public async Task<ResultDto<RatesDto>> GetAllResults(int limit, int page)
        {
            var result = new ResultDto<RatesDto>();

            var rates = await Task.Run(() =>
                _rateRepository.GetAllByWithLimit(null, x => x.PointsForAllGames, null, limit, page, x => x.User));

            if(rates.Count() == 0)
            {
                result.Errors.Add("Nie prawidłowe parametry filtrowania");
                return result;
            }

            var ratesDto = new RatesDto();
            var mappedRates = _mapper.Map<List<Rate>, List<RateDto>>(rates.ToList());

            for(int i = 0; i < rates.Count(); i++)
            {
                mappedRates[i].User = _mapper.Map<User, UserDto>(rates.ElementAt(i).User);
            }

            ratesDto.Rates = mappedRates;
            result.SuccessResult = ratesDto;

            return result;
        }

        public async Task<ResultDto<GetQuestionsByQuizDto>> GetQuestionsFromQuiz(int quizId, int userId)
        {
            var result = new ResultDto<GetQuestionsByQuizDto>();

            if(quizId < 0)
            {
                result.Errors.Add("Nie prawidłowy indektyfikator quizu");
                return result;
            }

            bool IsQuizExist = await Task.Run(() => _quizRepository.Exist(x => x.Id == quizId && x.UserId == userId));

            if (!IsQuizExist)
            {
                result.Errors.Add("Quiz o podanym identyfikatorze nie istnieje");
                return result;
            }

            var questions = await Task.Run(() => _questionRepository.GetAllBy(x => x.QuizId == quizId).ToList());

            var mappedQuestions = _mapper.Map<List<Question>, List<QuestionDto>>(questions);

            var getQuestionDto = new GetQuestionsByQuizDto();
            getQuestionDto.Questions = mappedQuestions;

            result.SuccessResult = getQuestionDto;

            return result;
        }

        public async Task<ResultDto<GetResultDto>> GetResultsForUser(int userId)
        {
            var result = new ResultDto<GetResultDto>();

            var quizes = await Task.Run(() =>
                _quizRepository.GetAllBy(x => x.UserId == userId).ToList()
            );

            var rate = await Task.Run(() =>
                _rateRepository.GetBy(x => x.UserId == userId)
            );

            if (quizes == null || rate == null)
            {
                result.Errors.Add("Jeszcze nie brałeś udziału w quizach");
                return result;
            }

            var rateDto = _mapper.Map<RateDto>(rate);

            var quizDtos = _mapper.Map<List<Quiz>, List<QuizDto>>(quizes);

            var resultDto = new GetResultDto(quizDtos, rateDto);

            result.SuccessResult = resultDto;
            
            return result;
        }

        public async Task<ResultDto<CreateQuizDto>> CreateQuiz(CreateQuizViewModel viewModel, int userId)
        {
            var result = new ResultDto<CreateQuizDto>();
            var questions = _mapper.Map<List<QuestionViewModel>, List<Question>>(viewModel.Questions);

            double effectiveness = CalculatePercentageRate(viewModel.NumberOfPositiveRates, viewModel.NumberOfNegativeRates);

            var quiz = _mapper.Map<Quiz>(viewModel);

            quiz.PointsForGame = CalculatePointsForEveryQuestion(questions, viewModel.QuizType);
            quiz.SecondsSpendOnQuiz = Math.Round(viewModel.Questions.Sum(x => x.TimeForAnswerInSeconds), 2);
            quiz.UserId = userId; quiz.RateInNumber = effectiveness;
            var insertedQuiz = await _quizRepository.InsertAndReturnObject(quiz);

            if (insertedQuiz == null)
            {
                result.Errors.Add("Wystapił błąd podczas dodawania wyniku do twojej historii. Ta gra nie zostanie uznana");
                return result;
            }

            foreach (var question in questions)
                question.QuizId = insertedQuiz.Id;

            int isQuestionsInserted = await _questionRepository.InsertList(questions);

            if (isQuestionsInserted == 0)
            {
                result.Errors.Add("Wystapił błąd podczas dodawania wyniku do twojej historii. Ta gra nie zostanie uznana");
                return result;
            }

            var quizes = _quizRepository.GetAllBy(x => x.UserId == userId).ToList();

            RateModel rateModel = CalculateCurrentRate(userId, quizes);

            var rate = _rateRepository.GetBy(x => x.UserId == userId);

            bool rateExist = true;
            if (rate == null)
            {
                rate = new Rate();
                rateExist = false;
            }

            rate.CurrentPercentageRate = rateModel.PercentageRate;
            rate.NumberOfPlayedGames = rateModel.CountOfQuizes;
            rate.PointsForAllGames = rateModel.PointsForAllGames;
            rate.UserId = userId;

            if (rateExist)
            {
                rate.CreationDate = rate.CreationDate;
                int isRateUpdated = _rateRepository.Update(rate);
                if (isRateUpdated == 0)
                {
                    result.Errors.Add("Wystąpił błąd podczas dodawania nowych danych do twojego rankingu");
                    return result;
                }
            }
            else
            {
                int isRateInserted = await _rateRepository.Insert(rate);
                if (isRateInserted == 0)
                {
                    result.Errors.Add("Wystąpił błąd podczas dodawania oceny");
                    return result;
                }
            }

            var quizDto = new CreateQuizDto();

            quizDto.ActualPoints = rate.PointsForAllGames;
            quizDto.NumberOfPlayedGames = rate.NumberOfPlayedGames;
            quizDto.Effectiveness = rate.CurrentPercentageRate;
            quizDto.NumberOfAllPositiveAnswers = rateModel.NumberOfAllPositiveAnswers;
            quizDto.NumberOfAllNegativeAnswers = rateModel.NumberOfAllNegativeAnswers;

            quizDto.TimeAverage = Math.Round(quizes.Sum(x => x.SecondsSpendOnQuiz) / rateModel.CountOfQuizes, 2);

            var rates = _rateRepository.GetAll(x => x.User.UserSetting).OrderByDescending(x => x.CurrentPercentageRate).ToList();

            var SimilarUsers = new List<SimilarUserDto>();

            var ratesWithoutRequestingUser = rates.Where(x => x.UserId != userId).Take(6).ToList();

            quizDto.PlaceInRank = GetPlaceInRank(ratesWithoutRequestingUser, rates.Single(x => x.UserId == userId).CurrentPercentageRate);

            if (ratesWithoutRequestingUser.Count() > 0)
            {
                foreach (var el in rates)
                {
                    SimilarUsers.Add(new SimilarUserDto(el.User.Username, el.User.Sex, el.User.UserSetting != null ? el.User.UserSetting.PathToAvatar : "",
                        el.NumberOfPlayedGames, el.Id, el.PointsForAllGames));
                }
                quizDto.SimilarUsers = SimilarUsers;

            }
            result.SuccessResult = quizDto;

            return result;
        }

        private int GetPlaceInRank(List<Rate> rates, double pointsForAllGamesForRequestingUser)
        {
            int result = 1;

            foreach(var rate in rates)
            {
                if (rate.CurrentPercentageRate > pointsForAllGamesForRequestingUser)
                    result++;
            }

            return result;
        }
        private double CalculatePercentageRate(int positiveCount, int negativeCount)
        {
            int numberOfAllAnswers = positiveCount + negativeCount;

            double rate = Math.Round((Convert.ToDouble(positiveCount) / Convert.ToDouble(numberOfAllAnswers) * 100), 2);

            return rate;
        }

        private RateModel CalculateCurrentRate(int userId, List<Quiz> quizes)
        {
            double percentageRate = 0;
            int numberOfAllPositiveAnswers = 0;
            int numberOfAllNegativeAnswers = 0;

            int countOfQuestions = 0;
            foreach(var quiz in quizes)
            {
                numberOfAllPositiveAnswers += quiz.NumberOfPositiveRates;
                numberOfAllNegativeAnswers += quiz.NumberOfNegativeRates;
                countOfQuestions += quiz.NumberOfNegativeRates + quiz.NumberOfPositiveRates;
            }

            double numberOfPositiveAnswers = quizes.Sum(x => x.NumberOfPositiveRates);

            if(numberOfPositiveAnswers != 0)
                percentageRate = Math.Round((numberOfPositiveAnswers / countOfQuestions) * 100, 2);

            double overAllPoints = Math.Round(quizes.Sum(x => x.PointsForGame), 3);

            return new RateModel(percentageRate, quizes.Count, overAllPoints, numberOfAllPositiveAnswers, numberOfAllNegativeAnswers);
        }

        private double CalculatePointsForEveryQuestion(List<Question> questions, string quizType)
        {
            double sum = 0;
            foreach(var question in questions)
            {
                question.PointsForQuestion = 0;

                if (question.CorrectAnswer == question.Answer)
                {
                    question.PointsForQuestion = Math.Round((TimeLimits[quizType] - question.TimeForAnswerInSeconds) / TimeLimits[quizType], 3);
                }

                if (!question.AnsweredBeforeSugestion)
                {
                    question.PointsForQuestion -= 0.02;
                }

                sum += question.PointsForQuestion;
            }
            return Math.Round(sum, 3);
        }

    }
}
