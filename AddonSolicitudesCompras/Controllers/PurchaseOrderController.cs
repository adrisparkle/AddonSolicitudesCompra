using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using AddonSolicitudesCompras.Models;
using System.Globalization;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;


namespace AddonSolicitudesCompras.Controllers
{
    public class PedidoController : ApiController
    {
        private ApplicationDbContext _context;


        public PedidoController()
        {

            _context = new ApplicationDbContext();
     
        }

        // GET api/Contract
        [HttpGet]
        [Route("api/PurchaseOrder/{id}")]
        public IHttpActionResult PurchaseOrder(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select oprq.\"DocNum\" as \"numero_solicitud\",\r\nopor.\"CardCode\" as \"codigo_proveedor\", \r\nopor.\"CardName\" as \"proveedor\",\r\nopor.\"BPLName\" as \"regional\",\r\nf.\"SeriesName\" as \"serie\",\r\nopor.\"U_UOrganiza\" as \"unidad_organizacional\",\r\nopor.\"DocNum\" as \"numero_documento\",\r\nopor.\"DocDate\" as \"fecha_contabilizacion\",\r\nopor.\"DocDueDate\" as \"fecha_valida\",\r\nopor.\"TaxDate\" as \"fecha_documento\",\r\nTO_VARCHAR(opor.\"U_DocEspTecnicas\")\tas \t\"espicificaciones_tecnicas\",\r\nTO_VARCHAR(opor.\"U_DocInfProyecto\")\tas \t\"informe_proyecto\",\r\nTO_VARCHAR(opor.\"U_InfCircunstanciado\")\tas \t\"informe_circunstanciado\",\r\nTO_VARCHAR(opor.\"U_APagoDirecto\")\tas \t\"pago_directo\",\r\nTO_VARCHAR(opor.\"U_Propuesta\")\tas \t\"propuesta\",\r\nTO_VARCHAR(opor.\"U_CuadroComparativo\")\tas \t\"cuadro_comparativo\",\r\nTO_VARCHAR(opor.\"U_ActaEvaluacion\")\tas \t\"acta_evaluacion\",\r\nTO_VARCHAR(opor.\"U_InformeProceso\")\tas \t\"informe_proceso\",\r\nTO_VARCHAR(opor.\"U_InformeLegal\")\tas \t\"informe_legal\",\r\nTO_VARCHAR(opor.\"U_Pliego\")\tas \t\"pliego\",\r\nTO_VARCHAR(opor.\"U_Contrato\")\tas \t\"contrato\"\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq\r\ninner join \"UCATOLICA\".\"PRQ1\" prq1\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"\r\ninner join \"UCATOLICA\".\"PQT1\" pqt1\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"\r\ninner join\"UCATOLICA\".\"OPQT\" opqt\r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\"\r\nand TO_VARCHAR(oprq.\"DocNum\") = TO_VARCHAR(pqt1.\"BaseRef\")\r\ninner join \"UCATOLICA\".\"POR1\" por1\r\non opqt.\"DocEntry\" = por1.\"BaseEntry\"\r\nand TO_VARCHAR(opqt.\"DocNum\") = TO_VARCHAR(por1.\"BaseRef\")\r\ninner join \"UCATOLICA\".\"OPOR\" opor\r\non opor.\"DocEntry\" = por1.\"DocEntry\"\r\nleft join ucatolica.\"NNM1\" f\r\non opor.\"Series\" = f.\"Series\"\r\nwhere opor.\"DocNum\" =" 
                               + id + 
                               " group by oprq.\"DocNum\",\r\nopor.\"CardCode\",\r\nopor.\"CardName\",\r\nopor.\"BPLName\",\r\nf.\"SeriesName\",\r\nopor.\"U_UOrganiza\",\r\nopor.\"DocNum\",\r\nopor.\"DocDate\",\r\nopor.\"DocDueDate\",\r\nopor.\"TaxDate\",\r\nTO_VARCHAR(opor.\"U_DocEspTecnicas\"),\r\nTO_VARCHAR(opor.\"U_DocInfProyecto\"),\r\nTO_VARCHAR(opor.\"U_InfCircunstanciado\"),\r\nTO_VARCHAR(opor.\"U_APagoDirecto\"),\r\nTO_VARCHAR(opor.\"U_Propuesta\"),\r\nTO_VARCHAR(opor.\"U_CuadroComparativo\"),\r\nTO_VARCHAR(opor.\"U_ActaEvaluacion\"),\r\nTO_VARCHAR(opor.\"U_InformeProceso\"),\r\nTO_VARCHAR(opor.\"U_InformeLegal\"),\r\nTO_VARCHAR(opor.\"U_Pliego\"),\r\nTO_VARCHAR(opor.\"U_Contrato\")";

            var rawresult = _context.Database.SqlQuery<PurchaseOrder>(queryProduct).ToList();
            var formatedData = rawresult.Select(x => new
            {
                x.numero_solicitud,
                x.codigo_proveedor,
                x.proveedor,
                x.numero_documento,
                x.serie,
                x.unidad_organizacional,
                x.regional,
                fecha_contabilizacion = x.fecha_contabilizacion.ToString("dd/MM/yyyy"),
                fecha_entrega = x.fecha_entrega.ToString("dd/MM/yyyy"),
                fecha_documento = x.fecha_documento.ToString("dd/MM/yyyy"),
                x.espicificaciones_tecnicas,
                x.informe_proyecto,
                x.informe_circunstanciado,
                x.pago_directo,
                x.propuesta,
                x.cuadro_comparativo,
                x.acta_evaluacion,
                x.informe_proceso,
                x.informe_legal,
                x.pliego,
                x.contrato,

            });
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/PurchaseOrderDetail/{id}")]
        public IHttpActionResult PurchaseOrderDetail(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select \r\npor1.\"ItemCode\" as \"codigo\"," +
                               "\r\npor1.\"Dscription\" as \"descripcion\"," +
                               "\r\npor1.\"FreeTxt\" as \"observaciones\"," +
                               "\r\npor1.\"Quantity\" as \"cantidad\"," +
                               "\r\npor1.\"TaxCode\" as \"impuesto\"," +
                               "\r\npor1.\"WhsCode\" as \"almacen\"," +
                               "\r\npor1.\"Project\" as \"proyecto\"," +
                               "\r\npor1.\"PriceAfVAT\" as \"precio_unitario\"," +
                               "\r\npor1.\"GTotal\" as \"total\"," +
                               "\r\npor1.\"OcrCode\" as \"unidad_organizacional\"," +
                               "\r\npor1.\"OcrCode2\" as \"pei_po\"," +
                               "\r\npor1.\"U_PR_SALDOPRES\" as \"saldo_presupuestado\"," +
                               "\r\npor1.\"U_MotivoSalida\" as \"motivo_salida\"" +
                               "\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq" +
                               "\r\ninner join \"UCATOLICA\".\"PRQ1\" prq1" +
                               "\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"PQT1\" pqt1" +
                               "\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"" +
                               "\r\nleft outer join\"UCATOLICA\".\"OPQT\" opqt" +
                               "\r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\"" +
                               "\r\nand oprq.\"DocNum\" = pqt1.\"BaseRef\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"POR1\" por1" +
                               "\r\non opqt.\"DocEntry\" = por1.\"BaseEntry\"" +
                               "\r\nand opqt.\"DocNum\" = por1.\"BaseRef\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"OPOR\" opor" +
                               "\r\non opor.\"DocEntry\" = por1.\"DocEntry\"" +
                               "\r\nleft join ucatolica.\"NNM1\" f" +
                               "\r\non opor.\"Series\" = f.\"Series\"" +
                               "\r\nwhere opor.\"DocNum\" ="+id +
                               " group by por1.\"ItemCode\"," +
                               "\r\npor1.\"Dscription\"," +
                               "\r\npor1.\"FreeTxt\"," +
                               "\r\npor1.\"Quantity\"," +
                               "\r\npor1.\"TaxCode\"," +
                               "\r\npor1.\"WhsCode\"," +
                               "\r\npor1.\"Project\"," +
                               "\r\npor1.\"PriceAfVAT\"," +
                               "\r\npor1.\"GTotal\"," +
                               "\r\npor1.\"OcrCode\"," +
                               "\r\npor1.\"OcrCode2\"," +
                               "\r\npor1.\"U_PR_SALDOPRES\"," +
                               "\r\npor1.\"U_MotivoSalida\"";

            var rawresult = _context.Database.SqlQuery<PurchaseOrderDetail>(queryProduct).ToList();

            var formatedData = rawresult.Select(x => new
            {
                x.codigo,
                x.descripcion,
                x.observaciones,
                cantidad = Decimal.ToInt32(x.cantidad),
                x.proyecto,
                x.almacen,
                x.impuesto,
                precio_unitario = Convert.ToSingle(x.precio_unitario),
                total = Convert.ToSingle(x.total),
                x.unidad_organizacional,
                x.pei_po,
                x.saldo_presupuestado,
                x.motivo_salida

            });
            return Ok(formatedData);
        }
    }
}
