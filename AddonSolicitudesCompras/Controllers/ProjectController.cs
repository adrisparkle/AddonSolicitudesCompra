using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AddonSolicitudesCompras.Models;

namespace AddonSolicitudesCompras.Controllers
{
    public class ProyectoController : ApiController
    {
        private ApplicationDbContext _context;

        public ProyectoController()
        {

            _context = new ApplicationDbContext();

        }

        // GET api/Contract
        [HttpGet]
        [Route("api/ProjectGeneral/")]
        public IHttpActionResult ProjectGeneral()
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct =
                "select\r\np.\"PrjCode\" as \"codigo_proyecto\",\r\np.\"PrjName\" as \"nombre_proyecto\",\r\np.\"U_Sucursal\" as \"regional\",\r\np.\"U_PEI_PO\" as \"pei_po\",\r\np.\"U_UORGANIZA\" as \"unidad_organizacional\",\r\np.\"ValidTo\" as \"valido_hasta\",\r\np.\"ValidFrom\" as \"valido_desde\"\r\nfrom ucatolica.oprj p\r\nwhere p.\"Active\" = 'Y'\r\nand p.\"ValidTo\" >= current_date\r\n group by\r\np.\"PrjCode\",\r\np.\"PrjName\",\r\np.\"U_Sucursal\",\r\np.\"U_PEI_PO\",\r\np.\"U_UORGANIZA\",\r\np.\"ValidTo\",\r\np.\"ValidFrom\"\r\norder by \r\np.\"PrjCode\",\r\np.\"PrjName\"";

