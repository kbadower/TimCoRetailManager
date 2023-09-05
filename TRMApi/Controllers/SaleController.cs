using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;
using TRMDataAccessLibrary.DataAccess;
using TRMDataAccessLibrary.Models;

namespace TRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        private readonly ISaleData _data;

        public SaleController(ISaleData data)
        {
            _data = data;
        }

        [HttpPost]
        [Authorize(Roles = "Cashier")]
        public void Post(SaleModel sale)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _data.SaveSale(sale, userId);
        }

        [HttpGet]
        [Route("GetSalesReport")]
        [Authorize(Roles = "Admin,Manager")]
        public List<SaleReportModel> GetSalesReport()
        {
            return _data.GetSaleReport();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetTaxRate")]
        public decimal GetTaxRate()
        {
            return _data.GetTaxRate();
        }
    }
}
