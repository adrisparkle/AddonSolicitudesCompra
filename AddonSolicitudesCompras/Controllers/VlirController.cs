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
                "('L4655',\r\n'L4654',\r\n'L4653',\r\n'L4652',\r\n'L4651',\r\n'L4650',\r\n'C4209',\r\n'C4210',\r\n'C4211',\r\n'C4212',\r\n'C4213',\r\n'C4214',\r\n'S10003',\r\n'S10004',\r\n'S10005',\r\n'S10006',\r\n'S10008',\r\n'S10007',\r\n'T2753',\r\n'T2754',\r\n'T2755',\r\n'T2756',\r\n'T2757',\r\n'U4769',\r\n'U4772',\r\n'U4760',\r\n'U4761',\r\n'U4762',\r\n'U4771',\r\n'U4770'\r\n)" +
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
                if (regional != "Todos")
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
                                   " in ('L4655',\r\n'L4654',\r\n'L4653',\r\n'L4652',\r\n'L4651'," +
                                   "\r\n'L4650',\r\n'C4209',\r\n'C4210',\r\n'C4211',\r\n'C4212'," +
                                   "\r\n'C4213',\r\n'C4214',\r\n'S10003',\r\n'S10004',\r\n'S10005'," +
                                   "\r\n'S10006',\r\n'S10008',\r\n'S10007',\r\n'T2753',\r\n'T2754'," +
                                   "\r\n'T2755',\r\n'T2756',\r\n'T2757',\r\n'U4769',\r\n'U4772'," +
                                   "\r\n'U4760',\r\n'U4761',\r\n'U4762',\r\n'U4771',\r\n'U4770')" +
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
                                   " between '"+initDate+"' and '"+endDate+"'\r\nand (c.\"GroupMask\" " +
                                   "like '4%'  \r\nor c.\"GroupMask\" like '5%')\r\nand d.\"PrjCode\"" +
                                   " in ('L4655',\r\n'L4654',\r\n'L4653',\r\n'L4652',\r\n'L4651'," +
                                   "\r\n'L4650',\r\n'C4209',\r\n'C4210',\r\n'C4211',\r\n'C4212'," +
                                   "\r\n'C4213',\r\n'C4214',\r\n'S10003',\r\n'S10004',\r\n'S10005'," +
                                   "\r\n'S10006',\r\n'S10008',\r\n'S10007',\r\n'T2753',\r\n'T2754'," +
                                   "\r\n'T2755',\r\n'T2756',\r\n'T2757',\r\n'U4769',\r\n'U4772'," +
                                   "\r\n'U4760',\r\n'U4761',\r\n'U4762',\r\n'U4771',\r\n'U4770')" +
                                   "\r\ngroup by d.\"U_Sucursal\",\r\nd.\"PrjCode\",c.\"FormatCode\"," +
                                   "\r\nc.\"AcctCode\",c.\"AcctName\",\r\na.\"RefDate\",a.\"Number\"," +
                                   "\r\nb.\"TransId\",b.\"Line_ID\",\r\nconcat(b.\"Ref1\"," +
                                   "concat(' ', b.\"Ref2\")),\r\nb.\"LineMemo\",b.\"Debit\"," +
                                   "b.\"Credit\",\r\na.\"LocTotal\"\r\norder by \r\nd.\"PrjCode\"," +
                                   "\r\na.\"RefDate\",\r\na.\"Number\",b.\"TransId\"," +
                                   "\r\nb.\"Line_ID\"";
                }
            }
            

            var rawresult = _context.Database.SqlQuery<VLIRInfo>(queryProduct).ToList();
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
        [Route("api/Regionales/")]
        public IHttpActionResult Regionales()
        {
            var query = "select \"U_Sucursal\" as \"regional\"\r\n from ucatolica.\"OPRJ\"" +
                        "\r\n where \"PrjCode\" in ('L4655',\r\n'L4654',\r\n'L4653'," +
                        "\r\n'L4652',\r\n'L4651',\r\n'L4650',\r\n'C4209',\r\n'C4210'," +
                        "\r\n'C4211',\r\n'C4212',\r\n'C4213',\r\n'C4214',\r\n'S10003'," +
                        "\r\n'S10004',\r\n'S10005',\r\n'S10006',\r\n'S10008'," +
                        "\r\n'S10007',\r\n'T2753',\r\n'T2754',\r\n'T2755',\r\n'T2756'," +
                        "\r\n'T2757',\r\n'U4769',\r\n'U4772',\r\n'U4760',\r\n'U4761'," +
                        "\r\n'U4762',\r\n'U4771',\r\n'U4770')" +
                        "\r\n group by \"U_Sucursal\"\r\n \r\n";
            var rawresult = _context.Database.SqlQuery<VLIR>(query).ToList();
            var formatedData = rawresult.Select(x => new
            {
                x.regional
            });
            return Ok(formatedData);
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
                        "\r\nand p.\"PrjCode\" in " +
                        "('L4655',\r\n'L4654'," +
                        "\r\n'L4653',\r\n'L4652',\r\n'L4651',\r\n'L4650'," +
                        "\r\n'C4209',\r\n'C4210',\r\n'C4211',\r\n'C4212'," +
                        "\r\n'C4213',\r\n'C4214',\r\n'S10003',\r\n'S10004'," +
                        "\r\n'S10005',\r\n'S10006',\r\n'S10008',\r\n'S10007'," +
                        "\r\n'T2753',\r\n'T2754',\r\n'T2755',\r\n'T2756'," +
                        "\r\n'T2757',\r\n'U4769',\r\n'U4772',\r\n'U4760'," +
                        "\r\n'U4761',\r\n'U4762',\r\n'U4771',\r\n'U4770')" +
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
                        "('L4655',\r\n'L4654'," +
                        "\r\n'L4653',\r\n'L4652',\r\n'L4651',\r\n'L4650'," +
                        "\r\n'C4209',\r\n'C4210',\r\n'C4211',\r\n'C4212'," +
                        "\r\n'C4213',\r\n'C4214',\r\n'S10003',\r\n'S10004'," +
                        "\r\n'S10005',\r\n'S10006',\r\n'S10008',\r\n'S10007'," +
                        "\r\n'T2753',\r\n'T2754',\r\n'T2755',\r\n'T2756'," +
                        "\r\n'T2757',\r\n'U4769',\r\n'U4772',\r\n'U4760'," +
                        "\r\n'U4761',\r\n'U4762',\r\n'U4771',\r\n'U4770')" +
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
