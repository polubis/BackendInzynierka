using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Inzynierka.Data.Dtos;
using Inzynierka.Data.ViewModels;

namespace Inzynierka.Services.Interfaces
{
    public interface IQuizService
    {
        Task<ResultDto<CreateQuizDto>> CreateQuiz(CreateQuizViewModel viewModel, int userId);
        Task<ResultDto<GetResultDto>> GetResults(int userId);
        Task<ResultDto<GetQuestionsByQuizDto>> GetQuestionsFromQuiz(int quizId);
    }
}
