using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dubbelvy.Models
{
    public class KPIMultiplePoint<T> : DashboardKPI
    {
        public List<string> DataLabels { get; set; }

        public List<T> Data { get; set; }

        public string XAxisLabel { get; set; }

        public string YAxisLabel { get; set; }
    }
}