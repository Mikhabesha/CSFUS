using CSFUF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSFUF.Extensions;
using Microsoft.AspNet.Identity;

namespace CSFUF.Controllers
{
    public class DecisionExperts2Controller : Controller
    {
        //
        // GET: /DecisionExperts2/
        CSFUFDB1 db = new CSFUFDB1();

        [Authorize(Roles = "Decision Expert")]
        public ActionResult Index(string ExpNameToSearch)
        {
          
            string sessionUsername = User.Identity.Name;
            string list2 = User.Identity.GetUserName();
            ViewBag.list2 = list2;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();

            var customers = from s in db.DecisionExpertsTask2 select s;
            string regionName = user1.Region;
            ViewBag.RegionName = regionName;

            customers = customers.Where(s => s.AssignedExpert.Contains(User.Identity.Name) && s.ApprovalStatus.Contains("Approved") && s.Region == user1.Region);

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
        public ActionResult Index( DateTime Start, DateTime End, string Name)
        {
            Name = User.Identity.Name;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == Name).FirstOrDefault();

            var customers = from s in db.DecisionExpertsTask2 select s;
            customers = db.DecisionExpertsTask2.Where(s => s.Region == user1.Region);
            string regionName = user1.Region;

            ViewBag.Counting2 = db.DicExp2SearchD(Start, End, Name, regionName).OrderByDescending(s => s.DateRecieved).ToList().Count;
              if (ViewBag.Counting2 == 0)
              {
                  ViewBag.Message2 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
              }
              return View(db.DicExp2SearchD(Start, End, Name, regionName).OrderByDescending(s => s.DateRecieved).ToList());
        }

        [Authorize(Roles = "Decision Expert")]
        public ActionResult Edit(int id)
        {

            CSFUFDB1 dbd = new CSFUFDB1();
            var List = ((from r in dbd.ApproverTasks select r.ApprovalStatus));
            var Lists = new List<string>(from r in dbd.PensionTypes select r.Pension_Type);
            ViewBag.ListNames = Lists;
            using (CSFUFDB1 dbModel = new CSFUFDB1())
            {
                return View(dbModel.DecisionExpertsTask2.Where(x => x.Id == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult Edit(int id, DecisionExpertsTask2 Expert)
        {

            var repos = from s in db.Reports select s;
                using (CSFUFDB1 DbModel = new CSFUFDB1())
                {
                    CSFUFDB1 dbd = new CSFUFDB1();
                    var List = ((from r in dbd.ApproverTasks select r.ApprovalStatus));
                    var Lists = new List<string>(from r in dbd.PensionTypes select r.Pension_Type);
                    ViewBag.ListNames = Lists;

                    DbModel.Entry(Expert).State = EntityState.Modified;
                    DbModel.SaveChanges();

                    DecisionExpertsTask2 dec = DbModel.DecisionExpertsTask2.Where(x => x.Id == id).FirstOrDefault();
                    Report rep = repos.Where(x => x.PrivateIDNo == dec.PrivateIDNo).FirstOrDefault();
                    Payment deX = new Payment();
                    dec.DateTransfered = DateTime.Today;
                    dec.DocStatus = "ወደክፍያ ተልኳል";

                    deX.FullName = dec.FullName;
                    deX.Allowance = dec.Allowance;
                    deX.ArchiveNo = dec.ArchiveNo;
                    deX.DateRecieved = dec.DateTransfered;

                    rep.PaymentDateRecived = dec.DateTransfered;/* The 1st line of code sets the arrival date of the payment table and the 2nd sets the payment status */
                    rep.PaymentStatus = dec.PaymentStatus;
                    rep.Allowance = dec.Allowance;
                    rep.DueDate = dec.DueDate;

                    deX.BDate = dec.BDate;
                    deX.DocCount = dec.DocCount;
                    deX.DueDate = dec.DueDate;
                    deX.Gender = dec.Gender;
                    deX.GovIDNo = dec.GovIDNo;
                    deX.MotherName = dec.MotherName;
                    deX.Id = dec.Id;
                    deX.OrgName = dec.OrgName;
                    deX.OrgTIN = dec.OrgTIN;
                    deX.PhoneNo = dec.PhoneNo;
                    deX.PrivateIDNo = dec.PrivateIDNo;
                    deX.AssignedExpert = dec.AssignedExpert;
                    deX.Bank = dec.Bank;
                    deX.BankBranch = dec.BankBranch;
                    deX.PensionerID = dec.PensionerID;
                    deX.ApprovalStatus = dec.ApprovalStatus;
                    deX.PaymentStatus = dec.PaymentStatus;
                    deX.RegionForUser = dec.Region;

                    DbModel.Entry(rep).State = EntityState.Modified;
                    DbModel.Payments.Add(deX);
                    DbModel.SaveChanges();
                }
                // TODO: Add update logic here
                this.AddNotification("Task Accepted!", NotificationType.SUCCESS);

                return RedirectToAction("Index");
            }
       

        [Authorize(Roles = "Decision Expert")]
        public ActionResult Details(int id)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.DecisionExpertsTask2.Where(x => x.Id == id).FirstOrDefault());
            }

        }
	}
}