using System.ComponentModel.DataAnnotations;

namespace Foodtable
{
    public class Group
    {
        public List<User> Users { get; set; }

        public int GroupId { get; set; }

        [MaxLength(70)]
        public string GroupName { get; set; }

        public GroupType GroupType { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EndOfOrderTime { get; set; }

        public DateTime? ValidUntil { get; set; }

        public DateTime Created { get; set; }

        public List<Order> Orders { get; set; }
    }
}
