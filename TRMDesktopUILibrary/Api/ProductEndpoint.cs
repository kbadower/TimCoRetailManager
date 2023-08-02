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
        private IAPIHelper _apiClient;

        public ProductEndpoint(IAPIHelper apiHelper)
        {
            _apiClient = apiHelper;
        }

        public async Task<List<ProductModel>> GetAllProducts()
        {
            using (HttpResponseMessage response = await _apiClient.ApiClient.GetAsync("/api/Product"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<ProductModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
