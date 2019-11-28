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
        public string PROYECTO_CODIGO { get; set; }
        public string proyecto_nombre { get; set; }
        public string sucursal { get; set; }
        public string cuenta { get; set; }
        public string codigo_cuenta { get; set; }
        public string nombre_cuenta { get; set; }
        public string unidad_organizacional { get; set; }
        public string pei_po { get; set; }
        public decimal total_cuenta { get; set; }
        public decimal total_dim { get; set; }
        public decimal ejecutado { get; set; }
       
    }

}