            var rawresult = _context.Database.SqlQuery<Project>(queryProduct).ToList();
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
        [Route("api/ProjectInfo/{id}")]
        public IHttpActionResult ProjectInfo(string id)
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct =
                "SELECT\r\nA.\"PROYECTO_CODIGO\",\r\nB.\"PrjName\" as \"proyecto_nombre\",\r\nA.\"SUCURSAL\" as \"regional\", \r\nA.\"FORMATCODE\" as \"cuenta\", \r\nA.\"ACCTCODE\" as \"codigo_cuenta\", \r\nA.\"ACCTNAME\" as \"nombre_cuenta\",  \r\nA.DIM1 as \"unidad_organizacional\", \r\nA.DIM2 as \"pei_po\", \r\nSUM(A.TOTAL_CUENTA) as \"total_cuenta\", \r\nSUM(A.TOTAL_DIM) as \"total_dim\", \r\nSUM(A.EJECUTADO) as \"ejecutado\"\r\nFROM\r\n((SELECT \r\n\tRIGHT(T5.\"FormatCode\",3) \"SUCURSAL\", \r\n\tT5.\"FormatCode\" \"FORMATCODE\",\r\n\tT5.\"AcctCode\" \"ACCTCODE\", \r\n\tT5.\"AcctName\" \"ACCTNAME\",\r\n\tT5.\"GroupMask\",\r\n\tT4.\"U_OCRCODE1\" AS DIM1,\r\n\tT4.\"U_OCRCODE2\" AS DIM2,\r\n\tT6.\"PrjCode\" as PROYECTO_CODIGO,\r\n\tIFNULL(MAP\r\n\t\t(T5.\"GroupMask\",\r\n\t\t4,\r\n\t\tSUM(T0.\"U_CREDLOC\" - T0.\"U_DEBELOC\"),\r\n\t\tSUM(T0.\"U_DEBELOC\"- T0.\"U_CREDLOC\")),\r\n\t\t0) AS TOTAL_CUENTA,\r\n\tIFNULL(MAP\r\n\t\t(T5.\"GroupMask\",\r\n\t\t4,\r\n\t\tSUM(T4.\"U_CREDLOC\" - T4.\"U_DEBELOC\"),\r\n\t\tSUM(T4.\"U_DEBELOC\" -T4.\"U_CREDLOC\")),\r\n\t\t0) AS TOTAL_DIM,\r\n\t0 EJECUTADO\r\n\tFROM \"UCATOLICA\".\"@PR_PRESUP\" T0 \r\n\tINNER JOIN \"UCATOLICA\".\"OBGS\" T2 \r\n\t\tON T0.\"U_INSTANCE\" = T2.\"AbsId\" \r\n\tINNER JOIN \"UCATOLICA\".\"@PR_PRESUP_NR\" T4 \r\n\t\tON T4.\"U_PRESUP_ID\"=T0.\"Code\" \r\n\t\tand T4.\"U_INSTANCE\"=T0.\"U_INSTANCE\"\r\n\tINNER JOIN \"UCATOLICA\".\"OACT\" T5 \r\n\t\tON T5.\"AcctCode\"=T0.\"U_ACCTCODE\" \r\n\t\tAND T5.\"Budget\"='Y'\r\n\tINNER JOIN \"UCATOLICA\".\"OPRJ\" T6\r\n\t\tON T6.\"PrjCode\" = T2.\"PrjCode\"\r\n\tWHERE YEAR(T2.\"FinancYear\")= YEAR(TO_DATE('01/01/2019','DD/MM/YYYY')) \r\n\tAND T2.\"IsMain\"='N'\r\n\tGROUP BY \r\n\t\tRIGHT(T5.\"FormatCode\",3), \r\n\t\tT5.\"FormatCode\",\r\n\t\tT5.\"AcctCode\", \r\n\t\tT5.\"AcctName\" , \r\n\t\tT5.\"GroupMask\", \r\n\t\tT4.\"U_OCRCODE1\" ,\r\n\t\tT6.\"PrjCode\",\r\n\t\tT4.\"U_OCRCODE2\")\r\nUNION\r\n(SELECT \r\n\tRIGHT(T1.\"FormatCode\",3) \"SUCURSAL\", \r\n\tT1.\"FormatCode\", \r\n\tT0.\"Account\" \"ACCTCODE\",  \r\n\tT1.\"AcctName\" \"ACCTNAME\", \r\n\tT1.\"GroupMask\", \r\n\tT0.\"ProfitCode\" \"DIM1\", \r\n\tT0.\"OcrCode2\" \"DIM2\",\r\n\tT7.\"PrjCode\" as PROYECTO_CODIGO,\r\n\t0 TOTAL_CUENTA, \r\n\t0 TOTAL_DIM, \r\n\tIFNULL( MAP\r\n\t\t(T1.\"GroupMask\",\r\n\t\t4,\r\n\t\tSUM(T0.\"Credit\"-T0.\"Debit\"),\r\n\t\tSUM(T0.\"Debit\"-T0.\"Credit\"))\r\n\t\t,0) AS \"EJECUTADO\"\r\nFROM \"UCATOLICA\".\"JDT1\" T0\r\nINNER JOIN \"UCATOLICA\".\"OACT\" T1 \r\n\tON T1.\"AcctCode\" = T0.\"Account\" \r\n\tand T1.\"Budget\"='Y'\r\nINNER JOIN \"UCATOLICA\".\"OPRJ\" T7\r\n\ton T0.\"Project\" = T7.\"PrjCode\"\r\nWHERE IFNULL(T0.\"ProfitCode\", '') LIKE '%' \r\nAND IFNULL(T0.\"OcrCode2\", '') LIKE '%'\r\nAND T0.\"RefDate\">= TO_DATE('01/01/2019','DD/MM/YYYY')\r\nAND T0.\"RefDate\"<= TO_DATE('09/09/2019','DD/MM/YYYY')\r\nGROUP BY \r\n\tRIGHT(T1.\"FormatCode\",3), \r\n\tT1.\"FormatCode\",\r\n\tT0.\"Account\", \r\n\tT1.\"AcctName\", \r\n\tT1.\"GroupMask\", \r\n\tT0.\"ProfitCode\",\r\n\tT7.\"PrjCode\", \r\n\tT0.\"OcrCode2\")) A\r\n\tINNER JOIN \"UCATOLICA\".\"OPRJ\" B\r\n\tON B.\"PrjCode\" = A.\"PROYECTO_CODIGO\"\r\n\twhere B.\"PrjCode\" = '" +
                id +
                "'\r\n GROUP BY \r\n\tA.\"SUCURSAL\", \r\n\tA.\"FORMATCODE\", \r\n\tA.\"ACCTCODE\", \r\n\tA.\"ACCTNAME\", \r\n\tB.\"PrjName\", \r\n\tA.\"GroupMask\",\r\n\tA.\"PROYECTO_CODIGO\",\r\n\tA.DIM1, \r\n\tA.DIM2\r\nORDER BY \r\n\tA.\"SUCURSAL\", \r\n\tA.\"FORMATCODE\",\r\n\tA.DIM1,\r\n\tA.DIM2";

