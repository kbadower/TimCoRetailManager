using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUILibrary.Models;

namespace TRMDesktopUILibrary.Api
{
    public class ProductEndpoint : IProductEndpoint
    {
        private readonly IAPIHelper _apiClient;

        public ProductEndpoint(IAPIHelper apiHelper)
        {
            _apiClient = apiHelper;
        }

        public async Task<List<ProductModel>> GetAllProducts()
        {
            var result = await _apiClient.GetAsync<List<ProductModel>>("/api/Product");
            return result;
        }

        public async Task CreateProduct(CreateProductModel model)
        {
            var data = new
            {
                model.ProductName,
                model.Description,
                model.RetailPrice,
                model.IsTaxable,
                model.QuantityInStock,
                model.ProductImage
            };

            using (HttpResponseMessage response = await _apiClient.ApiClient.PostAsJsonAsync("/api/Product", data))
            {
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task DeleteProdut(int id)
        {
            using (HttpResponseMessage response = await _apiClient.ApiClient.DeleteAsync("/api/Product?id=" + id))
            {
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
