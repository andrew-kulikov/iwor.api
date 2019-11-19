using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using iwor.api.DTOs;
using iwor.core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace iwor.api.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        ///     Log into application and receive token
        /// </summary>
        /// <param name="loginDto">User authentication private data (login, password)</param>
        /// <returns></returns>
        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> CreateToken([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var loginResult =
                await _signInManager.PasswordSignInAsync(loginDto.Username, loginDto.Password, false, false);

            if (!loginResult.Succeeded) return BadRequest();

            var user = await _userManager.FindByNameAsync(loginDto.Username);

            var token = new TokenDto { Token = GetToken(user) };

            return Ok(ResponseDto<TokenDto>.Ok(token));
        }

        [Authorize]
        [HttpPost]
        [Route("refreshtoken")]
        public async Task<IActionResult> RefreshToken()
        {
            var user = await _userManager.FindByNameAsync(
                User.Identity.Name ??
                User.Claims.Where(c => c.Properties.ContainsKey("unique_name")).Select(c => c.Value).FirstOrDefault()
            );

            var token = new TokenDto { Token = GetToken(user) };

            return Ok(ResponseDto<TokenDto>.Ok(token));
        }


        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = _mapper.Map<ApplicationUser>(registerDto);
            user.RegistrationDate = DateTime.Now;

            var identityResult = await _userManager.CreateAsync(user, registerDto.Password);

            if (!identityResult.Succeeded) return BadRequest(identityResult.Errors);

            await _signInManager.SignInAsync(user, false);

            var token = new TokenDto {Token = GetToken(user)};

            return Ok(ResponseDto<TokenDto>.Ok(token));
        }

        [Authorize]
        [HttpPost]
        [Route("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeDto passwordChangeDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user, passwordChangeDto.CurrentPassword, passwordChangeDto.NewPassword);

            if (result.Succeeded) return Ok();

            return Problem();
        }

        private string GetToken(IdentityUser user)
        {
            var utcNow = DateTime.UtcNow;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString(CultureInfo.InvariantCulture))
            };

            var signingKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Tokens:Key")));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddSeconds(_configuration.GetValue<int>("Tokens:Lifetime")),
                audience: _configuration.GetValue<string>("Tokens:Audience"),
                issuer: _configuration.GetValue<string>("Tokens:Issuer")
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}