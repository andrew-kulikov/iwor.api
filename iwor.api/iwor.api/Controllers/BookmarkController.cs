using System;
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
    [Route("api/bookmark")]
    [Authorize]
    public class BookmarkController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Bookmark> _repository;
        private readonly IBookmarkService _service;

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
        public async Task<ActionResult> AddBookmark([FromBody] BookmarkDto bookmarkDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookmark = _mapper.Map<Bookmark>(bookmarkDto);
            bookmark.UserId = userId;

            var result = await _repository.AddAsync(bookmark);

            return Ok(result);
        }

        [HttpGet]
        [Route("my/{id}")]
        public async Task<ActionResult> GetMyById(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookmark = await _service.GetUserBookmark(userId, id);

            if (bookmark == null) return NotFound();

            return Ok(bookmark);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var bookmark = await _repository.GetByIdAsync(id);

            if (bookmark == null) return NotFound();

            return Ok(bookmark);
        }

        [HttpDelete]
        [Route("my/{id}")]
        public async Task<ActionResult> DeleteMy(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.DeleteUserBookmark(userId, id);

            if (!result) return NotFound();

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var bookmark = await _repository.GetByIdAsync(id);

            if (bookmark == null) return NotFound();

            await _repository.DeleteAsync(bookmark);

            return Ok();
        }
    }
}