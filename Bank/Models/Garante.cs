using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bank.Models
{
    public class Garante : IPerson
    {
        public int GaranteID { get; set; }

        [StringLength(30, MinimumLength = 2, ErrorMessage = "El nombre de contener entre 2 y 30 caracteres.")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Las iniciales deben ser mausculas.")]
        public string Nombre { set; get; }
        [StringLength(30, MinimumLength = 2, ErrorMessage = "El apellido debe conttener entre 2 y 30 caracteres.")]
        public string Apellido { set; get; }
        [Required]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El numero de cedula debe estar compuesto por 13 caracteres.")]
        public string Cedula { get; set; }
        [StringLength(100)]
        public string Direccion { set; get; }
        public string Telefono { set; get; }

        public string FullName
        {
            get
            {
                return Nombre + " " + Apellido;
            }
        }

        public int PrestamoID { get; set; }
        public virtual Prestamo Prestamo { get; set; }
    }
}