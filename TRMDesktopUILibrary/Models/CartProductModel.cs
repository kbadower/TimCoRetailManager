using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUILibrary.Models
{
    public class CartProductModel
    {
        public ProductModel Product { get; set; }
        public int QuantityInCart { get; set; }
        public string CartText
        {
            get
            {
                return $"{Product.ProductName}  {QuantityInCart}x${Math.Round(Product.RetailPrice, 2)}";
            }
        }
    }
}
