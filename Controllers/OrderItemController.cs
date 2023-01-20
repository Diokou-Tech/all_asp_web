using AutoMapper;
using DutchTreat.Dtos;
using DutchTreat.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DutchTreat.Controllers
{
    [ApiController]
    [Route("api/orders/{orderId}/items")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderItemController : ControllerBase
    { 
        private readonly IDutchRepository _repo;
        private readonly IMapper _mapper;

        public OrderItemController(IDutchRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Get(int orderId)
        {
            var order = _repo.FindOrderById(orderId);
            if (order is null)
            {
                return NotFound();
            }
            if(order.Items is not null)
            return Ok(_mapper.Map<IEnumerable<OrderItemDto>>(order.Items));
            return NotFound();
        }
        [HttpGet("{id:int}")]
        public IActionResult Get(int orderId, int id)
        {
            var order = _repo.FindOrderById(orderId);
            if(order is null)
            {
            return NotFound();
            }
            var item = order.Items.FirstOrDefault( item => item.Id == id);
            if(item is null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<OrderItemDto>(item));
        }
    }
}
