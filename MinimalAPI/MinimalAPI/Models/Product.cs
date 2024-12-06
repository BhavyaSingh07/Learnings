using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Models
{
    public class Product
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }

        public override string ToString()
        {
            return$"Product ID: {Id}, Product Name: {ProductName}";
        }
    }
}
