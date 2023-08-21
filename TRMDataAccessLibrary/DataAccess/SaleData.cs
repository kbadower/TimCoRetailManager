using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public SaleData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SaveSale(SaleModel sale, string cashierId)
        {
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData productData = new ProductData(_configuration);
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

            using (SqlDataAccess _da = new SqlDataAccess(_configuration))
            {
                try
                {
                    _da.StartTransaction("TRMData");

                    _da.SaveDataInTransaction("spSale_Insert", saleDBModel);

                    saleDBModel.Id = _da.LoadDataInTransaction<int, dynamic>("spSale_Lookup",
                        new
                        {
                            saleDBModel.CashierId,
                            saleDBModel.SaleDate
                        })
                        .FirstOrDefault();

                    foreach (var item in details)
                    {
                        item.SaleId = saleDBModel.Id;
                        _da.SaveDataInTransaction("spSaleDetail_Insert", item);
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

        public List<SaleReportModel> GetSaleReport()
        {
            var result = new List<SaleReportModel>();
            using (SqlDataAccess _da = new SqlDataAccess(_configuration))
            {
                result = _da.LoadData<SaleReportModel, dynamic>("spSale_SaleReport", new { }, "TRMData").ToList();
            }

            return result;
        }
    }
}
