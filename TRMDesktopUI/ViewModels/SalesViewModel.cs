using AutoMapper;
using Caliburn.Micro;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TRMDesktopUI.Models;
using TRMDesktopUILibrary.Api;
using TRMDesktopUILibrary.Models;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
		private BindingList<ProductDisplayModel> _products;
        private int _itemQuantity = 1;
        private BindingList<CartProductDisplayModel> _cart = new();
        private ProductDisplayModel _selectedProduct;
        private CartProductDisplayModel _selectedCartProduct;

        private readonly IProductEndpoint _productEndpoint;
        private readonly IConfiguration _configuration;
        private readonly ISaleEndpoint _saleEndpoint;
        private readonly IMapper _mapper;
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;

        public SalesViewModel(IProductEndpoint productEndpoint, IConfiguration configuration, ISaleEndpoint saleEndpoint, IMapper mapper, StatusInfoViewModel status, IWindowManager window)
        {
            _productEndpoint = productEndpoint;
            _configuration = configuration;
            _saleEndpoint = saleEndpoint;
            _mapper = mapper;
            _status = status;
            _window = window;
        }

        public BindingList<ProductDisplayModel> Products
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

		public BindingList<CartProductDisplayModel> Cart
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
            decimal taxRate = _configuration.GetValue<decimal>("taxRate") / 100;

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

        public ProductDisplayModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public CartProductDisplayModel SelectedCartProduct
        {
            get { return _selectedCartProduct; }
            set
            {
                _selectedCartProduct = value;
                NotifyOfPropertyChange(() => SelectedCartProduct);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }


        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadProducts();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                if (ex.Message == "Unauthorized")
                {
                    _status.UpdateMessage("Permission denied.", "You do not have permission to view this page.");
                    await _window.ShowDialogAsync(_status, null, settings);
                }
                else
                {
                    _status.UpdateMessage("Fatal exception.", "Something went wrong loading the page.");
                    await _window.ShowDialogAsync(_status, null, settings);
                }

                await TryCloseAsync();
            }
        }

        public async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAllProducts();
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);

            Products = new BindingList<ProductDisplayModel>(products);
        }

        public void AddToCart()
		{
            CartProductDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
            }
            else
            {
                CartProductDisplayModel cartProduct = new()
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
            CartProductDisplayModel selectedCartProduct = Cart.FirstOrDefault(x => x == SelectedCartProduct);

            if (selectedCartProduct.QuantityInCart > 1)
            {
                selectedCartProduct.QuantityInCart -= 1;
                selectedCartProduct.Product.QuantityInStock += 1;
            }
            else
            {
                selectedCartProduct.Product.QuantityInStock += 1;
                Cart.Remove(selectedCartProduct);
            }

            NotifyOfPropertyChange(() => Subtotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckout);
            NotifyOfPropertyChange(() => CanAddToCart);
        }

		public async Task Checkout()
		{
			SaleModel sale = new();
            foreach (var product in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = product.Product.Id,
                    Quantity = product.QuantityInCart
                });
            }

            await _saleEndpoint.PostSale(sale);
            await ResetSalesViewModel();
        }

        public async Task ResetSalesViewModel()
        {
            Cart.Clear();
            SelectedProduct = null;
            SelectedCartProduct = null;
            ItemQuantity = 1;
            await LoadProducts();

            NotifyOfPropertyChange(() => Subtotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckout);
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

                if (SelectedCartProduct != null && SelectedCartProduct?.QuantityInCart > 0)
                {
                    output = true;
                }

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
