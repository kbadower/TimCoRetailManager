using System.Collections.Generic;
using TRMDataAccessLibrary.Models;

namespace TRMDataAccessLibrary.DataAccess
{
    public interface IProductData
    {
        List<ProductModel> GetAllProducts();
        ProductModel GetProductById(int id);
    }
}