using Dubbelvy.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dubbelvy.Controllers
{
    public class DashboardController : Controller
    {
        ApplicationDbContext _context;

        public DashboardController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var rows = new List<List<DashboardKPI>>();

            rows.Add(new List<DashboardKPI>());
            rows.Add(new List<DashboardKPI>());

            rows[0].Add(GetAuditCount());
            rows[0].Add(GetAutoFailRates());
            rows[1].Add(GetDisputeRates());
            rows[1].Add(GetOutstandingDisputeCount());

            return View(rows);
        }

        public ActionResult RenderChart(DashboardKPI model)
        {
            return PartialView(model);
        }

        private DashboardKPI GetAuditCount()
        {
            var monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);
            var kpi = new KPISinglePoint<int>
            {
                ColumnNumber = 4,
                Title = "Completed Audits",
                Type = "single",
                Data = _context.Audits.Count(a => a.AuditDateTime >= monthStart && a.AuditDateTime <= monthEnd),
                DataFormat = "G0",
                DataLabel = ""
            };

            return kpi;
        }

        private DashboardKPI GetAutoFailRates()
        {
            var kpi = new KPIMultiplePoint<double>
            {
                ColumnNumber = 8,
                Title = "Auto Fail Rate",
                Type = "multiple",
                Data = new List<double>(),
                DataFormat = "0.00%",
                DataLabels = new List<string>(),
                XAxisLabel = "Month",
                YAxisLabel = "%"
            };

            for (int i = 2; i >= 0; i--)
            {
                var monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(i * -1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);
                kpi.DataLabels.Add(monthStart.ToString("MMM yyyy"));

                var auditCount = _context.Audits.Count(d => d.AuditDateTime >= monthStart && d.AuditDateTime <= monthEnd);
                if (auditCount > 0)
                {
                    var autoFailCount = _context.Audits.Count(d => d.AuditDateTime >= monthStart && d.AuditDateTime <= monthEnd && d.Score == 0);
                    kpi.Data.Add(autoFailCount / auditCount);
                }
                else
                {
                    kpi.Data.Add(0);
                }
            }

            return kpi;
        }

        private DashboardKPI GetOutstandingDisputeCount()
        {
            var monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);
            var kpi = new KPISinglePoint<int>
            {
                ColumnNumber = 4,
                Title = "Outstanding Disputes",
                Type = "single",
                Data = _context.Disputes.Count(d => d.DecisionId == null && d.DecisionDateTime == null),
                DataFormat = "G0",
                DataLabel = ""
            };

            return kpi;
        }

        private DashboardKPI GetDisputeRates()
        {
            var kpi = new KPIMultiplePoint<double>
            {
                ColumnNumber = 8,
                Title = "Dispute Rate",
                Type = "multiple",
                Data = new List<double>(),
                DataFormat = "0.00%",
                DataLabels = new List<string>(),
                XAxisLabel = "Month",
                YAxisLabel = "%"
            };

            for (int i = 2; i >= 0; i--)
            {
                var monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(i * -1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);
                kpi.DataLabels.Add(monthStart.ToString("MMM yyyy"));

                var auditCount = _context.Audits.Count(d => d.AuditDateTime >= monthStart && d.AuditDateTime <= monthEnd);
                if(auditCount > 0)
                {
                    var disputeCount = _context.Disputes.Count(d => d.CreateDateTime >= monthStart && d.CreateDateTime <= monthEnd);
                    kpi.Data.Add(disputeCount / auditCount);
                }
                else
                {
                    kpi.Data.Add(0);
                }                
            }

            return kpi;
        }
    }
}