using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CSFUF.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using CSFUF.Extensions;

namespace CSFUF.Controllers
{
   // [Authorize(Roles = "Payment")]
     //[Authorize(Roles = "Decision")]
    public class DecisionController : Controller
    {
        //
        // GET: /Decision/
        CSFUFDB1 db = new CSFUFDB1();

         [Authorize(Roles = ("Decision"))]
        public ActionResult Index(string ExpNameToSearch)
        {
            string sessionUsername = User.Identity.Name;
            string list2 = User.Identity.GetUserName();
            ViewBag.list2 = list2;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            var decReg = from s in db.Decisions
                         select s;

            decReg = decReg.Where(s => s.Region == user1.Region);

            if (!String.IsNullOrEmpty(ExpNameToSearch))
            {
                ViewBag.Counting = decReg.Where(s => s.PrivateIDNo.Contains(ExpNameToSearch)).Count();
                if (ViewBag.Counting == 0)
                {
                    ViewBag.Message = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                }
                return View(decReg.Where(s => s.PrivateIDNo.Contains(ExpNameToSearch)));
            }
           else { 
                return View(decReg.OrderByDescending(s => s.DateRecieved).ToList());
                }
        }
        [HttpPost]
        [Authorize(Roles = ("Decision"))]
         public ActionResult Index(DateTime Start, DateTime End)
        {
            string sessionUsername = User.Identity.Name;
            string list2 = User.Identity.GetUserName();
            ViewBag.list2 = list2;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();

            var decReg = db.ConRegs.Where(s => s.Region == user1.Region);
            string regionName = user1.Region;
            ViewBag.RegionName = regionName;

            ViewBag.Counting2 = db.DecisionSearchWithDate(Start, End, regionName).OrderByDescending(s => s.DateRecieved).ToList().Count();
            if (ViewBag.Counting2 == 0)
            {
                ViewBag.Message2 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
            }
            return View(db.DecisionSearchWithDate(Start, End, regionName).OrderByDescending(s => s.DateRecieved).ToList());
   
        }

        [Authorize(Roles = ("BranchTeamLeader"))]
        public ActionResult IndexForBT(string ExpNameToSearch)
        {
            string sessionUsername = User.Identity.Name;
            string list2 = User.Identity.GetUserName();
            ViewBag.list2 = list2;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            var decReg = from s in db.Decisions
                         select s;

            decReg = decReg.Where(s => s.Region == user1.Region);

            if (!String.IsNullOrEmpty(ExpNameToSearch))
            {
                ViewBag.Counting = decReg.Where(s => s.PrivateIDNo.Contains(ExpNameToSearch)).Count();
                if (ViewBag.Counting == 0)
                {
                    ViewBag.Message = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                }
                return View(decReg.Where(s => s.PrivateIDNo.Contains(ExpNameToSearch)));
            }
            else
            {
                return View(decReg.OrderByDescending(s => s.DateRecieved).ToList());
            }
        }
        [HttpPost]
        [Authorize(Roles = ("BranchTeamLeader"))]
        public ActionResult IndexForBT(DateTime Start, DateTime End)
        {
            string sessionUsername = User.Identity.Name;
            string list2 = User.Identity.GetUserName();
            ViewBag.list2 = list2;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();

            var decReg = db.ConRegs.Where(s => s.Region == user1.Region);
            string regionName = user1.Region;
            ViewBag.RegionName = regionName;

            ViewBag.Counting2 = db.DecisionSearchWithDate(Start, End, regionName).OrderByDescending(s => s.DateRecieved).ToList().Count();
            if (ViewBag.Counting2 == 0)
            {
                ViewBag.Message2 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
            }
            return View(db.DecisionSearchWithDate(Start, End, regionName).OrderByDescending(s => s.DateRecieved).ToList());

        }

