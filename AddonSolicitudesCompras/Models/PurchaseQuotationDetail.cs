using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("PurchaseQuotationDetail")]
    public class PurchaseQuotationDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public string codigo { set; get; }
        public int numero_documento { set; get; }
        public string descripcion { set; get; }
        public string observaciones { get; set; }
        public DateTime fecha_necesaria { get; set; }
        public decimal cantidad { get; set; }
        public string proyecto { get; set; }
        public string almacen { get; set; }
        public string impuesto { get; set; }
        public decimal precio_unitario { get; set; }
        public decimal total { get; set; }
        public string unidad_organizacional { get; set; }
        public string pei_po { get; set; }

    }
}