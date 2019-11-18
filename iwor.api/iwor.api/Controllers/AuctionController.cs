using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using iwor.api.DTOs;
using iwor.core.Entities;
using iwor.core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iwor.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly IRepository<Auction> _repository;

        public AuctionController(IRepository<Auction> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<Auction>))]
        public async Task<ActionResult> GetAll()
        {
            var auctions = await _repository.ListAllAsync();
            return Ok(auctions);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Auction))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetById(Guid id)
        {
            var auction = await _repository.GetByIdAsync(id);

            if (auction == null) return NotFound();

            return Ok(auction);
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Add([FromBody] AuctionDto auction)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _repository.AddAsync(auction);

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

            return Ok();
        }
    }
}