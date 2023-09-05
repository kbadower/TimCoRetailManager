using System.Collections.Generic;
using TRMDataAccessLibrary.Models;

namespace TRMDataAccessLibrary.DataAccess
{
    public interface ISaleData
    {
        List<SaleReportModel> GetSaleReport();
        decimal GetTaxRate();
        void SaveSale(SaleModel sale, string cashierId);
    }
}