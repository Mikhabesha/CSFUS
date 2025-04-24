using CSFUF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSFUF.Controllers
{
    public class CustomerTrackerController : Controller
    {
        //
        // GET: /CustomerTracker/
        [HttpGet]
        public ActionResult Index()
        {
                return View();
        }
        [HttpPost]
        public ActionResult Index(string searching)
        {
            if (!String.IsNullOrEmpty(searching))
            {
                CSFUFDB1 db = new CSFUFDB1();
                var customers = from s in db.Reports
                                select s;
                customers = db.Reports.Where(s => s.PrivateIDNo == searching);
                if (customers.Any() != true)
                {
                    ViewBag.ErrorMsg = "ፍለጋዎ የለም። እባክዎ እንደገና የጡረታ መለያ ቁጥርን ብቻ በማስገባት ይሞክሩ!!";
                    return View();
                }
                return View(customers.OrderByDescending(s => s.DateRegistered).ToList());

            }
            else
            {
                return View();
            }

        }

        public ActionResult Details(int id, string sortOrder)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                var re = DbModel.Reports.Where(x => x.Id == id);
                Report rep = DbModel.Reports.Where(x => x.Id == id).FirstOrDefault();

                string Reg = rep.RegionRegistered;
                ViewBag.Region = Reg;
                return View(DbModel.Reports.Where(x => x.Id == id).FirstOrDefault());
            }

        }
       
    }
}