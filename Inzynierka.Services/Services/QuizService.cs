﻿using System;
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

        public async Task<ResultDto<GetQuestionsByQuizDto>> GetQuestionsFromQuiz(int quizId)
        {
            var result = new ResultDto<GetQuestionsByQuizDto>();

            if(quizId < 0)
            {
                result.Errors.Add("Nie prawidłowy indektyfikator quizu");
                return result;
            }

            var questions = await Task.Run(() => _questionRepository.GetAllBy(x => x.QuizId == quizId).ToList());

            var mappedQuestions = _mapper.Map<List<Question>, List<QuestionDto>>(questions);

            var getQuestionDto = new GetQuestionsByQuizDto();
            getQuestionDto.Questions = mappedQuestions;

            result.SuccessResult = getQuestionDto;

            return result;
        }

        public async Task<ResultDto<GetResultDto>> GetResults(int userId)
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

            var quiz = _mapper.Map<Quiz>(viewModel);
            quiz.UserId = userId;
            quiz.RateInNumber = await Task.Run(() =>
                calculateRate(viewModel.NumberOfPositiveRates, 
                    viewModel.NumberOfNegativeRates)
            );

            var insertedQuiz = await _quizRepository.InsertAndReturnObject(quiz);

            if(insertedQuiz == null)
            {
                result.Errors.Add("Wystapił błąd podczas dodawania wyniku do twojej historii. Ta gra nie zostanie uznana");
                return result;
            }

            var questions = _mapper.Map<List<QuestionViewModel> , List<Question>>(viewModel.Questions);

            foreach (var question in questions)
            {
                question.QuizId = insertedQuiz.Id;
                await _questionRepository.Insert(question);
            }

            RateModel rateModel = await Task.Run(() => calculateCurrentRate(userId));

            var rate = _rateRepository.GetBy(x => x.UserId == userId);

            if (rate != null)
            {
                rate.CurrentPercentageRate = rateModel.RateValue;
                rate.NumberOfPlayedGames = rateModel.CountOfQuizes;
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
        private double calculateRate(int positiveCount, int negativeCount)
        {
            int numberOfAllAnswers = positiveCount + negativeCount;

            double rate = Math.Round((Convert.ToDouble(positiveCount) / Convert.ToDouble(numberOfAllAnswers) * 100), 2);

            return rate;
        }

        private RateModel calculateCurrentRate(int userId)
        {
            double overAllRate;

            var quizes = _quizRepository.GetAllBy(x => x.UserId == userId);

            int countOfQuizes = quizes.Count();

            double overAllSum = quizes.Sum(x => x.RateInNumber);

            overAllRate = Math.Round(overAllSum / countOfQuizes, 2);

            return new RateModel(overAllRate, countOfQuizes);
        }
    }
}