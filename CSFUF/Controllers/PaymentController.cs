using CSFUF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSFUF.Controllers
{
    public class PaymentController : Controller
    {
        //
        // GET: /Payment/
        CSFUFDB1 db = new CSFUFDB1();

        [Authorize(Roles = "Payment")]
        public ActionResult Index(string ExpNameToSearch)
        {
            string sessionUsername = User.Identity.Name;
    
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            
            var customers = from s in db.Payments
                            select s;

            customers = customers.Where(s => s.RegionForUser == user1.Region);
            string regionName = user1.Region;
           
            if (!String.IsNullOrEmpty(ExpNameToSearch))
            {
                ViewBag.Counting = customers.Where(s => s.PrivateIDNo.Contains(ExpNameToSearch)).Count();
                if (ViewBag.Counting == 0)
                {
                    ViewBag.Message = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                }
                return View(customers.Where(s => s.PrivateIDNo.Contains(ExpNameToSearch)));

            }
            else
            {
                return View(customers.OrderByDescending(s => s.DateRecieved).ToList());
            }

        }
        [HttpPost]
        public ActionResult Index(DateTime Start, DateTime End)
        {
            string sessionUsername = User.Identity.Name;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();

            var customers = from s in db.Payments
                            select s;

            customers = customers.Where(s => s.Region == user1.Region);
            string regionName = user1.Region;

            ViewBag.Count = db.PaySearchD(Start, End, regionName).Count();
             ViewBag.Start = Start;
             ViewBag.End = End;
             ViewBag.Counting2 = db.PaySearchD(Start, End, regionName).OrderByDescending(s => s.DateRecieved).ToList().Count();
             if (ViewBag.Counting2 == 0)
             {
               ViewBag.Message2 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
             }
            return View(db.PaySearchD(Start,End, regionName).OrderByDescending(s=> s.DateRecieved).ToList());
        }

        [Authorize(Roles = "Payment")]
        public ActionResult Edit(int id)
        {
            CSFUFDB1 dbd = new CSFUFDB1();
            var Lists = new List<string>((from r in dbd.RegionsDbs select r.RegionName).ToList());
            ViewBag.ListNames = Lists;
            return View(db.Payments.Where(x=> x.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int id, Payment Pay)
        {
            /* this edit section first sets its own data...
             to the likes of a payment expert and at the ....
             * same time sets the report's..feilds for upddate purpose */
            var repos = from s in db.Reports select s;
            db.Entry(Pay).State = EntityState.Modified;
            db.SaveChanges();

            Payment pp = db.Payments.Where(x => x.Id == id).FirstOrDefault();
            Report rep = repos.Where(x => x.PrivateIDNo == pp.PrivateIDNo).FirstOrDefault();

            pp.PayExpert = User.Identity.Name;
            rep.AssignedRegion = pp.Region;
            rep.PaymentExpert = pp.PayExpert;
            rep.PaymentsWhen = pp.when;
            rep.PayLetterPrepDayForRep = pp.PayLetterPrepDay;
            rep.PayIssueDateForRep = pp.issueDate;
            rep.ReciepentPayExpForRep = pp.Recipent;
            rep.ReferenceNoForReport = pp.RefNo;

            db.Entry(Pay).State = EntityState.Modified;
            db.Entry(rep).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Payment")]
        public ActionResult Details(int id)
        {

            return View(db.Payments.Where(x => x.Id == id).FirstOrDefault());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateRegion()
        {
            return View();
        }

        //
        // POST: /Customer/Create
        [HttpPost]
        public ActionResult CreateRegion(RegionsDb Region)
        {

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                DbModel.RegionsDbs.Add(Region);
                DbModel.SaveChanges();

            }
            // TODO: Add insert logic here

            return RedirectToAction("Index");
        }

	}
}