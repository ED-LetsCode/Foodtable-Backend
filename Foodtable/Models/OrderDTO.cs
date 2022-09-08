using System.ComponentModel.DataAnnotations;

namespace Foodtable.Models
{
    public class OrderDTO
    {
        public int GroupId { get; set; }

        public int UserId { get; set; }

        public int OrderId { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        public string EatingSelection { get; set; }

        public string RestaurantName { get; set; }
    }
}
