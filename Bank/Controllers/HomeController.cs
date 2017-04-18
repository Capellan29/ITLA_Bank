using Bank.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bank.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Detalles()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string id = User.Identity.GetUserId();
            var usuario = db.Users.Find(id);


            Cliente cliente = db.Cliente.Find(usuario.ClienteID);
            
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }
    }
}