            var rawresult = _context.Database.SqlQuery<ProjectInfo>(queryProduct).ToList();
            var formatedData = rawresult.Select(x => new
            {
                x.PROYECTO_CODIGO,
                x.proyecto_nombre,
                x.sucursal,
                x.cuenta,
                x.codigo_cuenta,
                x.nombre_cuenta,
                x.unidad_organizacional,
                x.pei_po,
                total_cuenta = Convert.ToSingle(x.total_cuenta),
                total_dim = Convert.ToSingle(x.total_dim),
                ejecutado = Convert.ToSingle(x.ejecutado),

            });
            return Ok(formatedData);
        }

        [HttpGet]
        [Route("api/ProjectVLIR/")]
        public IHttpActionResult ProjectVLIR()
        {
            //convertir precio a float o double y cantidad a int!!
            var queryProduct =
                "select p.\"PrjCode\" as \"codigo_proyecto\", \r\np.\"PrjName\" as \"nombre_proyecto\", \r\np.\"U_Sucursal\" as \"regional\", \r\np.\"ValidTo\" as \"valido_hasta\", \r\np.\"ValidFrom\" as \"valido_desde\" \r\nfrom ucatolica.oprj p \r\nwhere p.\"Active\" = 'Y' \r\nand p.\"PrjCode\" in ('L4655',\r\n'L4654',\r\n'L4653',\r\n'L4652',\r\n'L4651',\r\n'L4650',\r\n'C4209',\r\n'C4210',\r\n'C4211',\r\n'C4212',\r\n'C4213',\r\n'C4214',\r\n'S10003',\r\n'S10004',\r\n'S10005',\r\n'S10006',\r\n'S10008',\r\n'S10007',\r\n'T2753',\r\n'T2754',\r\n'T2755')\r\ngroup by p.\"PrjCode\", \r\np.\"PrjName\",\r\np.\"U_Sucursal\", \r\np.\"ValidTo\", \r\np.\"ValidFrom\"\r\norder by  p.\"PrjCode\", \r\np.\"PrjName\",\r\np.\"U_Sucursal\", \r\np.\"ValidTo\", \r\np.\"ValidFrom\"";

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
                "select \r\nc.\"FormatCode\" as \"cuenta\",\r\na.\"RefDate\" as \"fecha\",\r\na.\"Number\" as \"numero_comprobante\",\r\nb.\"TransId\" as \"numero_transaccion\",\r\nd.\"U_Sucursal\" as \"sucursal\",\r\nb.\"LineMemo\" as \"descripcion\",\r\na.\"BaseRef\" as \"referencia\",\r\na.\"LocTotal\" as \"monto\",\r\nd.\"PrjCode\" as \"codigo_proyecto\"\r\nfrom ucatolica.ojdt a\r\ninner join ucatolica.jdt1 b\r\non a.\"TransId\" = b.\"TransId\"\r\ninner join ucatolica.oact c\r\non b.\"Account\" = c.\"AcctCode\"\r\nleft join ucatolica.oprj d\r\non b.\"Project\" = d.\"PrjCode\"\r\nleft join ucatolica.\"OPRC\" e\r\non e.\"PrcCode\" = b.\"ProfitCode\"\r\nleft join ucatolica.\"NNM1\" f\r\non a.\"Series\" = f.\"Series\"\r\nwhere b.\"Project\" = '" +
                id +
                "' group by\r\nc.\"FormatCode\",\r\na.\"RefDate\",\r\na.\"Number\",\r\nb.\"TransId\",\r\nd.\"U_Sucursal\",\r\nb.\"LineMemo\",\r\na.\"BaseRef\",\r\na.\"LocTotal\",\r\nd.\"PrjCode\"";

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
