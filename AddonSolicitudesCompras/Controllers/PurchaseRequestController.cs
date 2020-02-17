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
    public class ComprasController : ApiController
    {
        private ApplicationDbContext _context;


        public ComprasController()
        {

            _context = new ApplicationDbContext();

        }

        // GET api/Contract
        [HttpGet]
        [Route("api/PurchaseSearch")]
        public IHttpActionResult PurchaseSearch()
        {
            var query = "select oprq.\"DocNum\" as \"numero_documento\",\r\noprq.\"ReqName\" as \"solicitante\",\r\noprq.\"DocDate\" as \"fecha_contabilizacion\",\r\noprq.\"BPLName\" as \"regional\",\r\noprq.\"U_UOrganiza\" as \"unidad_organizacional\",\r\noprq.\"DocEntry\" as \"id\"\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq\r\nleft join ucatolica.\"NNM1\" f\r\non oprq.\"Series\" = f.\"Series\"\r\norder by oprq.\"DocDate\" desc";
            var rawresult = _context.Database.SqlQuery<PurchaseSearch>(query).ToList();
            var formatedData = rawresult.Select(x => new
            {
                x.id,
                x.solicitante,
                fecha_contabilizacion = x.fecha_contabilizacion.ToString("dd/MM/yyyy"),
                x.regional,
                x.unidad_organizacional,
                x.numero_documento
            });

            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/PurchaseSearchDetail/{parameter}")]
        public IHttpActionResult PurchaseSearchDetail(string parameter)
        {
            parameter = parameter.ToUpper();
            var query = "select " +
                        "oprq.\"DocNum\" as \"numero_documento\"," +
                        "\r\noprq.\"ReqName\" as \"solicitante\"," +
                        "\r\noprq.\"DocDate\" as \"fecha_contabilizacion\"," +
                        "\r\noprq.\"BPLName\" as \"regional\"," +
                        "\r\noprq.\"U_UOrganiza\" as \"unidad_organizacional\"," +
                        "\r\noprq.\"DocEntry\" as \"id\"" +
                        " from ucatolica.\"OPRQ\" oprq" +
                        "\r\nwhere TO_VARCHAR(oprq.\"DocNum\") like '%"+parameter+"%'" +
                        "\r\nor TO_VARCHAR(oprq.\"ReqName\") like '%" + parameter + "%'" +
                        "\r\nor TO_VARCHAR(oprq.\"DocDate\") like '%" + parameter + "%'" +
                        "\r\nor TO_VARCHAR(oprq.\"U_UOrganiza\") like '%" + parameter + "%'" +
                        "\r\nor TO_VARCHAR(oprq.\"BPLName\") like '%" + parameter + "%'";
            var rawresult = _context.Database.SqlQuery<PurchaseSearch>(query).ToList();
            var formatedData = rawresult.Select(x => new
            {
                x.id,
                x.solicitante,
                fecha_contabilizacion = x.fecha_contabilizacion.ToString("dd/MM/yyyy"),
                x.regional,
                x.unidad_organizacional,
                x.numero_documento
            });

            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/PurchaseRequest/{id}")]
        public IHttpActionResult PurchaseRequest(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select op.\"DocNum\" as \"id\"," +
                               " \r\nop.\"Requester\" as \"codigo_solicitante\", " +
                               "\r\nop.\"ReqName\" as \"solicitante\", " +
                               "\r\nf.\"SeriesName\" as \"serie\", " +
                               "\r\nop.\"BPLName\" as \"regional\"," +
                               "\r\nop.\"U_UOrganiza\" as \"unidad_organizacional\"," +
                               "\r\nop.\"DocDate\" as \"fecha_contabilizacion\"," +
                               "\r\nop.\"DocDueDate\" as \"fecha_valida\"," +
                               "\r\nop.\"TaxDate\" as \"fecha_documento\"," +
                               "\r\nop.\"ReqDate\" as \"fecha_requerida\"," +
                               "\r\nTO_VARCHAR(op.\"U_DocEspTecnicas\")\tas \t\"espicificaciones_tecnicas\"," +
                               "\r\nTO_VARCHAR(op.\"U_DocInfProyecto\")\tas \t\"informe_proyecto\",\r\nTO_VARCHAR(op.\"U_InfCircunstanciado\")\tas \t\"informe_circunstanciado\",\r\nTO_VARCHAR(op.\"U_APagoDirecto\")\tas \t\"pago_directo\",\r\nTO_VARCHAR(op.\"U_Propuesta\")\tas \t\"propuesta\",\r\nTO_VARCHAR(op.\"U_CuadroComparativo\")\tas \t\"cuadro_comparativo\",\r\nTO_VARCHAR(op.\"U_ActaEvaluacion\")\tas \t\"acta_evaluacion\",\r\nTO_VARCHAR(op.\"U_InformeProceso\")\tas \t\"informe_proceso\",\r\nTO_VARCHAR(op.\"U_InformeLegal\")\tas \t\"informe_legal\",\r\nTO_VARCHAR(op.\"U_Pliego\")\tas \t\"pliego\",\r\nTO_VARCHAR(op.\"U_Contrato\")\tas \t\"contrato\",\r\ncase \r\nwhen op.\"CANCELED\"='Y' THEN 'Cancelado'\r\nwhen op.\"CANCELED\"='N' THEN 'Aprobado'\r\nend as \"estado\"\r\nfrom \"UCATOLICA\".\"OPRQ\" op\r\nleft join ucatolica.\"NNM1\" \r\nf on op.\"Series\" = f.\"Series\"\r\nwhere op.\"DocEntry\" ="
                               + id +
                               " group by op.\"DocNum\", \r\nop.\"Requester\", op.\"ReqName\", \r\nf.\"SeriesName\", op.\"BPLName\",\r\nop.\"U_UOrganiza\", \r\nop.\"DocDate\", \r\nop.\"DocDueDate\", op.\"TaxDate\",\r\nop.\"ReqDate\",\r\nTO_VARCHAR(op.\"U_DocEspTecnicas\"),\r\nTO_VARCHAR(op.\"U_DocInfProyecto\"),\r\nTO_VARCHAR(op.\"U_InfCircunstanciado\"),\r\nTO_VARCHAR(op.\"U_APagoDirecto\"),\r\nTO_VARCHAR(op.\"U_Propuesta\"),\r\nTO_VARCHAR(op.\"U_CuadroComparativo\"),\r\nTO_VARCHAR(op.\"U_ActaEvaluacion\"),\r\nTO_VARCHAR(op.\"U_InformeProceso\"),\r\nTO_VARCHAR(op.\"U_InformeLegal\"),\r\nTO_VARCHAR(op.\"U_Pliego\"),\r\nTO_VARCHAR(op.\"U_Contrato\"),\r\nop.\"CANCELED\"";
            var rawresult = _context.Database.SqlQuery<PurchaseRequest>(queryProduct).ToList();
            var formatedData = rawresult.Select(x => new
            {
                x.codigo_solicitante,
                x.solicitante,
                x.serie,
                x.id,
                x.unidad_organizacional,
                x.regional,
                fecha_contabilizacion = x.fecha_contabilizacion.ToString("dd/MM/yyyy"),
                fecha_valida = x.fecha_valida.ToString("dd/MM/yyyy"),
                fecha_documento = x.fecha_documento.ToString("dd/MM/yyyy"),
                fecha_requerida = x.fecha_requerida.ToString("dd/MM/yyyy"),
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
        [Route("api/PurchaseRequestDetail/{id}")]
        public IHttpActionResult PurchaseRequestDetail(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select\r\nprq1.\"ItemCode\" as \"codigo\"," +
                               "\r\nprq1.\"Dscription\" as \"descripcion\"," +
                               "\r\nprq1.\"FreeTxt\" as \"observaciones\"," +
                               "\r\nprq1.\"LineVendor\" as \"proveedor\"," +
                               "\r\nprq1.\"PQTReqDate\" as \"fecha_necesaria\"," +
                               "\r\nprq1.\"Quantity\" as \"cantidad\"," +
                               "\r\nprq1.\"Project\" as \"proyecto\"," +
                               "\r\nprq1.\"WhsCode\" as \"almacen\"," +
                               "\r\nprq1.\"TaxCode\" as \"impuesto\"," +
                               "\r\nprq1.\"PriceAfVAT\" as \"precio_unitario\"," +
                               "\r\nprq1.\"GTotal\" as \"total\"," +
                               "\r\nprq1.\"OcrCode\" as \"unidad_organizacional\"," +
                               "\r\nprq1.\"OcrCode2\" as \"pei_po\"" +
                               "\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq\r\n" +
                               " inner join \"UCATOLICA\".\"PRQ1\" prq1\r\n" +
                               " on oprq.\"DocEntry\" = prq1.\"DocEntry\"" +
                               "\r\nwhere oprq.\"DocEntry\" = " + id
                               + " group by prq1.\"ItemCode\"," +
                               "\r\nprq1.\"Dscription\"," +
                               "\r\nprq1.\"FreeTxt\"," +
                               "\r\nprq1.\"LineVendor\"," +
                               "\r\nprq1.\"PQTReqDate\"," +
                               "\r\nprq1.\"Quantity\"," +
                               "\r\nprq1.\"Project\"," +
                               "\r\nprq1.\"WhsCode\"," +
                               "\r\nprq1.\"TaxCode\"," +
                               "\r\nprq1.\"PriceAfVAT\"," +
                               "\r\nprq1.\"GTotal\"," +
                               "\r\nprq1.\"OcrCode\"," +
                               "\r\nprq1.\"OcrCode2\"";

            var rawresult = _context.Database.SqlQuery<PurchaseRequestDetail>(queryProduct).ToList();

            var formatedData = rawresult.Select(x => new
            {
                x.codigo,
                x.descripcion,
                x.observaciones,
                x.proveedor,
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
        [HttpGet]
        [Route("api/solicitud/{id}")]
        public IHttpActionResult solicitud(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select\r\noprq.\"DocEntry\" as \"num_solicitud\",\r\noprq.\"DocNum\" as \"numero_solicitud\",\r\noprq.\"DocDate\" as \"fecha_solicitud\",\r\noprq.\"ReqName\" as \"solicitante\",\r\noprq.\"CANCELED\" as \"estado\"\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq \r\ninner join \"UCATOLICA\".\"PRQ1\" prq1 \r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"\r\ninner join ucatolica.\"NNM1\" f\r\non oprq.\"Series\" = f.\"Series\"\r\nleft outer join \"UCATOLICA\".\"PQT1\" pqt1 \r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\" \r\nleft outer join\"UCATOLICA\".\"OPQT\" opqt \r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\" \r\nand TO_VARCHAR(oprq.\"DocNum\") = TO_VARCHAR(pqt1.\"BaseRef\")\r\nleft outer join \"UCATOLICA\".\"POR1\" por1 \r\non TO_VARCHAR(opqt.\"DocEntry\") = TO_VARCHAR(por1.\"BaseEntry\")\r\nand TO_VARCHAR(opqt.\"DocNum\") =TO_VARCHAR( por1.\"BaseRef\") \r\nleft outer join \"UCATOLICA\".\"OPOR\" opor \r\non opor.\"DocEntry\" = por1.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"PDN1\" pdn1 \r\non TO_VARCHAR(pdn1.\"BaseRef\")= TO_VARCHAR(opor.\"DocNum\")\r\nand TO_VARCHAR(pdn1.\"BaseEntry\") = TO_VARCHAR(opor.\"DocEntry\")\r\nleft outer join \"UCATOLICA\".\"OPDN\" opdn \r\non opdn.\"DocEntry\" = pdn1.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"PCH1\" pch1 \r\non TO_VARCHAR(opor.\"DocEntry\") = TO_VARCHAR(pch1.\"BaseEntry\")\r\nand TO_VARCHAR(opor.\"DocEntry\") = TO_VARCHAR(pch1.\"BaseEntry\")\r\nleft outer join \"UCATOLICA\".\"OPCH\" opch \r\non opch.\"DocEntry\" = pch1.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"VPM2\" vpm2 \r\non opch.\"DocEntry\" = vpm2.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"OVPM\" ovpm \r\non vpm2.\"DocNum\" = ovpm.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"VPM1\" vpm1 \r\non vpm1.\"DocNum\" = ovpm.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"OJDT\" ojdt \r\non ojdt.\"CreatedBy\" = ovpm.\"DocEntry\" \r\nand TO_VARCHAR(ovpm.\"DocNum\") = TO_VARCHAR(ojdt.\"BaseRef\")\r\nleft outer join \"UCATOLICA\".\"JDT1\" jdt1 \r\non jdt1.\"TransId\" = ojdt.\"TransId\" \r\nand TO_VARCHAR(ojdt.\"BaseRef\") = TO_VARCHAR(jdt1.\"BaseRef\")\r\nand ojdt.\"CreatedBy\" = jdt1.\"CreatedBy\" \r\nwhere oprq.\"DocEntry\" = "
                               + id +
                               " group by oprq.\"DocNum\",\r\noprq.\"DocDate\",\r\noprq.\"CANCELED\",\r\noprq.\"DocEntry\",\r\noprq.\"ReqName\"";

            var rawresult = _context.Database.SqlQuery<GeneralRelations>(queryProduct).ToList();

            var formatedData = rawresult.Select(x => new
            {
                fecha_solicitud = x.fecha_solicitud.ToString("dd/MM/yyyy"),
                x.estado,
                x.solicitante,
                x.num_solicitud,
                x.numero_solicitud
            });
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/oferta/{id}")]
        public IHttpActionResult oferta(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select\r\nopqt.\"DocEntry\" as \"num_oferta\",\r\nopqt.\"DocNum\" as \"numero_oferta\",\r\nopqt.\"DocDate\" as \"fecha_oferta\",\r\nopqt.\"CANCELED\" as \"estado\",\r\nopqt.\"CardName\" as \"proveedor\"\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq \r\ninner join \"UCATOLICA\".\"PRQ1\" prq1 \r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"\r\ninner join ucatolica.\"NNM1\" f\r\non oprq.\"Series\" = f.\"Series\"\r\ninner join \"UCATOLICA\".\"PQT1\" pqt1 \r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\" \r\ninner join\"UCATOLICA\".\"OPQT\" opqt \r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\" \r\nand TO_VARCHAR(oprq.\"DocNum\") = TO_VARCHAR(pqt1.\"BaseRef\")\r\nleft outer join \"UCATOLICA\".\"POR1\" por1 \r\non TO_VARCHAR(opqt.\"DocEntry\") = TO_VARCHAR(por1.\"BaseEntry\")\r\nand TO_VARCHAR(opqt.\"DocNum\") =TO_VARCHAR( por1.\"BaseRef\") \r\nleft outer join \"UCATOLICA\".\"OPOR\" opor \r\non opor.\"DocEntry\" = por1.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"PDN1\" pdn1 \r\non TO_VARCHAR(pdn1.\"BaseRef\")= TO_VARCHAR(opor.\"DocNum\")\r\nand TO_VARCHAR(pdn1.\"BaseEntry\") = TO_VARCHAR(opor.\"DocEntry\")\r\nleft outer join \"UCATOLICA\".\"OPDN\" opdn \r\non opdn.\"DocEntry\" = pdn1.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"PCH1\" pch1 \r\non TO_VARCHAR(opor.\"DocEntry\") = TO_VARCHAR(pch1.\"BaseEntry\")\r\nand TO_VARCHAR(opor.\"DocEntry\") = TO_VARCHAR(pch1.\"BaseEntry\")\r\nleft outer join \"UCATOLICA\".\"OPCH\" opch \r\non opch.\"DocEntry\" = pch1.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"VPM2\" vpm2 \r\non opch.\"DocEntry\" = vpm2.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"OVPM\" ovpm \r\non vpm2.\"DocNum\" = ovpm.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"VPM1\" vpm1 \r\non vpm1.\"DocNum\" = ovpm.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"OJDT\" ojdt \r\non ojdt.\"CreatedBy\" = ovpm.\"DocEntry\" \r\nand TO_VARCHAR(ovpm.\"DocNum\") = TO_VARCHAR(ojdt.\"BaseRef\")\r\nleft outer join \"UCATOLICA\".\"JDT1\" jdt1 \r\non jdt1.\"TransId\" = ojdt.\"TransId\" \r\nand TO_VARCHAR(ojdt.\"BaseRef\") = TO_VARCHAR(jdt1.\"BaseRef\")\r\nand ojdt.\"CreatedBy\" = jdt1.\"CreatedBy\" \r\nwhere oprq.\"DocEntry\" = "
                               + id +
                               "  group by opqt.\"DocNum\",\r\nopqt.\"DocDate\",\r\nopqt.\"CANCELED\",\r\nopqt.\"DocEntry\",\r\nopqt.\"CardName\"";

            var rawresult = _context.Database.SqlQuery<GeneralRelations>(queryProduct).ToList();

            var formatedData = rawresult.Select(x => new
            {
                x.numero_oferta,
                fecha_oferta = x.fecha_oferta.HasValue ? x.fecha_oferta.Value.ToString("dd/MM/yyyy") : null,
                x.estado,
                x.num_oferta,
                x.proveedor

            });
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/pedido/{id}")]
        public IHttpActionResult pedido(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select\r\nopor.\"DocEntry\" as \"num_pedido\",\r\nopor.\"DocNum\" as \"numero_pedido\",\r\nopor.\"DocDate\" as \"fecha_pedido\",\r\nopor.\"CANCELED\" as \"estado\",\r\nopor.\"CardName\" as \"proveedor\"\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq \r\ninner join \"UCATOLICA\".\"PRQ1\" prq1 \r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"\r\ninner join ucatolica.\"NNM1\" f\r\non oprq.\"Series\" = f.\"Series\"\r\ninner join \"UCATOLICA\".\"PQT1\" pqt1 \r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\" \r\ninner join\"UCATOLICA\".\"OPQT\" opqt \r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\" \r\nand TO_VARCHAR(oprq.\"DocNum\") = TO_VARCHAR(pqt1.\"BaseRef\")\r\ninner join \"UCATOLICA\".\"POR1\" por1 \r\non TO_VARCHAR(opqt.\"DocEntry\") = TO_VARCHAR(por1.\"BaseEntry\")\r\nand TO_VARCHAR(opqt.\"DocNum\") =TO_VARCHAR( por1.\"BaseRef\") \r\ninner join \"UCATOLICA\".\"OPOR\" opor \r\non opor.\"DocEntry\" = por1.\"DocEntry\" \r\nleft join \"UCATOLICA\".\"PDN1\" pdn1 \r\non TO_VARCHAR(pdn1.\"BaseRef\")= TO_VARCHAR(opor.\"DocNum\")\r\nand TO_VARCHAR(pdn1.\"BaseEntry\") = TO_VARCHAR(opor.\"DocEntry\")\r\nleft join \"UCATOLICA\".\"OPDN\" opdn \r\non opdn.\"DocEntry\" = pdn1.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"PCH1\" pch1 \r\non TO_VARCHAR(opor.\"DocEntry\") = TO_VARCHAR(pch1.\"BaseEntry\")\r\nand TO_VARCHAR(opor.\"DocEntry\") = TO_VARCHAR(pch1.\"BaseEntry\")\r\nleft outer join \"UCATOLICA\".\"OPCH\" opch \r\non opch.\"DocEntry\" = pch1.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"VPM2\" vpm2 \r\non opch.\"DocEntry\" = vpm2.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"OVPM\" ovpm \r\non vpm2.\"DocNum\" = ovpm.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"VPM1\" vpm1 \r\non vpm1.\"DocNum\" = ovpm.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"OJDT\" ojdt \r\non ojdt.\"CreatedBy\" = ovpm.\"DocEntry\" \r\nand TO_VARCHAR(ovpm.\"DocNum\") = TO_VARCHAR(ojdt.\"BaseRef\")\r\nleft outer join \"UCATOLICA\".\"JDT1\" jdt1 \r\non jdt1.\"TransId\" = ojdt.\"TransId\" \r\nand TO_VARCHAR(ojdt.\"BaseRef\") = TO_VARCHAR(jdt1.\"BaseRef\")\r\nand ojdt.\"CreatedBy\" = jdt1.\"CreatedBy\" \r\nwhere oprq.\"DocEntry\" ="
                               + id +
                               "  group by opor.\"DocNum\",\r\nopor.\"DocDate\",\r\nopor.\"CANCELED\",\r\nopor.\"DocEntry\",\r\nopor.\"CardName\"";

            var rawresult = _context.Database.SqlQuery<GeneralRelations>(queryProduct).ToList();

            var formatedData = rawresult.Select(x => new
            {
                x.numero_pedido,
                x.estado,
                fecha_pedido = x.fecha_pedido.HasValue ? x.fecha_pedido.Value.ToString("dd/MM/yyyy") : null,
                x.num_pedido,
                x.proveedor
                
            });
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/mercancia/{id}")]
        public IHttpActionResult mercancia(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select\r\nopdn.\"DocEntry\" as \"num_mercancia\",\r\nopdn.\"DocNum\" as \"numero_mercancia\",\r\nopdn.\"DocDate\" as \"fecha_mercancia\",\r\nopdn.\"CANCELED\" as \"estado\",\r\nopdn.\"CardName\" as \"proveedor\"\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq \r\ninner join \"UCATOLICA\".\"PRQ1\" prq1 \r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"\r\ninner join ucatolica.\"NNM1\" f\r\non oprq.\"Series\" = f.\"Series\"\r\ninner join \"UCATOLICA\".\"PQT1\" pqt1 \r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\" \r\ninner join\"UCATOLICA\".\"OPQT\" opqt \r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\" \r\nand TO_VARCHAR(oprq.\"DocNum\") = TO_VARCHAR(pqt1.\"BaseRef\")\r\ninner join \"UCATOLICA\".\"POR1\" por1 \r\non TO_VARCHAR(opqt.\"DocEntry\") = TO_VARCHAR(por1.\"BaseEntry\")\r\nand TO_VARCHAR(opqt.\"DocNum\") =TO_VARCHAR( por1.\"BaseRef\") \r\ninner join \"UCATOLICA\".\"OPOR\" opor \r\non opor.\"DocEntry\" = por1.\"DocEntry\" \r\ninner join \"UCATOLICA\".\"PDN1\" pdn1 \r\non TO_VARCHAR(pdn1.\"BaseRef\")= TO_VARCHAR(opor.\"DocNum\")\r\nand TO_VARCHAR(pdn1.\"BaseEntry\") = TO_VARCHAR(opor.\"DocEntry\")\r\ninner join \"UCATOLICA\".\"OPDN\" opdn \r\non opdn.\"DocEntry\" = pdn1.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"PCH1\" pch1 \r\non TO_VARCHAR(opor.\"DocEntry\") = TO_VARCHAR(pch1.\"BaseEntry\")\r\nand TO_VARCHAR(opor.\"DocEntry\") = TO_VARCHAR(pch1.\"BaseEntry\")\r\nleft outer join \"UCATOLICA\".\"OPCH\" opch \r\non opch.\"DocEntry\" = pch1.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"VPM2\" vpm2 \r\non opch.\"DocEntry\" = vpm2.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"OVPM\" ovpm \r\non vpm2.\"DocNum\" = ovpm.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"VPM1\" vpm1 \r\non vpm1.\"DocNum\" = ovpm.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"OJDT\" ojdt \r\non ojdt.\"CreatedBy\" = ovpm.\"DocEntry\" \r\nand TO_VARCHAR(ovpm.\"DocNum\") = TO_VARCHAR(ojdt.\"BaseRef\")\r\nleft outer join \"UCATOLICA\".\"JDT1\" jdt1 \r\non jdt1.\"TransId\" = ojdt.\"TransId\" \r\nand TO_VARCHAR(ojdt.\"BaseRef\") = TO_VARCHAR(jdt1.\"BaseRef\")\r\nand ojdt.\"CreatedBy\" = jdt1.\"CreatedBy\" \r\nwhere oprq.\"DocEntry\" = "
                               + id +
                               "  group by opdn.\"DocNum\",\r\nopdn.\"DocDate\",\r\nopdn.\"CANCELED\",\r\nopdn.\"DocEntry\",\r\nopdn.\"CardName\"";

            var rawresult = _context.Database.SqlQuery<GeneralRelations>(queryProduct).ToList();

            var formatedData = rawresult.Select(x => new
            {
                x.estado,
                x.numero_mercancia,
                fecha_mercancia = x.fecha_mercancia.HasValue ? x.fecha_mercancia.Value.ToString("dd/MM/yyyy") : null,
                x.num_mercancia,
                x.proveedor
            });
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/factura/{id}")]
        public IHttpActionResult factura(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select\r\nopch.\"DocEntry\" as \"num_factura\",\r\nopch.\"DocNum\" as \"numero_factura\",\r\nopch.\"DocDate\" as \"fecha_factura\",\r\nopch.\"CANCELED\" as \"estado\",\r\nopch.\"CardName\" as \"proveedor\"\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq \r\ninner join \"UCATOLICA\".\"PRQ1\" prq1 \r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"\r\ninner join ucatolica.\"NNM1\" f\r\non oprq.\"Series\" = f.\"Series\"\r\ninner join \"UCATOLICA\".\"PQT1\" pqt1 \r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\" \r\ninner join\"UCATOLICA\".\"OPQT\" opqt \r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\" \r\nand TO_VARCHAR(oprq.\"DocNum\") = TO_VARCHAR(pqt1.\"BaseRef\")\r\ninner join \"UCATOLICA\".\"POR1\" por1 \r\non TO_VARCHAR(opqt.\"DocEntry\") = TO_VARCHAR(por1.\"BaseEntry\")\r\nand TO_VARCHAR(opqt.\"DocNum\") =TO_VARCHAR( por1.\"BaseRef\") \r\ninner join \"UCATOLICA\".\"OPOR\" opor \r\non opor.\"DocEntry\" = por1.\"DocEntry\" \r\nleft join \"UCATOLICA\".\"PDN1\" pdn1 \r\non TO_VARCHAR(pdn1.\"BaseRef\")= TO_VARCHAR(opor.\"DocNum\")\r\nand TO_VARCHAR(pdn1.\"BaseEntry\") = TO_VARCHAR(opor.\"DocEntry\")\r\nleft join \"UCATOLICA\".\"OPDN\" opdn \r\non opdn.\"DocEntry\" = pdn1.\"DocEntry\" \r\ninner join \"UCATOLICA\".\"PCH1\" pch1 \r\non TO_VARCHAR(opor.\"DocEntry\") = TO_VARCHAR(pch1.\"BaseEntry\")\r\nand TO_VARCHAR(opor.\"DocEntry\") = TO_VARCHAR(pch1.\"BaseEntry\")\r\ninner join \"UCATOLICA\".\"OPCH\" opch \r\non opch.\"DocEntry\" = pch1.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"VPM2\" vpm2 \r\non opch.\"DocEntry\" = vpm2.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"OVPM\" ovpm \r\non vpm2.\"DocNum\" = ovpm.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"VPM1\" vpm1 \r\non vpm1.\"DocNum\" = ovpm.\"DocEntry\" \r\nleft outer join \"UCATOLICA\".\"OJDT\" ojdt \r\non ojdt.\"CreatedBy\" = ovpm.\"DocEntry\" \r\nand TO_VARCHAR(ovpm.\"DocNum\") = TO_VARCHAR(ojdt.\"BaseRef\")\r\nleft outer join \"UCATOLICA\".\"JDT1\" jdt1 \r\non jdt1.\"TransId\" = ojdt.\"TransId\" \r\nand TO_VARCHAR(ojdt.\"BaseRef\") = TO_VARCHAR(jdt1.\"BaseRef\")\r\nand ojdt.\"CreatedBy\" = jdt1.\"CreatedBy\" \r\nwhere oprq.\"DocEntry\" ="
                               + id +
                               " group by opch.\"DocNum\",\r\nopch.\"DocDate\",\r\nopch.\"CANCELED\",\r\nopch.\"DocEntry\",\r\nopch.\"CardName\"";

            var rawresult = _context.Database.SqlQuery<GeneralRelations>(queryProduct).ToList();

            var formatedData = rawresult.Select(x => new
            {
                x.estado,
                x.numero_factura,
                fecha_factura = x.fecha_factura.HasValue ? x.fecha_factura.Value.ToString("dd/MM/yyyy") : null,
                x.num_factura,
                x.proveedor
            });
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/pago/{id}")]
        public IHttpActionResult pago(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select\r\novpm.\"DocEntry\" as \"num_pago\",\r\novpm.\"DocNum\" as \"numero_pago\",\r\novpm.\"DocDate\" as \"fecha_pago\",\r\novpm.\"Canceled\" as \"estado\",\r\novpm.\"CardName\" as \"proveedor\"\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq \r\ninner join \"UCATOLICA\".\"PRQ1\" prq1 \r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"\r\ninner join ucatolica.\"NNM1\" f\r\non oprq.\"Series\" = f.\"Series\"\r\ninner join \"UCATOLICA\".\"PQT1\" pqt1 \r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\" \r\ninner join\"UCATOLICA\".\"OPQT\" opqt \r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\" \r\nand TO_VARCHAR(oprq.\"DocNum\") = TO_VARCHAR(pqt1.\"BaseRef\")\r\ninner join \"UCATOLICA\".\"POR1\" por1 \r\non TO_VARCHAR(opqt.\"DocEntry\") = TO_VARCHAR(por1.\"BaseEntry\")\r\nand TO_VARCHAR(opqt.\"DocNum\") =TO_VARCHAR( por1.\"BaseRef\") \r\ninner join \"UCATOLICA\".\"OPOR\" opor \r\non opor.\"DocEntry\" = por1.\"DocEntry\" \r\nleft join \"UCATOLICA\".\"PDN1\" pdn1 \r\non TO_VARCHAR(pdn1.\"BaseRef\")= TO_VARCHAR(opor.\"DocNum\")\r\nand TO_VARCHAR(pdn1.\"BaseEntry\") = TO_VARCHAR(opor.\"DocEntry\")\r\nleft join \"UCATOLICA\".\"OPDN\" opdn \r\non opdn.\"DocEntry\" = pdn1.\"DocEntry\" \r\ninner join \"UCATOLICA\".\"PCH1\" pch1 \r\non TO_VARCHAR(opor.\"DocEntry\") = TO_VARCHAR(pch1.\"BaseEntry\")\r\nand TO_VARCHAR(opor.\"DocEntry\") = TO_VARCHAR(pch1.\"BaseEntry\")\r\ninner join \"UCATOLICA\".\"OPCH\" opch \r\non opch.\"DocEntry\" = pch1.\"DocEntry\" \r\ninner join \"UCATOLICA\".\"VPM2\" vpm2 \r\non opch.\"DocEntry\" = vpm2.\"DocEntry\" \r\ninner join \"UCATOLICA\".\"OVPM\" ovpm \r\non vpm2.\"DocNum\" = ovpm.\"DocEntry\" \r\ninner join \"UCATOLICA\".\"VPM1\" vpm1 \r\non vpm1.\"DocNum\" = ovpm.\"DocEntry\" \r\ninner join \"UCATOLICA\".\"OJDT\" ojdt \r\non ojdt.\"CreatedBy\" = ovpm.\"DocEntry\" \r\nand TO_VARCHAR(ovpm.\"DocNum\") = TO_VARCHAR(ojdt.\"BaseRef\")\r\ninner join \"UCATOLICA\".\"JDT1\" jdt1 \r\non jdt1.\"TransId\" = ojdt.\"TransId\" \r\nand TO_VARCHAR(ojdt.\"BaseRef\") = TO_VARCHAR(jdt1.\"BaseRef\")\r\nand ojdt.\"CreatedBy\" = jdt1.\"CreatedBy\" \r\nwhere oprq.\"DocEntry\" ="
                               + id +
                               "  group by ovpm.\"DocNum\",\r\novpm.\"DocDate\",\r\novpm.\"Canceled\",\r\novpm.\"DocEntry\",\r\novpm.\"CardName\"";

            var rawresult = _context.Database.SqlQuery<GeneralRelations>(queryProduct).ToList();

            var formatedData = rawresult.Select(x => new
            {
                x.estado,
                x.numero_pago,
                fecha_pago = x.fecha_pago.HasValue ? x.fecha_pago.Value.ToString("dd/MM/yyyy") : null,
                x.num_pago,
                x.proveedor
            });
            return Ok(formatedData);
        }
    }
}