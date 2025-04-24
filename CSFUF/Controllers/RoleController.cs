using CSFUF.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSFUF.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        ApplicationDbContext Context;
        public RoleController()
        {
            Context = new ApplicationDbContext();
        }
        //
        // GET: /Role/

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var Roles = Context.Roles.ToList();
            return View(Roles);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var Roles = new IdentityRole();
            return View(Roles);
        }
        [HttpPost]
        public ActionResult Create(IdentityRole Role)
        {

              Context.Roles.Add(Role);
              Context.SaveChanges();
              return RedirectToAction("Index");
                
        }


	}
}