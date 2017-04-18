using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bank.Models
{

    public enum TipoTransferencia
    {
        Deposito,
        Retiro,
        NetBanking
    }


    public class Transaccion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Numero")]
        public int TransaccionID { get; set; }

        [DataType(DataType.Currency), Column(TypeName = "money")]
        [Range(100,100000000,ErrorMessage = "No puede depositar o retirar menos de 100 pesos")]
        public decimal Monto { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true, NullDisplayText = "Debes introducir una fecha.")]
        public DateTime Fecha { get; set; }
        [StringLength(100, ErrorMessage = "Solo se permite un maximo de 100 caracteres por comentario.")]
        [UIHint("LimitedTextArea")]
        public string Comentario { get; set; }
        public TipoTransferencia Tipo { get; set; }

        public int CuentaID { get; set; }
        [ForeignKey("CuentaID")]
        public virtual Cuenta Cuenta { get; set; }

        public int CuentaIDFrom { get; set; }

    }
}