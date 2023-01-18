using DutchTreat.Data.Entities;
using System.Collections.Generic;

namespace DutchTreat.Repositories
{
	public interface IDutchRepository
	{
		void Add(Product product);
		IEnumerable<Product> FindAll();
		IEnumerable<Order> FindAllOrders();
        IEnumerable<Product> FindByCategory(string category);
        Product? FindProductById(int id);
        Order? FindOrderById(int id);
		bool SaveAll();
        void AddEntity(object model);
    }
}