using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bank.Models
{
    public enum EstadoCuenta
    {
        Solicitada,
        Activa,
        Inactiva
    }

    public class Cuenta
    {
        public int CuentaID { get; set; }

        public int Numero { get; set; }
        public decimal Saldo { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public EstadoCuenta Estado { get; set; }

        public int ClienteID { get; set; }
        public Cliente Titular { set; get; }
            
    }
}