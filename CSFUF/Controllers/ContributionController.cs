using CSFUF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSFUF.Extensions;
using Microsoft.Reporting.WebForms;
using Microsoft.AspNet.Identity;
using Report = CSFUF.Models.Report;

namespace CSFUF.Controllers
{
    public class ContributionController : Controller
    {
        //
        // GET: /Contribution/
        CSFUFDB1 db = new CSFUFDB1();
        [Authorize(Roles=("Contribution"))]
        public ActionResult Index(string searching)
        {
            string sessionUsername = User.Identity.Name;
            string list2 = User.Identity.GetUserName();
            ViewBag.list2 = list2;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();

            var conReg = from s in db.ConRegs
                         select s;
           
            conReg = conReg.Where(s => s.Region == user1.Region);
       
            string regionName = user1.Region;
            ViewBag.RegionName = regionName;

            if (!String.IsNullOrEmpty(searching))
            {
                ViewBag.Counting = conReg.Where(s => s.PrivateIDNo.Contains(searching)).Count();
                if (ViewBag.Counting == 0)
                {
                    ViewBag.Message = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                }
                return View(conReg.Where(s => s.PrivateIDNo.Contains(searching)));
            }
            else
            {
                return View(conReg.OrderByDescending(s => s.DateRecieved).ToList());
            }

        }

        [HttpPost]
        public ActionResult Index(DateTime Start, DateTime End)
        {
            string sessionUsername = User.Identity.Name;
            string list2 = User.Identity.GetUserName();
            ViewBag.list2 = list2;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();

            var conReg = db.ConRegs.Where(s => s.Region == user1.Region);
            string regionName = user1.Region;
            ViewBag.RegionName = regionName;

            ViewBag.Counting2 = db.ConSearchWithDates(Start, End, regionName).Count();
            if (ViewBag.Counting2 == 0)
            {
                ViewBag.Message2 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
            }
            return View(db.ConSearchWithDates(Start, End, regionName));
        }

        [Authorize(Roles = ("BranchTeamLeader"))]
        public ActionResult IndexForBT(string searching)
        {
            string sessionUsername = User.Identity.Name;
            string list2 = User.Identity.GetUserName();
            ViewBag.list2 = list2;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();

            var conReg = from s in db.ConRegs
                         select s;

            conReg = conReg.Where(s => s.Region == user1.Region);

            string regionName = user1.Region;
            ViewBag.RegionName = regionName;

            if (!String.IsNullOrEmpty(searching))
            {
                ViewBag.Counting = conReg.Where(s => s.PrivateIDNo.Contains(searching)).Count();
                if (ViewBag.Counting == 0)
                {
                    ViewBag.Message = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                }
                return View(conReg.Where(s => s.PrivateIDNo.Contains(searching)));
            }
            else
            {
                return View(conReg.OrderByDescending(s => s.DateRecieved).ToList());
            }

        }

        [HttpPost]
        public ActionResult IndexForBT(DateTime Start, DateTime End)
        {
            string sessionUsername = User.Identity.Name;
            string list2 = User.Identity.GetUserName();
            ViewBag.list2 = list2;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();

            var conReg = db.ConRegs.Where(s => s.Region == user1.Region);
            string regionName = user1.Region;
            ViewBag.RegionName = regionName;

            ViewBag.Counting2 = db.ConSearchWithDates(Start, End, regionName).Count();
            if (ViewBag.Counting2 == 0)
            {
                ViewBag.Message2 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
            }
            return View(db.ConSearchWithDates(Start, End, regionName));
        }


