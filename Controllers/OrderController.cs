using DutchTreat.Repositories;
using Microsoft.AspNetCore.Mvc;
using DutchTreat.Data.Entities;
using DutchTreat.Dtos;
using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace DutchTreat.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IDutchRepository _repo;
        private readonly IMapper _mapper;
        public OrderController(IDutchRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok( _mapper.Map<IEnumerable<OrderDto>>(_repo.FindAllOrders()) );
        }
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = _repo.FindOrderById(id);
                if (order is null)
                return NotFound();
                return Ok(_mapper.Map<OrderDto>(order));
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost]
        public IActionResult Post([FromBody] OrderDto model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var newOrder = new Order()
                    {
                        Id = model.OrderId,
                        OrderDate = model.OrderDate,
                        OrderNumber = model.OrderNumber,
                    };
                    _repo.AddEntity(newOrder);
                    if (_repo.SaveAll())
                    {
                        var orderDto = new OrderDto(newOrder);
                        return Created($"api/Order/{orderDto.OrderId}", orderDto);
                    }
                } catch(Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            return BadRequest(ModelState);
        }
    }
}
