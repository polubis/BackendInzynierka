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

            bool DidSomeoneModifiedQuestions = VerifyQuestionsRatingPointsAreNotModified(viewModel.Questions, 
                viewModel.NumberOfPositiveRates);

            if (DidSomeoneModifiedQuestions)
            {
                result.Errors.Add("Bawimy się w oszusta? Nie z nami takie numery :X");
                return result;
            }

            var questions = _mapper.Map<List<QuestionViewModel>, List<Question>>(viewModel.Questions);

            var questionsWithCalculatedPoints = await Task.Run(() => CalculatePointsForEveryQuestion(questions, viewModel.QuizType));

            bool isCalculatedPointsAreModified = CheckIsCalculatedQuestionPointsAreModified(viewModel.Questions, 
                questionsWithCalculatedPoints);

            if (isCalculatedPointsAreModified)
            {
                result.Errors.Add("Bawimy się w oszusta? Nie z nami takie numery :X");
                return result;
            }

            double sumOfAllPoints = questionsWithCalculatedPoints.Sum(x => x.PointsForQuestion);
            double RateInNumber = await Task.Run(() =>
                CalculatePercentageRate(viewModel.NumberOfPositiveRates,
                    viewModel.NumberOfNegativeRates)
            );

            var quiz = _mapper.Map<Quiz>(viewModel);
            quiz.UserId = userId;
            quiz.RateInNumber = RateInNumber;
            quiz.SecondsSpendOnQuiz = viewModel.Questions.Sum(x => x.TimeForAnswerInSeconds);
            quiz.PointsForGame = (sumOfAllPoints + RateInNumber) - quiz.SecondsSpendOnQuiz;


            var insertedQuiz = await _quizRepository.InsertAndReturnObject(quiz);

            if (insertedQuiz == null)
            {
                result.Errors.Add("Wystapił błąd podczas dodawania wyniku do twojej historii. Ta gra nie zostanie uznana");
                return result;
            }

            foreach (var question in questions)
            {
                question.QuizId = insertedQuiz.Id;
                await _questionRepository.Insert(question);
            }

            RateModel rateModel = await Task.Run(() => CalculateCurrentRate(userId));

            var rate = _rateRepository.GetBy(x => x.UserId == userId);

            if (rate != null)
            {
                rate.CurrentPercentageRate = rateModel.RateValue;
                rate.NumberOfPlayedGames = rateModel.CountOfQuizes;
                rate.PointsForAllGames = rateModel.PointsForAllGames;
                int isRateUpdated = _rateRepository.Update(rate);

                if(isRateUpdated == 0)
                {
                    result.Errors.Add("Wystąpił błąd podczas dodawania nowych danych do twojego rankingu");
                    return result;
                }
            }
            else
            {
                var newRate = new Rate();
                newRate.UserId = userId;
                newRate.CurrentPercentageRate = rateModel.RateValue;
                newRate.PointsForAllGames = rateModel.PointsForAllGames;
                newRate.NumberOfPlayedGames = 1;

                int isRateInserted = await _rateRepository.Insert(newRate);

                if(isRateInserted == 0)
                {
                    result.Errors.Add("Wystąpił błąd podczas dodawania oceny");
                    return result;
                }
            }

            return result;
        }
        private double CalculatePercentageRate(int positiveCount, int negativeCount)
        {
            int numberOfAllAnswers = positiveCount + negativeCount;

            double rate = Math.Round((Convert.ToDouble(positiveCount) / Convert.ToDouble(numberOfAllAnswers) * 100), 2);

            return rate;
        }

        private RateModel CalculateCurrentRate(int userId)
        {
            double overAllRate;

            var quizes = _quizRepository.GetAllBy(x => x.UserId == userId);

            int countOfQuizes = quizes.Count();

            double overAllSum = quizes.Sum(x => x.RateInNumber);

            overAllRate = Math.Round(overAllSum / countOfQuizes, 2);

            double overAllPoints = quizes.Sum(x => x.PointsForGame);

            return new RateModel(overAllRate, countOfQuizes, overAllPoints);
        }

        private bool VerifyQuestionsRatingPointsAreNotModified(List<QuestionViewModel> questions, int givenNumberOfPositiveRates)
        {
            int numberOfPositiveRates = 0;
            foreach(var question in questions)
                numberOfPositiveRates += question.Answer == question.CorrectAnswer ? 1 : 0;

            if (givenNumberOfPositiveRates != numberOfPositiveRates)
                return true;

            return false;
        }

        private List<Question> CalculatePointsForEveryQuestion(List<Question> questions, string quizType)
        {
            var clonedQuestions = _mapper.Map<List<Question>>(questions);

            foreach(var question in clonedQuestions)
            {
                if (question.CorrectAnswer == question.Answer)
                {
                    question.PointsForQuestion = QuizTypes[quizType] * (question.TimeForAnswerInSeconds + 1);
                    question.PointsForQuestion += question.AnsweredBeforeSugestion ? 5 : 0;
                    
                }
                else
                    question.PointsForQuestion = 0;
            }

            return clonedQuestions;
        }

        private bool CheckIsCalculatedQuestionPointsAreModified(List<QuestionViewModel> viewModelQuestions, 
            List<Question> calculatedPoints)
        {
            for (int i = 0; i < calculatedPoints.Count; i++)
                if (calculatedPoints[i].PointsForQuestion != viewModelQuestions.ElementAt(i).CalculatedPoints)
                    return true;

            return false;
        }
    }
}
