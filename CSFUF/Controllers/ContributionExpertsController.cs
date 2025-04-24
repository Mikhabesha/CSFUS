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
    public class ContributionExpertsController : Controller
    {
        //
        // GET: /ContributionExperts/
        CSFUFDB1 db = new CSFUFDB1();

        [Authorize( Roles=("Contribution Expert"))]
        public ActionResult Index(string searching)
        {
           
            string list2 = User.Identity.GetUserName();
            ViewBag.list2 = list2;
            string sessionUsername = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            string regionName = user1.Region;

            var customers = from s in db.ConExpTasks
                            select s;
     
          /* This Code is for Column Update 
           * 
           * UPDATE ConExpTasks
            SET Region = 'Head Office'
           */

           customers = customers.Where(s => s.AssignedExpert == sessionUsername && s.Region == user1.Region);
            
            
            if (!String.IsNullOrEmpty(searching))
            {
                ViewBag.Counting = customers.Where(s => s.PrivateIDNo.Contains(searching)).ToList().Count();
                
                if (ViewBag.Counting == 0)
                {
                    ViewBag.Message = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                    return View(customers.Where(s => s.PrivateIDNo.Contains(searching)).ToList());
                }
                return View(customers.Where(s => s.PrivateIDNo.Contains(searching)).ToList());
            }
            

                return View(customers.OrderByDescending(s => s.DateRecieved).ToList());
         
        }
        [HttpPost]
        public ActionResult Index(DateTime? Start, DateTime End, String Name, string ID)
        {
            Name = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == Name).FirstOrDefault();
            string regionName = user1.Region;

            ViewBag.Counting2 = db.ConSearchWithNameAndDate(Start, End, Name, regionName).OrderByDescending(s => s.DateRecieved).ToList().Count();

            if (ViewBag.Counting2 == 0)
            {
                ViewBag.Counting2 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
            }
            else
            {
                ViewBag.Counting2 = null;
                return View(db.ConSearchWithNameAndDate(Start, End, Name, regionName).OrderByDescending(s => s.DateRecieved).ToList());
            }
            return View(db.ConSearchWithNameAndDate(Start, End, Name, regionName).OrderByDescending(s => s.DateRecieved).ToList());

        }

        [Authorize(Roles = ("Contribution Expert"))]
        public ActionResult SendTo(int id)
        {
            CSFUFDB1 dbd = new CSFUFDB1();
            var Lists = new List<string>((from r in dbd.ContributionExperts select r.ConExpName).ToList());
            ViewBag.ListNames = Lists;

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.ConExpTasks.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        //
        // POST: /Customer/Delete/5
        [HttpPost]
        public ActionResult SendTo(int id, FormCollection collection, ConExpTask customer)
        {
           
                using (CSFUFDB1 DbModel = new CSFUFDB1())
                {
                   // Report Purpose....
                    var repos = from s in db.Reports
                                select s;
                    /*..report purpose ends here and starts..
                     * ..assigning value at the end of this ActionResult*/


                    ConExpTask con = DbModel.ConExpTasks.Where(x => x.Id == id).FirstOrDefault();
                    Report rep = repos.Where(s => s.PrivateIDNo == con.PrivateIDNo).FirstOrDefault();

                    Decision dic = new Decision();
                    con.DateTransfered = DateTime.Today;

                    dic.FullName = con.FullName;
                    //con.Allowance = Emp.Allowance;
                    dic.ArchiveNo = con.ArchiveNo;
                    //con.DateRecieved = Emp.DateRecieved;
                    dic.DateRecieved = con.DateTransfered;
                    dic.BDate = con.BDate;
                    dic.DocCount = con.DocCount;
                    dic.DocStatus = con.DocStatus;
                    dic.DueDate = con.DueDate;
                    dic.Gender = con.Gender;
                    dic.GovIDNo = con.GovIDNo;
                    dic.MotherName = con.MotherName;
                    dic.Id = con.Id;
                    dic.OrgName = con.OrgName;
                    dic.OrgTIN = con.OrgTIN;
                    dic.PhoneNo = con.PhoneNo;
                    dic.PrivateIDNo = con.PrivateIDNo;
                    dic.AssignedExpertNames = "አልተመደበም";
                    dic.Region = con.Region;

                    rep.DateTransferedToDecFromCon = dic.DateRecieved;

                    DbModel.Decisions.Add(dic);
                    DbModel.Entry(rep).State = EntityState.Modified;
                    DbModel.SaveChanges();

                }
                // TODO: Add Send logic here
                this.AddNotification("Success!!", NotificationType.SUCCESS);
                return RedirectToAction("Index");
        }
        //
        // GET: /Customer/Details/5

        [Authorize(Roles = ("Contribution Expert"))]
        public ActionResult Details(int id)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.ConExpTasks.Where(x => x.Id == id).FirstOrDefault());
            }

        }
        public ActionResult Edit(int id)
        {
            var list1 = new List<string>() {"Completed", "Pending" };
            ViewBag.list1 = list1;
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.ConExpTasks.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        //
        // POST: /Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ConExpTask customer)
        {
            var repos = from s in db.Reports
                        select s;

            /* Report means the FOLLOW UP REPORT,...
             ..the one menu which is available for all users. */
            Report rep = repos.Where(x => x.PrivateIDNo == customer.PrivateIDNo).FirstOrDefault();
           
            using (CSFUFDB1 DbModel = new CSFUFDB1())
                {
                    if (customer.DocStatus == "Pending")
                    {
                        customer.DateTransfered = null;
                        rep.DateTransferedToDecFromCon = customer.DateTransfered;
                    }
                    else
                    {
                        //customer.DateTransfered = DateTime.Today;
                        //rep.DateTransferedToDecFromCon = customer.DateTransfered;
                        customer.Remark = "";
                        
                    }
                    rep.DocStatusOfCon = customer.DocStatus;

                    DbModel.Entry(customer).State = EntityState.Modified;
                    DbModel.Entry(rep).State = EntityState.Modified;
                    DbModel.SaveChanges();
                }
                // TODO: Add update logic here

                return RedirectToAction("Index");
        }
	}
}