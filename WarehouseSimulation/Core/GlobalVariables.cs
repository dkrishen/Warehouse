﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSimulation.Core
{
    public static class GlobalVariables
    {
        public static Guid? SelectedDeliveryId { get; set; }
        public static Guid? SelectedDispatchId { get; set; }
        public static string? SelectedProductSku { get; set; }
        public static DateTime CurrentDate { get; set; }

        public const string SumpTitle = "Отстойник";
    }
}