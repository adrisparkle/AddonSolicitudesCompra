using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("VLIRInfo")]
    public class VLIRInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string sucursal { get; set; }
        public string codigo_proyecto { get; set; }
        public string cuenta { get; set; }
        public string codigo_cuenta { get; set; }
        public string nombre_cuenta { get; set; }
        public DateTime fecha { get; set; }
        public string numero_comprobante { get; set; }
        public string numero_transaccion { get; set; }
        public string linea_transaccion { get; set; }
        public string referencia { get; set; }
        public string descripcion { get; set; }
        public decimal debe { get; set; }
        public decimal haber { get; set; }
        public decimal monto_total { get; set; }
       
    }

}