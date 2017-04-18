using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bank.Models
{
    public enum EstadoCivil
    {
        Soltero_a,
        Comprometido_a,
        Casado_a,
        Viudo_a
    }

    public enum Sexo
    {
        Masculino,
        Femenino
    }

    public class Cliente : IPerson
    {
        public int ClienteID { get; set; }

        [StringLength(30, MinimumLength = 2, ErrorMessage = "El nombre de contener entre 2 y 30 caracteres.")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        public string Nombre { set; get; }
        [StringLength(30, MinimumLength = 2, ErrorMessage = "El apellido debe conttener entre 2 y 30 caracteres.")]
        public string Apellido { set; get; }
        [Required]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El numero de cedula debe estar compuesto por 13 caracteres.")]
        public string Cedula { get; set; }
        [StringLength(100)]
        public string Direccion { set; get; }
        [Required]
        [Display(Name = "Numero de Cuenta")]
        public int NumeroCuenta { set; get; }
        public Sexo Sexo { get; set; }
        public int Edad { get; set; }
        [Display(Name = "Estado Civil")]
        public EstadoCivil EstadoCivil { get; set; }
        [StringLength(60)]
        public string Correo { get; set; }
        [Required]
        [StringLength(12)]
        public string Telefono { get; set; }
        [StringLength(12)]
        public string Celular { get; set; }

        public virtual ICollection<Prestamo> Prestamo { set; get; }
        public virtual ICollection<Cuenta> Cuentas { set; get; }

        public string FullName
        {
            get
            {
                return Nombre + " " + Apellido;
            }
        }
    }
}