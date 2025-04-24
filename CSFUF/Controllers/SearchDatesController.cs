using CSFUF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Runtime.InteropServices;
using CSFUF.Extensions;
using Microsoft.Reporting.WebForms;

namespace CSFUF.Controllers
{
    
    public class SearchDatesController : Controller
    {
        //
        // GET: /SearchDates/
        CSFUFDB1 db = new CSFUFDB1();
        [Authorize(Roles=("Registration TeamLeader"))]
        public ActionResult Index()
        {
            string sessionUsername = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            string RegOfUser = user1.Region;

            EmployeeDB dd = new EmployeeDB();
            var customers = from s in db.EmployeeDBs
                            select s;
            customers = customers.Where(s => s.Position.Contains("Registration") && s.Region == RegOfUser);
            ViewBag.Search = new SelectList((from r in customers select r.FirstName).ToList());

           var filteredLists = from s in db.CustomerRegs
                                             select s;
            filteredLists = filteredLists.Where(s => s.Region == user1.Region);
            return View(filteredLists.OrderByDescending(s=> s.DateRecieved).ToList());
        }

         [HttpPost]
        public ActionResult Index(DateTime Start, DateTime End, string Search)
        {
            string sessionUsername = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            string RegOfUser = user1.Region;

            EmployeeDB dd = new EmployeeDB();
            var customers = from s in db.EmployeeDBs
                            select s;
            customers = customers.Where(s => s.Position.Contains("Registration") && s.Region == RegOfUser);
            ViewBag.Search = new SelectList((from r in customers select r.FirstName).ToList());

            var filteredLists = from s in db.CustomerRegs
                                select s;
            filteredLists = filteredLists.Where(s => s.Region == user1.Region);

            if (!String.IsNullOrEmpty(Search))
            {

                ViewBag.listCount =  db.GetDates(Start, End, Search).Count();
                ViewBag.DateStart = Start;
                ViewBag.DateEnd = End;
                return View(db.GetDates(Start, End, Search).OrderByDescending(s => s.DateRecieved));
            }
            else
            {
               
                ViewBag.listCount = db.GetDatesOnly(Start, End, RegOfUser).Count();
                ViewBag.DateStart = Start;
                ViewBag.DateEnd = End;
                return View(db.GetDatesOnly(Start, End, RegOfUser).OrderByDescending(s => s.DateRecieved));

            }

        }

        [Authorize(Roles = ("BranchTeamLeader"))]
        public ActionResult IndexForBT()
        {
            string sessionUsername = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            string RegOfUser = user1.Region;

            EmployeeDB dd = new EmployeeDB();
            var customers = from s in db.EmployeeDBs
                            select s;
            customers = customers.Where(s => s.Position.Contains("Registration") && s.Region == RegOfUser);
            ViewBag.Search = new SelectList((from r in customers select r.FirstName).ToList());

            var filteredLists = from s in db.CustomerRegs
                                select s;
            filteredLists = filteredLists.Where(s => s.Region == user1.Region);
            return View(filteredLists.OrderByDescending(s => s.DateRecieved).ToList());
        }

