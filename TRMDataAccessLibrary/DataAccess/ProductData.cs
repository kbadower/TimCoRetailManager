using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public ProductData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<ProductModel> GetAllProducts()
        {
            SqlDataAccess _da = new SqlDataAccess(_configuration);
            var output = _da.LoadData<ProductModel, dynamic>("spProduct_GetAll", new { }, "TRMData");
            return output;
        }

        public ProductModel GetProductById(int id)
        {
            SqlDataAccess _da = new SqlDataAccess(_configuration);
            var output = _da.LoadData<ProductModel, dynamic>("spProduct_GetById", new { Id = id }, "TRMData").FirstOrDefault();
            return output;
        }
    }
}
