﻿using DutchTreat.Data.Entities;

namespace DutchTreat.Dtos
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
