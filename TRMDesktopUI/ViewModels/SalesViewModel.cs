using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TRMDesktopUILibrary.Api;
using TRMDesktopUILibrary.Helpers;
using TRMDesktopUILibrary.Models;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
		private BindingList<ProductModel> _products;
        private int _itemQuantity = 1;
        private BindingList<CartProductModel> _cart = new BindingList<CartProductModel>();
        private ProductModel _selectedProduct;
        IProductEndpoint _productEndpoint;
        private readonly IConfigHelper _configHelper;
        ISaleEndpoint _saleEndpoint;

        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper, ISaleEndpoint saleEndpoint)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
            _saleEndpoint = saleEndpoint;
        }

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
                var subtotal = CalculateSubtotal();
                return subtotal.ToString("C", new CultureInfo("en-US", false));
            }
		}

        private decimal CalculateSubtotal()
        {
            decimal subtotal = Cart.Sum(c => c.Product.RetailPrice * c.QuantityInCart);

            return subtotal;
        }

        public string Tax
        {
            get
            {
                var tax = CalculateTax();
                return tax.ToString("C", new CultureInfo("en-US", false));
            }
        }

        private decimal CalculateTax()
        {
            decimal tax = 0;
            decimal taxRate = _configHelper.GetTaxRate() / 100;

            tax = Cart
                .Where(c => c.Product.IsTaxable)
                .Sum(c => c.Product.RetailPrice * taxRate * c.QuantityInCart);

            return tax;
        }

        public string Total
        {
            get
            {
                var total = CalculateSubtotal() + CalculateTax();
                return total.ToString("C", new CultureInfo("en-US", false));
            }
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
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckout);
        }

		public void RemoveFromCart()
		{
            NotifyOfPropertyChange(() => Subtotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckout);
        }

		public async Task Checkout()
		{
			SaleModel sale = new SaleModel();
            foreach (var product in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = product.Product.Id,
                    Quantity = product.QuantityInCart
                });
            }

            await _saleEndpoint.PostSale(sale);
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

                if (Cart.Count > 0)
                {
                    output = true;
                }

                return output;
            }
        }


    }
}
