using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using iwor.api.DTOs;
using iwor.core.Entities;
using iwor.core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace iwor.api.Controllers
{
    [ApiController]
    [Route("api/me")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public ProfileController(UserManager<ApplicationUser> userManager, IUserService userService, IMapper mapper)
        {
            _userManager = userManager;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserProfile))]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _userService.GetUserProfile(userId);

            return Ok(ResponseDto<UserProfile>.Ok(profile));
        }

        [HttpPut]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserProfile))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateInfo([FromBody] UpdateProfileDto update)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);

            user.Address = update.Address;
            user.Birthday = DateTime.ParseExact(update.Birthday, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            user.FirstName = update.FirstName;
            user.LastName = update.LastName;
            user.PhoneNumber = update.PhoneNumber;
            user.ImageUrl = update.ImageUrl;
            user.CardNumber = update.CardNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) return BadRequest( "Cannot update profile info");

            var profile = _mapper.Map<UserProfile>(user);

            return Ok(ResponseDto<UserProfile>.Ok(profile));
        }

        [HttpPost]
        [Route("balance")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserProfile))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddIncome([FromBody] AddBalanceDto income)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);

            user.Balance += income.Income;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) return BadRequest("Cannot update profile info");

            var profile = _mapper.Map<UserProfile>(user);

            return Ok(ResponseDto<UserProfile>.Ok(profile));
        }
    }
}