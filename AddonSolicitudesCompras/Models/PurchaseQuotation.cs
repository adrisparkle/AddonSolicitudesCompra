using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("PurchaseQuotation")]
    public class PurchaseQuotation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int numero_solicitud { get; set; }
        public string codigo_proveedor { set; get; }
        public string proveedor { set; get; }
        public string grupo_nombre { get; set; }
        public int grupo_numero { get; set; }
        public string regional { get; set; }
        public string serie { get; set; }
        public int numero_documento { get; set; }
        public string unidad_organizacional { get; set; }
        public DateTime fecha_contabilizacion { get; set; }
        public DateTime fecha_valida { get; set; }
        public DateTime fecha_documento { get; set; }
        public DateTime fecha_necesaria { get; set; }
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