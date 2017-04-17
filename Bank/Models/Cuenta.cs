using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    public enum TipoCuenta
    {
        Corriente,
        Ahorro
    }

    public class Cuenta
    {
        public int CuentaID { get; set; }
 
        [Display(Name = "Numero de Cuenta")]
        public int Numero { get; set; }
        [Required]
        [DataType(DataType.Currency), Column(TypeName = "money")]
        public decimal Saldo { get; set; }
        public EstadoCuenta Estado { get; set; }

        public int ClienteID { get; set; }
        public virtual Cliente Titular { set; get; }
            
    }
}