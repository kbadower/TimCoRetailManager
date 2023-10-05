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
    }
}
