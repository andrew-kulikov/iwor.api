using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using iwor.api.DTOs;
using iwor.core.Entities;
using iwor.core.Repositories;
using iwor.core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iwor.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookmarkController : ControllerBase
    {
        private readonly IBookmarkService _service;
        private readonly IRepository<Bookmark> _repository;
        private readonly IMapper _mapper;

        public BookmarkController(IRepository<Bookmark> repository, IMapper mapper, IBookmarkService service)
        {
            _repository = repository;
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAll()
        {
            var bookmarks = await _repository.ListAllAsync();

            return Ok(bookmarks);
        }

        [HttpGet]
        [Route("my")]
        public async Task<ActionResult> GetMy()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bookmarks = await _service.GetUserBookmarks(userId);

            return Ok(bookmarks);
        }

        [HttpPost]
        [Route("my")]
        public async Task<ActionResult> AddBookmark([FromBody]BookmarkDto bookmarkDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookmark = _mapper.Map<Bookmark>(bookmarkDto);
            bookmark.UserId = userId;

            var result = await _repository.AddAsync(bookmark);

            return Ok(result);
        }
    }
}
