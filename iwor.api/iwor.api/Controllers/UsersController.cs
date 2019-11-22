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

        public UsersController(IRepository<ApplicationUser> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<PublicUserProfile>))]
        public async Task<IActionResult> GetById()
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
    }
}