using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TRMDataAccessLibrary.Internal.DataAccess;
using TRMDataAccessLibrary.Models;

namespace TRMDataAccessLibrary.DataAccess
{
    public class SaleData
    {
        SqlDataAccess _da = new SqlDataAccess();

        public void SaveSale(SaleModel sale, string cashierId)
        {
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData productData = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate();

            foreach (var product in sale.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = product.ProductId,
                    Quantity = product.Quantity
                };

                var productInfo = productData.GetProductById(product.ProductId);

                if (productInfo == null)
                {
                    throw new Exception($"The product Id of {product.ProductId} was not found in database.");
                }

                detail.PurchasePrice = productInfo.RetailPrice * detail.Quantity;

                if (productInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate / 100);
                }

                details.Add(detail);
            }

            SaleDBModel saleDBModel = new SaleDBModel
            {
                Subtotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax)
            };

            saleDBModel.Total = saleDBModel.Subtotal + saleDBModel.Tax;
            saleDBModel.CashierId = cashierId;

            using (SqlDataAccess _da = new SqlDataAccess())
            {
                try
                {
                    _da.StartTransaction("TRMData");

                    _da.SaveDataInTransaction("spSaleInsert", saleDBModel);

                    saleDBModel.Id = _da.LoadDataInTransaction<int, dynamic>("spSaleLookup",
                        new
                        {
                            saleDBModel.CashierId,
                            saleDBModel.SaleDate
                        })
                        .FirstOrDefault();

                    foreach (var item in details)
                    {
                        item.SaleId = saleDBModel.Id;
                        _da.SaveDataInTransaction("spSaleDetailInsert", item);
                    }

                    _da.CommitTransaction();
                }
                catch
                {
                    _da.RollbackTransatction();
                    throw;
                }
            }
        }
    }
}
