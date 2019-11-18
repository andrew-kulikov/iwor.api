using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iwor.core.Entities;
using iwor.core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iwor.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController: ControllerBase
    {
        private readonly IRepository<Auction> _repository;

        public AuctionController(IRepository<Auction> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetAll()
        {
            var auctions = await _repository.ListAllAsync();
            return Ok(auctions);
        }
    }
}
