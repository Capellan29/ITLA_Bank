using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bank.Models;
using PagedList;


namespace Bank.Controllers
{
    
    public class ClienteController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Cliente
        public ActionResult Index(string currentFilter, string searchString, int? page, string mensaje)
        {
            ViewBag.Mensaje = mensaje;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var clientes = db.Cliente.Include(c => c.Prestamo);
            if (!String.IsNullOrEmpty(searchString))
            {
                clientes = clientes.Where(s => s.Nombre.ToUpper().Contains(searchString.ToUpper())
                                       || s.Apellido.ToUpper().Contains(searchString.ToUpper()));
            }
            clientes = clientes.OrderByDescending(c => c.Nombre);
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            
            return View(clientes.ToPagedList(pageNumber, pageSize));
        }

        // GET: Cliente/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            Cliente cliente = db.Cliente
                .Include(c => c.Prestamo)
                .Where(c => c.ClienteID == id)
                .Single();

            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClienteID,Nombre,Apellido,Cedula,Direccion,Edad,EstadoCivil,Correo,Telefono,Celular")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                int subedula = Convert.ToInt32(cliente.Cedula.Substring(4,4));
                int AccountNumber = new Random(subedula).Next();
                cliente.NumeroCuenta = AccountNumber;
                db.Cliente.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClienteID,Nombre,Apellido,Cedula,Direccion,Edad,EstadoCivil,Correo,Telefono,Celular,Sexo")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Cliente.Find(id);
            db.Cliente.Remove(cliente);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
