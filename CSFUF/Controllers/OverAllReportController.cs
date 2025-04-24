using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSFUF.Models;
using CSFUF.OverAllView;

namespace CSFUF.Controllers
{
    public class OverAllReportController : Controller
    {
        //
        // GET: /OverAllReport/
        public ActionResult Index( string searchId)
        {
            CSFUFDB1 Db = new CSFUFDB1();
            var all = new OverAllReports();
            all.Registration = Db.CustomerRegs.OrderByDescending(s => s.DateRecieved).ToList();
            //all.Contribution = Db.ConRegs.OrderByDescending(s => s.DateRecieved).ToList();
            //all.Decision = Db.Decisions.OrderByDescending(s => s.DateRecieved).ToList();
            //all.DecisionTasks = Db.DecisionExpertsTasks.OrderByDescending(s => s.DateRecieved).ToList();
            //all.Payment = Db.Payments.OrderByDescending(s => s.DateRecieved).ToList();
            if (!String.IsNullOrEmpty(searchId))
            {
                         
            all.Registration = Db.CustomerRegs.Where(s => s.PrivateIDNo.Contains(searchId)).OrderByDescending(s => s.DateRecieved).ToList();
            all.Contribution = Db.ConRegs.Where(s => s.PrivateIDNo.Contains(searchId)).OrderByDescending(s => s.DateRecieved).ToList();
            all.Decision = Db.Decisions.Where(s => s.PrivateIDNo.Contains(searchId)).OrderByDescending(s => s.DateRecieved).ToList();
            all.DecisionTasks = Db.DecisionExpertsTasks.Where(s => s.PrivateIDNo.Contains(searchId)).OrderByDescending(s => s.DateRecieved).ToList();
            all.Payment = Db.Payments.Where(s => s.PrivateIDNo.Contains(searchId)).OrderByDescending(s => s.DateRecieved).ToList();
            }
            return View(all);
        }

        public ActionResult Details(int id, string sortOrder)
        {
            CustomerReg cus = new CustomerReg();
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.Reports.Where(x => x.Id == id).FirstOrDefault());
            }

        }
	}
}