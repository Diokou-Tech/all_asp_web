using DutchTreat.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IDutchRepository _repo;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IDutchRepository repo, ILogger<ProductController> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repo.FindAll());
        }
        [HttpGet("{id:int}")]
        public ActionResult Get(int id)
        {
            var product = _repo.FindProductById(id);
            if(product is null)
            {
                return NotFound();
            }
            return Ok(product);
        }

    }
}
