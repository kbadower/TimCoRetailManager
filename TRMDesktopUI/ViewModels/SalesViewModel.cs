using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
		private BindingList<string> _products;
        private int _itemQuantity;
        private BindingList<string> _cart;
        private decimal _subtotal;
        private decimal _tax;
        private decimal _total;

        public BindingList<string> Products
		{
			get { return _products; }
			set 
			{ 
				_products = value;
                NotifyOfPropertyChange(() => Products);
            }
		}

		public int ItemQuantity
		{
			get { return _itemQuantity; }
			set 
			{ 
				_itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
            }
		}

		public BindingList<string> Cart
		{
			get { return _cart; }
			set 
			{
				_cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
		}

		public decimal Subtotal
		{
			get { return _subtotal; }
			set 
			{ 
				_subtotal = value;
                NotifyOfPropertyChange(() => Subtotal);
            }
		}

        public decimal Tax
        {
            get { return _tax; }
            set
            {
                _subtotal = value;
                NotifyOfPropertyChange(() => Tax);
            }
        }

        public decimal Total
        {
            get { return _total; }
            set
            {
                _subtotal = value;
                NotifyOfPropertyChange(() => Total);
            }
        }



        public void AddToCart()
		{
			throw new NotImplementedException();
		}

		public void RemoveFromCart()
		{
			throw new NotImplementedException();
		}

		public void Checkout()
		{
			throw new NotImplementedException();
		}

		public bool CanAddToCard
		{
            get
            {
                bool output = false;

                // make sure product is selected
                // make sure quantity is > 0

                return output;
            }
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;

                // make sure product in cart is selected

                return output;
            }
        }

        public bool CanCheckout
        {
            get
            {
                bool output = false;

                // make sure cart is not empty

                return output;
            }
        }


    }
}
