using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("ProjectInfo")]
    public class ProjectInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string formatcode { get; set; }
        public string acctname { get; set; }
        public string acctcode { get; set; }
        public string dim1 { get; set; }
        public string dim2 { get; set; }
        public decimal total_dim { get; set; }
        public decimal ejecutado { get; set; }
        public decimal solicitado { get; set; }
        public decimal comprometido { get; set; }
        public decimal total_cuenta { get; set; }
        public int numerador { get; set; }
       
    }

}