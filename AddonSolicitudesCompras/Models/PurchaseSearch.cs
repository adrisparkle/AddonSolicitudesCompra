using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("PurchaseSearch")]
    public class PurchaseSearch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string id { set; get; }
        public string solicitante { get; set; }
        public string regional { get; set; }
        public DateTime fecha_contabilizacion { get; set; }
        public string unidad_organizacional { get; set; }
        public int numero_documento { get; set; }

    }
}