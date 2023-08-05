using System;
using System.ComponentModel;

namespace TRMDesktopUI.Models
{
    public class CartProductDisplayModel : INotifyPropertyChanged
    {
        public ProductDisplayModel Product { get; set; }
        private int _quantityInCart;

        public int QuantityInCart
        {
            get { return _quantityInCart; }
            set 
            {
                _quantityInCart = value;
                CallPropertyChanged(nameof(QuantityInCart));
            }
        }

        public string CartText
        {
            get
            {
                return $"{Product.ProductName}  {QuantityInCart}x${Math.Round(Product.RetailPrice, 2)}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CallPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
