using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task.EF.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
      
        public virtual List<Product>? Products { get; set; }

        public User()
        {
            Products = new List<Product>();
        }

    }
}
