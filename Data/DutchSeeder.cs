using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
	public class DutchSeeder
	{
		private readonly DBContextDutch _context;
		private readonly IWebHostEnvironment _env;
		private readonly UserManager<StoreUser> _userManager;
		public DutchSeeder(DBContextDutch context, IWebHostEnvironment env, UserManager<StoreUser> userManager)
		{
			_context = context;
			_env = env;
			_userManager = userManager;
		}
		public async Task SeedAsync()
		{
			// verifier que la base de données existe 
			_context.Database.EnsureCreated();
			// creation user
			StoreUser user = await _userManager.FindByEmailAsync("diokoutech@gmail.com");
			if (user is null)
			{
				user = new StoreUser()
				{
					FirstName = "Cheikhou",
					LastName = "Diokou",
					Email = "diokoutech@gmail.com",
					UserName = "diokoutech@gmail.com",
				};
			}
			IdentityResult result = await _userManager.CreateAsync(user, "@1996Diokou");
			if (result != IdentityResult.Success)
			{
				throw new Exception("Erreur creation de l'utilisateur !");
			}
			if (!_context.Products.Any())
			{
				// seed datas in database
				var filePath = Path.Combine(_env.ContentRootPath, "Data/art.json");
				var json = File.ReadAllText(filePath);
				var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);
				//IEnumerable<Product> produ cts = JsonSerializer.Deserialize<IEnumerable<Product>>(json);
				// save products from json file 
				_context.Products.AddRange(products);
				var order = new Order()
				{
					OrderDate = DateTime.Today,
					OrderNumber = "10002",
					User = user,
					Items = new List<OrderItem>()
					{
						new OrderItem()
						{
							Product = products.FirstOrDefault(),
							Quantity = 5,
							UnitPrice = products.First().Price
						}
					}
				};
				_context.Orders.Add(order);
				// Persist DB
				_context.SaveChanges();
			}
		}
	}
}
