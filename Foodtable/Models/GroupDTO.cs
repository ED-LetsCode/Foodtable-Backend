using System.ComponentModel.DataAnnotations;

namespace Foodtable.Models
{
    public class GroupDTO
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public GroupType GroupType { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EndOfOrderTime { get; set; }
    }
}
