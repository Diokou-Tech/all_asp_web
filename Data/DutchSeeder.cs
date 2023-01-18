using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DutchTreat.Data
{
	public class DutchSeeder
	{
		private readonly DBContextDutch _context;
		private readonly IWebHostEnvironment _env;
		public DutchSeeder(DBContextDutch context, IWebHostEnvironment env )
		{
			_context = context;
			_env = env;
		}
		public void Seed()
		{  
				// verifier que la base de données existe 
				_context.Database.EnsureCreated();
			if (!_context.Products.Any())
			{
				// seed datas in database
				var filePath = Path.Combine(_env.ContentRootPath,"Data/art.json"); 
				var json = File.ReadAllText(filePath);
				var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);
				//IEnumerable<Product> products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);
				// save products from json file 
				_context.Products.AddRange(products);
				var order = new Order()
				{
					OrderDate = DateTime.Today,
					OrderNumber = "10002",
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
				_context.Orders.Add( order );	
				// Persist DB
				_context.SaveChanges();
			}
			}
	}
}
