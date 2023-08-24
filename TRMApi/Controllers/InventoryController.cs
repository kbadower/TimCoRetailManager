using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TRMDataAccessLibrary.DataAccess;
using TRMDataAccessLibrary.Models;

namespace TRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryData _data;

        public InventoryController(IInventoryData data)
        {
            _data = data;
        } 

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public void Post(InventoryModel item)
        {
            _data.SaveInventoryRecord(item);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public List<InventoryModel> Get()
        {
            return _data.GetInventory();
        }
    }
}
