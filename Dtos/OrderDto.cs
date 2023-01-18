using DutchTreat.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DutchTreat.Dtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        [Required(ErrorMessage = "Veuillez ajouter une Order Number.")]
        [MinLength(4,ErrorMessage ="Le order number doit avoir une valeur de 4 caractères.")]
        public string OrderNumber { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public OrderDto() { }
        public OrderDto( Order order )
        {
            if(order is null)
            {
                throw new Exception("Order null");
            }
            this.OrderNumber = order.OrderNumber;
            this.OrderDate = order.OrderDate;
            this.Items = order.Items;
            this.OrderId= order.Id;
        }

    }
}
