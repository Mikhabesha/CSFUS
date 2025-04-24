using CSFUF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSFUF.Controllers
{
    public class ApproverController : Controller
    {
        CSFUFDB1 db = new CSFUFDB1();
        [Authorize(Roles = ("Admin"))]
        public ActionResult Index(string searching)
        {
            var customers = from s in db.Approvers
                            select s;
            if (!String.IsNullOrEmpty(searching))
            {
                customers = customers.Where(s => s.ApproverName.Contains(searching));
                //customers = customers.Where(s => s.ArchiveNo.Value =);
            }

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(customers.ToList());
                //return View(DbModel.CustomerRegs.ToList());
            }

        }
        //
        // GET: /Approver/
        [Authorize(Roles = "Admin")]
        public ActionResult CreateApprover()
        {
            CSFUFDB1 dbd = new CSFUFDB1();
            var Lists = new List<string>((from r in dbd.Approvers select r.ApproverName).ToList());
            ViewBag.ListNames = Lists;
            return View();
        }

        //
        // POST: /Customer/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateApprover(Approver customer)
        {

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                DbModel.Approvers.Add(customer);
                DbModel.SaveChanges();

            }
            // TODO: Add insert logic here

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ApproverEdit(int id)
        {
            using (CSFUFDB1 dbModl = new CSFUFDB1())
            {
                return View(dbModl.Approvers.Where(x=> x.Id == id).FirstOrDefault());
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult ApproverEdit(int id, Approver Appr)
        {

            using (CSFUFDB1 dbMdl = new CSFUFDB1())
            {
                dbMdl.Entry(Appr).State = EntityState.Modified;
                dbMdl.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult ApproverDetails(int id)
        {
            using (CSFUFDB1 dbModl = new CSFUFDB1())
            {
                return View(dbModl.Approvers.Where(x => x.Id == id).FirstOrDefault());
            }
        }
        [Authorize(Roles = ("Admin"))]
        public ActionResult ApproverDelete(int id)
        {
            using (CSFUFDB1 dbModl = new CSFUFDB1())
            {
                return View(dbModl.Approvers.Where(x => x.Id == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult ApproverDelete(int id, FormCollection colec)
        {
            using(CSFUFDB1 db = new CSFUFDB1())
	        {
                Approver app = db.Approvers.Where(x => x.Id == id).FirstOrDefault();
                db.Approvers.Remove(app);
                db.SaveChanges();
	        }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = ("Approver"))]
        public ActionResult TeBeApprovedLists( string searchID)
        {
            string sessionUsername = User.Identity.Name;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            string regionName = user1.Region;
            ViewBag.RegionName = regionName;

            var customers = from s in db.ApproverTasks
                            select s;

            customers = customers.Where(s => s.AssignedApprover == sessionUsername && s.Region == regionName);

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                if (!String.IsNullOrEmpty(searchID))
                {
                    ViewBag.Counting = customers.Where(s => s.PrivateIDNo.Contains(searchID)).Count();
                    if (ViewBag.Counting == 0)
                    {
                        ViewBag.Message = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                    }
                    return View(customers.Where(s => s.PrivateIDNo.Contains(searchID)));
                }
                else
                {
                    return View(customers.OrderByDescending(s => s.DateRecieved).ToList());
                }
            } 
        }        
        [HttpPost]
        public ActionResult TeBeApprovedLists(DateTime Start, DateTime End, String Name)
        {  
            Name = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == Name).FirstOrDefault();
            string regionName = user1.Region;

            var customers = from s in db.ApproverTasks
                            select s;
           

            customers = customers.Where(s => s.AssignedApprover == Name && s.Region == regionName);
            ViewBag.Counting2 = db.ApproverSearch(Start, End, Name, regionName).Count();
            if (ViewBag.Counting2 == 0)
            {
                ViewBag.Message2 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
            }
            return View(db.ApproverSearch(Start, End, Name, regionName));
        }

        [Authorize(Roles = ("BranchTeamLeader"))]
        public ActionResult TeBeApprovedListsForBT(string searchID)
        {
            string sessionUsername = User.Identity.Name;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            string regionName = user1.Region;
            ViewBag.RegionName = regionName;

            var customers = from s in db.ApproverTasks
                            select s;

            customers = customers.Where(s => s.AssignedApprover == sessionUsername && s.Region == regionName);

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                if (!String.IsNullOrEmpty(searchID))
                {
                    ViewBag.Counting = customers.Where(s => s.PrivateIDNo.Contains(searchID)).Count();
                    if (ViewBag.Counting == 0)
                    {
                        ViewBag.Message = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                    }
                    return View(customers.Where(s => s.PrivateIDNo.Contains(searchID)));
                }
                else
                {
                    return View(customers.OrderByDescending(s => s.DateRecieved).ToList());
                }
            }
        }
        [HttpPost]
        public ActionResult TeBeApprovedListsForBT(DateTime Start, DateTime End, String Name)
        {
            Name = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == Name).FirstOrDefault();
            string regionName = user1.Region;

            var customers = from s in db.ApproverTasks
                            select s;


            customers = customers.Where(s => s.AssignedApprover == Name && s.Region == regionName);
            ViewBag.Counting2 = db.ApproverSearch(Start, End, Name, regionName).Count();
            if (ViewBag.Counting2 == 0)
            {
                ViewBag.Message2 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
            }
            return View(db.ApproverSearch(Start, End, Name, regionName));
        }

        //[Authorize(Roles = ("Approver"))]
        public ActionResult Edit(int id)
        {
            CSFUFDB1 dbd = new CSFUFDB1();
            var List = ((from r in dbd.ApproverTasks select r.ApprovalStatus));
            var Lists = new List<string>() { "Approved", "Pending" };
            ViewBag.ListNames = Lists;
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {

                return View(DbModel.ApproverTasks.Where(x => x.Id == id).FirstOrDefault());
            }

        }
        // POST: /Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ApproverTask customer)
        {
            var repos = from s in db.Reports select s;

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                DbModel.Entry(customer).State = EntityState.Modified;
                DbModel.SaveChanges();

                ApproverTask con = DbModel.ApproverTasks.Where(x => x.Id == id).FirstOrDefault();
                DecisionExpertsTask2 dic = new DecisionExpertsTask2();
                Report rep = repos.Where(x => x.PrivateIDNo == con.PrivateIDNo).FirstOrDefault();
                con.DateTransfered = DateTime.Today; 

                if (con.ApprovalStatus == "Pending")
                {
                    rep.ApprovalStatus = con.ApprovalStatus;
                    con.DateTransfered = null;
                    rep.ApprovedDate = con.DateTransfered;
                }
                else
                {
                    rep.ApprovalStatus = con.ApprovalStatus;
                    rep.ApprovedDate = con.DateTransfered; 
                }
                DbModel.Entry(rep).State = EntityState.Modified;

                dic.FullName = con.FullName;
                //con.Allowance = Emp.Allowance;
                dic.ArchiveNo = con.ArchiveNo;
                //con.DateRecieved = Emp.DateRecieved;
                dic.DateRecieved = con.DateTransfered;
                dic.BDate = con.BDate;
                dic.DocCount = con.DocCount;
                dic.DueDate = con.DueDate;
                dic.Gender = con.Gender;
                dic.GovIDNo = con.GovIDNo;
                dic.MotherName = con.MotherName;
                dic.Id = con.Id;
                dic.OrgName = con.OrgName;
                dic.OrgTIN = con.OrgTIN;
                dic.PhoneNo = con.PhoneNo;
                dic.PrivateIDNo = con.PrivateIDNo;
                dic.AssignedExpert = con.AssignedExpertNames;
                dic.ApprovalStatus = con.ApprovalStatus;
                dic.DocStatus = con.DocStatus;
                dic.Region = con.Region;
                //con.SentFrom = Emp.SentFrom;// i was here the last time

                DbModel.DecisionExpertsTask2.Add(dic);
                DbModel.SaveChanges();
            }
            // TODO: Add update logic here    
            if (User.IsInRole("BranchTeamLeader"))
            {
                return RedirectToAction("TeBeApprovedListsForBT");
            }
            return RedirectToAction("TeBeApprovedLists");
        }
       
        //[Authorize(Roles = ("Approver"))]
        public ActionResult Details(int id)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.ApproverTasks.Where(x => x.Id == id).FirstOrDefault());
            }

        }

      
	}
}