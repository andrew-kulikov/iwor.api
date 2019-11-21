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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iwor.api.Controllers
{
    [ApiController]
    [Route("api/favorites")]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Bookmark> _repository;
        private readonly IBookmarkService _service;

        public FavoritesController(IRepository<Bookmark> repository, IMapper mapper, IBookmarkService service)
        {
            _repository = repository;
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        [Route("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAll()
        {
            var bookmarks = await _repository.ListAllAsync();

            return Ok(bookmarks);
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuctionDto))]
        public async Task<ActionResult> GetMy()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bookmarks = await _service.GetUserBookmarks(userId);
            var favoriteAuctions = bookmarks.Select(b => _mapper.Map<AuctionDto>(b.Auction));

            return Ok(ResponseDto<IEnumerable<AuctionDto>>.Ok(favoriteAuctions));
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> AddBookmark([FromBody] FavoriteDto favoriteDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookmark = _mapper.Map<Bookmark>(favoriteDto);
            bookmark.UserId = userId;

            var result = await _repository.AddAsync(bookmark);

            return Ok(ResponseDto<Bookmark>.Ok(result));
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuctionDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetMyById(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookmark = await _service.GetUserBookmark(userId, id);

            if (bookmark == null) return NotFound();

            var auction = _mapper.Map<AuctionDto>(bookmark.Auction);

            return Ok(ResponseDto<AuctionDto>.Ok(auction));
        }

        [HttpGet]
        [Route("all/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var bookmark = await _repository.GetByIdAsync(id);

            if (bookmark == null) return NotFound();

            var auction = _mapper.Map<AuctionDto>(bookmark.Auction);

            return Ok(ResponseDto<AuctionDto>.Ok(auction));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteMy(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.DeleteUserBookmark(userId, id);

            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete]
        [Route("all/{id}")]
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