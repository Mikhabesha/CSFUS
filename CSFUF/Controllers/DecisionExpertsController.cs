using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSFUF.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using CSFUF.Extensions;


namespace CSFUF.Controllers
{
    public class DecisionExpertsController : Controller
    {
        //
        // GET: /DecisionExperts/
        CSFUFDB1 db = new CSFUFDB1();

        [Authorize(Roles = "Decision Expert")]
        public ActionResult Index(string ExpNameToSearch)
        {
            CSFUFDB1 dbd = new CSFUFDB1();
            var Lists = new SelectList((from r in dbd.WusaneExperts select r.ExpertName).ToList());
            ViewBag.ExpNameToSearch = Lists;

            var customers = from s in db.DecisionExpertsTasks
                           select s;

            if (!String.IsNullOrEmpty(ExpNameToSearch))
            {
                customers = customers.Where(s => s.AssignedExpert.Contains(ExpNameToSearch));

            }

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(customers.OrderByDescending(s => s.DateRecieved).ToList());
              
            }

        }

        //
        // GET: /Customer/Details/5
        public ActionResult Details(int id)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.DecisionExpertsTasks.Where(x => x.Id == id).FirstOrDefault());
            }

        }

        [Authorize(Roles = "Decision Expert")]
        public ActionResult Edit(int id)
        {
            var list1 = new List<string>() { "Approved", "Pending" };
            ViewBag.list1 = list1;
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {

                return View(DbModel.DecisionExpertsTasks.Where(x => x.Id == id).FirstOrDefault());
            }

        }

        //
        // POST: /Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, DecisionExpertsTask customer)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                var repos = from s in db.Reports
                          select s;

                DbModel.Entry(customer).State = EntityState.Modified;
                DbModel.SaveChanges();

                DecisionExpertsTask dec = DbModel.DecisionExpertsTasks.Where(x => x.Id == id).FirstOrDefault();
                DecisionDistro2 deX = new DecisionDistro2();

                Report rep = repos.Where(x => x.PrivateIDNo == dec.PrivateIDNo && x.RegionRegistered == dec.Region).FirstOrDefault();
                dec.DateTransfered = DateTime.Today;

                deX.FullName = dec.FullName;
                deX.Allowance = dec.Allowance;
                deX.ArchiveNo = dec.ArchiveNo;
                deX.DateRecieved = dec.DateTransfered;

                rep.PhoneNo = dec.PhoneNo;  /* this line copies Phone info. from WUSANE EXPERT...
                                            *  to reports phone OR it basically Overrites the phone information..
                                             *  for the report purpose     */
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
                deX.AssignedExpertNames = dec.AssignedExpert;
                deX.Region = dec.Region;
             
   
                DbModel.DecisionDistro2.Add(deX);
                DbModel.Entry(rep).State = EntityState.Modified;
                DbModel.SaveChanges();
            }
            this.AddNotification("Success!!", NotificationType.SUCCESS);
            // TODO: Add update logic here    
            return RedirectToAction("filterByUserName");
        }

        [Authorize(Roles = "Decision Expert")]
        public ActionResult filterByUserName(string ExpNameToSearch)
        {
            string sessionUsername = User.Identity.Name;
            string list2 = User.Identity.GetUserName();
            ViewBag.list2 = list2;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();

            var customers = db.DecisionExpertsTasks.Where(s => s.Region == user1.Region);
            string regionName = user1.Region;
            ViewBag.RegionName = regionName;

              customers = customers.Where(s => s.AssignedExpert == sessionUsername);

              if (!String.IsNullOrEmpty(ExpNameToSearch))
              {
                  ViewBag.Counting = customers.Where(s => s.PrivateIDNo.Contains(ExpNameToSearch)).ToList().Count();
                  if (ViewBag.Counting == 0)
                  {
                      ViewBag.Message = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                      return View(customers.Where(s => s.PrivateIDNo.Contains(ExpNameToSearch)).ToList());
                  }
                  return View(customers.Where(s => s.PrivateIDNo.Contains(ExpNameToSearch)).ToList());

              }
              else
              {
                  return View(customers.OrderByDescending(s => s.DateRecieved).ToList());
              }
            
        }
        [HttpPost]
        public ActionResult filterByUserName(DateTime start, DateTime end, String Name)
        { 
            Name = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == Name).FirstOrDefault();

            var customers = db.DecisionExpertsTasks.Where(s => s.Region == user1.Region);
            string regionName = user1.Region;
            ViewBag.RegionName = regionName;

           
            ViewBag.Counting2 = db.DecisionEXPDateSearchNew(start, end, Name, regionName).OrderByDescending(s => s.DateRecieved).ToList().Count();

            if (ViewBag.Counting2 == 0)
            {
                ViewBag.Message2 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
            }
            else
            {
                ViewBag.Counting2 = null;
                return View(db.DecisionEXPDateSearchNew(start, end, Name, regionName).OrderByDescending(s => s.DateRecieved).ToList());
            }
            return View(db.DecisionEXPDateSearchNew(start , end, Name, regionName).OrderByDescending(s=> s.DateRecieved).ToList());
        }
       
	}
}