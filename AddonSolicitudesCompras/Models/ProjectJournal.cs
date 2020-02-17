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
            public int numero_transaccion { get; set; }
            public int numero_linea { get; set; }
            public string glosa { get; set; }
            public decimal monto { get; set; }
            public string cuenta { get; set; }
            public int uo { get; set; }
            public string proyecto { get; set; }
        }
    
}