using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("PurchaseQuotationDetail")]
    public class PurchaseQuotationDetail: PurchaseRequestDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int numero_documento { set; get; }

    }
}