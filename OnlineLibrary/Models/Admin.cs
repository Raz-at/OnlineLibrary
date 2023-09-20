using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Models
{
    public class Admin
    {
        [Key]
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
