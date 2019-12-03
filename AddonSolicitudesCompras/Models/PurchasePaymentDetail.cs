using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("PurchasePaymentDetail")]
    public class PurchasePaymentDetail: PurchaseRequestDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public string sujeto_a_retencion { get; set; }
        public string retencion_a_aplicar { get; set; }

    }
}