﻿using System;

namespace Common.Core.DataAccess
{
    public class DataTableQuery
    {
        public Type DataTable { get; set; }

        public string PivotField { get; set; }

        public bool IsDynamic { get; set; }
    }
}
