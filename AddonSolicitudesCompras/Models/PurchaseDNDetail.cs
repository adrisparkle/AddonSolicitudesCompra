using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("PurchaseDNDetail")]
    public class PurchaseDNDetail: PurchaseRequestDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public string saldo_presupuestado { get; set; }
        public string motivo_salida { get; set; }

    }
}