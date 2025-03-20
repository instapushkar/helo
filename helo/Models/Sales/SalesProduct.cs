using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using helo.Models.Product;

namespace helo.Models.Sales
{
    public class SalesProduct
    {
        public int Id { get; set; }

        [ForeignKey("ProductData")]
        public int ProductId { get; set; }
        public ProductData Product { get; set; } 

        [Required]
        public int QuantitySold { get; set; }
        [Required]
        public double TotalPrice { get; set; }
    }
}
