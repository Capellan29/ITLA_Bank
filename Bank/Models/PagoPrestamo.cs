using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bank.Models
{
    public class PagoPrestamo
    {
        public int PagoPrestamoID { get; set; }
        [Required]
        public int Periodo { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Cuota { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Mora { get; set; }
        [Required]
        [DataType(DataType.Date), Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-YYYY}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        public int PrestamoID { get; set; }

        public virtual Prestamo Prestamo { get; set; }
    }
}