using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Inzynierka.Data.HelpModels;
using Inzynierka.Data.ViewModels;
using Inzynierka.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// Dodac udostepnianie globalne, sprobowac zrobic usuwanie motywow kaskadowe
// Dodac sciaganie listy wszystkich motywow dostepnych dla usera
// Dodac sciaganie listy motywow globalnych.
// Dodac dodawanie motywu globalne
// Stworzyc usuwanie konta uzytkownia
// Stworzyc dodawanie utworow, sciaganie, usuwanie 
// Popracowac nad dodawaniem zdjec avatara bo cos jest tam nie tak
// Poprawic rejestracje -> jak wyrznie sie email to usunac utworzone konto
// Stworzyc grupy, przypisywanie do grup,
// Stworzyc posty w grupach i chat
// Przetestowac jeszcze raz tworzenie quizow 
// Popracowac na systemem oceniania



namespace Inzynierka.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MotivesController : Controller
    {
        private readonly IMotiveService _motiveService;
        public MotivesController(IMotiveService motiveService)
        {
            _motiveService = motiveService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddMotive([FromBody]MotiveViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);

            var result = await _motiveService.AddMotive(viewModel, userId);

            if (result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpPut("{motiveId}")]
        public async Task<IActionResult> EditMotive([FromBody]MotiveViewModel viewModel, int motiveId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);

            var result = await _motiveService.EditMotive(viewModel, motiveId, userId);

            if (result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{motiveId}")]
        public async Task<IActionResult> DeleteMotive(int motiveId)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);

            var result = await _motiveService.DeleteMotive(motiveId, userId);

            if (result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("share/{userToShareId}/{motiveId}")]
        public async Task<IActionResult> ShareMotive(int userToShareId, int motiveId)
        {
            int userWhichSharingId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);

            var result = await _motiveService.ShareMotive(userToShareId, userWhichSharingId, motiveId);

            if (result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}