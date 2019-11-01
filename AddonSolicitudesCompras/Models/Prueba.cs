using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("Prueba")]
    public class Prueba
    {
        public object page { get; set; }
        public object per_page { get; set; }
        public object total { get; set; }
        public object total_pages { get; set; }
        public object[] data { get; set; }

    }
}