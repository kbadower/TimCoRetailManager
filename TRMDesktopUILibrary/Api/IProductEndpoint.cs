using System.Collections.Generic;
using System.Threading.Tasks;
using TRMDesktopUILibrary.Models;

namespace TRMDesktopUILibrary.Api
{
    public interface IProductEndpoint
    {
        Task CreateProduct(CreateProductModel model);
        Task DeleteProdut(int id);
        Task<List<ProductModel>> GetAllProducts();
    }
}