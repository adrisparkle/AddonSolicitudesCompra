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
            var query = "select \r\nop.\"DocNum\" as \"id\"," +
                        " \r\nop.\"ReqName\" as \"solicitante\"," +
                        "\r\nop.\"DocDate\" as \"fecha_contabilizacion\"," +
                        " \r\nop.\"BPLName\" as \"regional\"," +
                        "\r\nop.\"U_UOrganiza\" as \"unidad_organizacional\" " +
                        "\r\nfrom \"UCATOLICA\".\"OPRQ\" op" +
                        "\r\nleft join ucatolica.\"NNM1\" f" +
                        "\r\non op.\"Series\" = f.\"Series\"" +
                        "\r\norder by op.\"DocDate\" desc";
            var rawresult = _context.Database.SqlQuery<PurchaseSearch>(query).ToList();
            var formatedData = rawresult.Select(x => new
            {
                x.id,
                x.solicitante,
                fecha_contabilizacion = x.fecha_contabilizacion.ToString("dd/MM/yyyy"),
                x.regional,
                x.unidad_organizacional
            });

            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/PurchaseRequest/{id}")]
        public IHttpActionResult PurchaseRequest(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select \r\nop.\"DocNum\" as \"id\", \r\nop.\"Requester\" as " +
                               "\"codigo_solicitante\", \r\nop.\"ReqName\" as \"solicitante\", " +
                               "\r\nf.\"SeriesName\" as \"serie\", \r\nop.\"BPLName\" as \"regional\"," +
                               "\r\nop.\"U_UOrganiza\" as \"unidad_organizacional\", " +
                               "\r\nop.\"DocDate\" as \"fecha_contabilizacion\", " +
                               "\r\nop.\"DocDueDate\" as \"fecha_valida\", \r\nop.\"TaxDate\" as " +
                               "\"fecha_documento\", \r\nop.\"ReqDate\" as \"fecha_requerida\"" +
                               "\r\nfrom \"UCATOLICA\".\"OPRQ\" op\r\nleft join ucatolica.\"NNM1\" " +
                               "f\r\non op.\"Series\" = f.\"Series\"\r\nwhere op.\"DocNum\" = " + id
                + " group by op.\"DocNum\", \r\nop.\"Requester\", \r\nop.\"ReqName\", " +
                               "\r\nf.\"SeriesName\", \r\nop.\"BPLName\"," +
                               "\r\nop.\"U_UOrganiza\", " +
                               "\r\nop.\"DocDate\", " +
                               "\r\nop.\"DocDueDate\", \r\nop.\"TaxDate\"," +
                               " \r\nop.\"ReqDate\"";
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

            });
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/PurchaseRelations/{id}")]
        public IHttpActionResult PurchaseRelations(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select \r\noprq.\"DocNum\" as \"numero_solicitud\", \r\noprq.\"DocDate\" as \"fecha_solicitud\", \r\nopqt.\"DocNum\" as \"numero_oferta\", \r\nopqt.\"DocDate\" as \"fecha_oferta\",\r\nopor.\"DocNum\" as \"numero_pedido\", \r\nopor.\"DocDate\" as \"fecha_pedido\",\r\nopdn.\"DocNum\" as \"numero_mercancia\", \r\nopdn.\"DocDate\" as \"fecha_mercancia\",\r\nopch.\"DocNum\" as \"numero_factura\", \r\nopch.\"DocDate\" as \"fecha_factura\",\r\novpm.\"DocNum\" as \"numero_pago\", \r\novpm.\"DocDate\" as \"fecha_pago\"\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq\r\ninner join \"UCATOLICA\".\"PRQ1\" prq1\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"PQT1\" pqt1\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"\r\nleft outer join\"UCATOLICA\".\"OPQT\" opqt\r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\"\r\nand oprq.\"DocNum\" = pqt1.\"BaseRef\"\r\nleft outer join \"UCATOLICA\".\"POR1\" por1\r\non opqt.\"DocEntry\" = por1.\"BaseEntry\"\r\nand opqt.\"DocNum\" = por1.\"BaseRef\"\r\nleft outer join \"UCATOLICA\".\"OPOR\" opor\r\non opor.\"DocEntry\" = por1.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"PDN1\" pdn1\r\non pdn1.\"BaseRef\" = opor.\"DocNum\"\r\nand pdn1.\"BaseEntry\" = opor.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"OPDN\" opdn\r\non opdn.\"DocEntry\" = pdn1.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"PCH1\" pch1\r\non opor.\"DocEntry\" = pch1.\"BaseEntry\"\r\nand opor.\"DocEntry\" = pch1.\"BaseEntry\"\r\nleft outer join \"UCATOLICA\".\"OPCH\" opch\r\non opch.\"DocEntry\" = pch1.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"VPM2\" vpm2\r\non opch.\"DocEntry\" = vpm2.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"OVPM\" ovpm\r\non vpm2.\"DocNum\" = ovpm.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"VPM1\" vpm1\r\non vpm1.\"DocNum\" = ovpm.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"OJDT\" ojdt\r\non ojdt.\"CreatedBy\" = ovpm.\"DocEntry\"\r\nand ovpm.\"DocNum\" = ojdt.\"BaseRef\"\r\nleft outer join \"UCATOLICA\".\"JDT1\" jdt1\r\non jdt1.\"TransId\" = ojdt.\"TransId\"\r\nand ojdt.\"BaseRef\" = jdt1.\"BaseRef\"\r\nand ojdt.\"CreatedBy\" = jdt1.\"CreatedBy\"\r\nwhere oprq.\"DocNum\" = " 
                               + id + 
                               " group by oprq.\"DocNum\", \r\noprq.\"DocDate\", \r\nopqt.\"DocNum\", \r\nopqt.\"DocDate\",\r\nopor.\"DocNum\", \r\nopor.\"DocDate\",\r\nopdn.\"DocNum\", \r\nopdn.\"DocDate\",\r\nopch.\"DocNum\", \r\nopch.\"DocDate\",\r\novpm.\"DocNum\", \r\novpm.\"DocDate\"";

            var rawresult = _context.Database.SqlQuery<GeneralRelations>(queryProduct).ToList();
            
            var formatedData = rawresult.Select(x => new
            {
                x.numero_solicitud,
                fecha_solicitud = x.fecha_solicitud.ToString("dd/MM/yyyy"),
                x.numero_oferta,
                fecha_oferta = x.fecha_oferta.HasValue ? x.fecha_oferta.Value.ToString("dd/MM/yyyy") : string.Empty,
                x.numero_pedido,
                fecha_pedido = x.fecha_pedido.HasValue ? x.fecha_pedido.Value.ToString("dd/MM/yyyy") : string.Empty,
                x.numero_mercancia,
                fecha_mercancia = x.fecha_mercancia.HasValue ? x.fecha_mercancia.Value.ToString("dd/MM/yyyy") : string.Empty,
                x.numero_factura,
                fecha_factura = x.fecha_factura.HasValue ? x.fecha_factura.Value.ToString("dd/MM/yyyy") : string.Empty,
                x.numero_pago,
                fecha_pago = x.fecha_pago.HasValue ? x.fecha_pago.Value.ToString("dd/MM/yyyy") : string.Empty,

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
                               "\r\nwhere oprq.\"DocNum\" = "+ id
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
            var queryProduct = "select " +
                               "\r\noprq.\"DocNum\" as \"numero_solicitud\"," +
                               "\r\noprq.\"DocDate\" as \"fecha_solicitud\"" +
                               "from \"UCATOLICA\".\"OPRQ\" oprq\r\ninner join \"UCATOLICA\".\"PRQ1\" prq1\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"PQT1\" pqt1\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"\r\nleft outer join\"UCATOLICA\".\"OPQT\" opqt\r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\"\r\nand oprq.\"DocNum\" = pqt1.\"BaseRef\"\r\nleft outer join \"UCATOLICA\".\"POR1\" por1\r\non opqt.\"DocEntry\" = por1.\"BaseEntry\"\r\nand opqt.\"DocNum\" = por1.\"BaseRef\"\r\nleft outer join \"UCATOLICA\".\"OPOR\" opor\r\non opor.\"DocEntry\" = por1.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"PDN1\" pdn1\r\non pdn1.\"BaseRef\" = opor.\"DocNum\"\r\nand pdn1.\"BaseEntry\" = opor.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"OPDN\" opdn\r\non opdn.\"DocEntry\" = pdn1.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"PCH1\" pch1\r\non opor.\"DocEntry\" = pch1.\"BaseEntry\"\r\nand opor.\"DocEntry\" = pch1.\"BaseEntry\"\r\nleft outer join \"UCATOLICA\".\"OPCH\" opch\r\non opch.\"DocEntry\" = pch1.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"VPM2\" vpm2\r\non opch.\"DocEntry\" = vpm2.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"OVPM\" ovpm\r\non vpm2.\"DocNum\" = ovpm.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"VPM1\" vpm1\r\non vpm1.\"DocNum\" = ovpm.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"OJDT\" ojdt\r\non ojdt.\"CreatedBy\" = ovpm.\"DocEntry\"\r\nand ovpm.\"DocNum\" = ojdt.\"BaseRef\"\r\nleft outer join \"UCATOLICA\".\"JDT1\" jdt1\r\non jdt1.\"TransId\" = ojdt.\"TransId\"\r\nand ojdt.\"BaseRef\" = jdt1.\"BaseRef\"\r\nand ojdt.\"CreatedBy\" = jdt1.\"CreatedBy\"\r\nwhere oprq.\"DocNum\" = "
                               + id +
                               " group by oprq.\"DocNum\"," +
                               "\r\noprq.\"DocDate\"";

            var rawresult = _context.Database.SqlQuery<GeneralRelations>(queryProduct).ToList();

            var formatedData = rawresult.Select(x => new
            {
                x.numero_solicitud,
                fecha_solicitud = x.fecha_solicitud.ToString("dd/MM/yyyy"),
            });
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/oferta/{id}")]
        public IHttpActionResult oferta(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select \r\noprq.\"DocNum\" as \"numero_solicitud\"," +
                               "\r\nopqt.\"DocNum\" as \"numero_oferta\", " +
                               "\r\nopqt.\"DocDate\" as \"fecha_oferta\"" +
                               "\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq" +
                               "\r\ninner join \"UCATOLICA\".\"PRQ1\" prq1" +
                               "\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"" +
                               "\r\ninner join \"UCATOLICA\".\"PQT1\" pqt1" +
                               "\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"" +
                               "\r\ninner join\"UCATOLICA\".\"OPQT\" opqt" +
                               "\r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\"" +
                               "\r\nand oprq.\"DocNum\" = pqt1.\"BaseRef\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"POR1\" por1" +
                               "\r\non opqt.\"DocEntry\" = por1.\"BaseEntry\"" +
                               "\r\nand opqt.\"DocNum\" = por1.\"BaseRef\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"OPOR\" opor" +
                               "\r\non opor.\"DocEntry\" = por1.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"PDN1\" pdn1" +
                               "\r\non pdn1.\"BaseRef\" = opor.\"DocNum\"" +
                               "\r\nand pdn1.\"BaseEntry\" = opor.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"OPDN\" opdn" +
                               "\r\non opdn.\"DocEntry\" = pdn1.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"PCH1\" pch1" +
                               "\r\non opor.\"DocEntry\" = pch1.\"BaseEntry\"" +
                               "\r\nand opor.\"DocEntry\" = pch1.\"BaseEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"OPCH\" opch" +
                               "\r\non opch.\"DocEntry\" = pch1.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"VPM2\" vpm2" +
                               "\r\non opch.\"DocEntry\" = vpm2.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"OVPM\" ovpm" +
                               "\r\non vpm2.\"DocNum\" = ovpm.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"VPM1\" vpm1" +
                               "\r\non vpm1.\"DocNum\" = ovpm.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"OJDT\" ojdt" +
                               "\r\non ojdt.\"CreatedBy\" = ovpm.\"DocEntry\"" +
                               "\r\nand ovpm.\"DocNum\" = ojdt.\"BaseRef\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"JDT1\" jdt1" +
                               "\r\non jdt1.\"TransId\" = ojdt.\"TransId\"" +
                               "\r\nand ojdt.\"BaseRef\" = jdt1.\"BaseRef\"" +
                               "\r\nand ojdt.\"CreatedBy\" = jdt1.\"CreatedBy\"" +
                               "\r\nwhere oprq.\"DocNum\" = "
                               + id +
                               " group by oprq.\"DocNum\", " +
                               "\r\nopqt.\"DocNum\"," +
                               " \r\nopqt.\"DocDate\"\r\n";

            var rawresult = _context.Database.SqlQuery<GeneralRelations>(queryProduct).ToList();

            var formatedData = rawresult.Select(x => new
            {
                x.numero_solicitud,
               // fecha_solicitud = x.fecha_solicitud.ToString("dd/MM/yyyy"),
                x.numero_oferta,
                fecha_oferta = x.fecha_oferta.HasValue ? x.fecha_oferta.Value.ToString("dd/MM/yyyy") : string.Empty,
               // x.numero_pedido,
                //fecha_pedido = x.fecha_pedido.HasValue ? x.fecha_pedido.Value.ToString("dd/MM/yyyy") : string.Empty,
                //x.numero_mercancia,
                //fecha_mercancia = x.fecha_mercancia.HasValue ? x.fecha_mercancia.Value.ToString("dd/MM/yyyy") : string.Empty,
                //x.numero_factura,
                //fecha_factura = x.fecha_factura.HasValue ? x.fecha_factura.Value.ToString("dd/MM/yyyy") : string.Empty,
                //x.numero_pago,
                //fecha_pago = x.fecha_pago.HasValue ? x.fecha_pago.Value.ToString("dd/MM/yyyy") : string.Empty,
            });
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/pedido/{id}")]
        public IHttpActionResult pedido(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select \r\noprq.\"DocNum\" as \"numero_solicitud\"," +
                               "\r\nopor.\"DocNum\" as \"numero_pedido\"," +
                               "\r\nopor.\"DocDate\" as \"fecha_pedido\"" +
                               "\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq" +
                               "\r\ninner join \"UCATOLICA\".\"PRQ1\" prq1" +
                               "\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"" +
                               "\r\ninner join \"UCATOLICA\".\"PQT1\" pqt1" +
                               "\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"" +
                               "\r\ninner join\"UCATOLICA\".\"OPQT\" opqt" +
                               "\r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\"" +
                               "\r\nand oprq.\"DocNum\" = pqt1.\"BaseRef\"" +
                               "\r\ninner join \"UCATOLICA\".\"POR1\" por1" +
                               "\r\non opqt.\"DocEntry\" = por1.\"BaseEntry\"" +
                               "\r\nand opqt.\"DocNum\" = por1.\"BaseRef\"" +
                               "\r\ninner join \"UCATOLICA\".\"OPOR\" opor" +
                               "\r\non opor.\"DocEntry\" = por1.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"PDN1\" pdn1" +
                               "\r\non pdn1.\"BaseRef\" = opor.\"DocNum\"" +
                               "\r\nand pdn1.\"BaseEntry\" = opor.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"OPDN\" opdn" +
                               "\r\non opdn.\"DocEntry\" = pdn1.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"PCH1\" pch1" +
                               "\r\non opor.\"DocEntry\" = pch1.\"BaseEntry\"" +
                               "\r\nand opor.\"DocEntry\" = pch1.\"BaseEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"OPCH\" opch" +
                               "\r\non opch.\"DocEntry\" = pch1.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"VPM2\" vpm2" +
                               "\r\non opch.\"DocEntry\" = vpm2.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"OVPM\" ovpm" +
                               "\r\non vpm2.\"DocNum\" = ovpm.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"VPM1\" vpm1" +
                               "\r\non vpm1.\"DocNum\" = ovpm.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"OJDT\" ojdt" +
                               "\r\non ojdt.\"CreatedBy\" = ovpm.\"DocEntry\"" +
                               "\r\nand ovpm.\"DocNum\" = ojdt.\"BaseRef\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"JDT1\" jdt1" +
                               "\r\non jdt1.\"TransId\" = ojdt.\"TransId\"" +
                               "\r\nand ojdt.\"BaseRef\" = jdt1.\"BaseRef\"" +
                               "\r\nand ojdt.\"CreatedBy\" = jdt1.\"CreatedBy\"" +
                               "\r\nwhere oprq.\"DocNum\" = "
                               + id +
                               " group by oprq.\"DocNum\"," +
                               "\r\noprq.\"DocDate\"," +
                               "\r\nopor.\"DocNum\"," +
                               "\r\nopor.\"DocDate\"";

            var rawresult = _context.Database.SqlQuery<GeneralRelations>(queryProduct).ToList();

            var formatedData = rawresult.Select(x => new
            {
                x.numero_solicitud,
                // fecha_solicitud = x.fecha_solicitud.ToString("dd/MM/yyyy"),
                //x.numero_oferta,
                //fecha_oferta = x.fecha_oferta.HasValue ? x.fecha_oferta.Value.ToString("dd/MM/yyyy") : string.Empty,
                x.numero_pedido,
                fecha_pedido = x.fecha_pedido.HasValue ? x.fecha_pedido.Value.ToString("dd/MM/yyyy") : string.Empty,
                //x.numero_mercancia,
                //fecha_mercancia = x.fecha_mercancia.HasValue ? x.fecha_mercancia.Value.ToString("dd/MM/yyyy") : string.Empty,
                //x.numero_factura,
                //fecha_factura = x.fecha_factura.HasValue ? x.fecha_factura.Value.ToString("dd/MM/yyyy") : string.Empty,
                //x.numero_pago,
                //fecha_pago = x.fecha_pago.HasValue ? x.fecha_pago.Value.ToString("dd/MM/yyyy") : string.Empty,
            });
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/mercancia/{id}")]
        public IHttpActionResult mercancia(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select \r\noprq.\"DocNum\" as \"numero_solicitud\"," +
                               "\r\nopdn.\"DocNum\" as \"numero_mercancia\"," +
                               "\r\nopdn.\"DocDate\" as \"fecha_mercancia\"" +
                               "\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq" +
                               "\r\ninner join \"UCATOLICA\".\"PRQ1\" prq1" +
                               "\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"" +
                               "\r\ninner join \"UCATOLICA\".\"PQT1\" pqt1" +
                               "\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"" +
                               "\r\ninner join\"UCATOLICA\".\"OPQT\" opqt" +
                               "\r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\"" +
                               "\r\nand oprq.\"DocNum\" = pqt1.\"BaseRef\"" +
                               "\r\ninner join \"UCATOLICA\".\"POR1\" por1" +
                               "\r\non opqt.\"DocEntry\" = por1.\"BaseEntry\"" +
                               "\r\nand opqt.\"DocNum\" = por1.\"BaseRef\"" +
                               "\r\ninner join \"UCATOLICA\".\"OPOR\" opor" +
                               "\r\non opor.\"DocEntry\" = por1.\"DocEntry\"" +
                               "\r\ninner join \"UCATOLICA\".\"PDN1\" pdn1" +
                               "\r\non pdn1.\"BaseRef\" = opor.\"DocNum\"" +
                               "\r\nand pdn1.\"BaseEntry\" = opor.\"DocEntry\"" +
                               "\r\ninner join \"UCATOLICA\".\"OPDN\" opdn" +
                               "\r\non opdn.\"DocEntry\" = pdn1.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"PCH1\" pch1" +
                               "\r\non opor.\"DocEntry\" = pch1.\"BaseEntry\"" +
                               "\r\nand opor.\"DocEntry\" = pch1.\"BaseEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"OPCH\" opch" +
                               "\r\non opch.\"DocEntry\" = pch1.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"VPM2\" vpm2" +
                               "\r\non opch.\"DocEntry\" = vpm2.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"OVPM\" ovpm" +
                               "\r\non vpm2.\"DocNum\" = ovpm.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"VPM1\" vpm1" +
                               "\r\non vpm1.\"DocNum\" = ovpm.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"OJDT\" ojdt" +
                               "\r\non ojdt.\"CreatedBy\" = ovpm.\"DocEntry\"" +
                               "\r\nand ovpm.\"DocNum\" = ojdt.\"BaseRef\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"JDT1\" jdt1" +
                               "\r\non jdt1.\"TransId\" = ojdt.\"TransId\"" +
                               "\r\nand ojdt.\"BaseRef\" = jdt1.\"BaseRef\"" +
                               "\r\nand ojdt.\"CreatedBy\" = jdt1.\"CreatedBy\"" +
                               "\r\nwhere oprq.\"DocNum\" = "
                               + id +
                               " group by oprq.\"DocNum\"," +
                               "\r\noprq.\"DocDate\"," +
                               "\r\nopdn.\"DocNum\"," +
                               "\r\nopdn.\"DocDate\"";

            var rawresult = _context.Database.SqlQuery<GeneralRelations>(queryProduct).ToList();

            var formatedData = rawresult.Select(x => new
            {
                x.numero_solicitud,
                // fecha_solicitud = x.fecha_solicitud.ToString("dd/MM/yyyy"),
                //x.numero_oferta,
                //fecha_oferta = x.fecha_oferta.HasValue ? x.fecha_oferta.Value.ToString("dd/MM/yyyy") : string.Empty,
               // x.numero_pedido,
               // fecha_pedido = x.fecha_pedido.HasValue ? x.fecha_pedido.Value.ToString("dd/MM/yyyy") : string.Empty,
                x.numero_mercancia,
                fecha_mercancia = x.fecha_mercancia.HasValue ? x.fecha_mercancia.Value.ToString("dd/MM/yyyy") : string.Empty,
                //x.numero_factura,
                //fecha_factura = x.fecha_factura.HasValue ? x.fecha_factura.Value.ToString("dd/MM/yyyy") : string.Empty,
                //x.numero_pago,
                //fecha_pago = x.fecha_pago.HasValue ? x.fecha_pago.Value.ToString("dd/MM/yyyy") : string.Empty,
            });
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/factura/{id}")]
        public IHttpActionResult factura(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select \r\noprq.\"DocNum\" as \"numero_solicitud\", " +
                               "\r\nopch.\"DocNum\" as \"numero_factura\"," +
                               " \r\nopch.\"DocDate\" as \"fecha_factura\"" +
                               "\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq" +
                               "\r\ninner join \"UCATOLICA\".\"PRQ1\" prq1" +
                               "\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"" +
                               "\r\ninner join \"UCATOLICA\".\"PQT1\" pqt1" +
                               "\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"" +
                               "\r\ninner join\"UCATOLICA\".\"OPQT\" opqt" +
                               "\r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\"" +
                               "\r\nand oprq.\"DocNum\" = pqt1.\"BaseRef\"" +
                               "\r\ninner join \"UCATOLICA\".\"POR1\" por1" +
                               "\r\non opqt.\"DocEntry\" = por1.\"BaseEntry\"" +
                               "\r\nand opqt.\"DocNum\" = por1.\"BaseRef\"" +
                               "\r\ninner join \"UCATOLICA\".\"OPOR\" opor\r" +
                               "\non opor.\"DocEntry\" = por1.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"PDN1\" pdn1" +
                               "\r\non pdn1.\"BaseRef\" = opor.\"DocNum\"" +
                               "\r\nand pdn1.\"BaseEntry\" = opor.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"OPDN\" opdn" +
                               "\r\non opdn.\"DocEntry\" = pdn1.\"DocEntry\"" +
                               "\r\ninner join \"UCATOLICA\".\"PCH1\" pch1" +
                               "\r\non opor.\"DocEntry\" = pch1.\"BaseEntry\"" +
                               "\r\nand opor.\"DocEntry\" = pch1.\"BaseEntry\"" +
                               "\r\ninner join \"UCATOLICA\".\"OPCH\" opch" +
                               "\r\non opch.\"DocEntry\" = pch1.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"VPM2\" vpm2" +
                               "\r\non opch.\"DocEntry\" = vpm2.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"OVPM\" ovpm" +
                               "\r\non vpm2.\"DocNum\" = ovpm.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"VPM1\" vpm1" +
                               "\r\non vpm1.\"DocNum\" = ovpm.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"OJDT\" ojdt" +
                               "\r\non ojdt.\"CreatedBy\" = ovpm.\"DocEntry\"" +
                               "\r\nand ovpm.\"DocNum\" = ojdt.\"BaseRef\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"JDT1\" jdt1" +
                               "\r\non jdt1.\"TransId\" = ojdt.\"TransId\"" +
                               "\r\nand ojdt.\"BaseRef\" = jdt1.\"BaseRef\"" +
                               "\r\nand ojdt.\"CreatedBy\" = jdt1.\"CreatedBy\"" +
                               "\r\nwhere oprq.\"DocNum\" = "
                               + id +
                               " group by oprq.\"DocNum\"," +
                               "\r\noprq.\"DocDate\"," +
                               "\r\nopch.\"DocNum\"," +
                               "\r\nopch.\"DocDate\"";

            var rawresult = _context.Database.SqlQuery<GeneralRelations>(queryProduct).ToList();

            var formatedData = rawresult.Select(x => new
            {
                x.numero_solicitud,
                // fecha_solicitud = x.fecha_solicitud.ToString("dd/MM/yyyy"),
                //x.numero_oferta,
                //fecha_oferta = x.fecha_oferta.HasValue ? x.fecha_oferta.Value.ToString("dd/MM/yyyy") : string.Empty,
                // x.numero_pedido,
                // fecha_pedido = x.fecha_pedido.HasValue ? x.fecha_pedido.Value.ToString("dd/MM/yyyy") : string.Empty,
                // x.numero_mercancia,
                // fecha_mercancia = x.fecha_mercancia.HasValue ? x.fecha_mercancia.Value.ToString("dd/MM/yyyy") : string.Empty,
                x.numero_factura,
                fecha_factura = x.fecha_factura.HasValue ? x.fecha_factura.Value.ToString("dd/MM/yyyy") : string.Empty,
                //x.numero_pago,
                //fecha_pago = x.fecha_pago.HasValue ? x.fecha_pago.Value.ToString("dd/MM/yyyy") : string.Empty,
            });
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/pago/{id}")]
        public IHttpActionResult pago(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select \r\noprq.\"DocNum\" as \"numero_solicitud\", " +
                               "\r\novpm.\"DocNum\" as \"numero_pago\"," +
                               " \r\novpm.\"DocDate\" as \"fecha_pago\"" +
                               "\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq" +
                               "\r\ninner join \"UCATOLICA\".\"PRQ1\" prq1" +
                               "\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"" +
                               "\r\ninner join \"UCATOLICA\".\"PQT1\" pqt1" +
                               "\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"" +
                               "\r\ninner join\"UCATOLICA\".\"OPQT\" opqt" +
                               "\r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\"" +
                               "\r\nand oprq.\"DocNum\" = pqt1.\"BaseRef\"" +
                               "\r\ninner join \"UCATOLICA\".\"POR1\" por1" +
                               "\r\non opqt.\"DocEntry\" = por1.\"BaseEntry\"" +
                               "\r\nand opqt.\"DocNum\" = por1.\"BaseRef\"" +
                               "\r\ninner join \"UCATOLICA\".\"OPOR\" opor" +
                               "\r\non opor.\"DocEntry\" = por1.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"PDN1\" pdn1" +
                               "\r\non pdn1.\"BaseRef\" = opor.\"DocNum\"" +
                               "\r\nand pdn1.\"BaseEntry\" = opor.\"DocEntry\"" +
                               "\r\nleft outer join \"UCATOLICA\".\"OPDN\" opdn" +
                               "\r\non opdn.\"DocEntry\" = pdn1.\"DocEntry\"" +
                               "\r\ninner join \"UCATOLICA\".\"PCH1\" pch1\r" +
                               "\non opor.\"DocEntry\" = pch1.\"BaseEntry\"" +
                               "\r\nand opor.\"DocEntry\" = pch1.\"BaseEntry\"" +
                               "\r\ninner join \"UCATOLICA\".\"OPCH\" opch" +
                               "\r\non opch.\"DocEntry\" = pch1.\"DocEntry\"" +
                               "\r\ninner join \"UCATOLICA\".\"VPM2\" vpm2" +
                               "\r\non opch.\"DocEntry\" = vpm2.\"DocEntry\"" +
                               "\r\ninner join \"UCATOLICA\".\"OVPM\" ovpm" +
                               "\r\non vpm2.\"DocNum\" = ovpm.\"DocEntry\"" +
                               "\r\ninner join \"UCATOLICA\".\"VPM1\" vpm1" +
                               "\r\non vpm1.\"DocNum\" = ovpm.\"DocEntry\"" +
                               "\r\ninner join \"UCATOLICA\".\"OJDT\" ojdt" +
                               "\r\non ojdt.\"CreatedBy\" = ovpm.\"DocEntry\"" +
                               "\r\nand ovpm.\"DocNum\" = ojdt.\"BaseRef\"" +
                               "\r\ninner join \"UCATOLICA\".\"JDT1\" jdt1" +
                               "\r\non jdt1.\"TransId\" = ojdt.\"TransId\"" +
                               "\r\nand ojdt.\"BaseRef\" = jdt1.\"BaseRef\"" +
                               "\r\nand ojdt.\"CreatedBy\" = jdt1.\"CreatedBy\"" +
                               "\r\nwhere oprq.\"DocNum\" = "
                               + id +
                               " group by oprq.\"DocNum\"," +
                               "\r\noprq.\"DocDate\"," +
                               "\r\novpm.\"DocNum\"," +
                               "\r\novpm.\"DocDate\"";

            var rawresult = _context.Database.SqlQuery<GeneralRelations>(queryProduct).ToList();

            var formatedData = rawresult.Select(x => new
            {
                x.numero_solicitud,
                // fecha_solicitud = x.fecha_solicitud.ToString("dd/MM/yyyy"),
                //x.numero_oferta,
                //fecha_oferta = x.fecha_oferta.HasValue ? x.fecha_oferta.Value.ToString("dd/MM/yyyy") : string.Empty,
                // x.numero_pedido,
                // fecha_pedido = x.fecha_pedido.HasValue ? x.fecha_pedido.Value.ToString("dd/MM/yyyy") : string.Empty,
                // x.numero_mercancia,
                // fecha_mercancia = x.fecha_mercancia.HasValue ? x.fecha_mercancia.Value.ToString("dd/MM/yyyy") : string.Empty,
                //x.numero_factura,
                //fecha_factura = x.fecha_factura.HasValue ? x.fecha_factura.Value.ToString("dd/MM/yyyy") : string.Empty,
                x.numero_pago,
                fecha_pago = x.fecha_pago.HasValue ? x.fecha_pago.Value.ToString("dd/MM/yyyy") : string.Empty,
            });
            return Ok(formatedData);
        }
    }
}
