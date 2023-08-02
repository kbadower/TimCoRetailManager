
using System.Collections.Generic;
using System.Web.Http;
using TRMDataAccessLibrary.DataAccess;
using TRMDataAccessLibrary.Models;

namespace TRMDataManager.Controllers
{
    [Authorize]
    public class ProductController : ApiController
    {

        [HttpGet]
        public List<ProductModel> GetAllProducts()
        {
            ProductData productData = new ProductData();

            var output = productData.GetAllProducts();
            return output;
        }
    }
}
