using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("GeneralRelations")]
    public class GeneralRelations: PurchaseRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int numero_solicitud { set; get; }
        public DateTime fecha_solicitud { get; set; }
        public int? numero_oferta { set; get; }
        public DateTime? fecha_oferta { get; set; }
        public int? numero_pedido { set; get; }
        public DateTime? fecha_pedido { get; set; }
        public int? numero_mercancia { set; get; }
        public DateTime? fecha_mercancia { get; set; }
        public int? numero_factura { set; get; }
        public DateTime? fecha_factura { get; set; }
        public int? numero_pago { set; get; }
        public DateTime? fecha_pago { get; set; }
        public string num_solicitud { get; set; }
        public string num_oferta { get; set; }
        public string num_pedido { get; set; }
        public string num_mercancia { get; set; }
        public string num_factura { get; set; }
        public string num_pago { get; set; }
        public string proveedor { get; set; }


    }
}