using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataAccessLibrary.Internal.DataAccess;
using TRMDataAccessLibrary.Models;

namespace TRMDataAccessLibrary.DataAccess
{
    public class ProductData
    {
        SqlDataAccess _da = new SqlDataAccess();

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
    }
}
