using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using TRMDataAccessLibrary.Models;

namespace TRMDataAccessLibrary.DataAccess
{
    public class InventoryData : IInventoryData
    {
        private readonly IConfiguration _configuration;
        private readonly ISqlDataAccess _da;

        public InventoryData(IConfiguration configuration, ISqlDataAccess da)
        {
            _configuration = configuration;
            _da = da;
        }

        public List<InventoryModel> GetInventory()
        {
            return _da.LoadData<InventoryModel, dynamic>("spInventory_GetAll", new { }, "TRMData");
        }

        public void SaveInventoryRecord(InventoryModel item)
        {
            _da.SaveData<InventoryModel>("spInventory_Insert", item, "TRMData");
        }
    }
}
