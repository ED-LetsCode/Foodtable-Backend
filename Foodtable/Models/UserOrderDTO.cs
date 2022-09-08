namespace Foodtable.Models
{
    public class UserOrderDTO
    {
        public int GroupId { get; set; }

        public int UserId { get; set; }

        public int UserOrderId { get; set; }

        public int OrderId { get; set; }

        public string ProductName { get; set; }

        public int AmountOfProduct { get; set; }
    }
}
