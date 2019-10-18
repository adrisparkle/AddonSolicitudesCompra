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
    public class FacturaController : ApiController
    {
        private ApplicationDbContext _context;


        public FacturaController()
        {

            _context = new ApplicationDbContext();
     
        }

        // GET api/Contract
        [HttpGet]
        [Route("api/PurchaseCheck/{id}")]
        public IHttpActionResult PurchaseCheck(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select \r\nopor.\"CardCode\" as \"codigo_proveedor\",\r\nopor.\"CardName\" as \"proveedor\",\r\nopor.\"BPLName\" as \"regional\",\r\nf.\"SeriesName\" as \"serie\",\r\nopor.\"U_UOrganiza\" as \"unidad_organizacional\",\r\nopor.\"DocNum\" as \"numero_documento\",\r\nopor.\"DocDate\" as \"fecha_contabilizacion\", \r\nopor.\"DocDueDate\" as \"fecha_valida\", \r\nopor.\"TaxDate\" as \"fecha_documento\"\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq\r\ninner join \"UCATOLICA\".\"PRQ1\" prq1\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"PQT1\" pqt1\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"\r\nleft outer join\"UCATOLICA\".\"OPQT\" opqt\r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\"\r\nand oprq.\"DocNum\" = pqt1.\"BaseRef\"\r\nleft outer join \"UCATOLICA\".\"POR1\" por1\r\non opqt.\"DocEntry\" = por1.\"BaseEntry\"\r\nand opqt.\"DocNum\" = por1.\"BaseRef\"\r\nleft outer join \"UCATOLICA\".\"OPOR\" opor\r\non opor.\"DocEntry\" = por1.\"DocEntry\"\r\nleft join ucatolica.\"NNM1\" f\r\non opor.\"Series\" = f.\"Series\"\r\nwhere oprq.\"DocNum\" = " + id;
            var rawresult = _context.Database.SqlQuery<PurchaseCheck>(queryProduct).ToList();
            var formatedData = rawresult.Select(x => new
            {
                x.codigo_proveedor,
                x.proveedor,
                x.numero_documento,
                x.numero_factura,
                x.serie,
                x.unidad_organizacional,
                x.regional,
                fecha_contabilizacion = x.fecha_contabilizacion.ToString("dd/MM/yyyy"),
                fecha_entrega = x.fecha_entrega.ToString("dd/MM/yyyy"),
                fecha_documento = x.fecha_documento.ToString("dd/MM/yyyy"),

            });
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/PurchaseCheckDetail/{id}")]
        public IHttpActionResult PurchaseCheckDetail(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select \r\npch1.\"ItemCode\" as \"codigo\",\r\npch1.\"Dscription\" as \"descripcion\",\r\npch1.\"Quantity\" as \"cantidad\",\r\npch1.\"TaxCode\" as \"impuesto\",\r\npch1.\"WhsCode\" as \"almacen\",\r\npch1.\"Project\" as \"proyecto\",\r\npch1.\"PriceAfVAT\" as \"precio_unitario\",\r\npch1.\"GTotal\" as \"total\",\r\npch1.\"OcrCode\" as \"unidad_organizacional\",\r\npch1.\"OcrCode2\" as \"pei_po\",\r\npch1.\"WtLiable\" as \"sujeto_a_retencion\",\r\npch1.\"U_WTAX\" as \"retencion_a_aplicar\"\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq\r\ninner join \"UCATOLICA\".\"PRQ1\" prq1\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"PQT1\" pqt1\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"\r\nleft outer join\"UCATOLICA\".\"OPQT\" opqt\r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\"\r\nand oprq.\"DocNum\" = pqt1.\"BaseRef\"\r\nleft outer join \"UCATOLICA\".\"POR1\" por1\r\non opqt.\"DocEntry\" = por1.\"BaseEntry\"\r\nand opqt.\"DocNum\" = por1.\"BaseRef\"\r\nleft outer join \"UCATOLICA\".\"OPOR\" opor\r\non opor.\"DocEntry\" = por1.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"PCH1\" pch1\r\non opor.\"DocEntry\" = pch1.\"BaseEntry\"\r\nand opor.\"DocEntry\" = pch1.\"BaseEntry\"\r\nleft outer join \"UCATOLICA\".\"OPCH\" op\r\non op.\"DocEntry\" = pch1.\"DocEntry\"\r\nwhere oprq.\"DocNum\" = "+id;

            var rawresult = _context.Database.SqlQuery<PurchaseCheckDetail>(queryProduct).ToList();

            var formatedData = rawresult.Select(x => new
            {
                x.codigo,
                x.descripcion,
                cantidad = Decimal.ToInt32(x.cantidad),
                x.proyecto,
                x.almacen,
                x.impuesto,
                precio_unitario = Convert.ToSingle(x.precio_unitario),
                total = Convert.ToSingle(x.total),
                x.unidad_organizacional,
                x.pei_po,
                x.sujeto_a_retencion,
                x.retencion_a_aplicar

            });
            return Ok(formatedData);
        }
    }
}
