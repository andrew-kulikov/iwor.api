using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using iwor.api.DTOs;
using iwor.core.Entities;
using iwor.core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace iwor.api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ApplicationUser> _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(IRepository<ApplicationUser> repository, IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
        }


        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<PublicUserProfile>))]
        public async Task<IActionResult> GetAll()
        {
            var users = await _repository.ListAllAsync();
            var profiles = _mapper.Map<ICollection<UserProfile>>(users)
                .OfType<PublicUserProfile>()
                .ToList();

            return Ok(ResponseDto<ICollection<PublicUserProfile>>.Ok(profiles));
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PublicUserProfile))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user == null) return NotFound();

            var profile = _mapper.Map<UserProfile>(user) as PublicUserProfile;

            return Ok(ResponseDto<PublicUserProfile>.Ok(profile));
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user == null)
                return StatusCode(404, ResponseDto<int>.NotFound("Пользователя с таким id не существует"));

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (isAdmin) StatusCode(400, ResponseDto<int>.BadRequest("Невозможно удалить администратора"));

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded) return NoContent();

            return StatusCode(400, ResponseDto<int>.BadRequest("Невозможно удалить пользователя"));
        }
    }
}