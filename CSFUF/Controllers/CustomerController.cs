using CSFUF.Extensions;
using CSFUF.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;

namespace CSFUF.Controllers
{


    public class CustomerController : Controller
    {
        //
        // GET: /Customer/
        CSFUFDB1 db = new CSFUFDB1();
        // [CSFUF.Controllers.AccountController.Roles("Customers","Admin")]

        public ActionResult ExportToExcell()
        {

            try
            {
                Excel.Application App = new Excel.Application();
                Excel.Workbook WB = App.Workbooks.Add(System.Reflection.Missing.Value);
                Excel.Worksheet WS = WB.ActiveSheet;

                CSFUFDB1 dbs = new CSFUFDB1();
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

                foreach (CustomerReg Cus in dbs.CustomerRegs)
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
            return RedirectToAction("Report");
        }


        [Authorize(Roles = "Registration")]
        public ActionResult Index(string searching, string SearchPhone)
        {
            var customers = from s in db.CustomerRegs
                            select s;

            if (!String.IsNullOrEmpty(searching))
            {
                customers = customers.Where(s => s.MotherName.Contains(searching));
            }
            if (!String.IsNullOrEmpty(SearchPhone))
            {
                customers = customers.Where(s => s.PhoneNo.Contains(SearchPhone));
            }
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(customers.OrderByDescending(s => s.DateRecieved).ToList());

            }

        }

