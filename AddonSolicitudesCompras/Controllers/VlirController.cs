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
                "(select \"PrjCode\" from admnalrrhh.\"VLIRProjects\")" +
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
        [Route("api/ProjectVLIRInfo/{id}/{initDate}/{endDate}/{regional}")]
        public IHttpActionResult ProjectVLIRInfo(string id, string initDate, string endDate, string regional)
        {
            //convertir precio a float o double y cantidad a int!!

            var queryProduct = "";
            if (id!="Todos")
            {
                queryProduct =
                "select \r\nc.\"FormatCode\" as \"cuenta\"," +
                "\r\nc.\"AcctName\" as \"nombre_cuenta\"," +
                "\r\nTO_VARCHAR (a.\"RefDate\", 'DD/MM/YYYY') as \"fecha_comprobante\"," +
                "\r\na.\"Number\" as \"numero_comprobante\"," +
                "\r\nb.\"TransId\" as \"numero_transaccion\"," +
                "\r\nb.\"Line_ID\" as \"linea_transaccion\"," +
                "\r\nd.\"U_Sucursal\" as \"regional\"," +
                "\r\nb.\"LineMemo\" as \"glosa\"," +
                "\r\nconcat(b.\"Ref1\",concat(' ', b.\"Ref2\")) as \"referencia\"," +
                "\r\nb.\"Debit\"," +
                "\r\nb.\"Credit\"," +
                "\r\nTO_VARCHAR (b.\"U_FECHAFAC\", 'DD/MM/YYYY') as \"fecha_fac\"," +
                "\r\nd.\"PrjCode\" as \"codigo_proyecto\"" +
                "\r\nfrom ucatolica.ojdt a" +
                "\r\ninner join ucatolica.jdt1 b" +
                "\r\non a.\"TransId\" = b.\"TransId\"" +
                "\r\ninner join   ucatolica.oact c" +
                "\r\non b.\"Account\" = c.\"AcctCode\"" +
                "\r\nleft join ucatolica.oprj d" +
                "\r\non b.\"Project\" = d.\"PrjCode\"" +
                "\r\nwhere a.\"RefDate\" between '"+initDate+"' and '"+endDate+"'" +
                "\r\nand a.\"StornoToTr\" is null" +
                "\r\nand a.\"TransId\" not in (select z.\"StornoToTr\" from ucatolica.ojdt z where z.\"StornoToTr\" is not null)" +
                "\r\nand (c.\"GroupMask\" like '4%'  " +
                "\r\nor c.\"GroupMask\" like '5%')" +
                "\r\nand d.\"PrjCode\" = '" +id+"'"+
                "\r\ngroup by" +
                "\r\nd.\"U_Sucursal\"," +
                "\r\nd.\"PrjCode\",c.\"FormatCode\"," +
                "\r\nc.\"AcctCode\",c.\"AcctName\"," +
                "\r\na.\"RefDate\"," +
                "\r\na.\"Number\"," +
                "\r\nb.\"TransId\",b.\"Line_ID\"," +
                "\r\nconcat(b.\"Ref1\",concat(' ', b.\"Ref2\"))," +
                "\r\nb.\"LineMemo\",b.\"Debit\",b.\"Credit\"," +
                "\r\nb.\"U_FECHAFAC\"" +
                "\r\norder by" +
                "\r\nd.\"U_Sucursal\"," +
                "\r\nd.\"PrjCode\"," +
                "\r\na.\"RefDate\"," +
                "\r\na.\"Number\",b.\"TransId\"," +
                "\r\nb.\"Line_ID\"";
            }
            else
            {
                if (regional != "Todos")
                {
                    queryProduct = "select \r\nc.\"FormatCode\" as \"cuenta\"," +
                                   "\r\nc.\"AcctName\" as \"nombre_cuenta\"," +
                                   "\r\nTO_VARCHAR (a.\"RefDate\", 'DD/MM/YYYY') as \"fecha_comprobante\"," +
                                   "\r\na.\"Number\" as \"numero_comprobante\"," +
                                   "\r\nb.\"TransId\" as \"numero_transaccion\"," +
                                   "\r\nb.\"Line_ID\" as \"linea_transaccion\"," +
                                   "\r\nd.\"U_Sucursal\" as \"regional\"," +
                                   "\r\nb.\"LineMemo\" as \"glosa\"," +
                                   "\r\nconcat(b.\"Ref1\",concat(' ', b.\"Ref2\")) as \"referencia\"," +
                                   "\r\nb.\"Debit\"," +
                                   "\r\nb.\"Credit\"," +
                                   "\r\nTO_VARCHAR (b.\"U_FECHAFAC\", 'DD/MM/YYYY') as \"fecha_fac\"," +
                                   "\r\nd.\"PrjCode\" as \"codigo_proyecto\"" +
                                   "\r\nfrom ucatolica.ojdt a" +
                                   "\r\ninner join ucatolica.jdt1 b" +
                                   "\r\non a.\"TransId\" = b.\"TransId\"" +
                                   "\r\ninner join   ucatolica.oact c" +
                                   "\r\non b.\"Account\" = c.\"AcctCode\"" +
                                   "\r\nleft join ucatolica.oprj d" +
                                   "\r\non b.\"Project\" = d.\"PrjCode\"" +
                                   "\r\nwhere a.\"RefDate\" between '"+initDate+"' and '"+endDate+"'" +
                                   "\r\nand d.\"U_Sucursal\" ='" +regional+"'"+
                                   "\r\nand a.\"StornoToTr\" is null" +
                                   "\r\nand a.\"TransId\" not in (select z.\"StornoToTr\" from ucatolica.ojdt z where z.\"StornoToTr\" is not null)" +
                                   "\r\nand (c.\"GroupMask\" like '4%'  " +
                                   "\r\nor c.\"GroupMask\" like '5%')" +
                                   "\r\nand d.\"PrjCode\" in (select \"PrjCode\" from admnalrrhh.\"VLIRProjects\")" +
                                   "\r\ngroup by" +
                                   "\r\nd.\"U_Sucursal\"," +
                                   "\r\nd.\"PrjCode\",c.\"FormatCode\"," +
                                   "\r\nc.\"AcctCode\",c.\"AcctName\"," +
                                   "\r\na.\"RefDate\"," +
                                   "\r\na.\"Number\"," +
                                   "\r\nb.\"TransId\",b.\"Line_ID\"," +
                                   "\r\nconcat(b.\"Ref1\",concat(' ', b.\"Ref2\"))," +
                                   "\r\nb.\"LineMemo\"," +
                                   "\r\nb.\"Debit\"," +
                                   "\r\nb.\"Credit\"," +
                                   "\r\nb.\"U_FECHAFAC\"\r\norder by" +
                                   "\r\nd.\"U_Sucursal\",\r\nd.\"PrjCode\"," +
                                   "\r\na.\"RefDate\",\r\na.\"Number\",b.\"TransId\"," +
                                   "\r\nb.\"Line_ID\"";
                }
                else
                {
                    queryProduct = "select \r\nc.\"FormatCode\" as \"cuenta\"," +
                                   "\r\nc.\"AcctName\" as \"nombre_cuenta\"," +
                                   "\r\nTO_VARCHAR (a.\"RefDate\", 'DD/MM/YYYY') as \"fecha_comprobante\"," +
                                   "\r\na.\"Number\" as \"numero_comprobante\"," +
                                   "\r\nb.\"TransId\" as \"numero_transaccion\"," +
                                   "\r\nb.\"Line_ID\" as \"linea_transaccion\"," +
                                   "\r\nd.\"U_Sucursal\" as \"regional\"," +
                                   "\r\nb.\"LineMemo\" as \"glosa\"," +
                                   "\r\nconcat(b.\"Ref1\",concat(' ', b.\"Ref2\")) as \"referencia\"," +
                                   "\r\nb.\"Debit\"," +
                                   "\r\nb.\"Credit\"," +
                                   "\r\nTO_VARCHAR (b.\"U_FECHAFAC\", 'DD/MM/YYYY') as \"fecha_fac\"," +
                                   "\r\nd.\"PrjCode\" as \"codigo_proyecto\"" +
                                   "\r\nfrom ucatolica.ojdt a" +
                                   "\r\ninner join ucatolica.jdt1 b" +
                                   "\r\non a.\"TransId\" = b.\"TransId\"" +
                                   "\r\ninner join   ucatolica.oact c" +
                                   "\r\non b.\"Account\" = c.\"AcctCode\"" +
                                   "\r\nleft join ucatolica.oprj d" +
                                   "\r\non b.\"Project\" = d.\"PrjCode\"" +
                                   "\r\nwhere a.\"RefDate\" between '" + initDate + "' and '" + endDate + "'" +
                                   "\r\nand a.\"StornoToTr\" is null" +
                                   "\r\nand a.\"TransId\" not in (select z.\"StornoToTr\" from ucatolica.ojdt z where z.\"StornoToTr\" is not null)" +
                                   "\r\nand (c.\"GroupMask\" like '4%'  " +
                                   "\r\nor c.\"GroupMask\" like '5%')" +
                                   "\r\nand d.\"PrjCode\" in (select \"PrjCode\" from admnalrrhh.\"VLIRProjects\")" +
                                   "\r\ngroup by" +
                                   "\r\nd.\"U_Sucursal\"," +
                                   "\r\nd.\"PrjCode\",c.\"FormatCode\"," +
                                   "\r\nc.\"AcctCode\",c.\"AcctName\"," +
                                   "\r\na.\"RefDate\"," +
                                   "\r\na.\"Number\"," +
                                   "\r\nb.\"TransId\",b.\"Line_ID\"," +
                                   "\r\nconcat(b.\"Ref1\",concat(' ', b.\"Ref2\"))," +
                                   "\r\nb.\"LineMemo\"," +
                                   "\r\nb.\"Debit\"," +
                                   "\r\nb.\"Credit\"," +
                                   "\r\nb.\"U_FECHAFAC\"\r\norder by" +
                                   "\r\nd.\"U_Sucursal\",\r\nd.\"PrjCode\"," +
                                   "\r\na.\"RefDate\",\r\na.\"Number\",b.\"TransId\"," +
                                   "\r\nb.\"Line_ID\"";
                }
            }
            

            var rawresult = _context.Database.SqlQuery<VLIRInfo>(queryProduct).ToList();
            var formatedData = rawresult.Select(x => new
            {
                
                x.regional,
                x.codigo_proyecto,
                x.cuenta,
                x.codigo_cuenta,
                x.nombre_cuenta,
                fecha = x.fecha_comprobante.ToString("dd/MM/yyyy"),
                x.numero_comprobante,
                x.numero_transaccion,
                x.linea_transaccion,
                x.referencia,
                x.glosa,
                debe = Convert.ToSingle(x.Debit),
                haber = Convert.ToSingle(x.Credit),
                monto_total = Convert.ToSingle(x.Debit - x.Credit),
                fecha_fac = x.fecha_fac.HasValue ? x.fecha_fac.Value.ToString("dd/MM/yyyy") : null,

            });
            queryProduct = "";
            return Ok(formatedData);
        }

        [HttpGet]
        [Route("api/Regionales/")]
        public IHttpActionResult Regionales()
        {
            var query = "select op.\"U_Sucursal\" as \"regional\",\r\nbr.\"Id\" as \"codigo_proyecto\"\r\nfrom ucatolica.\"OPRJ\" op\r\ninner join admnalrrhh.\"Branches\" br\r\non br.\"Abr\" = op.\"U_Sucursal\"\r\nwhere op.\"PrjCode\"in (select \"PrjCode\" from admnalrrhh.\"VLIRProjects\") \r\ngroup by op.\"U_Sucursal\" , br.\"Id\"";
            var rawresult = _context.Database.SqlQuery<VLIR>(query).ToList();
            var formatedData = rawresult.Select(x => new
            {
                x.regional
            });
            var aux = "select 'Todas' \"codigo_proyecto\" ,\r\n'Todos' \"nombre_proyecto\" ,\r\n'Todos' \"regional\" ,\r\n'00/00/0000' \"valido_hasta\" ,\r\n'00/00/0000' \"valido_desde\" \r\n from Dummy";
            var auxresult = _context.Database.SqlQuery<VLIRFake>(aux).ToList();
            var formatedAux = auxresult.Select(x => new
            {
                x.regional
            });
            return Ok(formatedAux.Concat(formatedData));
        }

        [HttpGet]
        [Route("api/FiltroRegionales/{id}")]
        public IHttpActionResult FiltroRegionales(string id)
        {
            var query = "";
            if (id != "Todos")
            {
                query = "select p.\"PrjCode\" as \"codigo_proyecto\", " +
                        "\r\np.\"PrjName\" as \"nombre_proyecto\", " +
                        "\r\np.\"U_Sucursal\" as \"regional\", " +
                        "\r\np.\"ValidTo\" as \"valido_hasta\", " +
                        "\r\np.\"ValidFrom\" as \"valido_desde\" " +
                        "\r\nfrom ucatolica.oprj p " +
                        "\r\nwhere p.\"Active\" = 'Y' " +
                        "\r\nand p.\"PrjCode\" " +
                        "in (select \"PrjCode\" from admnalrrhh.\"VLIRProjects\")" +
                        "\r\nand p.\"U_Sucursal\" = '" + id + "'" +
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
            }
            else
            {
                query = "select p.\"PrjCode\" as \"codigo_proyecto\", " +
                        "\r\np.\"PrjName\" as \"nombre_proyecto\", " +
                        "\r\np.\"U_Sucursal\" as \"regional\", " +
                        "\r\np.\"ValidTo\" as \"valido_hasta\", " +
                        "\r\np.\"ValidFrom\" as \"valido_desde\" " +
                        "\r\nfrom ucatolica.oprj p " +
                        "\r\nwhere p.\"Active\" = 'Y' " +
                        "\r\nand p.\"PrjCode\" in " +
                        " (select \"PrjCode\" from admnalrrhh.\"VLIRProjects\")" +
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
            }

            var aux = "select 'Todos' \"codigo_proyecto\" ,\r\n'Todos' \"nombre_proyecto\" ,\r\n'Todos' \"regional\" ,\r\n'00/00/0000' \"valido_hasta\" ,\r\n'00/00/0000' \"valido_desde\" \r\n from Dummy";
            var rawresult = _context.Database.SqlQuery<VLIR>(query).ToList();
            var auxresult = _context.Database.SqlQuery<VLIRFake>(aux).ToList();
            var formatedData = rawresult.Select(x => new
            {
                nombre = x.codigo_proyecto + " - " + x.nombre_proyecto,
                x.codigo_proyecto,
                x.nombre_proyecto,
                x.regional,
                valido_hasta = x.valido_hasta.ToString("dd/MM/yyyy"),
                valido_desde = x.valido_desde.ToString("dd/MM/yyyy"),
            });
            var formatedAux = auxresult.Select(x => new
            {
                nombre = x.codigo_proyecto + " - " + x.nombre_proyecto,
                x.codigo_proyecto,
                x.nombre_proyecto,
                x.regional,
                x.valido_hasta,
                x.valido_desde
            });
            return Ok(formatedAux.Concat(formatedData));
            }
        [HttpGet]
        [Route("api/ProjectNameVLIR/{id}")]
        public IHttpActionResult ProjectNameVLIR(string id)
        {
            var queryP = "";
            if (id == "Todos")
            {
                queryP =
                    "select 'Todos' \"PROYECTO_CODIGO\", " +
                    "\r\n'Todos' \"proyecto_nombre\"," +
                    "\r\n'1999-12-31' \"valido_desde\"," +
                    "\r\n'2030-12-31' \"valido_hasta\"," +
                    "\r\n'Todos' \"unidad_organizacional\"," +
                    "\r\n'Todos' \"pei_po\"," +
                    "\r\n'Todos' \"regional\"\r\nfrom \"DUMMY\"";
            }
            else
            {
                queryP =
                    "select op.\"PrjCode\" as \"PROYECTO_CODIGO\"," +
                    "\r\nop.\"PrjName\" as \"proyecto_nombre\", " +
                    "\r\nop.\"ValidFrom\" as \"valido_desde\"," +
                    "\r\nop.\"ValidTo\" as \"valido_hasta\"," +
                    "\r\nop.\"U_UORGANIZA\" as \"unidad_organizacional\"," +
                    "\r\nop.\"U_PEI_PO\" as \"pei_po\"," +
                    "\r\nop.\"U_Sucursal\" as \"regional\"" +
                    "\r\nfrom \"UCATOLICA\".\"OPRJ\" op " +
                    "\r\nwhere op.\"PrjCode\" = '" + id + "'";
            }

            var rawres = _context.Database.SqlQuery<ProjectName>(queryP).ToList();
            var formData = rawres.Select(x => new
            {
                x.PROYECTO_CODIGO,
                x.proyecto_nombre,
                x.unidad_organizacional,
                x.pei_po,
                x.regional,
                valido_hasta = x.valido_hasta.ToString("dd/MM/yyyy"),
                valido_desde = x.valido_desde.ToString("dd/MM/yyyy"),
            });
            return Ok(formData);
        }

    }

}
