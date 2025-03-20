using System.ComponentModel.DataAnnotations;

namespace helo.Models.Product
{
    public class ProductData
    {
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]

        public double Price { get; set; }
    }
}
