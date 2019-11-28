using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("Project")]
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string codigo_proyecto { get; set; }
        public string nombre_proyecto { get; set; }
        public string regional { get; set; }
        public string pei_po { get; set; }
        public string unidad_organizacional { get; set; }
        public DateTime valido_desde { get; set; }
        public DateTime valido_hasta { get; set; }
        public DateTime fecha_documento { get; set; }
    }

}