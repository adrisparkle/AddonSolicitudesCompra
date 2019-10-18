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
            var queryProduct = "select\r\nop.\"CardCode\" as \"codigo_proveedor\",\r\nop.\"CardName\" as \"proveedor\",\r\nop.\"PQTGrpSer\" as \"grupo_nombre\",\r\nop.\"PQTGrpNum\" as \"grupo_numero\",\r\nop.\"BPLName\" as \"regional\",\r\nf.\"SeriesName\" as \"serie\",\r\nop.\"U_UOrganiza\" as \"unidad_organizacional\",\r\nop.\"DocNum\" as \"numero_documento\",\r\nop.\"DocDate\" as \"fecha_contabilizacion\", \r\nop.\"DocDueDate\" as \"fecha_valida\", \r\nop.\"TaxDate\" as \"fecha_documento\", \r\nop.\"ReqDate\" as \"fecha_necesaria\"\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq\r\ninner join \"UCATOLICA\".\"PRQ1\" prq1\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"PQT1\" pqt1\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"\r\nleft outer join\"UCATOLICA\".\"OPQT\" op\r\non op.\"DocEntry\" = pqt1.\"DocEntry\"\r\nleft join ucatolica.\"NNM1\" f\r\non op.\"Series\" = f.\"Series\"\r\nand oprq.\"DocNum\" = pqt1.\"BaseRef\"\r\nwhere oprq.\"DocNum\" = "+id;
            var rawresult = _context.Database.SqlQuery<PurchaseQuotation>(queryProduct).ToList();
            var formatedData = rawresult.Select(x => new
            {
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

            });
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/PurchaseQuotationDetail/{id}")]
        public IHttpActionResult PurchaseQuotationDetail(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct =
                "select\r\npqt1.\"ItemCode\" as \"codigo\",\r\npqt1.\"Dscription\" as \"descripcion\",\r\npqt1.\"FreeTxt\" as \"observaciones\",\r\npqt1.\"PQTReqDate\" as \"fecha_necesaria\",\r\npqt1.\"PQTReqQty\" as \"cantidad\",\r\npqt1.\"TaxCode\" as \"impuesto\",\r\npqt1.\"WhsCode\" as \"almacen\",\r\npqt1.\"Project\" as \"proyecto\",\r\npqt1.\"PriceAfVAT\" as \"precio_unitario\",\r\npqt1.\"GTotal\" as \"total\",\r\npqt1.\"OcrCode\" as \"unidad_organizacional\",\r\npqt1.\"OcrCode2\" as \"pei_po\"\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq\r\ninner join \"UCATOLICA\".\"PRQ1\" prq1\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"PQT1\" pqt1\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"\r\nleft outer join\"UCATOLICA\".\"OPQT\" op\r\non op.\"DocEntry\" = pqt1.\"DocEntry\"\r\nleft join ucatolica.\"NNM1\" f\r\non op.\"Series\" = f.\"Series\"\r\nand oprq.\"DocNum\" = pqt1.\"BaseRef\"\r\nwhere oprq.\"DocNum\" = " +
                id;

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
        /*--query para detalle de solicitud de compra
         *
         * select
prq1."ItemCode" as "codigo",
prq1."Dscription" as "descripcion",
prq1."FreeTxt" as "observaciones",
prq1."LineVendor" as "proveedor",
prq1."PQTReqDate" as "fecha_necesaria",
prq1."Quantity" as "cantidad",
prq1."Project" as "proyecto",
prq1."WhsCode" as "almacen",
prq1."TaxCode" as "impuesto",
prq1."PriceAfVAT" as "precio_unitario",
prq1."GTotal" as "total",
prq1."OcrCode" as "unidad_organizacional",
prq1."OcrCode2" as "pei_po"
from "UCATOLICA"."OPRQ" oprq
inner join "UCATOLICA"."PRQ1" prq1
on oprq."DocEntry" = prq1."DocEntry"
where oprq."DocNum" = 3000527
         */
    }
}