        [Authorize(Roles = ("Contribution"))]
        public ActionResult Edit(int id)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.ConRegs.Where(x => x.Id == id).FirstOrDefault());
            }
        }
        public ActionResult ExportDownloads(string ReportType)
        {
            string sessionUsername = User.Identity.Name;
            string list2 = User.Identity.GetUserName();
            ViewBag.list2 = list2;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();

            var cusReg = db.ConRegs.Where(s => s.Region == user1.Region);

            LocalReport localRep = new LocalReport();
            localRep.ReportPath = Server.MapPath("~/Models/Report/ConReport.rdlc");

            ReportDataSource DataSource = new ReportDataSource();
            DataSource.Name = "ConDataSet";
            DataSource.Value = cusReg.ToList();
            localRep.DataSources.Add(DataSource);
            string reportType = ReportType;
            string mimeType;
            string encoding;
            string fileExtension;

            if (reportType == "Excel")
            {
                fileExtension = "xlsx";
            }
            else
            {
                fileExtension = "pdf";
            }
            string[] strems;
            Warning[] warnings;
            byte[] RenderByte;
            RenderByte = localRep.Render(ReportType, "", out mimeType, out encoding, out fileExtension
                , out strems, out warnings);
            Response.AddHeader("content-disposition", "attachment;filename= Customer_Report." + fileExtension);
            return File(RenderByte, fileExtension);


            // return View();
        }
     

        [HttpPost]
        public ActionResult Edit(int id, ConReg customer)
        {
            try
            {
                using (CSFUFDB1 DbModel = new CSFUFDB1())
                {
                    DbModel.Entry(customer).State = EntityState.Modified;
                    DbModel.SaveChanges();
                }
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //[Authorize(Roles = ("Contribution"))]
        public ActionResult SendTo(int id)
        {
            CSFUFDB1 dbd = new CSFUFDB1();
            var Lists = new List<string>((from r in dbd.ContributionExperts select r.ConExpName).ToList());
            ViewBag.ListNames = Lists;

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.ConRegs.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult SendTo(int id, FormCollection collection, ConReg customer)
        {

            string sessionUsername = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();

            
            var repos = from s in db.Reports
                        select s;
            try
            {
                using (CSFUFDB1 DbModel = new CSFUFDB1())
                {
                    ConReg con = DbModel.ConRegs.Where(x => x.Id == id).FirstOrDefault();
                    Report repp = repos.Where(x => x.PrivateIDNo == con.PrivateIDNo).FirstOrDefault();
                    ConExpTask dic = new ConExpTask();
                    con.DateTransfered = DateTime.Today;

                    repp.TaskRecievedDateCon = con.DateTransfered;
                    dic.FullName = con.FullName;
                    dic.ArchiveNo = con.ArchiveNo;
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
                    dic.AssignedExpert = con.AssignedExpert;
                    dic.Region = con.Region;
                   
                    DbModel.Entry(repp).State = EntityState.Modified;
                    DbModel.ConExpTasks.Add(dic);
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

        //[Authorize(Roles = ("Contribution"))]
        public ActionResult Details(int id)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.ConRegs.Where(x => x.Id == id).FirstOrDefault());
            }

        }

        // This Delete ActionResult Purpose is for sending back files to Registration from Contribution 
        public ActionResult Delete(int id)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.ConRegs.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var custs = from s in db.CustomerRegs
                        select s;
            try
            {
                using (CSFUFDB1 DbModel = new CSFUFDB1())
                {

                    ConReg con = DbModel.ConRegs.Where(x => x.Id == id).FirstOrDefault();
                    CustomerReg cus = custs.Where(x => x.PrivateIDNo == con.PrivateIDNo).FirstOrDefault();
                   
                    cus.DateTransfered = null;

                    DbModel.Entry(cus).State = EntityState.Modified;
                    DbModel.SaveChanges();

                    DbModel.ConRegs.Remove(con);
                    DbModel.SaveChanges();

                    this.AddNotification("Success!!", NotificationType.SUCCESS);
                    return RedirectToAction("Index");
                }
                // TODO: Add delete logic here

                
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AssignExp(int id)
        {
            string sessionUsername = User.Identity.Name;
            string list2 = User.Identity.GetUserName();
            ViewBag.list2 = list2;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();

            string regionName = user1.Region;
 
            CSFUFDB1 dbd = new CSFUFDB1();
            var Lists = new List<string>((from r in dbd.ContributionExperts where r.Region == user1.Region select r.ConExpName).ToList());
            ViewBag.ListNames = Lists;
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {

                return View(DbModel.ConRegs.Where(x => x.Id == id).FirstOrDefault());
            }

        }

        [HttpPost]
        public ActionResult AssignExp(int id, ConReg customer)
        {
            var repos = from s in db.Reports
                        select s;

            CSFUF.Models.Report rep = repos.Where(x => x.PrivateIDNo == customer.PrivateIDNo).FirstOrDefault();

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                //rep.DateTransferedToCon = customer.DateRecieved;
                rep.TaskRecievedDateCon = customer.DateTransfered;
               // rep.DocStatusOfCon = customer.DocStatus;
                rep.AssignedConExpert = customer.AssignedExpert;
                rep.DocStatusOfCon = "ለባለሞያ ተልኳል";

                DbModel.Entry(customer).State = EntityState.Modified;
                DbModel.Entry(rep).State = EntityState.Modified;
                DbModel.SaveChanges();
            }
            // TODO: Add update logic here    
            CSFUFDB1 DbModel2 = new CSFUFDB1();
            return RedirectToAction("SendTo" + "/" + id);
        }

        [Authorize(Roles = ("Admin"))]
        public ActionResult CreateExpert()
        {
            CSFUFDB1 dbd = new CSFUFDB1();
            var Lists = new List<string>((from r in dbd.ContributionExperts select r.ConExpName).ToList());
            ViewBag.ListNames = Lists;
            return View();
        }

        [HttpPost]
        public ActionResult CreateExpert(ContributionExpert customer)
        {

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                DbModel.ContributionExperts.Add(customer);
                DbModel.SaveChanges();

            }
            // TODO: Add insert logic here

            return RedirectToAction("Index");
        }
	}
}