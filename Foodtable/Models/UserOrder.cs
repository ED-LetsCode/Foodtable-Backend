using System.ComponentModel.DataAnnotations;

namespace Foodtable.Models
{
    public class UserOrder
    {
        public int OrderId { get; set; }

        [Key]
        public int UserOrderId { get; set; }

        public int UserId { get; set; }

        [MaxLength(100)]
        public string ProductName { get; set; }

        public int AmountOfProduct { get; set; }

        public Order Order { get; set; }
    }
}
