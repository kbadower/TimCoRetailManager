using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TRMDesktopUILibrary.Api;
using TRMDesktopUILibrary.Models;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
		private BindingList<ProductModel> _products;
        private int _itemQuantity = 1;
        private BindingList<CartProductModel> _cart = new BindingList<CartProductModel>();
        private decimal _subtotal;
        private decimal _tax;
        private decimal _total;
        private ProductModel _selectedProduct;
        IProductEndpoint _productEndpoint;
            
        public BindingList<ProductModel> Products
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
                NotifyOfPropertyChange(() => CanAddToCart);
            }
		}

		public BindingList<CartProductModel> Cart
		{
			get { return _cart; }
			set 
			{
				_cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
		}

		public string Subtotal
		{
			get
            {
                _subtotal = 0;
                foreach (var product in Cart)
                {
                    _subtotal += product.Product.RetailPrice * product.QuantityInCart;
                }
                return _subtotal.ToString("C");
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

        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
        }

        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        public async Task LoadProducts()
        {
            var products = await _productEndpoint.GetAllProducts();
            Products = new BindingList<ProductModel>(products);
        }

        public void AddToCart()
		{
            CartProductModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                CartProductModel cartProduct = new CartProductModel()
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(cartProduct);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => Subtotal);
		}

		public void RemoveFromCart()
		{
            NotifyOfPropertyChange(() => Subtotal);
        }

		public void Checkout()
		{
			throw new NotImplementedException();
		}

        public bool CanAddToCart
		{
            get
            {
                bool output = false;

                if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }

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
