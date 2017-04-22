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
    public class CuentasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cuentas
        [Authorize]
        public ActionResult Index()
        {
            var cuentas = db.Cuentas.Include(c => c.Titular);
            if (!User.IsInRole("admin"))
            {
                string id = User.Identity.GetUserId();
                var usuario = db.Users.Find(id);

                cuentas = cuentas
                    .Where(c => c.ClienteID == usuario.ClienteID);
                return View("index", cuentas.ToList());
            }
           
            return View(cuentas.ToList());
        }

        // GET: Cuentas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuenta cuenta = db.Cuentas.Find(id);
            if (cuenta == null)
            {
                return HttpNotFound();
            }
            return View(cuenta);
        }

        // GET: Cuentas/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create(int? id)
        {
            ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "FullName", id);
            return View();
        }

        // POST: Cuentas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]

        public ActionResult Create([Bind(Include = "CuentaID,Saldo,Estado,ClienteID")] Cuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                cuenta.Numero = db.Cliente.Find(cuenta.ClienteID).NumeroCuenta;
                db.Cuentas.Add(cuenta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "FullName", cuenta.ClienteID);
            return View(cuenta);
        }

        // GET: Cuentas/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuenta cuenta = db.Cuentas.Find(id);
            if (cuenta == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "FullName", cuenta.ClienteID);
            return View(cuenta);
        }

        // POST: Cuentas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CuentaID,Saldo,Estado,ClienteID")] Cuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cuenta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteID = new SelectList(db.Cliente, "ClienteID", "FullName", cuenta.ClienteID);
            return View(cuenta);
        }

        // GET: Cuentas/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuenta cuenta = db.Cuentas.Find(id);
            
            if (cuenta == null)
            {
                return HttpNotFound();
            }
            return View(cuenta);
        }

        // POST: Cuentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cuenta cuenta = db.Cuentas.Find(id);
            db.Cuentas.Remove(cuenta);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public ActionResult Deposito(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cuenta cuenta = db.Cuentas.Find(id);
            if (cuenta == null)
            {
                return HttpNotFound();
            }
            Transaccion transaccion = new Transaccion();
            transaccion.Cuenta = cuenta;
            return View(transaccion);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Deposito([Bind(Include = "Monto,Fecha,Comentario")] Transaccion Transaccion, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuenta cuenta = db.Cuentas.Find(id);
            if (cuenta == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                Transaccion.CuentaID = (int)id;
                Transaccion.TransaccionID = new Random(cuenta.Numero * (DateTime.Now.Day + DateTime.Now.Second / (DateTime.Now.Month))).Next();
                Transaccion.Tipo = TipoTransferencia.Deposito;
                Transaccion.CuentaIDFrom = 0;
                db.Entry(Transaccion).State = EntityState.Added;
                Transaccion.Cuenta.Saldo += Transaccion.Monto;
                db.SaveChanges();
                RedirectToAction("Index");
            }
            Transaccion.Cuenta = cuenta;
            return View(Transaccion);
        }


        [Authorize(Roles = "admin")]
        public ActionResult Retiro(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cuenta cuenta = db.Cuentas.Find(id);
            if (cuenta == null)
            {
                return HttpNotFound();
            }
            Transaccion transaccion = new Transaccion();
            transaccion.Cuenta = cuenta;
            return View(transaccion);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Retiro([Bind(Include = "Monto,Fecha,Comentario")] Transaccion Transaccion, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuenta cuenta = db.Cuentas.Find(id);
            if (cuenta == null)
            {
                return HttpNotFound();
            }
                        
            if (ModelState.IsValid)
            {
                Transaccion.Cuenta = cuenta;
                if (Transaccion.Cuenta.Saldo > (Transaccion.Monto - 100))
                {
                    Transaccion.CuentaID = (int)id;
                    Transaccion.TransaccionID = new Random(cuenta.Numero * (DateTime.Now.Day + DateTime.Now.Second / (DateTime.Now.Month))).Next();
                    Transaccion.Tipo = TipoTransferencia.Retiro;
                    Transaccion.CuentaIDFrom = 0;
                    db.Entry(Transaccion).State = EntityState.Added;
                    Transaccion.Cuenta.Saldo -= Transaccion.Monto;
                    db.SaveChanges();
                    RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Monto", "Su saldo es inferior a la cantidad intenta retirar.");
                }
            }
            Transaccion.Cuenta = cuenta;
            return View(Transaccion);
        }

        public ActionResult NetTransaccion()
        {

            string UserID = User.Identity.GetUserId();
            var Usuario = db.Users.Where(u => u.Id == UserID).FirstOrDefault();
            var Cliente = db.Cliente.Find(Usuario.ClienteID);

            if (Cliente.Cuentas == null)
            {
                
                return HttpNotFound();
            }

            ViewBag.CuentaIDFrom = new SelectList(Cliente.Cuentas, "Numero", "Numero");
            ViewBag.CuentaID = new SelectList(db.Cuentas.Where(c => c.ClienteID != Cliente.ClienteID), "Numero", "Numero");
            Transaccion transaccion = new Transaccion();
            return View(transaccion);
        }

        [HttpPost]
        public ActionResult NetTransaccion([Bind(Include = "CuentaIDFrom,CuentaID,Monto,Fecha,Comentario")] Transaccion Transaccion)
        {
            string UserID = User.Identity.GetUserId();
            var Usuario = db.Users.Where(u => u.Id == UserID).FirstOrDefault();
            var Cliente = db.Cliente.Find(Usuario.ClienteID);

            Cuenta cuenta = db.Cuentas.Where(c => c.Numero == Transaccion.CuentaID).FirstOrDefault();
            Cuenta CuentaFrom = db.Cuentas.Where(c => c.Numero == Transaccion.CuentaIDFrom).FirstOrDefault();
            if (ModelState.IsValid)
            {
                Transaccion.Cuenta = cuenta;
                if (CuentaFrom.Saldo > (Transaccion.Monto - 100))
                {
                    Transaccion.TransaccionID = new Random((cuenta.Numero * db.Transaccion.Count()) / Cliente.Edad).Next();
                    Transaccion.Tipo = TipoTransferencia.NetBanking;
                    Transaccion.CuentaIDFrom = Cliente.ClienteID;
                    db.Entry(Transaccion).State = EntityState.Added;
                    Transaccion.Cuenta.Saldo += Transaccion.Monto;
                    db.SaveChanges();
                    RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Monto", "Su saldo es inferior a la cantidad intenta transferir.");
                }
            }
            Transaccion.Cuenta = cuenta;
            return View(Transaccion);
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
