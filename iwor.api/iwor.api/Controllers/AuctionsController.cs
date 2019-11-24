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
using iwor.core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace iwor.api.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    [Authorize]
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionService _auctionService;
        private readonly IMapper _mapper;
        private readonly IRepository<PriceRaise> _raiseRepository;
        private readonly IRepository<Auction> _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuctionsController(IRepository<Auction> repository, IMapper mapper,
            IRepository<PriceRaise> raiseRepository, IAuctionService auctionService, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _raiseRepository = raiseRepository;
            _auctionService = auctionService;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<AuctionDto>))]
        public async Task<IActionResult> GetAll([FromQuery] AuctionStatus? status, [FromQuery] string userId,
            [FromQuery] string filter)
        {
            var spec = new AuctionSpecification(status, userId, filter);
            var auctions = await _repository.ListAsync(spec);

            var dtos = await GetDtos(auctions);

            return Ok(ResponseDto<ICollection<AuctionDto>>.Ok(dtos));
        }

        [HttpGet]
        [Route("active")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<AuctionDto>))]
        public async Task<IActionResult> GetActive()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var auctions = await _auctionService.GetUserActiveAuctions(userId);

            var dtos = await GetDtos(auctions);

            return Ok(ResponseDto<ICollection<AuctionDto>>.Ok(dtos));
        }

        [HttpGet]
        [Route("owned")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<AuctionDto>))]
        public async Task<IActionResult> GetOwned()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var auctions = await _auctionService.GetUserOwnedAuctions(userId);

            var dtos = await GetDtos(auctions);

            return Ok(ResponseDto<ICollection<AuctionDto>>.Ok(dtos));
        }

        private async Task<ICollection<AuctionDto>> GetDtos(IEnumerable<Auction> auctions)
        {
            var results = new List<AuctionDto>();

            foreach (var auction in auctions)
            {
                var raises = await _raiseRepository
                    .ListAsync(new AuctionPriceRaiseSpecification(auctionId: auction.Id));

                var dto = _mapper.Map<AuctionDto>(auction);
                dto.PriceRaises = _mapper.Map<ICollection<PriceRaiseDto>>(raises);

                results.Add(dto);
            }

            return results;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuctionDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetById(Guid id)
        {
            var auction = await _repository.GetByIdAsync(id);

            if (auction == null) return NotFound();

            var dto = _mapper.Map<AuctionDto>(auction);
            var raises = await _raiseRepository.ListAsync(new AuctionPriceRaiseSpecification(auctionId: auction.Id));
            dto.PriceRaises = _mapper.Map<ICollection<PriceRaiseDto>>(raises);

            return Ok(ResponseDto<AuctionDto>.Ok(dto));
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuctionDto))]
        public async Task<ActionResult> Add([FromBody] NewAuctionDto newAuctionDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var auction = _mapper.Map<Auction>(newAuctionDto);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            auction.OwnerId = userId;

            var result = await _repository.AddAsync(auction);

            var dto = _mapper.Map<AuctionDto>(result);

            return Ok(ResponseDto<AuctionDto>.Ok(dto));
        }

        [HttpPost]
        [Route("{id}/raise")]
        public async Task<ActionResult> RaisePrice(Guid id, [FromBody] NewPriceRaiseDto newPriceRaiseDto)
        {
            var curLastRaise = (await _raiseRepository.ListAllAsync())
                .Where(r => r.AuctionId == id)
                .OrderByDescending(r => r.Date)
                .FirstOrDefault();

            var raise = _mapper.Map<PriceRaise>(newPriceRaiseDto);

            raise.RaisedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            raise.AuctionId = id;
            raise.Date = DateTime.Now;

            // FEATURE to update user balance on each price raise (automatically returns to prev user)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);

            if (user.Balance < raise.EndPrice)
                return StatusCode(400, ResponseDto<int>.BadRequest("Недостаточный баланс"));

            var result = await _raiseRepository.AddAsync(raise);

            if (result == null) return BadRequest();

            if (curLastRaise != null)
            {
                var prevUser = await _userManager.FindByIdAsync(curLastRaise.RaisedUserId);
                prevUser.Balance += curLastRaise.EndPrice;
                await _userManager.UpdateAsync(prevUser);
            }

            user.Balance -= raise.EndPrice;
            await _userManager.UpdateAsync(user);

            var resultDto = _mapper.Map<PriceRaiseDto>(result);
            return Ok(ResponseDto<PriceRaiseDto>.Ok(resultDto));
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var auction = await _repository.GetByIdAsync(id);

            if (auction == null) return NotFound();

            await _repository.DeleteAsync(auction);

            return NoContent();
        }
    }
}