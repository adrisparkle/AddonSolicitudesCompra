using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("Account")]
    public class Account: PurchaseRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int num_solicitud { get; set; }
        public int numero_asiento { get; set; }
        public int numero_origen { get; set; }
        public int numero_transaccion { get; set; }
        public string comentario { set; get; }
        public string referencia1 { get; set; }
        public string referencia2 { get; set; }
    }
}