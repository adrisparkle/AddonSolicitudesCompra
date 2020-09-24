using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AddonSolicitudesCompras.Models;

namespace AddonSolicitudesCompras.Controllers
{
    public class FricaController : ApiController
    {
        private ApplicationDbContext _context;
        private string dbName = "ucatolica";

        public FricaController()
        {

            _context = new ApplicationDbContext();

        }
        [HttpGet]
        [Route("api/ProjectFRICA/")]
        public IHttpActionResult ProjectFRICA()
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct =
                "select  \r\n d.\"U_Sucursal\" as \"regional\", \r\n" +
                " d.\"PrjCode\" as \"codigo_proyecto\",\r\n " +
                "d.\"PrjName\" as \"nombre_proyecto\"\r\n " +
                "from  ucatolica.oprj d" +
                "\r\n where d.\"PrjCode\" in " +
                "('L4170','L4102','L4093','L4095','L4100','L4290','L4092'," +
                "\r\n'L4099','L4094','L4101','L4097','L4579','L4599','L4600','L5489'," +
                "'L4959','L4801',\r\n'L4809','L5490','L5651','L5649','L5679','L5699'," +
                "'C3263','C4116','C3283','C3433',\r\n'S3073','S3053','S10002')" +
                "\r\ngroup by\r\nd.\"U_Sucursal\", \r\n d.\"PrjCode\",\r\n d.\"PrjName\"," +
                "\r\n d.\"ValidTo\",\r\n d.\"ValidFrom\"\r\n order by\r\n d.\"PrjCode\"";

