using CSFUF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSFUF.Controllers
{
    public class PensionTypesController : Controller
    {
        //
        // GET: /PensionTypes/

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            CSFUFDB1 DbModel = new CSFUFDB1();

            return View(DbModel.PensionTypes.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreatePension()
        {
            return View();
        }

        //
        // POST: /Customer/Create
        [HttpPost]
        public ActionResult CreatePension(PensionType Pension)
        {

            using (CSFUFDB1 DbModel = new CSFUFDB1())
            {
                DbModel.PensionTypes.Add(Pension);
                DbModel.SaveChanges();

            }
            // TODO: Add insert logic here

            return RedirectToAction("Index");
        }
	}
}