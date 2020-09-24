using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("FRICA")]
    public class FRICA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string codigo_proyecto { get; set; }
        public string nombre_proyecto { get; set; }
        public string regional { get; set; }
    }

}