using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bank.Models
{
    public enum Estado
    {
        Solicitado,
        Aprovado,
        Pagado
    }

    public class Prestamo
    {
        public int PrestamoID { get; set; }
        [Required]
        public int Plazo { get; set; }
        [Required]
        [DataType(DataType.Currency), Column(TypeName = "money")]
        public decimal Monto { get; set; }
        [Required]
        [Display(Name = "Tasa de interes")]
        public decimal TasaInteres { get; set; }
        [Required]
        [Display(Name = "Tasa de mora")]
        public decimal TasaMora { get; set; }
        [DataType(DataType.Currency), Column(TypeName = "money")]
        public decimal Deuda { get; set; }
        [Display(Name = "Proximo pago")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ProximoPago { get; set; }
        public Estado Estado { get; set; }

        public int GaranteID { get; set; }
        public int ClienteID { get; set; }

        public virtual Garante Garante { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<PagoPrestamo> Pagos { get; set; }
    }
}