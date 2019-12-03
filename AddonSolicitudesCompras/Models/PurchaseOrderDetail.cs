using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("PurchaseOrderDetail")]
    public class PurchaseOrderDetail: PurchaseRequestDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public string saldo_presupuestado { get; set; }
        public string motivo_salida { get; set; }

    }
}