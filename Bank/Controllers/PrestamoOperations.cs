using Bank.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bank.Controllers
{

    public class PrestamoOperations
    {
        ApplicationDbContext db = new ApplicationDbContext();

        private double ClacularCuota(Prestamo prestamo)
        {
            double tasaInteres = 0, Monto = 0, Plazo = 0, Interes = 0, Cuota = 0;
            tasaInteres = Convert.ToDouble(prestamo.TasaInteres);
            Plazo = prestamo.Plazo;
            Monto = Convert.ToDouble(prestamo.Monto);
            Interes = tasaInteres / 12 / 100;
            Interes = Interes / (1 - (Math.Pow((1 + Interes), Plazo * -1)));
            Cuota = Monto * Interes;
            return Cuota;
        }

        public PagoPrestamo CargarPago(int? id, DateTime fecha)
        {
            PagoPrestamo pago = new PagoPrestamo();
            Prestamo prestamo = db.Prestamo.Find(id);
            Double Cuota = ClacularCuota(prestamo);
            double Mora = CalcularMora(fecha, prestamo.ProximoPago, Convert.ToDouble(prestamo.TasaMora), Cuota);
            pago.Mora = (Mora != 0) ? Convert.ToDecimal(Mora.ToString("#.##")) : 0;
            pago.Cuota = Convert.ToDecimal(Cuota.ToString("#.##"));
            pago.Prestamo = prestamo;
            return pago;
        }

        private double CalcularMora(DateTime fechaActual, DateTime fechaPago, double tasa, double cuota)
        {
            double Mora = 0;
            int dias = (fechaActual - fechaPago).Days;
            if (dias > 0)
            {
                tasa = tasa / 100;
                Mora = ((cuota * tasa) / 30) * dias;
            }
            return Mora;
        }

        public string[] Pagar(PagoPrestamo pago, int? id)
        {

            DateTime fecha = pago.Fecha;
            pago = CargarPago(id, fecha);
            pago.Fecha = fecha;

            Prestamo prestamo = pago.Prestamo;
            
            double Interes = Convert.ToDouble((prestamo.TasaInteres / 12 / 100) * prestamo.Deuda);
            double Amortizado = Convert.ToDouble(pago.Cuota) - Interes;
            double Deuda = Convert.ToDouble(prestamo.Deuda) - Amortizado;
            prestamo.Deuda = Convert.ToDecimal(Deuda.ToString("#.##"));
            prestamo.ProximoPago = prestamo.ProximoPago.AddMonths(1);

            pago.Periodo = db.Pago.Where(p => p.PrestamoID == prestamo.PrestamoID).Count();
            pago.Periodo++;
            if (prestamo.Plazo == db.Pago.Where(p => p.PrestamoID == prestamo.PrestamoID).Count())
            {
                prestamo.Estado = Estado.Pagado;
                prestamo.Deuda = 0;
            }
            db.Pago.Add(pago);
            db.Entry(prestamo).State = EntityState.Modified;
            db.SaveChanges();

            string[] valores = new string[11]
            {
                prestamo.Cliente.Nombre + " " + prestamo.Cliente.Apellido,
                prestamo.Monto.ToString("#.##") + " $",
                prestamo.TasaInteres.ToString() + " %",
                prestamo.Plazo.ToString() + " Meses",
                prestamo.TasaInteres.ToString() + " %",
                "Cuota",
                pago.Fecha.ToShortDateString(),
                pago.Cuota.ToString("#.##") + " $",
                pago.Mora.ToString("#.##") + " $",
                (pago.Mora + pago.Cuota).ToString("#.##") + " $",
                prestamo.ProximoPago.ToShortDateString()
            };
            return valores;
        }

    }
}