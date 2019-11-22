using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using iwor.api.DTOs;
using iwor.core.Entities;
using iwor.core.Repositories;
using iwor.core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iwor.api.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    [Authorize]
    public class AuctionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepository<PriceRaise> _raiseRepository;
        private readonly IRepository<Auction> _repository;

        public AuctionsController(IRepository<Auction> repository, IMapper mapper,
            IRepository<PriceRaise> raiseRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _raiseRepository = raiseRepository;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<AuctionDto>))]
        public async Task<IActionResult> GetAll([FromQuery] AuctionStatus? status, [FromQuery] string userId, [FromQuery] string filter)
        {
            var spec = new AuctionSpecification(status, userId, filter);
            var auctions = await _repository.ListAsync(spec);

            var results = new List<AuctionDto>();

            foreach (var auction in auctions)
            {
                var dto = _mapper.Map<AuctionDto>(auction);
                var raises = await _raiseRepository.ListAsync(
                    new AuctionPriceRaiseSpecification(pr => pr.AuctionId == auction.Id));
                dto.PriceRaises = _mapper.Map<ICollection<PriceRaiseDto>>(raises);

                lock (results)
                {
                    results.Add(dto);
                }
            }

            return Ok(ResponseDto<List<AuctionDto>>.Ok(results));
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
            var raises = await _raiseRepository.ListAsync(new AuctionPriceRaiseSpecification(pr => pr.AuctionId == id));
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
            var raise = _mapper.Map<PriceRaise>(newPriceRaiseDto);

            raise.RaisedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            raise.AuctionId = id;
            raise.Date = DateTime.Now;

            var result = await _raiseRepository.AddAsync(raise);

            if (result == null) return BadRequest();

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