        [Authorize(Roles = ("Decision Distributor"))]
        public ActionResult IndexForDistribution(string ExpNameToSearch)
        {
            string sessionUsername = User.Identity.Name;
            string list2 = User.Identity.GetUserName();
            ViewBag.list2 = list2;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            var decReg = from s in db.Decisions
                         select s;

            decReg = decReg.Where(s => s.Region == user1.Region);
            string regionName = user1.Region;
            ViewBag.RegionName = regionName;

            if (!String.IsNullOrEmpty(ExpNameToSearch))
            {
                ViewBag.Counting = decReg.Where(s => s.PrivateIDNo.Contains(ExpNameToSearch)).Count();
                if (ViewBag.Counting == 0)
                {
                    ViewBag.Message = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                }
                return View(decReg.Where(s => s.PrivateIDNo.Contains(ExpNameToSearch)));
            }
            else
            {
                return View(decReg.OrderByDescending(s => s.DateRecieved).ToList());
            }
        }
        [HttpPost]
        [Authorize(Roles = ("Decision Distributor"))]
        public ActionResult IndexForDistribution(DateTime Start, DateTime End)
        {
            string sessionUsername = User.Identity.Name;
            string list2 = User.Identity.GetUserName();
            ViewBag.list2 = list2;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            var decReg = from s in db.Decisions
                         select s;
            decReg = decReg.Where(s => s.Region == user1.Region);
            string regionName = user1.Region;
            ViewBag.RegionName = regionName;

            ViewBag.Counting2 = db.DecisionSearchWithDate(Start, End, regionName).OrderByDescending(s => s.DateRecieved).ToList().Count();
            if (ViewBag.Counting2 == 0)
            {
                ViewBag.Message2 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
            }
            return View(db.DecisionSearchWithDate(Start, End, regionName).OrderByDescending(s => s.DateRecieved).ToList());

        }


