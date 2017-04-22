using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bank.Models;
using Microsoft.AspNet.Identity;

namespace Bank.Controllers
{

    [Authorize]
    public class PrestamosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Prestamos
        public ActionResult Index(Estado? status)
        {
            var prestamo = db.Prestamo
                .Include(p => p.Cliente)
                .Include(p => p.Garante);
            if (status != null)
            {
                prestamo = prestamo
                    .Where(p => p.Estado == status);
            }
            if (!User.IsInRole("admin"))
            {
                string id = User.Identity.GetUserId();
                var usuario = db.Users.Find(id);

                prestamo = prestamo
                    .Where(p => p.ClienteID == usuario.ClienteID);
            }

            return View(prestamo.ToList());
        }

        // GET: Prestamos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prestamo prestamo = db.Prestamo
                .Include(p => p.Garante)
                .Where(p => p.PrestamoID == (int)id).Single();
            if (prestamo == null)
            {
                return HttpNotFound();
            }
            return View(prestamo);
        }

        // GET: Prestamos/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create(int? id)
        {
            ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "FullName",id);
            return View();
        }

        // POST: Prestamos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PrestamoID,Plazo,Monto,TasaInteres,TasaMora,Deuda,ProximoPago,Estado,GaranteID,ClienteID")] Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                prestamo.ProximoPago = DateTime.Now;
                db.Prestamo.Add(prestamo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "Nombre", prestamo.ClienteID);
            return View(prestamo);
        }

        // GET: Prestamos/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prestamo prestamo = db.Prestamo.Find(id);
            if (prestamo == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "Nombre", prestamo.ClienteID);
            return View(prestamo);
        }

        // POST: Prestamos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "PrestamoID,Plazo,Monto,TasaInteres,TasaMora,Deuda,ProximoPago,Estado,GaranteID,ClienteID")] Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prestamo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "Nombre", prestamo.ClienteID);
            return View(prestamo);
        }

        // GET: Prestamos/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prestamo prestamo = db.Prestamo.Find(id);
            if (prestamo == null)
            {
                return HttpNotFound();
            }
            return View(prestamo);
        }

        // POST: Prestamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Prestamo prestamo = db.Prestamo.Find(id);
            db.Prestamo.Remove(prestamo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Aprobar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prestamo prestamo = db.Prestamo.Find(id);
            if (prestamo == null)
            {
                return HttpNotFound();
            }
            return View(prestamo);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Otorgar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prestamo prestamo = db.Prestamo.Find(id);
            if (prestamo == null)
            {
                return HttpNotFound();
            }

            if (prestamo.Estado == Estado.Solicitado)
            {
                prestamo.Estado = Estado.Aprovado;
                prestamo.Deuda = prestamo.Monto;
                prestamo.ProximoPago = DateTime.Now.AddMonths(1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Pagar(int? id)
        {
            PrestamoOperations po = new PrestamoOperations();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PagoPrestamo pago = po.CargarPago(id, DateTime.Now);
            
            return View(pago);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Pagar([Bind(Include = "Fecha")] PagoPrestamo pago, int id)
        {
            PrestamoOperations po = new PrestamoOperations();
            ViewBag.Valores = po.Pagar(pago,id);
            return View("Ticket");
        }

        public ActionResult Amortizacion()
        {
            return View();
        }

    }
}