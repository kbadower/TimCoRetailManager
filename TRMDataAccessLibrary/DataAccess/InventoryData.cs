using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using TRMDataAccessLibrary.Internal.DataAccess;
using TRMDataAccessLibrary.Models;

namespace TRMDataAccessLibrary.DataAccess
{
    public class InventoryData
    {
        private readonly IConfiguration _configuration;

        public InventoryData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<InventoryModel> GetInventory()
        {
            var result = new List<InventoryModel>();
            using(SqlDataAccess _da = new SqlDataAccess(_configuration))
            {
                result = _da.LoadData<InventoryModel, dynamic>("spInventory_GetAll", new { }, "TRMData");
            }
            return result;
        }

        public void SaveInventoryRecord(InventoryModel item)
        {
            using (SqlDataAccess _da = new SqlDataAccess(_configuration))
            {
                _da.SaveData<InventoryModel>("spInventory_Insert", item, "TRMData");
            }
        }
    }
}
