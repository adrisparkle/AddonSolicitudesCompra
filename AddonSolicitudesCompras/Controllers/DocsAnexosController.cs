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
    public class DocsAnexosController : ApiController
    {
        private ApplicationDbContext _context;


        public DocsAnexosController()
        {

            _context = new ApplicationDbContext();
     
        }

        // GET api/Contract
        [HttpGet]
        [Route("api/DocsAnexos/{id}")]
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
    }
}
