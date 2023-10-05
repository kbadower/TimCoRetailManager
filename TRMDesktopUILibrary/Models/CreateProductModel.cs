using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUILibrary.Models
{
    public class CreateProductModel
    {
        [Required(ErrorMessage = "Product name is required.")]
        [DisplayName("Name")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [DisplayName("Description")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Retail price")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal RetailPrice { get; set; }

        [Required]
        [DisplayName("Quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Not a valid quantity.")]
        public int QuantityInStock { get; set; }

        [Required]
        [DisplayName("Taxable?")]
        public bool IsTaxable { get; set; }

        [DisplayName("Image url")]
        public string ProductImage { get; set; }
    }
}
