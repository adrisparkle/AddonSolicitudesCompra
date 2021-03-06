﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("PurchaseOrder")]
    public class PurchaseOrder: PurchaseRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int num_solicitud { get; set; }
        public string codigo_proveedor { set; get; }
        public string proveedor { set; get; }
        public int numero_documento { get; set; }
        public DateTime fecha_entrega { get; set; }
    }
}