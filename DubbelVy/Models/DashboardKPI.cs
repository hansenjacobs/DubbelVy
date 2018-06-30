using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dubbelvy.Models
{
    public class DashboardKPI
    {
        public int ColumnNumber { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public string DataFormat { get; set; }
    }
}