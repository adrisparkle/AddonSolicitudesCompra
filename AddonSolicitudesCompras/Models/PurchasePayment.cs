using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("PurchasePayment")]
    public class PurchasePayment: PurchaseRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int numero_solicitud { get; set; }
        public string codigo_proveedor { set; get; }
        public string proveedor { set; get; }
        public int numero_operacion { get; set; }
        public int numero_documento { get; set; }
        public DateTime fecha_vencimiento { get; set; }
        public DateTime fecha_operacion { get; set; }
    }
}