        [HttpPost]
        public ActionResult IndexForBT(DateTime Start, DateTime End, string Search)
        {
            string sessionUsername = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            string RegOfUser = user1.Region;

            EmployeeDB dd = new EmployeeDB();
            var customers = from s in db.EmployeeDBs
                            select s;
            customers = customers.Where(s => s.Position.Contains("Registration") && s.Region == RegOfUser);
            ViewBag.Search = new SelectList((from r in customers select r.FirstName).ToList());

            var filteredLists = from s in db.CustomerRegs
                                select s;
            filteredLists = filteredLists.Where(s => s.Region == user1.Region);

            if (!String.IsNullOrEmpty(Search))
            {

                ViewBag.listCount = db.GetDates(Start, End, Search).Count();
                ViewBag.DateStart = Start;
                ViewBag.DateEnd = End;
                return View(db.GetDates(Start, End, Search).OrderByDescending(s => s.DateRecieved));
            }
            else
            {

                ViewBag.listCount = db.GetDatesOnly(Start, End, RegOfUser).Count();
                ViewBag.DateStart = Start;
                ViewBag.DateEnd = End;
                return View(db.GetDatesOnly(Start, End, RegOfUser).OrderByDescending(s => s.DateRecieved));

            }

        }
        public ActionResult ExportDownloads( string ReportType)
         {
            string sessionUsername = User.Identity.Name;
            string list2 = User.Identity.GetUserName();
            ViewBag.list2 = list2;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();

            var cusReg = db.CustomerRegs.Where(s => s.Region == user1.Region);

            LocalReport localRep = new LocalReport();
            localRep.ReportPath = Server.MapPath("~/Models/Report/CustomerNew.rdlc");

             ReportDataSource DataSource = new ReportDataSource();
             DataSource.Name = "DataSet2";
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
        public ActionResult ExportToExcell(DateTime? Start, DateTime? End)
         {

             try
             {
                 //var customers = from s in db.CustomerRegs
                 //                select s;
                 //var customers = ((from r in db.CustomerRegs select db.GetDatesOnly(Start, End)).ToList());
                 
                 //customers = customers.Where(s=> s. == Start);/* i was here last time ######################*/
                 Microsoft.Office.Interop.Excel.Application App = new Microsoft.Office.Interop.Excel.Application();
                 Microsoft.Office.Interop.Excel.Workbook WB = App.Workbooks.Add(System.Reflection.Missing.Value);
                 Microsoft.Office.Interop.Excel.Worksheet WS = WB.ActiveSheet;

                
                 WS.Cells[1, 1] = "Full Name";
                 WS.Cells[1, 2] = "Date Recieved";
                 WS.Cells[1, 3] = "Org. Name";
                 WS.Cells[1, 4] = "Archive No";
                 WS.Cells[1, 5] = "Org. TIN";
                 WS.Cells[1, 6] = "Phone No";
                 WS.Cells[1, 7] = "Private ID No";
                 WS.Cells[1, 8] = "Mother's Name";
                 WS.Cells[1, 9] = "Transfered Date";
                 WS.Cells[1, 10] = "Doc Status";
                 int row = 2;

                 foreach (CustomerReg Cus in db.CustomerRegs)
                 {
                     WS.Cells[row, 1] = Cus.FullName;
                     WS.Cells[row, 2] = Cus.DateRecieved;
                     WS.Cells[row, 3] = Cus.OrgName;
                     WS.Cells[row, 4] = Cus.ArchiveNo;
                     WS.Cells[row, 5] = Cus.OrgTIN;
                     WS.Cells[row, 6] = Cus.PhoneNo;
                     WS.Cells[row, 7] = Cus.PrivateIDNo;
                     WS.Cells[row, 8] = Cus.MotherName;
                     WS.Cells[row, 9] = Cus.DateTransfered;
                     WS.Cells[row, 10] = Cus.DocStatus;
                     row++;
                 }
                 WB.SaveAs("Exported Documents");
                 WB.Close();
                 Marshal.FinalReleaseComObject(WB);

                 App.Quit();
                 Marshal.FinalReleaseComObject(App);
                 ViewBag.result = "Exported!";
             }
             catch (Exception ex)
             {

                 ViewBag.result = ex.Message;
             }
             this.AddNotification("Success!!", NotificationType.SUCCESS);
             return RedirectToAction("Index");
         }


       // [Authorize(Roles = ("Registration TeamLeader"))]
         public ActionResult Details(int id)
         {
             using (CSFUFDB1 DbModel = new CSFUFDB1())
             {
                 return View(DbModel.CustomerRegs.Where(x => x.Id == id).FirstOrDefault());
             }

         }

        [Authorize(Roles = ("Registration TeamLeader"))]
         public ActionResult Edit(int id)
         {
             var list2 = new List<string>() { User.Identity.GetUserName() };
             ViewBag.list2 = list2;

             using (CSFUFDB1 DbModel = new CSFUFDB1())
             {

                 return View(DbModel.CustomerRegs.Where(x => x.Id == id).FirstOrDefault());
             }

         }

         // POST: /Customer/Edit/5
         [HttpPost]
         public ActionResult Edit(int id, CustomerReg customer)
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
	}
}