﻿using System;
using System.Configuration;
using System.Globalization;

namespace TRMDesktopUILibrary.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        // TODO: Move this from config to the API
        public decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];

            bool isValidTaxRate = decimal.TryParse(rateText, out decimal output);

            if (isValidTaxRate == false)
            {
                throw new ConfigurationErrorsException("Error setting up tax rate.");
            }

            return output;
        }
    }
}
