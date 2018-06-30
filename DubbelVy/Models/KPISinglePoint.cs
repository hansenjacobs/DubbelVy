using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dubbelvy.Models
{
    public class KPISinglePoint<T> : DashboardKPI
    {
        public string DataLabel { get; set; }

        public T Data { get; set; }
    }
}