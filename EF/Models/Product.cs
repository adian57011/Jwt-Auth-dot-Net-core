using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task.EF.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        [Required]
        public int? Qty { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }
        public virtual User? Users { get; set; }
    }
}
