using System.Security.Claims;
using System.Threading.Tasks;
using iwor.core.Entities;
using iwor.core.Services;
using Microsoft.AspNetCore.Authorization;
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

        public ProfileController(UserManager<ApplicationUser> userManager, IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserProfile(userId);

            return Ok(user);
        }

        [HttpPut]
        [Route("")]
        public async Task<ActionResult> UpdateInfo([FromBody] UserProfile profile)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            profile.Id = userId;

            var result = await _userService.UpdateProfile(profile);

            if (result == null) return BadRequest("Cannot update profile info");

            return Ok(result);
        }
    }
}