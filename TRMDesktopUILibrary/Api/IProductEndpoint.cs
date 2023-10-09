using System.Collections.Generic;
using System.Threading.Tasks;
using TRMDesktopUILibrary.Models;

namespace TRMDesktopUILibrary.Api
{
    public interface IProductEndpoint
    {
        Task CreateProduct(CreateProductModel model);
        Task DeleteProduct(int id);
        Task<List<ProductModel>> GetAllProducts();
        Task UpdateProduct(ProductModel model);
    }
}