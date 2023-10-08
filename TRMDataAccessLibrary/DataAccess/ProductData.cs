using System.Collections.Generic;
using System.Linq;
using TRMApi.Models;
using TRMDataAccessLibrary.Models;

namespace TRMDataAccessLibrary.DataAccess
{
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess _da;

        public ProductData(ISqlDataAccess da)
        {
            _da = da;
        }

        public List<ProductModel> GetAllProducts()
        {
            var output = _da.LoadData<ProductModel, dynamic>("spProduct_GetAll", new { }, "TRMData");
            return output;
        }

        public ProductModel GetProductById(int id)
        {
            var output = _da.LoadData<ProductModel, dynamic>("spProduct_GetById", new { Id = id }, "TRMData").FirstOrDefault();
            return output;
        }

        public void CreateProduct(CreateProductModel model)
        {
            _da.SaveData("spProduct_Insert",
                new
                {
                    model.ProductName,
                    model.Description,
                    model.RetailPrice,
                    model.QuantityInStock,
                    model.IsTaxable,
                    model.ProductImage
                }, "TRMData");
        }

        public void DeleteProduct(int id)
        {
            _da.SaveData("spProduct_Delete", new { Id = id }, "TRMData");
        }
    }
}
