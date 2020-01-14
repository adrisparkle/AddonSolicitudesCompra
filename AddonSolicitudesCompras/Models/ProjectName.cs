using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddonSolicitudesCompras.Models
{
        [CustomSchema("ProjectName")]
        public class ProjectName
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public string PROYECTO_CODIGO { get; set; }
            public string proyecto_nombre { get; set; }
            public string unidad_organizacional { get; set; }
            public string pei_po { get; set; }
            public DateTime valido_hasta { get; set; }
            public DateTime valido_desde { get; set; }

        }
    
}