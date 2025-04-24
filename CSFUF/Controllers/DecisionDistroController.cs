using CSFUF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSFUF.Extensions;
using PagedList;


namespace CSFUF.Controllers
{
    
    public class DecisionDistroController : Controller
    {
        //
        // GET: /DecisionDistro/
        CSFUFDB1 db = new CSFUFDB1();

        //[Authorize(Roles = "Decision")]
        public ActionResult AssignApprover(int id)
        {
            string sessionUsername = User.Identity.Name;
            //string list2 = User.Identity.GetUserName();
           
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            string regionName = user1.Region;

            //var Lists = new List<string>((from r in dbd.ContributionExperts where r.Region == user1.Region select r.ConExpName).ToList());
            //ViewBag.ListNames = Lists;

            CSFUFDB1 dbd = new CSFUFDB1();
            var Lists = new List<string>(from r in dbd.Approvers where r.Region == user1.Region select r.ApproverName).ToList();
            ViewBag.ListNames = Lists;

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.DecisionDistro2.Where(x => x.Id == id).FirstOrDefault());
            }

        }
        [HttpPost]
        public ActionResult AssignApprover(int id, DecisionDistro2 customer)
        {
            var repos = from s in db.Reports select s;

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                DbModel.Entry(customer).State = EntityState.Modified;
                DbModel.SaveChanges();

                DecisionDistro2 dec2 = DbModel.DecisionDistro2.Where(x => x.Id == id).FirstOrDefault();
                Report rep = repos.Where(x => x.PrivateIDNo == dec2.PrivateIDNo).FirstOrDefault();

                rep.AssignedApprover = dec2.AssinedApprover;/* This line assigns a value for "approverAssigned" field on reports table*/

                DbModel.Entry(rep).State = EntityState.Modified;
                DbModel.SaveChanges();
            }
            // TODO: Add update logic here    
            CSFUFDB1 DbModel2 = new CSFUFDB1();
            return RedirectToAction("SendTo" + "/" + id);
        }


        [Authorize(Roles = "Decision")]
        public ActionResult Distro2(string sortOrder, string currentFilter, string searchString, int? page)
        {
            string sessionUsername = User.Identity.Name;
            
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            string regionName = user1.Region;


            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var customers = from s in db.DecisionDistro2
                            select s;
            customers = customers.Where(s => s.Region == user1.Region);

            switch (sortOrder)
            {
                case "name_desc":
                    customers = customers.OrderByDescending(s => s.FullName);
                    break;
                case "Date":
                    customers = customers.OrderBy(s => s.DateRecieved);
                    break;
                case "date_desc":
                    customers = customers.OrderByDescending(s => s.DateRecieved);
                    break;
                default:
                    customers = customers.OrderBy(s => s.FullName);
                    break;
            }

            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.Counting = customers.Where(s => s.PrivateIDNo.Contains(searchString)).Count();
                if ( ViewBag.Counting == 0)
                {
                    ViewBag.Message = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                }
                return View(customers.Where(s => s.PrivateIDNo.Contains(searchString)));

            }
            else
            {
                return View(customers.OrderByDescending(s => s.DateRecieved).ToList());
            }
           
        }
        [HttpPost]
        public ActionResult Distro2(DateTime Start, DateTime End)
        {
            string sessionUsername = User.Identity.Name;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            string regionName = user1.Region;

            ViewBag.Counting2 = db.DistroDateSearch(Start, End, regionName).OrderByDescending(s => s.DateRecieved).ToList().Count();
            if (ViewBag.Counting2 == 0)
            {
                ViewBag.Message2 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
            }
            return View(db.DistroDateSearch(Start, End, regionName).OrderByDescending(s => s.DateRecieved).ToList());
        }

        [Authorize(Roles = "BranchTeamLeader")]
        public ActionResult Distro2ForBT(string sortOrder, string currentFilter, string searchString, int? page)
        {
            string sessionUsername = User.Identity.Name;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            string regionName = user1.Region;


            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var customers = from s in db.DecisionDistro2
                            select s;
            customers = customers.Where(s => s.Region == user1.Region);

            switch (sortOrder)
            {
                case "name_desc":
                    customers = customers.OrderByDescending(s => s.FullName);
                    break;
                case "Date":
                    customers = customers.OrderBy(s => s.DateRecieved);
                    break;
                case "date_desc":
                    customers = customers.OrderByDescending(s => s.DateRecieved);
                    break;
                default:
                    customers = customers.OrderBy(s => s.FullName);
                    break;
            }

            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.Counting = customers.Where(s => s.PrivateIDNo.Contains(searchString)).Count();
                if (ViewBag.Counting == 0)
                {
                    ViewBag.Message = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                }
                return View(customers.Where(s => s.PrivateIDNo.Contains(searchString)));

            }
            else
            {
                return View(customers.OrderByDescending(s => s.DateRecieved).ToList());
            }

        }
        [HttpPost]
        public ActionResult Distro2ForBT(DateTime Start, DateTime End)
        {
            string sessionUsername = User.Identity.Name;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            string regionName = user1.Region;

            ViewBag.Counting2 = db.DistroDateSearch(Start, End, regionName).OrderByDescending(s => s.DateRecieved).ToList().Count();
            if (ViewBag.Counting2 == 0)
            {
                ViewBag.Message2 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
            }
            return View(db.DistroDateSearch(Start, End, regionName).OrderByDescending(s => s.DateRecieved).ToList());
        }

        [Authorize(Roles = "Decision")]
        public ActionResult Edit2(int id)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {

                return View(DbModel.DecisionDistro2.Where(x => x.Id == id).FirstOrDefault());
            }

        }
        [HttpPost]
        public ActionResult Edit2(int id, DecisionDistro2 customer)
        {
            var list1 = new List<string>() { "Approved", "Pending" };
            ViewBag.list1 = list1;
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                DbModel.Entry(customer).State = EntityState.Modified;
                DbModel.SaveChanges();
            }
            this.AddNotification("Success!!", NotificationType.SUCCESS);

            // TODO: Add update logic here    
            return RedirectToAction("Distro2");
        }

        //[Authorize(Roles = "Decision")]
        public ActionResult Details2(int id)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.DecisionDistro2.Where(x => x.Id == id).FirstOrDefault());
            }

        }

        //[Authorize(Roles = "Decision")]
        public ActionResult SendTo(int id)
        {
            CSFUFDB1 dbd = new CSFUFDB1();
            var Lists = new List<string>((from r in dbd.Approvers select r.ApproverName).ToList());
            ViewBag.ListNames = Lists;

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.DecisionDistro2.Where(x => x.Id == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult SendTo(int id, FormCollection collection, DecisionDistro2 customer)
        {
            var repos = from s in db.Reports select s;
            try
            {
                using (CSFUFDB1 DbModel = new CSFUFDB1())
                {

                    DecisionDistro2 dec = DbModel.DecisionDistro2.Where(x => x.Id == id).FirstOrDefault();
                    ApproverTask deX = new ApproverTask();
                    Report rep = repos.Where(x => x.PrivateIDNo == dec.PrivateIDNo).FirstOrDefault();
                    dec.DateTransfered = DateTime.Today;

                    deX.FullName = dec.FullName;
                    deX.Allowance = dec.Allowance;
                    deX.ArchiveNo = dec.ArchiveNo;
                    deX.DateRecieved = dec.DateTransfered;

                    rep.DateTransferedOfDecExpert = dec.DateTransfered;/* this line copies Date info. from WUSANE TEAMLEADER...
                                                                        *  to reports "transferDateToApprover" Field  */
                    rep.DocStatusOfDec = "Sent To Approver";
                    rep.ApprovalStatus = "ገና ያልተሰራ";
                    //deX.DateTransfered = dec.DateRecieved;
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
                    deX.AssignedExpertNames = dec.AssignedExpertNames;
                    deX.AssignedApprover = dec.AssinedApprover;
                    deX.ApprovalStatus = "ገና ያልተሰራ";
                    deX.Region = dec.Region;
                    dec.ExpertNames = dec.AssignedExpertNames;

                    DbModel.Entry(rep).State = EntityState.Modified;
                    DbModel.ApproverTasks.Add(deX);
                    DbModel.SaveChanges();

                }
                // TODO: Add Send logic here
                this.AddNotification("Success!!", NotificationType.SUCCESS);
                if (User.IsInRole("BranchTeamLeader"))
                {
                    return RedirectToAction("Distro2ForBT");
                }
                return RedirectToAction("Distro2");
            }
            catch
            {
                return View();
            }
        }
	}
}