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
    public class MercanciaController : ApiController
    {
        private ApplicationDbContext _context;


        public MercanciaController()
        {

            _context = new ApplicationDbContext();
     
        }

        // GET api/Contract
        [HttpGet]
        [Route("api/PurchaseDN/{id}")]
        public IHttpActionResult PurchaseDN(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select oprq.\"DocNum\" as \"numero_solicitud\",\r\nopdn.\"CardCode\" as \"codigo_proveedor\", \r\nopdn.\"CardName\" as \"proveedor\",\r\nopdn.\"BPLName\" as \"regional\",\r\nf.\"SeriesName\" as \"serie\",\r\nopdn.\"U_UOrganiza\" as \"unidad_organizacional\",\r\nopdn.\"DocNum\" as \"numero_documento\",\r\nopdn.\"DocDate\" as \"fecha_contabilizacion\",\r\nopdn.\"DocDueDate\" as \"fecha_valida\",\r\nopdn.\"TaxDate\" as \"fecha_documento\"\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq\r\ninner join \"UCATOLICA\".\"PRQ1\" prq1\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"\r\ninner join \"UCATOLICA\".\"PQT1\" pqt1\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"\r\ninner join\"UCATOLICA\".\"OPQT\" opqt\r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\"\r\nand oprq.\"DocNum\" = pqt1.\"BaseRef\"\r\ninner join \"UCATOLICA\".\"POR1\" por1\r\non opqt.\"DocEntry\" = por1.\"BaseEntry\"\r\nand opqt.\"DocNum\" = por1.\"BaseRef\"\r\ninner join \"UCATOLICA\".\"OPOR\" opor\r\non opor.\"DocEntry\" = por1.\"DocEntry\"\r\ninner join \"UCATOLICA\".\"PDN1\" pdn1\r\non pdn1.\"BaseRef\" = opor.\"DocNum\"\r\nand pdn1.\"BaseEntry\" = opor.\"DocEntry\"\r\ninner join \"UCATOLICA\".\"OPDN\" opdn\r\non opdn.\"DocEntry\" = pdn1.\"DocEntry\"\r\nleft join ucatolica.\"NNM1\" f\r\non opdn.\"Series\" = f.\"Series\"\r\nwhere opdn.\"DocNum\" =" + id+
                                " group by oprq.\"DocNum\",\r\nopdn.\"CardCode\",\r\nopdn.\"CardName\",\r\nopdn.\"BPLName\",\r\nf.\"SeriesName\",\r\nopdn.\"U_UOrganiza\",\r\nopdn.\"DocNum\",\r\nopdn.\"DocDate\",\r\nopdn.\"DocDueDate\",\r\nopdn.\"TaxDate\"";
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

            });
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/PurchaseDNDetail/{id}")]
        public IHttpActionResult PurchaseDNDetail(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select pdn1.\"ItemCode\" as \"codigo\",\r\npdn1.\"Dscription\" as \"descripcion\",\r\npdn1.\"FreeTxt\" as \"observaciones\",\r\npdn1.\"Quantity\" as \"cantidad\",\r\npdn1.\"TaxCode\" as \"impuesto\",\r\npdn1.\"WhsCode\" as \"almacen\",\r\npdn1.\"Project\" as \"proyecto\",\r\npdn1.\"PriceAfVAT\" as \"precio_unitario\",\r\npdn1.\"GTotal\" as \"total\",\r\npdn1.\"OcrCode\" as \"unidad_organizacional\",\r\npdn1.\"OcrCode2\" as \"pei_po\",\r\npdn1.\"U_PR_SALDOPRES\" as \"saldo_presupuestado\",\r\npdn1.\"U_MotivoSalida\" as \"motivo_salida\"\r\nfrom \"UCATOLICA\".\"OPRQ\" oprq\r\ninner join \"UCATOLICA\".\"PRQ1\" prq1\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"PQT1\" pqt1\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"\r\nleft outer join\"UCATOLICA\".\"OPQT\" opqt\r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\"\r\nand oprq.\"DocNum\" = pqt1.\"BaseRef\"\r\nleft outer join \"UCATOLICA\".\"POR1\" por1\r\non opqt.\"DocEntry\" = por1.\"BaseEntry\"\r\nand opqt.\"DocNum\" = por1.\"BaseRef\"\r\nleft outer join \"UCATOLICA\".\"OPOR\" opor\r\non opor.\"DocEntry\" = por1.\"DocEntry\"\r\ninner join \"UCATOLICA\".\"PDN1\" pdn1\r\non pdn1.\"BaseRef\" = opor.\"DocNum\"\r\nand pdn1.\"BaseEntry\" = opor.\"DocEntry\"\r\ninner join \"UCATOLICA\".\"OPDN\" opdn\r\non opdn.\"DocEntry\" = pdn1.\"DocEntry\"\r\nleft join ucatolica.\"NNM1\" f\r\non opdn.\"Series\" = f.\"Series\"\r\nwhere opdn.\"DocNum\" =" + id
                               + " group by pdn1.\"ItemCode\",\r\npdn1.\"Dscription\",\r\npdn1.\"FreeTxt\",\r\npdn1.\"Quantity\",\r\npdn1.\"TaxCode\",\r\npdn1.\"WhsCode\",\r\npdn1.\"Project\",\r\npdn1.\"PriceAfVAT\",\r\npdn1.\"GTotal\",\r\npdn1.\"OcrCode\",\r\npdn1.\"OcrCode2\",\r\npdn1.\"U_PR_SALDOPRES\",\r\npdn1.\"U_MotivoSalida\"";

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
