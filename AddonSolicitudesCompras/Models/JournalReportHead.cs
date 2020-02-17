using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddonSolicitudesCompras.Models
{
        [CustomSchema("ProjectName")]
        public class JournalReportHead
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public string sucursal { get; set; }
            public string nombre_proyecto { get; set; }
            public string codigo_proyecto { get; set; }
            public string cuenta { get; set; }
            public string codigo_cuenta { get; set; }
            public string nombre_cuenta { get; set; }
            public decimal monto { get; set; }

        }
    
}