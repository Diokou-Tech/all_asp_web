using DutchTreat.Data.Entities;

namespace DutchTreat.Dtos
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }  
        public int ProductId { get; set; }  
        public string ProductCategory { get; set; }
    }
}
