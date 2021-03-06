﻿using System;
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
        [HttpGet]
        public async Task<IActionResult> GetSounds()
        {
            string categoryName = "sound";
            var result = await _soundsService.GetSoundNamesByCategory(categoryName);

            if (result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("chords")]
        public async Task<IActionResult> GetChords()
        {
            string categoryName = "chord";
            var result = await _soundsService.GetSoundNamesByCategory(categoryName);

            if (result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("seed/{type}")]
        public async Task<IActionResult> SeedProbes(string type)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);

            var result = await _soundsService.SeedSounds(userId, type);

            if (result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("getsoundsmixed")]
        public async Task<IActionResult> GetSoundsAndChords()
        {
            var result = await _soundsService.GetSoundsAndChords();

            if (result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);
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