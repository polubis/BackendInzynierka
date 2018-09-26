using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Inzynierka.Data.ViewModels;
using Inzynierka.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inzynierka.API.Controllers
{
    [Route("api/[controller]")]
    public class SoundsController : Controller
    {
        private readonly ISoundService _soundsService;
        public SoundsController(ISoundService soundsService)
        {
            _soundsService = soundsService;
        }

        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] SoundViewModel soundModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int user = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);

            var result = await _soundsService.UploadSounds(soundModel, user);
            
            if(result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("getsounds/{type}")]
        public async Task<IActionResult> GetSoundsByType(string type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _soundsService.DownloadZippedSounds(type);

            if(result.IsError)
            {
                return BadRequest(result);
            }

            var zipFileStream = System.IO.File.OpenRead(result.SuccessResult.Path);

            return File(zipFileStream, "application/zip");
        }

        [Authorize]
        [HttpGet("getsoundsmixed")]
        public async Task<IActionResult> GetSoundsMixed()
        {
            var result = await _soundsService.DownloadZippedSoundsMixed();

            if (result.IsError)
            {
                return BadRequest(result);
            }

            var zipFileStream = System.IO.File.OpenRead(result.SuccessResult.Path);

            return File(zipFileStream, "application/zip");
        }

        [AllowAnonymous]
        [HttpGet("getintervals/{type}/{numberOfIntervalsToTake}")]
        public async Task<IActionResult> GetIntervalsBy(string type, int numberOfIntervalsToTake)
        {
            var result = await _soundsService.DownloadZippedIntervalsByType(type, numberOfIntervalsToTake);

            if (result.IsError)
            {
                return BadRequest(result);
            }

            var zipFileStream = System.IO.File.OpenRead(result.SuccessResult.Path);

            return File(zipFileStream, "application/zip");
        }
    }
}