using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using TRMDataAccessLibrary.Models;

namespace TRMDataAccessLibrary.DataAccess
{
    public class SaleData : ISaleData
    {
        private readonly IProductData _productData;
        private readonly ISqlDataAccess _da;
        private readonly IConfiguration _config;

        public SaleData(IProductData productData, ISqlDataAccess da, IConfiguration config)
        {
            _productData = productData;
            _da = da;
            _config = config;
        }

        public decimal GetTaxRate()
        {
            string rateText = _config.GetValue<string>("TaxRate");

            bool isValidTaxRate = decimal.TryParse(rateText, out decimal output);

            if (isValidTaxRate == false)
            {
                throw new ConfigurationErrorsException("Error setting up tax rate.");
            }

            output = output / 100;
            return output;

        }

        public void SaveSale(SaleModel sale, string cashierId)
        {
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            var taxRate = GetTaxRate();

            foreach (var product in sale.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = product.ProductId,
                    Quantity = product.Quantity
                };

                var productInfo = _productData.GetProductById(product.ProductId);

                if (productInfo == null)
                {
                    throw new Exception($"The product Id of {product.ProductId} was not found in database.");
                }

                detail.PurchasePrice = productInfo.RetailPrice * detail.Quantity;

                if (productInfo.IsTaxable)
                {
                    detail.Tax = taxRate;
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

        public List<SaleReportModel> GetSaleReport()
        {
            var result = _da.LoadData<SaleReportModel, dynamic>("spSale_SaleReport", new { }, "TRMData").ToList();
            return result;
        }
    }
}
