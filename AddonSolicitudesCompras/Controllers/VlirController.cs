using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AddonSolicitudesCompras.Models;

namespace AddonSolicitudesCompras.Controllers
{
    public class VlirController : ApiController
    {
        private ApplicationDbContext _context;
        private string dbName = "ucatolica";

        public VlirController()
        {

            _context = new ApplicationDbContext();

        }

        [HttpGet]
        [Route("api/ProjectVLIR/")]
        public IHttpActionResult ProjectVLIR()
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct =
                "select p.\"PrjCode\" as \"codigo_proyecto\", " +
                "\r\np.\"PrjName\" as \"nombre_proyecto\", " +
                "\r\np.\"U_Sucursal\" as \"regional\", " +
                "\r\np.\"ValidTo\" as \"valido_hasta\", " +
                "\r\np.\"ValidFrom\" as \"valido_desde\" " +
                "\r\nfrom "+dbName+".oprj p " +
                "\r\nwhere p.\"Active\" = 'Y' " +
                "\r\nand p.\"PrjCode\" in " +
                "('L4655'," +
                "\r\n'L4654'," +
                "\r\n'L4653'," +
                "\r\n'L4652'," +
                "\r\n'L4651'," +
                "\r\n'L4650'," +
                "\r\n'C4209'," +
                "\r\n'C4210'," +
                "\r\n'C4211'," +
                "\r\n'C4212'," +
                "\r\n'C4213'," +
                "\r\n'C4214'," +
                "\r\n'S10003'," +
                "\r\n'S10004'," +
                "\r\n'S10005'," +
                "\r\n'S10006'," +
                "\r\n'S10008'," +
                "\r\n'S10007'," +
                "\r\n'T2753'," +
                "\r\n'T2754'," +
                "\r\n'T2755')" +
                "\r\ngroup by " +
                "p.\"PrjCode\", " +
                "\r\np.\"PrjName\"," +
                "\r\np.\"U_Sucursal\", " +
                "\r\np.\"ValidTo\", " +
                "\r\np.\"ValidFrom\"" +
                "\r\norder by  " +
                "p.\"PrjCode\", " +
                "\r\np.\"PrjName\"," +
                "\r\np.\"U_Sucursal\", " +
                "\r\np.\"ValidTo\", " +
                "\r\np.\"ValidFrom\"";

            var rawresult = _context.Database.SqlQuery<VLIR>(queryProduct).ToList();
            var formatedData = rawresult.Select(x => new
            {

                nombre = x.codigo_proyecto + " - " + x.nombre_proyecto,
                x.codigo_proyecto,
                x.nombre_proyecto,
                x.regional,
                valido_hasta = x.valido_hasta.ToString("dd/MM/yyyy"),
                valido_desde = x.valido_desde.ToString("dd/MM/yyyy"),

            });
            return Ok(formatedData);
        }

        [HttpGet]
        [Route("api/ProjectVLIRInfo/{id}")]
        public IHttpActionResult ProjectVLIRInfo(string id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct =
                "select " +
                "\r\nc.\"FormatCode\" as \"cuenta\"," +
                "\r\na.\"RefDate\" as \"fecha\"," +
                "\r\na.\"Number\" as \"numero_comprobante\"," +
                "\r\nb.\"TransId\" as \"numero_transaccion\"," +
                "\r\nd.\"U_Sucursal\" as \"sucursal\"," +
                "\r\nb.\"LineMemo\" as \"descripcion\"," +
                "\r\na.\"BaseRef\" as \"referencia\"," +
                "\r\na.\"LocTotal\" as \"monto\"," +
                "\r\nd.\"PrjCode\" as \"codigo_proyecto\"" +
                "\r\nfrom "+dbName+".ojdt a" +
                "\r\ninner join " + dbName + ".jdt1 b" +
                "\r\non a.\"TransId\" = b.\"TransId\"" +
                "\r\ninner join " + dbName + ".oact c" +
                "\r\non b.\"Account\" = c.\"AcctCode\"" +
                "\r\nleft join " + dbName + ".oprj d" +
                "\r\non b.\"Project\" = d.\"PrjCode\"" +
                "\r\nleft join " + dbName + ".\"OPRC\" e" +
                "\r\non e.\"PrcCode\" = b.\"ProfitCode\"" +
                "\r\nleft join " + dbName + ".\"NNM1\" f" +
                "\r\non a.\"Series\" = f.\"Series\"" +
                "\r\nwhere b.\"Project\" = '" +
                id +
                "' group by\r\nc.\"FormatCode\"," +
                "\r\na.\"RefDate\"," +
                "\r\na.\"Number\"," +
                "\r\nb.\"TransId\"," +
                "\r\nd.\"U_Sucursal\"," +
                "\r\nb.\"LineMemo\"," +
                "\r\na.\"BaseRef\"," +
                "\r\na.\"LocTotal\"," +
                "\r\nd.\"PrjCode\"";

            var rawresult = _context.Database.SqlQuery<VLIRInfo>(queryProduct).ToList();
            var formatedData = rawresult.Select(x => new
            {
                
                x.sucursal,
                x.cuenta,
                fecha = x.fecha.ToString("dd/MM/yyyy"),
                x.numero_comprobante,
                x.numero_transaccion,
                x.descripcion,
                x.referencia,
                x.codigo_proyecto,
                monto = Convert.ToSingle(x.monto),

            });
            return Ok(formatedData);
        }
    }

}
