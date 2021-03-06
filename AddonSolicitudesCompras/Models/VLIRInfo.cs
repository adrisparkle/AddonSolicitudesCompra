﻿using System;
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
        public string regional { get; set; }
        public string codigo_proyecto { get; set; }
        public string cuenta { get; set; }
        public string codigo_cuenta { get; set; }
        public string nombre_cuenta { get; set; }
        public DateTime fecha_comprobante { get; set; }
        public string numero_comprobante { get; set; }
        public string numero_transaccion { get; set; }
        public string linea_transaccion { get; set; }
        public string referencia { get; set; }
        public string glosa { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal monto_total { get; set; }
        public DateTime? fecha_fac { get; set; }

    }

}