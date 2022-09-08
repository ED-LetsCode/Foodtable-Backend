using Foodtable.Models;
using System.ComponentModel.DataAnnotations;

namespace Foodtable
{
    public class Order
    {
        public Group Group { get; set; }

        public int GroupId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? OrderDate { get; set; }

        [Key]
        public int OrderId { get; set; }

        [MaxLength(100)]
        public string EatingSelection { get; set; }

        [MaxLength(50)]
        public string RestaurantName { get; set; }

        public List<UserOrder> UserOrders { get; set; }

    }
}
