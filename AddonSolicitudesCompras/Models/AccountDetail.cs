using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("AccountDetail")]
    public class AccountDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string cuenta { get; set; }
        public string nombre_cuenta { get; set; }
        public DateTime fecha_contabilizacion { get; set; }
        public DateTime fecha_valida { get; set; }
        public DateTime fecha_documento { get; set; }
        public string comentario { set; get; }
        public string referencia1 { get; set; }
        public string referencia2 { get; set; }
        public string referencia3 { get; set; }
        public string proyecto { get; set; }
        public string cuenta_mayor { get; set; }
        public string codigo_cuenta { get; set; }
        public string cuenta_oficial { get; set; }
        public string debito_bs { get; set; }
        public string credito_bs { get; set; }
        public string debito_ms { get; set; }
        public string credito_ms { get; set; }
    }
}