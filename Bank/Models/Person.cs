using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bank.Models
{
    public interface IPerson
    {
        string Nombre { set; get; }
        string Apellido { set; get; }
        string Cedula { get; set; }
        string Direccion { set; get; }
        
    }
}