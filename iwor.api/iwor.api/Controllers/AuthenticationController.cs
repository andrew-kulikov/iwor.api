using System;
using System.Collections.Generic;
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
        private readonly IMapper _mapper;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

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
        [ProducesResponseType(200, Type = typeof(TokenDto))]
        public async Task<IActionResult> CreateToken([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var loginResult =
                await _signInManager.PasswordSignInAsync(loginDto.Username, loginDto.Password, false, false);

            if (!loginResult.Succeeded) return BadRequest();

            var user = await _userManager.FindByNameAsync(loginDto.Username);

            var token = new TokenDto {Token = await GetToken(user)};

            return Ok(ResponseDto<TokenDto>.Ok(token));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("token/validate")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult ValidateToken()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Ok(ResponseDto<bool>.Ok(userId != null));
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

            var token = new TokenDto {Token = await GetToken(user)};

            return Ok(ResponseDto<TokenDto>.Ok(token));
        }


        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto.Password != registerDto.PasswordConfirmation)
                return StatusCode(400, ResponseDto<int>.BadRequest("Пароли не совпадают"));

            var user = _mapper.Map<ApplicationUser>(registerDto);
            user.RegistrationDate = DateTime.Now;

            var identityResult = await _userManager.CreateAsync(user, registerDto.Password);

            if (!identityResult.Succeeded) return BadRequest(identityResult.Errors);

            await _signInManager.SignInAsync(user, false);

            var token = new TokenDto {Token = await GetToken(user)};

            return Ok(ResponseDto<TokenDto>.Ok(token));
        }

        [HttpPost]
        [Route("register/validate")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ValidateRegistration([FromBody] RegisterDto registerDto)
        {
            if (registerDto.Password != registerDto.PasswordConfirmation)
                return StatusCode(400, ResponseDto<int>.BadRequest("Пароли не совпадают"));

            var user = await _userManager.FindByNameAsync(registerDto.Username);

            if (user != null)
                return StatusCode(400, ResponseDto<int>.BadRequest("Пользователь с таким именем существует"));

            user = await _userManager.FindByEmailAsync(registerDto.Email);

            if (user != null)
                return StatusCode(400, ResponseDto<int>.BadRequest("Пользователь с таким email существует"));

            return Ok(ResponseDto<int>.Ok());
        }

        [Authorize]
        [HttpPost]
        [Route("changepassword")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeDto passwordChangeDto)
        {
            if (passwordChangeDto.NewPassword != passwordChangeDto.NewPasswordConfirmation)
                return StatusCode(400, ResponseDto<int>.BadRequest("Пароли не совпадают"));

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user, passwordChangeDto.CurrentPassword,
                passwordChangeDto.NewPassword);

            if (result.Succeeded) return Ok(ResponseDto<int>.Ok());

            return StatusCode(400, ResponseDto<int>.BadRequest("Неверный текущий пароль"));
        }

        private async Task<string> GetToken(ApplicationUser user)
        {
            var utcNow = DateTime.UtcNow;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString(CultureInfo.InvariantCulture))
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

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