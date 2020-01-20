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
    public class OfertaController : ApiController
    {
        private ApplicationDbContext _context;


        public OfertaController()
        {

            _context = new ApplicationDbContext();
     
        }

        // GET api/Contract
        [HttpGet]
        [Route("api/PurchaseQuotation/{id}")]
        public IHttpActionResult PurchaseQuotation(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select oprq.\"DocEntry\" as \"num_solicitud\",\r\nop.\"CardCode\" as \"codigo_proveedor\",\r\nop.\"CardName\" as \"proveedor\",\r\nop.\"PQTGrpSer\" as \"grupo_nombre\",\r\nop.\"PQTGrpNum\" as \"grupo_numero\",\r\nop.\"BPLName\" as \"regional\",\r\nf.\"SeriesName\" as \"serie\",\r\nop.\"U_UOrganiza\" as \"unidad_organizacional\",\r\nop.\"DocNum\" as \"numero_documento\",\r\nop.\"DocDate\" as \"fecha_contabilizacion\", \r\nop.\"DocDueDate\" as \"fecha_valida\", \r\nop.\"TaxDate\" as \"fecha_documento\", \r\nop.\"ReqDate\" as \"fecha_necesaria\",\r\ncase \r\nwhen op.\"CANCELED\"='Y' THEN 'Cancelado'\r\nwhen op.\"CANCELED\"='N' THEN 'Aprobado'\r\nend as \"estado\",\r\nTO_VARCHAR(op.\"U_DocEspTecnicas\")\tas \t\"espicificaciones_tecnicas\",\r\nTO_VARCHAR(op.\"U_DocInfProyecto\")\tas \t\"informe_proyecto\",\r\nTO_VARCHAR(op.\"U_InfCircunstanciado\")\tas \t\"informe_circunstanciado\",\r\nTO_VARCHAR(op.\"U_APagoDirecto\")\tas \t\"pago_directo\",\r\nTO_VARCHAR(op.\"U_Propuesta\")\tas \t\"propuesta\",\r\nTO_VARCHAR(op.\"U_CuadroComparativo\")\tas \t\"cuadro_comparativo\",\r\nTO_VARCHAR(op.\"U_ActaEvaluacion\")\tas \t\"acta_evaluacion\",\r\nTO_VARCHAR(op.\"U_InformeProceso\")\tas \t\"informe_proceso\",\r\nTO_VARCHAR(op.\"U_InformeLegal\")\tas \t\"informe_legal\",\r\nTO_VARCHAR(op.\"U_Pliego\")\tas \t\"pliego\",\r\nTO_VARCHAR(op.\"U_Contrato\")\tas \t\"contrato\"\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq\r\ninner join \"UCATOLICA\".\"PRQ1\" prq1\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"\r\ninner join \"UCATOLICA\".\"PQT1\" pqt1\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"\r\ninner join\"UCATOLICA\".\"OPQT\" op\r\non op.\"DocEntry\" = pqt1.\"DocEntry\"\r\nand TO_VARCHAR(oprq.\"DocNum\") = TO_VARCHAR(pqt1.\"BaseRef\")\r\ninner join ucatolica.\"NNM1\" f\r\non op.\"Series\" = f.\"Series\"\r\nwhere op.\"DocEntry\" ="
                               + id +
                               " group by oprq.\"DocEntry\",\r\nop.\"CardCode\",\r\nop.\"CardName\",\r\nop.\"PQTGrpSer\",\r\nop.\"PQTGrpNum\",\r\nop.\"BPLName\",\r\nf.\"SeriesName\",\r\nop.\"U_UOrganiza\",\r\nop.\"DocNum\",\r\nop.\"DocDate\",\r\nop.\"DocDueDate\",\r\nop.\"TaxDate\",\r\nop.\"ReqDate\",\r\nTO_VARCHAR(op.\"U_DocEspTecnicas\"),\r\nTO_VARCHAR(op.\"U_DocInfProyecto\"),\r\nTO_VARCHAR(op.\"U_InfCircunstanciado\"),\r\nTO_VARCHAR(op.\"U_APagoDirecto\"),\r\nTO_VARCHAR(op.\"U_Propuesta\"),\r\nTO_VARCHAR(op.\"U_CuadroComparativo\"),\r\nTO_VARCHAR(op.\"U_ActaEvaluacion\"),\r\nTO_VARCHAR(op.\"U_InformeProceso\"),\r\nTO_VARCHAR(op.\"U_InformeLegal\"),\r\nTO_VARCHAR(op.\"U_Pliego\"),\r\nTO_VARCHAR(op.\"U_Contrato\"),\r\nOP.\"CANCELED\"";
            var rawresult = _context.Database.SqlQuery<PurchaseQuotation>(queryProduct).ToList();
            var formatedData = rawresult.Select(x => new
            {
                x.num_solicitud,
                x.codigo_proveedor,
                x.proveedor,
                x.grupo_nombre,
                x.grupo_numero,
                x.numero_documento,
                x.serie,
                x.unidad_organizacional,
                x.regional,
                fecha_contabilizacion = x.fecha_contabilizacion.ToString("dd/MM/yyyy"),
                fecha_valida = x.fecha_valida.ToString("dd/MM/yyyy"),
                fecha_documento = x.fecha_documento.ToString("dd/MM/yyyy"),
                fecha_necesaria = x.fecha_necesaria.ToString("dd/MM/yyyy"),
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
                x.estado

            });
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/PurchaseQuotationDetail/{id}")]
        public IHttpActionResult PurchaseQuotationDetail(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct =
                "select\r\npqt1.\"ItemCode\" as \"codigo\"," +
                "\r\npqt1.\"Dscription\" as \"descripcion\"," +
                "\r\npqt1.\"FreeTxt\" as \"observaciones\"," +
                "\r\npqt1.\"PQTReqDate\" as \"fecha_necesaria\"," +
                "\r\npqt1.\"PQTReqQty\" as \"cantidad\"," +
                "\r\npqt1.\"TaxCode\" as \"impuesto\"," +
                "\r\npqt1.\"WhsCode\" as \"almacen\"," +
                "\r\npqt1.\"Project\" as \"proyecto\"," +
                "\r\npqt1.\"PriceAfVAT\" as \"precio_unitario\"," +
                "\r\npqt1.\"GTotal\" as \"total\"," +
                "\r\npqt1.\"OcrCode\" as \"unidad_organizacional\"," +
                "\r\npqt1.\"OcrCode2\" as \"pei_po\"" +
                "\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq" +
                "\r\ninner join \"UCATOLICA\".\"PRQ1\" prq1" +
                "\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"" +
                "\r\nleft outer join \"UCATOLICA\".\"PQT1\" pqt1" +
                "\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"" +
                "\r\nleft outer join\"UCATOLICA\".\"OPQT\" op" +
                "\r\non op.\"DocEntry\" = pqt1.\"DocEntry\"" +
                "\r\nleft join ucatolica.\"NNM1\" f" +
                "\r\non op.\"Series\" = f.\"Series\"" +
                "\r\nand oprq.\"DocNum\" = pqt1.\"BaseRef\"" +
                "\r\nwhere op.\"DocEntry\" = " +
                id + " group by pqt1.\"ItemCode\"," +
                "\r\npqt1.\"Dscription\"," +
                "\r\npqt1.\"FreeTxt\"," +
                "\r\npqt1.\"PQTReqDate\"," +
                "\r\npqt1.\"PQTReqQty\"," +
                "\r\npqt1.\"TaxCode\"," +
                "\r\npqt1.\"WhsCode\"," +
                "\r\npqt1.\"Project\"," +
                "\r\npqt1.\"PriceAfVAT\"," +
                "\r\npqt1.\"GTotal\"," +
                "\r\npqt1.\"OcrCode\"," +
                "\r\npqt1.\"OcrCode2\"";

            var rawresult = _context.Database.SqlQuery<PurchaseQuotationDetail>(queryProduct).ToList();

            var formatedData = rawresult.Select(x => new
            {
                x.codigo,
                x.descripcion,
                x.numero_documento,
                x.observaciones,
                fecha_necesaria = x.fecha_necesaria.ToString("dd/MM/yyyy"),
                cantidad = Decimal.ToInt32(x.cantidad),
                x.proyecto,
                x.almacen,
                x.impuesto,
                precio_unitario = Convert.ToSingle(x.precio_unitario),
                total = Convert.ToSingle(x.total),
                x.unidad_organizacional,
                x.pei_po,

            });
            return Ok(formatedData);
        }
       
    }
}
