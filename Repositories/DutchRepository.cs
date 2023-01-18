using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace DutchTreat.Repositories
{
	public class DutchRepository : IDutchRepository
	{
		private readonly DBContextDutch _context;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DBContextDutch context, ILogger<DutchRepository> logger)
		{
			_context = context;
			_logger = logger;
        }
		public IEnumerable<Product> FindAll()
		{
			_logger.LogInformation("En train de recuperer les produits");
			return _context.Products.Take(15).ToList();
		}
		public IEnumerable<Product> FindByCategory(string category)
		{
			return _context.Products.Where(p => p.Category == category);
		}
		public void Add(Product product)
		{
			if(!_context.Products.Any(p => p.Title == product.Title))
			{
				_context.Products.Add(product);
			}
			else
			{
				_logger.LogError("Utilisateur deja dans le systeme !");
			}
		}
        public IEnumerable<Order> FindAllOrders()
        {
			return _context.Orders.Include(o => o.Items).ThenInclude(i => i.Product).OrderBy(p => p.OrderDate);
        }
        public Product FindProductById(int id)
        {
			return _context.Products.SingleOrDefault(p => p.Id == id);
        }

        public Order FindOrderById(int id)
        {
			return _context.Orders.Include(o => o.Items).ThenInclude(o=> o.Product).SingleOrDefault(p => p.Id == id);
        }

        public bool SaveAll()
        {
			return _context.SaveChanges() > 0;
        }

        public void AddEntity(object model)
        {
			_context.Add(model);
        }
    }
}
