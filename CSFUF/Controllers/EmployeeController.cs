using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSFUF.Models;
using System.Data.Entity;
using PagedList;

namespace CSFUF.Controllers
{
     [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/
         
        CSFUFDB1 db = new CSFUFDB1();
        public ActionResult Index( string searching)
        {
            var employees = from s in db.EmployeeDBs
                            select s;
            if (!String.IsNullOrEmpty(searching))
            {
                employees = employees.Where(s => s.FirstName.Contains(searching));
               
            }
           
            using (CSFUFDB1 DbEmp = new CSFUFDB1() )
            {
                return View(employees.ToList());
               
            }
          
        }

        //
        // GET: /Employee/Details/5

         [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.EmployeeDBs.Where(x => x.Id == id).FirstOrDefault());
            }
        }
        private ApplicationDbContext Context;
        //
        // GET: /Employee/Create

         [Authorize(Roles = "Admin")]
        public ActionResult Create(int id = 0)
        {
            Context = new ApplicationDbContext();
            ViewBag.Name = new List<string>((from r in Context.Roles select r.Name).ToList());
           
            return View();
        }
       

        //
        // POST: /Employee/Create
        [HttpPost]
        public ActionResult Create(EmployeeDB DbEmpp)
        {
            try
            {
                using (CSFUFDB1 DbEmp = new CSFUFDB1())
                {
                    DbEmp.EmployeeDBs.Add(DbEmpp);
                    DbEmp.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Employee/Edit/5

         [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.EmployeeDBs.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        //
        // POST: /Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, EmployeeDB Emp)
        {
            try
            {
                using (CSFUFDB1 DbModel = new CSFUFDB1())
                {
                    DbModel.Entry(Emp).State = EntityState.Modified;
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

        //
        // GET: /Employee/Delete/5

         [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                return View(DbModel.EmployeeDBs.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        //
        // POST: /Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (CSFUFDB1 DbModel = new CSFUFDB1())
                {
                   
                    EmployeeDB Emp = DbModel.EmployeeDBs.Where(x => x.Id == id).FirstOrDefault();
                    DbModel.EmployeeDBs.Remove(Emp);
                    DbModel.SaveChanges();
                    
                }
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
