using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> GetAll()
        {
            var auctions = await _repository.ListAllAsync();

            var results = new List<AuctionDto>();

            foreach (var auction in auctions)
            {
                var dto = _mapper.Map<AuctionDto>(auction);
                var raises = await _raiseRepository.ListAsync(new AuctionPriceRaiseSpecification(pr => pr.AuctionId == auction.Id));
                dto.PriceRaises = raises.ToList();

                lock (results) { results.Add(dto); }
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
            dto.PriceRaises = raises.ToList();

            return Ok(ResponseDto<Auction>.Ok(auction));
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Auction))]
        public async Task<ActionResult> Add([FromBody] NewAuctionDto newAuctionDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var auction = _mapper.Map<Auction>(newAuctionDto);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            auction.OwnerId = userId;

            var result = await _repository.AddAsync(auction);

            return Ok(ResponseDto<Auction>.Ok(auction));
        }

        [HttpPost]
        [Route("{id}/raise")]
        public async Task<ActionResult> RaisePrice()
        {
            return Ok();
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