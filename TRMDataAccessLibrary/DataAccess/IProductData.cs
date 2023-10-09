using System.Collections.Generic;
using TRMApi.Models;
using TRMDataAccessLibrary.Models;

namespace TRMDataAccessLibrary.DataAccess
{
    public interface IProductData
    {
        List<ProductModel> GetAllProducts();
        ProductModel GetProductById(int id);
        void CreateProduct(CreateProductModel product);
        void DeleteProduct(int id);
        void UpdateProduct(ProductModel model);
    }
}