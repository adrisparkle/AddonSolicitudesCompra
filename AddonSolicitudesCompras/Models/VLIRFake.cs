using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("VLIRFake")]
    public class VLIRFake
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string codigo_proyecto { get; set; }
        public string nombre_proyecto { get; set; }
        public string regional { get; set; }
        public string pei_po { get; set; }
        public string unidad_organizacional { get; set; }
        public string valido_desde { get; set; }
        public string valido_hasta { get; set; }
    }

}