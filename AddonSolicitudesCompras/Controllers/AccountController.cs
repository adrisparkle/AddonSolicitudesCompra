using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using AddonSolicitudesCompras.Models;

namespace AddonSolicitudesCompras.Controllers
{
    
    public class AsientoController : ApiController
    {
        private ApplicationDbContext _context;
        private string dbName = "ucatolica";

        public AsientoController()
        {

            _context = new ApplicationDbContext();
     
        }
        // GET api/Contract
        [HttpGet]
        [Route("api/AccountEntry/{id}")]
        public IHttpActionResult AccountEntry(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select " +
                               "\r\nf.\"SeriesName\" as \"serie\"," +
                               "\r\nojdt.\"Number\" as \"numero_asiento\"," +
                               "\r\nojdt.\"RefDate\" as \"fecha_contabilizacion\"," +
                               "\r\nojdt.\"DueDate\" as \"fecha_valida\"," +
                               "\r\nojdt.\"TaxDate\" as \"fecha_documento\"," +
                               "\r\nojdt.\"Memo\" as \"comentario\"," +
                               "\r\nTO_VARCHAR(ojdt.\"BaseRef\") as \"numero_origen\"," +
                               "\r\nojdt.\"TransId\" as \"numero_transaccion\"," +
                               "\r\nojdt.\"Ref1\" as \"referencia1\"," +
                               "\r\nojdt.\"Ref2\" as \"referencia2\"" +
                               "\r\nfrom " + dbName + ".\"OPRQ\" oprq" +
                               "\r\ninner join " + dbName + ".\"PRQ1\" prq1" +
                               "\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"" +
                               "\r\nleft outer join " + dbName + ".\"PQT1\" pqt1" +
                               "\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"" +
                               "\r\nleft outer join " + dbName + ".\"OPQT\" opqt" +
                               "\r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\"" +
                               "\r\nleft outer join " + dbName + ".\"POR1\" por1" +
                               "\r\non opqt.\"DocEntry\" = por1.\"BaseEntry\"" +
                               "\r\nleft outer join " + dbName + ".\"OPOR\" opor" +
                               "\r\non opor.\"DocEntry\" = por1.\"DocEntry\"" +
                               "\r\nleft outer join " + dbName + ".\"PCH1\" pch1" +
                               "\r\non opor.\"DocEntry\" = pch1.\"BaseEntry\"" +
                               "\r\nand opor.\"DocEntry\" = pch1.\"BaseEntry\"" +
                               "\r\nleft outer join " + dbName + ".\"OPCH\" opch" +
                               "\r\non opch.\"DocEntry\" = pch1.\"DocEntry\"" +
                               "\r\nleft outer join " + dbName + ".\"VPM2\" vpm2" +
                               "\r\non opch.\"DocEntry\" = vpm2.\"DocEntry\"" +
                               "\r\nleft outer join " + dbName + ".\"OVPM\" ovpm" +
                               "\r\non vpm2.\"DocNum\" = ovpm.\"DocEntry\"" +
                               "\r\nleft outer join " + dbName + ".\"VPM1\" vpm1" +
                               "\r\non vpm1.\"DocNum\" = ovpm.\"DocEntry\"" +
                               "\r\nleft outer join " + dbName + ".\"OJDT\" ojdt" +
                               "\r\non ojdt.\"BaseRef\" = ovpm.\"DocNum\"" +
                               "\r\nand ojdt.\"CreatedBy\" = ovpm.\"DocEntry\"" +
                               "\r\nleft outer join " + dbName + ".\"JDT1\" jdt1" +
                               "\r\non jdt1.\"TransId\" = ojdt.\"TransId\"" +
                               "\r\nleft outer join " + dbName + ".\"OACT\" oact" +
                               "\r\non oact.\"AcctCode\" = jdt1.\"Account\"" +
                               "\r\nor oact.\"AcctCode\" = jdt1.\"Account\"" +
                               "\r\nleft join " + dbName + ".\"NNM1\" f" +
                               "\r\non opch.\"Series\" = f.\"Series\"" +
                               "\r\nwhere ovpm.\"DocNum\" = " + id + "" +
                               "\r\n group by\r\nf.\"SeriesName\"," +
                               "\r\nojdt.\"Number\",\r\nojdt.\"RefDate\"," +
                               "\r\nojdt.\"DueDate\",\r\nojdt.\"TaxDate\"," +
                               "\r\nojdt.\"Memo\",\r\nojdt.\"BaseRef\"," +
                               "\r\nojdt.\"TransId\",\r\nojdt.\"Ref1\"," +
                               "\r\nojdt.\"Ref2\"";

            var rawresult = _context.Database.SqlQuery<Account>(queryProduct).ToList();
            var formatedData = rawresult.Select(x => new
            {
                
                x.numero_asiento,
                x.serie,
                x.numero_origen,
                x.numero_transaccion,
                fecha_contabilizacion = x.fecha_contabilizacion.ToString("dd/MM/yyyy"),
                fecha_valida = x.fecha_valida.ToString("dd/MM/yyyy"),
                fecha_documento = x.fecha_documento.ToString("dd/MM/yyyy"),
                x.referencia1,
                x.referencia2,
                x.comentario,

            });
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/AccountEntryDetail/{id}")]
        public IHttpActionResult AccountEntryDetail(int id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct = "select \r\njdt1.\"ShortName\" as \"cuenta\"," +
                               "\r\njdt1.\"Ref1\" as \"referencia1\"," +
                               "\r\njdt1.\"Ref2\" as \"referencia2\"," +
                               "\r\njdt1.\"Ref3Line\" as \"referencia3\"," +
                               "\r\njdt1.\"ContraAct\" as \"cuenta_mayor\"," +
                               "\r\njdt1.\"RefDate\" as \"fecha_contabilizacion\"," +
                               "\r\njdt1.\"DueDate\" as \"fecha_valida\"," +
                               "\r\njdt1.\"TaxDate\" as \"fecha_documento\"," +
                               "\r\njdt1.\"Project\" as \"proyecto\"," +
                               "\r\njdt1.\"LineMemo\" as \"comentario\"," +
                               "\r\noact.\"AcctCode\" as \"codigo_cuenta\"," +
                               "\r\noact.\"AcctName\" as \"nombre_cuenta\"," +
                               "\r\noact.\"FormatCode\" as \"cuenta_oficial\"," +
                               "\r\njdt1.\"Debit\" as \"debito_bs\"," +
                               "\r\njdt1.\"Credit\" as \"credito_bs\"," +
                               "\r\njdt1.\"SYSDeb\" as \"debito_ms\"," +
                               "\r\njdt1.\"SYSCred\" as \"credito_ms\"" +
                               "\r\nfrom " + dbName + ".\"OPRQ\" oprq" +
                               "\r\ninner join " + dbName + ".\"PRQ1\" prq1" +
                               "\r\non oprq.\"DocEntry\" = prq1.\"DocEntry\"" +
                               "\r\nleft outer join " + dbName + ".\"PQT1\" pqt1" +
                               "\r\non oprq.\"DocEntry\" = pqt1.\"BaseEntry\"" +
                               "\r\nleft outer join " + dbName + ".\"OPQT\" opqt" +
                               "\r\non opqt.\"DocEntry\" = pqt1.\"DocEntry\"" +
                               "\r\nleft outer join " + dbName + ".\"POR1\" por1" +
                               "\r\non opqt.\"DocEntry\" = por1.\"BaseEntry\"" +
                               "\r\nleft outer join " + dbName + ".\"OPOR\" opor\r\non opor.\"DocEntry\" = por1.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"PCH1\" pch1\r\non opor.\"DocEntry\" = pch1.\"BaseEntry\"\r\nand opor.\"DocEntry\" = pch1.\"BaseEntry\"\r\nleft outer join \"UCATOLICA\".\"OPCH\" opch\r\non opch.\"DocEntry\" = pch1.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"VPM2\" vpm2\r\non opch.\"DocEntry\" = vpm2.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"OVPM\" ovpm\r\non vpm2.\"DocNum\" = ovpm.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"VPM1\" vpm1\r\non vpm1.\"DocNum\" = ovpm.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"OJDT\" ojdt\r\non ojdt.\"BaseRef\" = ovpm.\"DocNum\"\r\nand ojdt.\"CreatedBy\" = ovpm.\"DocEntry\"\r\nleft outer join \"UCATOLICA\".\"JDT1\" jdt1\r\non jdt1.\"TransId\" = ojdt.\"TransId\"\r\nleft outer join \"UCATOLICA\".\"OACT\" oact\r\non oact.\"AcctCode\" = jdt1.\"Account\"\r\nor oact.\"AcctCode\" = jdt1.\"Account\"\r\nleft join ucatolica.\"NNM1\" f\r\non opch.\"Series\" = f.\"Series\"\r\nwhere ovpm.\"DocNum\" = " + id;

            var rawresult = _context.Database.SqlQuery<AccountDetail>(queryProduct).ToList();

            var formatedData = rawresult.Select(x => new
            {
                x.cuenta,
                x.nombre_cuenta,
                x.referencia1,
                x.referencia2,
                x.referencia3,
                x.proyecto,
                fecha_contabilizacion = x.fecha_contabilizacion.ToString("dd/MM/yyyy"),
                fecha_valida = x.fecha_valida.ToString("dd/MM/yyyy"),
                fecha_documento = x.fecha_documento.ToString("dd/MM/yyyy"),
                x.comentario,
                x.cuenta_mayor,
                x.codigo_cuenta,
                x.cuenta_oficial,
                debito_bs = Convert.ToSingle(x.debito_bs),
                credito_bs = Convert.ToSingle(x.credito_bs),
                debito_ms = Convert.ToSingle(x.debito_ms),
                credito_ms = Convert.ToSingle(x.credito_ms),
                
            });
            return Ok(formatedData);
        }
    }
    
}
