using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUILibrary.Api;
using TRMDesktopUILibrary.Models;

namespace TRMDesktopUI.Library.Tests
{
    public class ProductEndpointTests
    {
        private ProductEndpoint _sut;
        private readonly IAPIHelper _apiHelper = Substitute.For<IAPIHelper>();

        public ProductEndpointTests()
        {
            _sut = new ProductEndpoint(_apiHelper);
        }

        [Test]
        public async Task GetAllProducts_ShouldReturnProducts_WhenProductsExist()
        {
            // Arrange
            var products = new List<ProductModel>()
            {
                new ProductModel { Id = 1, ProductName = "Test name", RetailPrice = 1.99m, Description = "Test description", IsTaxable = true, ProductImage = null, QuantityInStock = 1 }
            };
            _apiHelper.GetAsync<List<ProductModel>>("/api/Product").Returns(products);

            // Act
            var result = await _sut.GetAllProducts();

            // Assert
            Assert.That(result.First(), Is.EqualTo(products.First()));
        }

        [Test]
        public async Task GetAllProducts_ShouldReturnNull_WhenNoProducts()
        {
            // Arrange
            List<ProductModel> products = null;
            _apiHelper.GetAsync<List<ProductModel>>("/api/Product").Returns(products);

            // Act
            var result = await _sut.GetAllProducts();

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
