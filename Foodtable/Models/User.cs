using System.ComponentModel.DataAnnotations;

namespace Foodtable
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(25)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(25)]
        public string LastName { get; set; }

        public List<Group> Groups { get; set; }
    }
}