        //[Authorize(Roles = ("Decision"))]
        public ActionResult Details(int id)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.Decisions.Where(x => x.Id == id).FirstOrDefault());
            }
            
        }

        [Authorize(Roles = ("Decision Distributor"))]
        public ActionResult DetailsForDistributor(int id)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.Decisions.Where(x => x.Id == id).FirstOrDefault());
            }

        }


        // this codes are not necessary (specifically CREATE ACTION RESULT )
        //
        // GET: /Customer/Create
        //[Authorize(Roles = ("Decision"))]
        public ActionResult Create()
        {
            var list = new List<string>() { "Male", "Femal" };
            ViewBag.list = list;
            var list1 = new List<string>() { "--Select--","Registration -- ምዝገባ", "Contribution -- መዋጮ", "Decision -- ዉሳኔ", "Payment -- ክፍያ" };
            ViewBag.list1 = list1;  
            return View();
        }

        //
        // POST: /Customer/Create
        [HttpPost]
        //[Authorize(Roles = ("Decision"))]
        public ActionResult Create(Decision customer)
        {
                using (CSFUFDB1 DbModel = new CSFUFDB1())
                { 
                    DbModel.Decisions.Add(customer);
                    DbModel.SaveChanges();
                  
                }
                // TODO: Add insert logic here

                return RedirectToAction("Index");
           
        }

        // starting from above --- this codes are not necessary (specifically  the "CREATE"  ACTION RESULT )

        [Authorize(Roles = ("Decision"))]
        public ActionResult Edit(int id)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {

                return View(DbModel.Decisions.Where(x => x.Id == id).FirstOrDefault());
            }
            
        }
        [HttpPost]
        [Authorize(Roles = ("Decision"))]
        public ActionResult Edit(int id, Decision customer)
        {
                using (CSFUFDB1 DbModel = new CSFUFDB1())
                {
                    DbModel.Entry(customer).State = EntityState.Modified;
                    DbModel.SaveChanges();
                }
                this.AddNotification("Success!!", NotificationType.SUCCESS);
                // TODO: Add update logic here    
                return RedirectToAction("Index");
        }
 
         [Authorize(Roles="Admin")]
        public ActionResult Delete(int id)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.Decisions.Where(x => x.Id == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {

                using (CSFUFDB1 DbModel = new CSFUFDB1())
                {

                    Decision Emp = DbModel.Decisions.Where(x => x.Id == id).FirstOrDefault();
                  //  Decision con = new Decision();
                    DbModel.Decisions.Remove(Emp);
                    DbModel.SaveChanges();

                }
                // TODO: Add delete logic here

                return RedirectToAction("Index");
        }

        //[Authorize(Roles = ("Decision"))]
        public ActionResult SendTo(int id)
        {
            CSFUFDB1 dbd = new CSFUFDB1();
            var Lists = new List<string>(from r in dbd.WusaneExperts select r.ExpertName).ToList();
            ViewBag.ListNames = Lists;

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.Decisions.Where(x => x.Id == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult SendTo(int id, FormCollection collection, Decision customer)
        {

            try
            {
                using (CSFUFDB1 DbModel = new CSFUFDB1())
                {
                    var repos = from s in db.Reports
                                select s;
                    

                    Decision dec = DbModel.Decisions.Where(x => x.Id == id).FirstOrDefault();
                    DecisionExpertsTask deX = new DecisionExpertsTask();
                    Report rep = repos.Where(x => x.PrivateIDNo == dec.PrivateIDNo).FirstOrDefault();

                    dec.DateTransfered = DateTime.Today;

                    
                    deX.FullName = dec.FullName;
                    deX.Allowance = dec.Allowance;
                    deX.ArchiveNo = dec.ArchiveNo;
                    deX.DateRecieved = dec.DateTransfered;

                    rep.DateRecievedToDecExpert = dec.DateTransfered; /* this line copies date info. for WUSANE EXPERT...
                                                                       * as to when he/she recieved the file     */
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
                    deX.AssignedExpert = dec.AssignedExpertNames;
                    dec.ExpertNames = dec.AssignedExpertNames;
                    deX.Region = dec.Region;

                    DbModel.Entry(rep).State = EntityState.Modified;
                    DbModel.DecisionExpertsTasks.Add(deX);
                    DbModel.SaveChanges();

                }
                // TODO: Add Send logic here
                this.AddNotification("Success!!", NotificationType.SUCCESS);
                if (User.IsInRole("BranchTeamLeader"))
                {
                    return RedirectToAction("IndexForBT");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = ("Decision Distributor"))]
        public ActionResult SendToDistro(int id)
        {
            CSFUFDB1 dbd = new CSFUFDB1();
            var Lists = new List<string>((from r in dbd.WusaneExperts select r.ExpertName).ToList());
            ViewBag.ListNames = Lists;

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.Decisions.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        //
        // POST: /Customer/Delete/5
        [HttpPost]
        public ActionResult SendToDistro(int id, FormCollection collection, Decision customer)
        {
            try
            {
                using (CSFUFDB1 DbModel = new CSFUFDB1())
                {
                    var repos = from s in db.Reports
                                select s;


                    Decision dec = DbModel.Decisions.Where(x => x.Id == id).FirstOrDefault();
                    DecisionExpertsTask deX = new DecisionExpertsTask();
                    Report rep = repos.Where(x => x.PrivateIDNo == dec.PrivateIDNo).FirstOrDefault();

                    dec.DateTransfered = DateTime.Today;


                    deX.FullName = dec.FullName;
                    deX.Allowance = dec.Allowance;
                    deX.ArchiveNo = dec.ArchiveNo;
                    deX.DateRecieved = dec.DateTransfered;

                    rep.DateRecievedToDecExpert = dec.DateTransfered; /* this line copies date info. for WUSANE EXPERT...
                                                                       * as to when he/she recieved the file     */
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
                    deX.AssignedExpert = dec.AssignedExpertNames;
                    dec.ExpertNames = dec.AssignedExpertNames;
                    deX.Region = dec.Region;

                    DbModel.Entry(rep).State = EntityState.Modified;
                    DbModel.DecisionExpertsTasks.Add(deX);
                    DbModel.SaveChanges();

                }
                // TODO: Add Send logic here
                this.AddNotification("Success!!", NotificationType.SUCCESS);
                return RedirectToAction("IndexForDistribution");
            }
            catch
            {
                return View();
            }
        }

        //[Authorize(Roles = ("Decision"))]
        public ActionResult AssignExp(int id)
        {
            CSFUFDB1 dbd = new CSFUFDB1();
            string sessionUsername = User.Identity.Name;
            string list2 = User.Identity.GetUserName();
            ViewBag.list2 = list2;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();

            var decReg = db.WusaneExperts.Where(s => s.Region == user1.Region);
            string regionName = user1.Region;
            ViewBag.RegionName = regionName;

            var Lists = new List<string>((from r in decReg select r.ExpertName).ToList());
            ViewBag.ListNames = Lists;
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                //return View(decReg.OrderByDescending(s => s.DateRecieved).ToList());
                return View(DbModel.Decisions.Where(x => x.Id == id).FirstOrDefault());
            }

        }
        [HttpPost]
        public ActionResult AssignExp(int id, Decision customer)
        {
            string sessionUsername = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();

            var repos = from s in db.Reports
                        select s;
            repos = repos.Where(x => x.RegionRegistered == user1.Region);
            Report rep = repos.Where(x => x.PrivateIDNo == customer.PrivateIDNo).FirstOrDefault();


            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                rep.AssignedDecExpert = customer.AssignedExpertNames;
                DbModel.Entry(rep).State = EntityState.Modified;
                DbModel.Entry(customer).State = EntityState.Modified;
                DbModel.SaveChanges();
            }
            // TODO: Add update logic here    
            CSFUFDB1 DbModel2 = new CSFUFDB1();
            return RedirectToAction("SendTo" +"/" + id);
        }

        [Authorize(Roles = ("Decision Distributor"))]
        public ActionResult AssignExpForDistrubitor(int id)
        {
            string sessionUsername = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();

            var decReg = db.WusaneExperts.Where(s => s.Region == user1.Region);
            string regionName = user1.Region;
            ViewBag.RegionName = regionName;

            var Lists = new List<string>((from r in decReg select r.ExpertName).ToList());
            ViewBag.ListNames = Lists;
            CSFUFDB1 dbd = new CSFUFDB1();
            ViewBag.ListNames = Lists;
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.Decisions.Where(x => x.Id == id).FirstOrDefault());
            }

        }
        [HttpPost]
        public ActionResult AssignExpForDistrubitor(int id, Decision customer)
        {
            string sessionUsername = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();

            var repos = from s in db.Reports
                        select s;
            repos = repos.Where(x => x.RegionRegistered == user1.Region);
            Report rep = repos.Where(x => x.PrivateIDNo == customer.PrivateIDNo).FirstOrDefault();


            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                rep.AssignedDecExpert = customer.AssignedExpertNames;
                DbModel.Entry(rep).State = EntityState.Modified;
                DbModel.Entry(customer).State = EntityState.Modified;
                DbModel.SaveChanges();
            }
            // TODO: Add update logic here    
            CSFUFDB1 DbModel2 = new CSFUFDB1();
            return RedirectToAction("SendToDistro" + "/" + id);
        }


        [Authorize(Roles = ("Admin"))]
        public ActionResult CreateExpert()
        {
            CSFUFDB1 dbd = new CSFUFDB1();
            var Lists = new List<string>((from r in dbd.WusaneExperts select r.ExpertName).ToList());
            ViewBag.ListNames = Lists;  
            return View();
        }

        [HttpPost]
        public ActionResult CreateExpert(WusaneExpert customer)
        {
            
                using (CSFUFDB1 DbModel = new CSFUFDB1())
                {
                    DbModel.WusaneExperts.Add(customer);
                    DbModel.SaveChanges();

                }
                // TODO: Add insert logic here
                this.AddNotification("Success!!", NotificationType.SUCCESS);
                return RedirectToAction("CreateExpert");
         }

       
        
    }
}
