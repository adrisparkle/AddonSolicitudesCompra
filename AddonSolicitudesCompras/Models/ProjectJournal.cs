using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddonSolicitudesCompras.Models
{
        [CustomSchema("ProjectJournal")]
    public class ProjectJournal
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public DateTime fecha { get; set; }
            public int trans_id { get; set; }
            public int line_id { get; set; }
            public string memo { get; set; }
            public decimal Debit { get; set; }
            public decimal Credit { get; set; }
            public decimal total { get; set; }
            public string cuenta { get; set; }
            public int unidad_organizacional { get; set; }
            public string nombre { get; set; }
            public string codigo { get; set; }

        }
    
}