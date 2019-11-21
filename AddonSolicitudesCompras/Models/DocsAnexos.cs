using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("DocsAnexos")]
    public class DocsAnexos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { set; get; }
        public string espicificaciones_tecnicas { set; get; }
        public string informe_proyecto { get; set; }
        public string informe_circunstanciado { get; set; }
        public string pago_directo { get; set; }
        public string propuesta { get; set; }
        public string cuadro_comparativo { get; set; }
        public string acta_evaluacion { get; set; }
        public string informe_proceso { get; set; }
        public string informe_legal { get; set; }
        public string pliego { get; set; }
        public string contrato { get; set; }

    }
}