            var rawresult = _context.Database.SqlQuery<FRICA>(queryProduct).ToList();
            var formatedData = rawresult.Select(x => new
            {

                nombre = x.codigo_proyecto + " - " + x.nombre_proyecto,
                x.codigo_proyecto,
                x.nombre_proyecto,
                x.regional,

            });
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/ProjectFRICAInfo/{id}/{initDate}/{endDate}/{regional}")]
        public IHttpActionResult ProjectFRICAInfo(string id, string initDate, string endDate, string regional)
        {
            //convertir precio a float o double y cantidad a int!!

            var queryProduct = "";
            if (id != "Todos")
            {
                queryProduct =
                "select \r\nd.\"U_Sucursal\" as \"sucursal\"," +
                "\r\nd.\"PrjCode\" as \"codigo_proyecto\" ," +
                "\r\nc.\"FormatCode\" as \"cuenta\"," +
                "\r\nc.\"AcctCode\" as \"codigo_cuenta\"," +
                "\r\nc.\"AcctName\" as \"nombre_cuenta\"," +
                "\r\na.\"RefDate\" as \"fecha\"," +
                "\r\na.\"Number\" as \"numero_comprobante\"," +
                "\r\nb.\"TransId\" as \"numero_transaccion\"," +
                "\r\nb.\"Line_ID\" as \"linea_transaccion\"," +
                "\r\nconcat(b.\"Ref1\",concat(' ', b.\"Ref2\")) as \"referencia\"," +
                "\r\nb.\"LineMemo\" as \"descripcion\"," +
                "\r\nb.\"Debit\" as \"debe\"," +
                "\r\nb.\"Credit\" as \"haber\"," +
                "\r\n(b.\"Debit\"-b.\"Credit\") as \"monto_total\"" +
                "\r\nfrom ucatolica.ojdt a" +
                "\r\ninner join   ucatolica.jdt1 b" +
                "\r\non a.\"TransId\" = b.\"TransId\"" +
                "\r\ninner join   ucatolica.oact c" +
                "\r\non b.\"Account\" = c.\"AcctCode\"" +
                "\r\nleft join   ucatolica.oprj d " +
                "\r\non b.\"Project\" = d.\"PrjCode\"" +
                "\r\nwhere b.\"Project\" = '" + id + "'" +
                " and a.\"RefDate\" between '" + initDate +
                "' and '" + endDate + "'\r\n" +
                "and (c.\"GroupMask\" like '4%'  " +
                "or c.\"GroupMask\" like '5%')" +
                "\r\ngroup by\r\nd.\"U_Sucursal\"," +
                "\r\nd.\"PrjCode\",\r\nc.\"FormatCode\"," +
                "\r\nc.\"AcctCode\",\r\nc.\"AcctName\"," +
                "\r\na.\"RefDate\",\r\na.\"Number\"," +
                "\r\nb.\"TransId\",\r\nb.\"Line_ID\"," +
                "\r\nconcat(b.\"Ref1\",concat(' ', b.\"Ref2\"))," +
                "\r\nb.\"LineMemo\",\r\nb.\"Debit\",\r\nb.\"Credit\"," +
                "\r\na.\"LocTotal\"\r\norder by\r\na.\"RefDate\"," +
                "\r\na.\"Number\",\r\nb.\"TransId\"," +
                "\r\nb.\"Line_ID\"";
            }
            else
            {
                if (regional != "Todas")
                {
                    /*queryProduct = "select d.\"U_Sucursal\" as \"sucursal\"," +
                               "\r\nd.\"PrjCode\" as \"codigo_proyecto\" ," +
                               "\r\nc.\"FormatCode\" as \"cuenta\"," +
                               "\r\nc.\"AcctCode\" as \"codigo_cuenta\"," +
                               "\r\nc.\"AcctName\" as \"nombre_cuenta\"," +
                               "\r\na.\"RefDate\" as \"fecha\"," +
                               "\r\na.\"Number\" as \"numero_comprobante\"," +
                               "\r\nb.\"TransId\" as \"numero_transaccion\"," +
                               "\r\nb.\"Line_ID\" as \"linea_transaccion\"," +
                               "\r\nconcat(b.\"Ref1\",concat(' ', b.\"Ref2\")) as \"referencia\"," +
                               "\r\nb.\"LineMemo\" as \"descripcion\"," +
                               "\r\nb.\"Debit\" as \"debe\"," +
                               "\r\nb.\"Credit\" as \"haber\"," +
                               "\r\n(b.\"Debit\"-b.\"Credit\") as \"monto_total\"" +
                               "\r\nfrom ucatolica.ojdt a" +
                               "\r\ninner join ucatolica.jdt1 b" +
                               "\r\non a.\"TransId\" = b.\"TransId\"" +
                               "\r\ninner join   ucatolica.oact c" +
                               "\r\non b.\"Account\" = c.\"AcctCode\"" +
                               "\r\nleft join ucatolica.oprj d" +
                               "\r\non b.\"Project\" = d.\"PrjCode\"" +
                               "\r\nwhere a.\"RefDate\" between '" + initDate + "' and '" + endDate + "'" +
                               "\r\nand (c.\"GroupMask\" like '4%'  " +
                               "\r\nor c.\"GroupMask\" like '5%')" +
                               "\r\nand d.\"U_Sucursal\" = '" + regional + "'" +
                               "\r\ngroup by d.\"U_Sucursal\"," +
                               "\r\nd.\"PrjCode\",c.\"FormatCode\"," +
                               "\r\nc.\"AcctCode\",c.\"AcctName\"," +
                               "\r\na.\"RefDate\",a.\"Number\"," +
                               "\r\nb.\"TransId\",b.\"Line_ID\"," +
                               "\r\nconcat(b.\"Ref1\",concat(' ', b.\"Ref2\"))," +
                               "\r\nb.\"LineMemo\",b.\"Debit\",b.\"Credit\"," +
                               "\r\na.\"LocTotal\"order by a.\"RefDate\"," +
                               "\r\na.\"Number\",b.\"TransId\"," +
                               "\r\nb.\"Line_ID\"";*/
                    queryProduct = "select d.\"U_Sucursal\" as \"sucursal\"," +
                                   "\r\nd.\"PrjCode\" as \"codigo_proyecto\" ," +
                                   "\r\nc.\"FormatCode\" as \"cuenta\"," +
                                   "\r\nc.\"AcctCode\" as \"codigo_cuenta\"," +
                                   "\r\nc.\"AcctName\" as \"nombre_cuenta\"," +
                                   "\r\na.\"RefDate\" as \"fecha\"," +
                                   "\r\na.\"Number\" as \"numero_comprobante\"," +
                                   "\r\nb.\"TransId\" as \"numero_transaccion\"," +
                                   "\r\nb.\"Line_ID\" as \"linea_transaccion\"," +
                                   "\r\nconcat(b.\"Ref1\"," +
                                   "concat(' ', b.\"Ref2\")) as \"referencia\"," +
                                   "\r\nb.\"LineMemo\" as \"descripcion\"," +
                                   "\r\nb.\"Debit\" as \"debe\"," +
                                   "\r\nb.\"Credit\" as \"haber\"," +
                                   "\r\n(b.\"Debit\"-b.\"Credit\") as \"monto_total\"" +
                                   "\r\nfrom ucatolica.ojdt a" +
                                   "\r\ninner join ucatolica.jdt1 b" +
                                   "\r\non a.\"TransId\" = b.\"TransId\"" +
                                   "\r\ninner join   ucatolica.oact c\r\non b.\"Account\" = " +
                                   "c.\"AcctCode\"\r\nleft join ucatolica.oprj d\r\non " +
                                   "b.\"Project\" = d.\"PrjCode\"\r\nwhere a.\"RefDate\"" +
                                   " between '" + initDate + "' and '" + endDate + "'" +
                                   "\r\nand d.\"U_Sucursal\" = '" + regional + "'" +
                                   "\r\nand (c.\"GroupMask\" " +
                                   "like '4%'  \r\nor c.\"GroupMask\" like '5%')\r\nand d.\"PrjCode\"" +
                                   " in ('L4170','L4102','L4093','L4095','L4100','L4290','L4092',\r\n'L4099','L4094','L4101','L4097','L4579','L4599','L4600','L5489','L4959','L4801',\r\n'L4809','L5490','L5651','L5649','L5679','L5699','C3263','C4116','C3283','C3433',\r\n'S3073','S3053','S10002')" +
                                   "\r\ngroup by d.\"U_Sucursal\",\r\nd.\"PrjCode\",c.\"FormatCode\"," +
                                   "\r\nc.\"AcctCode\",c.\"AcctName\",\r\na.\"RefDate\",a.\"Number\"," +
                                   "\r\nb.\"TransId\",b.\"Line_ID\",\r\nconcat(b.\"Ref1\"," +
                                   "concat(' ', b.\"Ref2\")),\r\nb.\"LineMemo\",b.\"Debit\"," +
                                   "b.\"Credit\",\r\na.\"LocTotal\"\r\norder by \r\nd.\"PrjCode\"," +
                                   "\r\na.\"RefDate\",\r\na.\"Number\",b.\"TransId\"," +
                                   "\r\nb.\"Line_ID\"";
                }
                else
                {
                    queryProduct = "select d.\"U_Sucursal\" as \"sucursal\"," +
                                   "\r\nd.\"PrjCode\" as \"codigo_proyecto\" ," +
                                   "\r\nc.\"FormatCode\" as \"cuenta\"," +
                                   "\r\nc.\"AcctCode\" as \"codigo_cuenta\"," +
                                   "\r\nc.\"AcctName\" as \"nombre_cuenta\"," +
                                   "\r\na.\"RefDate\" as \"fecha\"," +
                                   "\r\na.\"Number\" as \"numero_comprobante\"," +
                                   "\r\nb.\"TransId\" as \"numero_transaccion\"," +
                                   "\r\nb.\"Line_ID\" as \"linea_transaccion\"," +
                                   "\r\nconcat(b.\"Ref1\"," +
                                   "concat(' ', b.\"Ref2\")) as \"referencia\"," +
                                   "\r\nb.\"LineMemo\" as \"descripcion\"," +
                                   "\r\nb.\"Debit\" as \"debe\"," +
                                   "\r\nb.\"Credit\" as \"haber\"," +
                                   "\r\n(b.\"Debit\"-b.\"Credit\") as \"monto_total\"" +
                                   "\r\nfrom ucatolica.ojdt a" +
                                   "\r\ninner join ucatolica.jdt1 b" +
                                   "\r\non a.\"TransId\" = b.\"TransId\"" +
                                   "\r\ninner join   ucatolica.oact c\r\non b.\"Account\" = " +
                                   "c.\"AcctCode\"\r\nleft join ucatolica.oprj d\r\non " +
                                   "b.\"Project\" = d.\"PrjCode\"\r\nwhere a.\"RefDate\"" +
                                   " between '" + initDate + "' and '" + endDate + "'\r\nand (c.\"GroupMask\" " +
                                   "like '4%'  \r\nor c.\"GroupMask\" like '5%')\r\nand d.\"PrjCode\"" +
                                   " in ('L4170','L4102','L4093','L4095','L4100','L4290','L4092',\r\n'L4099','L4094','L4101','L4097','L4579','L4599','L4600','L5489','L4959','L4801',\r\n'L4809','L5490','L5651','L5649','L5679','L5699','C3263','C4116','C3283','C3433',\r\n'S3073','S3053','S10002')" +
                                   "\r\ngroup by d.\"U_Sucursal\",\r\nd.\"PrjCode\",c.\"FormatCode\"," +
                                   "\r\nc.\"AcctCode\",c.\"AcctName\",\r\na.\"RefDate\",a.\"Number\"," +
                                   "\r\nb.\"TransId\",b.\"Line_ID\",\r\nconcat(b.\"Ref1\"," +
                                   "concat(' ', b.\"Ref2\")),\r\nb.\"LineMemo\",b.\"Debit\"," +
                                   "b.\"Credit\",\r\na.\"LocTotal\"\r\norder by \r\nd.\"PrjCode\"," +
                                   "\r\na.\"RefDate\",\r\na.\"Number\",b.\"TransId\"," +
                                   "\r\nb.\"Line_ID\"";
                }
            }


            var rawresult = _context.Database.SqlQuery<FRICAInfo>(queryProduct).ToList();
            var formatedData = rawresult.Select(x => new
            {

                x.sucursal,
                x.codigo_proyecto,
                x.cuenta,
                x.codigo_cuenta,
                x.nombre_cuenta,
                fecha = x.fecha.ToString("dd/MM/yyyy"),
                x.numero_comprobante,
                x.numero_transaccion,
                x.linea_transaccion,
                x.referencia,
                x.descripcion,
                debe = Convert.ToSingle(x.debe),
                haber = Convert.ToSingle(x.haber),
                monto_total = Convert.ToSingle(x.monto_total),

            });
            queryProduct = "";
            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/RegionalesFRICA/")]
        public IHttpActionResult RegionalesFRICA()
        {
            var query = "select  \r\n d.\"U_Sucursal\" as \"regional\"\r\n from  ucatolica.oprj d\r\n where d.\"PrjCode\" in ('L4170','L4102','L4093','L4095','L4100','L4290','L4092',\r\n'L4099','L4094','L4101','L4097','L4579','L4599','L4600','L5489','L4959','L4801',\r\n'L4809','L5490','L5651','L5649','L5679','L5699','C3263','C4116','C3283','C3433',\r\n'S3073','S3053','S10002')\r\ngroup by\r\nd.\"U_Sucursal\" ";
            var rawresult = _context.Database.SqlQuery<FRICA>(query).ToList();
            var formatedData = rawresult.Select(x => new
            {
                x.regional
            });
            var aux = "  select  \r\n 'Todas' as \"regional\" from dummy";
            var auxresult = _context.Database.SqlQuery<FRICAFake>(aux).ToList();
            var formatedAux = auxresult.Select(x => new
            {
                x.regional
            });
            return Ok(formatedAux.Concat(formatedData));
        }
        [HttpGet]
        [Route("api/FiltroRegionalesFRICA/{id}")]
        public IHttpActionResult FiltroRegionalesFRICA(string id)
        {
            var query = "";
            if (id != "Todas")
            {
                query = "  select  \r\n d.\"U_Sucursal\" as \"sucursal\", " +
                        "\r\n d.\"PrjCode\" as \"codigo_proyecto\"," +
                        "\r\n d.\"PrjName\" as \"nombre_proyecto\"" +
                        "\r\n from  ucatolica.oprj d\r\n where d.\"PrjCode\" in " +
                        "('L4170','L4102','L4093','L4095','L4100','L4290','L4092',\r\n" +
                        "'L4099','L4094','L4101','L4097','L4579','L4599','L4600','L5489'," +
                        "'L4959','L4801',\r\n'L4809','L5490','L5651','L5649','L5679','L5699'," +
                        "'C3263','C4116','C3283','C3433',\r\n'S3073','S3053','S10002')" +
                        " and d.\"U_Sucursal\"='" + id +"'"+
                        "\r\ngroup by\r\nd.\"U_Sucursal\", \r\n d.\"PrjCode\",\r\n " +
                        "d.\"PrjName\",\r\n d.\"ValidTo\",\r\n d.\"ValidFrom\"\r\n " +
                        "order by\r\n d.\"PrjCode\"\r\n ";
            }
            else
            {
                query = "  select  \r\n d.\"U_Sucursal\" as \"sucursal\", \r\n" +
                        " d.\"PrjCode\" as \"codigo_proyecto\",\r\n d.\"PrjName\" " +
                        "as \"nombre_proyecto\"\r\n from  ucatolica.oprj d" +
                        "\r\n where d.\"PrjCode\" in ('L4170','L4102','L4093','L4095','L4100'," +
                        "'L4290','L4092',\r\n'L4099','L4094','L4101','L4097','L4579','L4599'," +
                        "'L4600','L5489','L4959','L4801',\r\n'L4809','L5490','L5651','L5649'," +
                        "'L5679','L5699','C3263','C4116','C3283','C3433',\r\n'S3073','S3053'," +
                        "'S10002')\r\ngroup by\r\nd.\"U_Sucursal\", \r\n d.\"PrjCode\",\r\n " +
                        "d.\"PrjName\",\r\n d.\"ValidTo\",\r\n d.\"ValidFrom\"\r\n order by" +
                        "\r\n d.\"PrjCode\"\r\n ";
            }

            var aux = "select 'Todos' \"codigo_proyecto\" ," +
                      "\r\n'Todos' \"nombre_proyecto\" ," +
                      "\r\n'Todos' \"regional\" " +
                      "\r\n from Dummy";
            var rawresult = _context.Database.SqlQuery<FRICA>(query).ToList();
            var auxresult = _context.Database.SqlQuery<FRICAFake>(aux).ToList();
            var formatedData = rawresult.Select(x => new
            {
                nombre = x.codigo_proyecto + " - " + x.nombre_proyecto,
                x.codigo_proyecto,
                x.nombre_proyecto,
                x.regional
            });
            var formatedAux = auxresult.Select(x => new
            {
                nombre = x.codigo_proyecto + " - " + x.nombre_proyecto,
                x.codigo_proyecto,
                x.nombre_proyecto,
                x.regional,
            });
            return Ok(formatedAux.Concat(formatedData));
        }
        [HttpGet]
        [Route("api/NameFRICA/{id}")]
        public IHttpActionResult NameFRICA(string id)
        {
            var queryP = "";
            if (id == "Todos")
            {
                queryP =
                    "select 'Todos' \"PROYECTO_CODIGO\", " +
                    "\r\n'Todos' \"proyecto_nombre\"," +
                    "\r\n'Todos' \"regional\"\r\nfrom \"DUMMY\"";
            }
            else
            {
                queryP =
                    "select op.\"PrjCode\" as \"PROYECTO_CODIGO\"," +
                    "\r\nop.\"PrjName\" as \"proyecto_nombre\", " +
                    "\r\nop.\"U_Sucursal\" as \"regional\"" +
                    "\r\nfrom \"UCATOLICA\".\"OPRJ\" op " +
                    "\r\nwhere op.\"PrjCode\" = '" + id + "'";
            }

            var rawres = _context.Database.SqlQuery<ProjectName>(queryP).ToList();
            var formData = rawres.Select(x => new
            {
                x.PROYECTO_CODIGO,
                x.proyecto_nombre,
                x.regional,
            });
            return Ok(formData);
        }
    }

}
