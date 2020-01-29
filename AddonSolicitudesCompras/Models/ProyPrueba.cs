using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("ProyPrueba")]
    public class ProyPrueba
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string FORMATCODE { get; set; }
        public string ACCTCODE { get; set; }
        public string ACCTNAME { get; set; }
        public int DIM1 { get; set; }
        public string DIM2 { get; set; }
        public decimal TOTAL_CUENTA { get; set; }
        public decimal TOTAL_DIM { get; set; }
        public decimal SOLICITADO { get; set; }
        public decimal COMPROMETIDO { get; set; }
        public decimal EJECUTADO { get; set; }
        public string SUCURSAL { get; set; }
        public string PrjCode { get; set; }
        public string PrjName { get; set; }
    }

}