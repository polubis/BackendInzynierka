﻿using System;
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
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPictureService _pictureService;
        public UsersController(IUserService userService, IPictureService pictureService)
        {
            _userService = userService;
            _pictureService = pictureService;
        }

        [Authorize]
        [HttpPatch("changeuserdata")]
        public async Task<IActionResult> ChangeUserData([FromBody]ChangeUserDataViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);

            var result = await _userService.ChangeUserData(viewModel, userId);

            if(result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Register(registerModel);

            if (result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.Login(loginViewModel);

            if (result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("register/activate/{link}")]
        public async Task<IActionResult> ActivateAccount(string link)
        {
            var result = await _userService.ConfirmRegister(link);

            if (result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        [Authorize]
        [HttpPut("setting")]
        public async Task<IActionResult> ChangeSetting([FromForm] ChangeUserSettingViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);

            var result = await _userService.ChangeSetting(viewModel, userId);

            if (result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpPost("setting")]
        public async Task<IActionResult> CreateSetting([FromForm] ChangeUserSettingViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);

            var result = await _userService.CreateSetting(viewModel, userId);

            if (result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