        [Authorize(Roles = "Registration TeamLeader")]
        public ActionResult IndexForTeamLeader(string searching, string SearchPhone)
        {
            var customers = from s in db.CustomerRegs
                            select s;

            if (!String.IsNullOrEmpty(searching))
            {
                customers = customers.Where(s => s.MotherName.Contains(searching));
            }

            if (!String.IsNullOrEmpty(SearchPhone))
            {
                customers = customers.Where(s => s.PhoneNo.Contains(SearchPhone));
            }


            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(customers.OrderByDescending(s => s.DateRecieved).ToList());

            }

        }
        [Authorize(Roles = "BranchTeamLeader")]
        public ActionResult IndexForBTeamLeader(string searching, string SearchPhone)
        {
            var customers = from s in db.CustomerRegs
                            select s;

            if (!String.IsNullOrEmpty(searching))
            {
                customers = customers.Where(s => s.MotherName.Contains(searching));
            }

            if (!String.IsNullOrEmpty(SearchPhone))
            {
                customers = customers.Where(s => s.PhoneNo.Contains(SearchPhone));
            }


            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(customers.OrderByDescending(s => s.DateRecieved).ToList());

            }

        }

        //
        // GET: /Customer/Details/5
        [Authorize(Roles = "Registration")]
        public ActionResult Details(int id, string sortOrder)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.CustomerRegs.Where(x => x.Id == id).FirstOrDefault());
            }

        }
        //[Authorize(Roles = "Registration TeamLeader")]
        public ActionResult DetailsForTD(int id, string sortOrder)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.CustomerRegs.Where(x => x.Id == id).FirstOrDefault());
            }

        }

        //
        // GET: /Customer/Create
        ApplicationUser bd = new ApplicationUser();

        [Authorize(Roles = "Registration")]
        public ActionResult Create()
        {
            string username = User.Identity.Name;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == username).FirstOrDefault();

            if (user1 != null)
            {
                string regionName = user1.Region;
                ViewBag.RegionName = regionName;
            }
            else
            {
                ViewBag.RegionName = "Not Available!!";
                // Handle user not found scenario
            }


            var list2 = new List<string>() { User.Identity.GetUserName() };
            ViewBag.list2 = list2;
            var list = new List<string>() { "Male", "Femal" };
            ViewBag.list = list;

            var list1 = new List<string>() { "Registration -- ምዝገባ" };
            ViewBag.list1 = list1;

            return View();
        }

        // POST: /Customer/Create
        [HttpPost]
        public ActionResult Create(CustomerReg customer)
        {
            string sessionUsername = User.Identity.Name;
            string list2 = User.Identity.GetUserName();
            ViewBag.list2 = list2;

            var list = new List<string>() { "Male", "Femal" };
            ViewBag.list = list;

            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            string regionName = user1.Region;
            ViewBag.RegionName = regionName;
           


            var list1 = new List<string>() { "Registration -- ምዝገባ" };
            ViewBag.list1 = list1;

            try
            {
                Report rep = new Report();
                using (CSFUFDB1 DbModel = new CSFUFDB1())
                {
                    customer.DateRecieved = DateTime.Today;
                    DbModel.CustomerRegs.Add(customer);
                    DbModel.SaveChanges();

                    rep.FullName = customer.FullName;
                    rep.Gender = customer.Gender;
                    rep.OrgName = customer.OrgName;
                    rep.OrgTIN = customer.OrgTIN;
                    rep.PhoneNo = customer.PhoneNo;
                    rep.PrivateIDNo = customer.PrivateIDNo;
                    rep.MotherName = customer.MotherName;
                    rep.DateRegistered = customer.DateRecieved;
                    rep.RegionRegistered = customer.Region;

                    rep.RegisteredExpertName = customer.ExpertName;
                    rep.DocStatusOfReg = customer.DocStatus;

                    DbModel.Reports.Add(rep);
                    DbModel.SaveChanges();

                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

            this.AddNotification("Success!!", NotificationType.SUCCESS);
            return RedirectToAction("Index");

        }

        [Authorize(Roles = "Registration")]
        public ActionResult Edit(int id)
        {
            var list2 = new List<string>() { User.Identity.GetUserName() };
            ViewBag.list2 = list2;

            var list = new List<string>() { "Male", "Femal" };
            ViewBag.list = list;

            var list1 = new List<string>() { "Registration -- ምዝገባ" };
            ViewBag.list1 = list1;

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {

                return View(DbModel.CustomerRegs.Where(x => x.Id == id).FirstOrDefault());
            }

        }

        // POST
        [HttpPost]
        public ActionResult Edit(int id, CustomerReg customer)
        {
            var list2 = new List<string>() { User.Identity.GetUserName() };
            ViewBag.list2 = list2;

            var list = new List<string>() { "Male", "Femal" };
            ViewBag.list = list;

            var list1 = new List<string>() { "Registration -- ምዝገባ" };
            ViewBag.list1 = list1;

            var repos = from s in db.Reports
                        select s;

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                DbModel.Entry(customer).State = EntityState.Modified;
                DbModel.SaveChanges();

                CustomerReg cus = DbModel.CustomerRegs.Where(x => x.Id == id).FirstOrDefault();
                Report rep = repos.Where(x => x.PrivateIDNo == cus.PrivateIDNo).FirstOrDefault();

                rep.FullName = cus.FullName;
                rep.Gender = cus.Gender;
                rep.OrgName = cus.OrgName;
                rep.OrgTIN = cus.OrgTIN;
                rep.PhoneNo = cus.PhoneNo;
                rep.PrivateIDNo = cus.PrivateIDNo;
                rep.MotherName = cus.MotherName;
                rep.DateRegistered = cus.DateRecieved;
                rep.RegisteredExpertName = cus.ExpertName;
                rep.DocStatusOfReg = cus.DocStatus;

                DbModel.Entry(rep).State = EntityState.Modified;
                DbModel.SaveChanges();

            }
            this.AddNotification("Success!!", NotificationType.SUCCESS);
            return RedirectToAction("Index");
        }

        //
        // GET: /Customer/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.CustomerRegs.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        //
        // POST: /Customer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {

            try
            {
                using (CSFUFDB1 DbModel = new CSFUFDB1())
                {

                    CustomerReg Emp = DbModel.CustomerRegs.Where(x => x.Id == id).FirstOrDefault();
                    //ConReg con = new ConReg();
                    DbModel.CustomerRegs.Remove(Emp);
                    DbModel.SaveChanges();

                }
                // TODO: Add delete logic here
                this.AddNotification("Success!!", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Registration")]
        public ActionResult SendTo(int id)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.CustomerRegs.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        //
        // POST: /Customer/Delete/5
        [HttpPost]
        public ActionResult SendTo(int id, FormCollection collection, CustomerReg customer)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                var repos = from s in db.Reports
                            select s;

                CustomerReg Emp = DbModel.CustomerRegs.Where(x => x.Id == id).FirstOrDefault();
                ConReg con = new ConReg();
                Report rep = repos.Where(x => x.PrivateIDNo == Emp.PrivateIDNo).FirstOrDefault();
                Emp.DateTransfered = DateTime.Today;

                con.FullName = Emp.FullName;
                con.ArchiveNo = Emp.ArchiveNo;
                con.DateRecieved = Emp.DateTransfered;

                rep.DateTransferedToCon = Emp.DateTransfered;

                con.BDate = Emp.BDate;
                con.DocCount = Emp.DocCount;
                con.DueDate = Emp.DueDate;
                con.Gender = Emp.Gender;
                con.GovIDNo = Emp.GovIDNo;
                con.MotherName = Emp.MotherName;
                con.Id = Emp.Id;
                con.OrgName = Emp.OrgName;
                con.OrgTIN = Emp.OrgTIN;
                con.PhoneNo = Emp.PhoneNo;
                con.PrivateIDNo = Emp.PrivateIDNo;
                Emp.DocStatus = Emp.DocStatus;
                con.DocStatus = Emp.DocStatus;
                con.Region = Emp.Region;
                con.AssignedExpert = "አልተመደበም";

                // DbModel.Entry(customer).State = EntityState.Modified;
                DbModel.Entry(rep).State = EntityState.Modified;
                DbModel.ConRegs.Add(con);
                DbModel.SaveChanges();

            }
            this.AddNotification("Success!!", NotificationType.SUCCESS);
            return RedirectToAction("Index");
        }
        
        [Authorize(Roles = "Registration TeamLeader")]
        public ActionResult Report(string SearchID, string SearchPhone, string sortOrder, string currentFilter, string searchString, int? page)
        {
            string sessionUsername = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();


            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var customers = from s in db.CustomerRegs
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

            var list = new List<string>() { "SearchID", "SearchPhone" };
            ViewBag.list = list;

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(SearchID))
            {
                customers = customers.Where(s => s.PrivateIDNo.Contains(SearchID) && s.Region.Contains(user1.Region));
                ViewBag.Counting1 = customers.ToPagedList(pageNumber, pageSize).Count();

            }
            if (!String.IsNullOrEmpty(SearchPhone))
            {
                //customers = customers.Where(s => s.PhoneNo.Contains(SearchPhone));
                customers = customers.Where(s => s.PhoneNo.Contains(SearchPhone) && s.Region.Contains(user1.Region));

                ViewBag.Counting2 = customers.ToPagedList(pageNumber, pageSize).Count();
            }


            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                if (ViewBag.Counting1 == 0)
                {
                    ViewBag.Message1 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                }
                if (ViewBag.Counting2 == 0)
                {
                    ViewBag.Message2 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                }
                return View(customers.ToPagedList(pageNumber, pageSize));
            }
        }

        [Authorize(Roles = "BranchTeamLeader")]
        public ActionResult BTReport(string SearchID, string SearchPhone, string sortOrder, string currentFilter, string searchString, int? page)
        {
            string sessionUsername = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();


            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var customers = from s in db.CustomerRegs
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

            var list = new List<string>() { "SearchID", "SearchPhone" };
            ViewBag.list = list;

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(SearchID))
            {
                customers = customers.Where(s => s.PrivateIDNo.Contains(SearchID) && s.Region.Contains(user1.Region));
                ViewBag.Counting1 = customers.ToPagedList(pageNumber, pageSize).Count();

            }
            if (!String.IsNullOrEmpty(SearchPhone))
            {
                //customers = customers.Where(s => s.PhoneNo.Contains(SearchPhone));
                customers = customers.Where(s => s.PhoneNo.Contains(SearchPhone) && s.Region.Contains(user1.Region));

                ViewBag.Counting2 = customers.ToPagedList(pageNumber, pageSize).Count();
            }


            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                if (ViewBag.Counting1 == 0)
                {
                    ViewBag.Message1 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                }
                if (ViewBag.Counting2 == 0)
                {
                    ViewBag.Message2 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                }
                return View(customers.ToPagedList(pageNumber, pageSize));
            }
        }
        [Authorize(Roles = "Registration")]
        public ActionResult ReportForExp(string sessionUsername, string SearchID, string SearchPhone, string sortOrder, string currentFilter, string searchString, int? page)
        {
            sessionUsername = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();
            string RegOfUser = user1.Region;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var customers = from s in db.CustomerRegs
                            select s;

            customers = customers.Where(s => s.ExpertName.Contains(User.Identity.Name) && s.Region == RegOfUser);

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

            var list = new List<string>() { "SearchID", "SearchPhone" };
            ViewBag.list = list;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (!String.IsNullOrEmpty(SearchID))
            {

                customers = customers.Where(s => s.PrivateIDNo.Contains(SearchID));
                ViewBag.Counting1 = customers.ToPagedList(pageNumber, pageSize).Count();
            }
            if (!String.IsNullOrEmpty(SearchPhone))
            {
                customers = customers.Where(s => s.PhoneNo.Contains(SearchPhone));
                ViewBag.Counting2 = customers.ToPagedList(pageNumber, pageSize).Count();
            }

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                if (ViewBag.Counting1 == 0)
                {
                    ViewBag.Message1 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                }
                if (ViewBag.Counting2 == 0)
                {
                    ViewBag.Message2 = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                }
                return View(customers.ToPagedList(pageNumber, pageSize));
            }
        }

        [Authorize(Roles = "Registration")]
        public ActionResult SearchByMother(string search)
        {
            string sessionUsername = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();

            var customers = from s in db.CustomerRegs
                            select s;
            //customers = customers.Where(s => s.ExpertName.Contains(User.Identity.Name));
            customers = customers.Where(s => s.ExpertName == sessionUsername && s.Region == user1.Region);


            ViewBag.Counting1 = customers.Where(s => s.MotherName == search).ToList().Count();
            if (!String.IsNullOrEmpty(search))
            {
                customers = customers.Where(s => s.MotherName == search);
                ViewBag.Counting1 = customers.Where(s => s.MotherName == search).ToList().Count();
                if (ViewBag.Counting1 == 0)
                {
                    ViewBag.Message = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                }
            }
            return View(customers.OrderByDescending(s => s.DateRecieved).ToList());
        }
        [Authorize(Roles = "Registration")]
        public ActionResult SearchByPhone(string SearchByPh)
        {
            string sessionUsername = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();


            var customers = from s in db.CustomerRegs
                            select s;
            //customers = customers.Where(s => s.ExpertName.Contains(User.Identity.Name));
            customers = customers.Where(s => s.ExpertName.Contains(User.Identity.Name) && s.Region.Contains(user1.Region));

            if (!String.IsNullOrEmpty(SearchByPh))
            {
                customers = customers.Where(s => s.PhoneNo.Contains(SearchByPh));
                ViewBag.Counting2 = customers.ToList().Count();
                if (ViewBag.Counting2 == 0)
                {
                    ViewBag.Message = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                }
            }
            return View(customers.OrderByDescending(s => s.DateRecieved).ToList());

        }
        [Authorize(Roles = "Registration")]
        public ActionResult SearchById(string SearchId)
        {
            string sessionUsername = User.Identity.Name;
            Entities2 users = new Entities2();
            AspNetUser user1 = users.AspNetUsers.Where(x => x.UserName == sessionUsername).FirstOrDefault();


            var customers = from s in db.CustomerRegs
                            select s;
            //customers = customers.Where(s => s.ExpertName.Contains(User.Identity.Name));
            customers = customers.Where(s => s.ExpertName == sessionUsername && s.Region == user1.Region);


            if (!String.IsNullOrEmpty(SearchId))
            {
                customers = customers.Where(s => s.PrivateIDNo.Contains(SearchId));
                ViewBag.Counting3 = customers.Where(s => s.PrivateIDNo.Contains(SearchId)).Count();
                if (ViewBag.Counting3 == 0)
                {
                    ViewBag.Message = "ፍለጋዎ የለም! እባክዎ እንደገና ይሞክሩ!";
                }


            }
            return View(customers.OrderByDescending(s => s.DateRecieved).ToList());
        }

        public ActionResult NewView(int page = 1, string sort = "FullName", string sortDir = "", string search = "")
        {
            int pagesize = 10;
            int totalRecords = 0;
            if (page < 1) page = 1;
            int skip = (page * pagesize) - pagesize;
            var Data = SortedCustomers(search, sort, sortDir, skip, pagesize, out totalRecords);
            ViewBag.TotalRow = totalRecords;
            return View(Data);

        }
        public List<CustomerReg> SortedCustomers(string search, string sort, string sortDir, int skip, int pagesize, out int totalRecords)
        {
            using (CSFUFDB1 Db2 = new CSFUFDB1())
            {

                var v = (from a in Db2.CustomerRegs
                         where
                         a.MotherName.Contains(search) ||
                         a.FullName.Contains(search)
                         select a);
                totalRecords = v.Count();
                v = v.OrderBy(sort + " " + sortDir);
                if (pagesize > 0)
                {
                    v = v.Skip(skip).Take(pagesize);
                }
                return v.ToList();

            }
        }
    }
}
