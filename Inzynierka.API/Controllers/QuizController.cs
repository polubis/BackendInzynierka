using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Inzynierka.Data.ViewModels;
using Inzynierka.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inzynierka.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class QuizController : Controller
    {
        private readonly IQuizService _quizService;
        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [Authorize]
        [HttpPost("createresult")]
        public async Task<IActionResult> CreateQuizResult([FromBody] CreateQuizViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);

            var result = await _quizService.CreateQuiz(viewModel, userId);

            if (result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("results")]
        public async Task<IActionResult> GetResultsForUser()
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);

            var result = await _quizService.GetResultsForUser(userId);

            if (result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        [Authorize]
        [HttpGet("{quizId}/questions")]
        public async Task<IActionResult> GetQuestionsFromQuiz(int quizId)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);

            var result = await _quizService.GetQuestionsFromQuiz(quizId, userId);

            if (result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("results/{limit}/{page}")]
        public async Task<IActionResult> GetAllRates(int limit, int page)
        {
            var result = await _quizService.GetAllResults(limit, page);

            if (result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}