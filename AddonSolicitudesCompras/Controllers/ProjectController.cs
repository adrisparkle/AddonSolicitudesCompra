using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AddonSolicitudesCompras.Logic;
using AddonSolicitudesCompras.Models;
using AddonSolicitudesCompras.Models.Auth;

namespace AddonSolicitudesCompras.Controllers
{
    public class ProyectoController : ApiController
    {
        private ApplicationDbContext _context;
        private string dbName = "ucatolica";

        public ProyectoController()
        {

            _context = new ApplicationDbContext();

        }
        [HttpGet]
        [Route("api/ProjectGeneral/{user}")]
        public IHttpActionResult ProjectGeneral(int user)
        {
            FiltroUser us = new FiltroUser(user);
            //convertir precio a float o double y cantidad a int!!
            var queryProduct =
                "select p.\"PrjCode\" as \"codigo_proyecto\"," +
                "\r\np.\"PrjName\" as \"nombre_proyecto\"," +
                "\r\np.\"U_Sucursal\" as \"regional\"," +
                "\r\np.\"U_PEI_PO\" as \"pei_po\"," +
                "\r\np.\"U_UORGANIZA\" as \"unidad_organizacional\"," +
                "\r\np.\"ValidTo\" as \"valido_hasta\", p.\"ValidFrom\" " +
                "\r\nas \"valido_desde\" from  ucatolica.oprj p " +
                "\r\nwhere p.\"Active\" = 'Y' " +
                "\r\nand p.\"ValidTo\" >= current_date  " +
                "\r\ngroup by p.\"PrjCode\"," +
                "\r\np.\"PrjName\", p.\"U_Sucursal\", p.\"U_PEI_PO\", " +
                "\r\np.\"U_UORGANIZA\", p.\"ValidTo\", p.\"ValidFrom\" order by  " +
                "\r\np.\"PrjCode\", p.\"PrjName\"";

            var rawresult = _context.Database.SqlQuery<Project>(queryProduct).ToList();
            var AD = new ADClass();
            var data = AD.FiltrarRegional(new FiltroUser(user), rawresult.AsQueryable()).ToList();
            var formatedData = data.Select(x => new
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
        [Route("api/ProjectInfo/{id}/{initDate}")]
        public IHttpActionResult ProjectInfo(string id, string initDate)
        {
            //convertir precio a float o double y cantidad a int!!
            var query4 = "SELECT\r\nA.FORMATCODE,\r\nA.ACCTCODE,\r\nA.SUCURSAL,\r\nA.ACCTNAME,\r\nA.DIM1,\r\nA.DIM2,\r\nSUM(A.TOTAL_DIM) TOTAL_DIM,\r\nSUM(A.SOLICITADO) SOLICITADO,\r\nSUM(A.COMPROMETIDO) COMPROMETIDO,\r\nSUM(A.EJECUTADO)EJECUTADO,\r\nSUM(A.TOTAL_CUENTA)TOTAL_CUENTA\r\nFROM (\r\n\tSELECT\r\n\tTO_VARCHAR(T2.\"PrjCode\") as \"PROYECTO\",\r\n\tOP.\"PrjName\" as \"NOMBRE\",\r\n\tT5.\"FormatCode\" as \"FORMATCODE\",\r\n\tT5.\"AcctCode\" as \"ACCTCODE\",\r\n\tT5.\"AcctName\" as \"ACCTNAME\",\r\n\tT4.\"U_OCRCODE1\" AS DIM1,\r\n\tT4.\"U_OCRCODE2\" AS DIM2,\r\n\tIFNULL(SUM(T0.\"U_DEBELOC\"-T0.\"U_CREDLOC\"),0) AS TOTAL_CUENTA,\r\n\tIFNULL(SUM(T4.\"U_DEBELOC\"-T4.\"U_CREDLOC\"),0) AS TOTAL_DIM,\r\n\t0 SOLICITADO,\r\n\t0 COMPROMETIDO,\r\n\t0 EJECUTADO,\r\n\tRIGHT(T5.\"FormatCode\",3) \"SUCURSAL\"\r\n\tFROM ucatolica.\"@PR_PRESUP\" T0 \r\n\tleft JOIN ucatolica.\"OBGS\" T2 \r\n\tON T0.\"U_INSTANCE\" = T2.\"AbsId\" \r\n\tleft JOIN ucatolica.\"@PR_PRESUP_NR\" T4 \r\n\tON T4.\"U_PRESUP_ID\"=T0.\"Code\" \r\n\tand T4.\"U_INSTANCE\"=T0.\"U_INSTANCE\"\r\n\tleft JOIN ucatolica.\"OACT\" T5 \r\n\tON T5.\"AcctCode\"=T0.\"U_ACCTCODE\" \r\n\tleft join  ucatolica.\"OPRJ\" OP\r\n\ton T2.\"PrjCode\" = OP.\"PrjCode\"\r\n\tGROUP BY \r\n\tT0.\"U_FINANCYEAR\", \r\n\tT0.\"U_ACCTCODE\", \r\n\tT2.\"FinancYear\",\r\n\tT4.\"U_OCRCODE1\",\r\n\tT4.\"U_OCRCODE2\",\r\n\tT5.\"AcctCode\",\r\n\tT5.\"FormatCode\",\r\n\tT5.\"AcctName\",\r\n\tT2.\"PrjCode\",\r\n\tOP.\"PrjName\"\r\n\tUNION\r\n\tSELECT \r\n\tTO_VARCHAR(PJ.\"PrjCode\") as \"PROYECTO\",\r\n\tPJ.\"PrjName\" as \"NOMBRE\",\r\n\tT0.\"FormatCode\",\r\n\tT1.\"ACCTCODE\",\r\n\tT0.\"AcctName\",\r\n\tT1.\"DIM1\",\r\n\tT1.\"DIM2\",\r\n\t0,\r\n\t0,\r\n\tSUM(T1.\"SOLICITADO\"),\r\n\t0,\r\n\t0,\r\n\tRIGHT(T0.\"FormatCode\",3) \"SUCURSAL\"\r\n    FROM ucatolica.SOLICITADO T1\r\n    INNER JOIN ucatolica.\"OACT\" T0 \r\n    ON T0.\"AcctCode\"=T1.\"ACCTCODE\"\r\n    left JOIN ucatolica.\"JDT1\" J1\r\n    on J1.\"Account\" = T0.\"AcctCode\"\r\n    left JOIN ucatolica.\"OJDT\" J0\r\n    on J0.\"TransId\" = J1.\"TransId\"\r\n    left JOIN ucatolica.\"OPRJ\" PJ\r\n    on TO_VARCHAR(PJ.\"PrjCode\") = TO_VARCHAR(J1.\"Project\")\r\n    where TO_VARCHAR(PJ.\"PrjCode\") is not null \r\n\tand TO_VARCHAR(PJ.\"PrjCode\") != ''\r\n    GROUP BY \r\n    T0.\"FormatCode\",\r\n    T1.\"ACCTCODE\",\r\n    T0.\"AcctName\",\r\n    T1.\"DIM1\",\r\n    T1.\"DIM2\",\r\n   TO_VARCHAR(PJ.\"PrjCode\"),\r\n   PJ.\"PrjName\"\r\n\tUNION\r\n\tSELECT \r\n\tTO_VARCHAR(PJ.\"PrjCode\") as \"PROYECTO\",\r\n\tPJ.\"PrjName\" as \"NOMBRE\",\r\n\tT0.\"FormatCode\",\r\n\tT1.\"ACCTCODE\",\r\n\tT0.\"AcctName\",\r\n\tT1.\"DIM1\",\r\n\tT1.\"DIM2\",\r\n\t0,\r\n\t0,\r\n\t0,\r\n\tSUM(T1.COMPROMETIDO),\r\n\t0,\r\n\tRIGHT(T0.\"FormatCode\",3) \"SUCURSAL\"\r\n    FROM ucatolica.COMPROMETIDO T1\r\n    INNER JOIN ucatolica.\"OACT\" T0 \r\n    ON T0.\"AcctCode\"=T1.\"ACCTCODE\"\r\n    left JOIN ucatolica.\"JDT1\" J1\r\n    on J1.\"Account\" = T0.\"AcctCode\"\r\n    left JOIN ucatolica.\"OJDT\" J0\r\n    on J0.\"TransId\" = J1.\"TransId\"\r\n    left JOIN ucatolica.\"OPRJ\" PJ\r\n    on TO_VARCHAR(PJ.\"PrjCode\") = TO_VARCHAR(J1.\"Project\")\r\n    where TO_VARCHAR(PJ.\"PrjCode\") is not null \r\n\tand TO_VARCHAR(PJ.\"PrjCode\") != ''\r\n    GROUP BY \r\n    T0.\"FormatCode\",\r\n    T1.\"ACCTCODE\",\r\n    T0.\"AcctName\",\r\n    T1.\"DIM1\",\r\n    T1.\"DIM2\",\r\n    TO_VARCHAR(PJ.\"PrjCode\"),\r\n    PJ.\"PrjName\"\r\n    UNION\r\n\tSELECT \r\n\tTO_VARCHAR(PJ.\"PrjCode\") as \"PROYECTO\",\r\n\tPJ.\"PrjName\" as \"NOMBRE\",\r\n\tT0.\"FormatCode\",\r\n\tT1.\"ACCTCODE\",\r\n\tT0.\"AcctName\",\r\n\tT1.\"DIM1\",\r\n\tT1.\"DIM2\",\r\n\t0,\r\n\t0,\r\n\t0,\r\n\t0,\r\n\tSUM(T1.EJECUTADO),\r\n\tRIGHT(T0.\"FormatCode\",3) \"SUCURSAL\"\r\n    FROM ucatolica.EJECUTADO T1\r\n    INNER JOIN ucatolica.\"OACT\" T0 \r\n    ON T0.\"AcctCode\"=T1.\"ACCTCODE\"\r\n    left JOIN ucatolica.\"JDT1\" J1\r\n    on J1.\"Account\" = T0.\"AcctCode\"\r\n    left JOIN ucatolica.\"OJDT\" J0\r\n    on J0.\"TransId\" = J1.\"TransId\"\r\n    left JOIN ucatolica.\"OPRJ\" PJ\r\n    on TO_VARCHAR(PJ.\"PrjCode\") = TO_VARCHAR(J1.\"Project\")\r\n    WHERE T1.\"DIM1\" NOT IN (select \"PrcCode\" from ucatolica.\"OPRC\" WHERE \"DimCode\"=1 and \"U_ELIMINAR_REP\"='Y')\r\n\tAND T1.\"DIM2\" NOT IN (select \"PrcCode\" from ucatolica.\"OPRC\" WHERE \"DimCode\"=2 and \"U_ELIMINAR_REP\"='Y')\r\n\tand TO_VARCHAR(PJ.\"PrjCode\") is not null \r\n\tand TO_VARCHAR(PJ.\"PrjCode\") != ''\r\n    GROUP BY \r\n    T0.\"FormatCode\",\r\n    T1.\"ACCTCODE\",\r\n    T0.\"AcctName\",\r\n    T1.\"DIM1\",\r\n    T1.\"DIM2\",\r\n    TO_VARCHAR(PJ.\"PrjCode\"),\r\n    PJ.\"PrjName\") A\r\n    WHERE (A.TOTAL_CUENTA + A.TOTAL_DIM + A.SOLICITADO + A.COMPROMETIDO + A.EJECUTADO)<>0\r\n    AND (A.PROYECTO != NULL OR A.PROYECTO != '')\r\n    AND A.PROYECTO = '" + id + "'\r\n    GROUP BY A.PROYECTO, A.NOMBRE, A.FORMATCODE,A.ACCTCODE,A.DIM1, A.DIM2,A.ACCTNAME,A.SUCURSAL\r\n\tORDER BY 1,4,5;";
            var query = "call ucatolica.SP_PRS_RPTANUALACUMULADO_PROY('"+id+"','"+initDate+"','Todos')";
            //var rawresult1 = _context.Database.SqlQuery<ProjectInfo>(query).ToList();
            var rawresult1 = _context.Database.SqlQuery<ProyPrueba>(query).ToList();
            var formatedData = rawresult1.Select(x => new
            {
                x.FORMATCODE,
                x.ACCTCODE,
                x.ACCTNAME,
                x.DIM1,
                x.DIM2,
                TOTAL_CUENTA = Convert.ToSingle(x.TOTAL_CUENTA),
                TOTAL_DIM = Convert.ToSingle(x.TOTAL_DIM),
                SOLICITADO = Convert.ToSingle(x.SOLICITADO),
                COMPROMETIDO = Convert.ToSingle(x.COMPROMETIDO),
                EJECUTADO = Convert.ToSingle(x.EJECUTADO),
                x.SUCURSAL,
                x.PrjCode,
                x.PrjName
            });

            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/ProjectName/{id}")]
        public IHttpActionResult ProjectName(string id)
        {
            var queryP =
                "select op.\"PrjCode\" as \"PROYECTO_CODIGO\"," +
                "\r\nop.\"PrjName\" as \"proyecto_nombre\", " +
                "\r\nop.\"ValidFrom\" as \"valido_desde\"," +
                "\r\nop.\"ValidTo\" as \"valido_hasta\"," +
                "\r\nop.\"U_PEI_PO\" as \"pei_po\"," +
                "\r\nop.\"U_Sucursal\" as \"regional\"" +
                "\r\nfrom \"UCATOLICA\".\"OPRJ\" op " +
                "\r\nwhere op.\"PrjCode\" = '" + id + "'" +
                " group by " +
                "op.\"PrjCode\"," +
                "\r\nop.\"PrjName\", " +
                "\r\nop.\"ValidFrom\"," +
                "\r\nop.\"ValidTo\"," +
                "\r\nop.\"U_PEI_PO\"," +
                "\r\nop.\"U_Sucursal\"";

            var rawres = _context.Database.SqlQuery<ProjectName>(queryP).ToList();
            var formData = rawres.Select(x => new
            {
                x.PROYECTO_CODIGO,
                x.proyecto_nombre,
                x.pei_po,
                x.regional,
                valido_hasta = x.valido_hasta.ToString("dd/MM/yyyy"),
                valido_desde = x.valido_desde.ToString("dd/MM/yyyy"),
            });
            return Ok(formData);
        }
        [HttpGet]
        [Route("api/ProjectInfoDetail/{account}/{project}/{initDate}/{endDate}")]
        public IHttpActionResult ProjectInfoDetail(string account, string project, string initDate, string endDate)
        {
            var queryP =
                "select  j1.\"RefDate\" as \"fecha\", " +
                "\r\n j0.\"TransId\" as \"numero_transaccion\"," +
                "\r\n j1.\"Line_ID\" as \"numero_linea\",  " +
                "\r\n j0.\"Memo\" as \"glosa\",  " +
                "\r\n j1.\"Debit\"-j1.\"Credit\" as \"monto\"" +
                "\r\n from ucatolica.\"OJDT\" j0   " +
                "\r\n inner join ucatolica.\"JDT1\" j1 " +
                "\r\n on j1.\"TransId\" = j0.\"TransId\" " +
                "\r\n inner join ucatolica.\"OACT\" oa" +
                "\r\n on oa.\"AcctCode\" = j1.\"Account\"" +
                "\r\n where j1.\"Account\" = '"+account+"' " +
                " and j1.\"Project\" = '"+project+"'" +
                " and j1.\"RefDate\" between '" + initDate + "' and '"+endDate+"'" +
                "\r\n group by j1.\"RefDate\", " +
                "\r\n j0.\"TransId\", " +
                "\r\n j1.\"Line_ID\", \r\n j0.\"Memo\", " +
                "\r\n j1.\"Debit\" - j1.\"Credit\", " +
                "\r\noa.\"FormatCode\", \r\n j1.\"ProfitCode\", " +
                "\r\n j1.\"Project\" \r\n order by \r\n j1.\"RefDate\", " +
                "\r\n j0.\"TransId\", \r\n j1.\"Line_ID\"";
            var querytot =
                "select  '' as \"fecha\", \r\n'' as \"numero_transaccion\", " +
                " \r\n'' as \"numero_linea\",  \r\n'Total' as \"glosa\",  " +
                "\r\n sum(j1.\"Debit\"-j1.\"Credit\") as \"monto\" " +
                "\r\n from ucatolica.\"OJDT\" j0\r\n inner join ucatolica.\"JDT1\" j1 " +
                "\r\n on j1.\"TransId\" = j0.\"TransId\" \r\n inner join ucatolica.\"OACT\" oa" +
                "\r\n on oa.\"AcctCode\" = j1.\"Account\"\r\n " +
                "where j1.\"Account\" = '"+account+"'" +
                " and j1.\"Project\" = '"+project+"'" +
            " and j1.\"RefDate\" between '" + initDate + "' and '"+endDate+"'";


            var rawres = _context.Database.SqlQuery<ProjectJournal>(queryP).ToList();
            var formData = rawres.Select(x => new
            {
                fecha = x.fecha.ToString("dd/MM/yyyy"),
                x.numero_transaccion,
                x.numero_linea,
                x.glosa,
                x.monto
            });
           var totalQuery = _context.Database.SqlQuery<ProjectJournalFake>(querytot).ToList();
            var totalData = totalQuery.Select(x => new
            {
                x.fecha,
                x.numero_transaccion,
                x.numero_linea,
                x.glosa,
                x.monto
            });
            return Ok(formData.Concat(totalData));
        }
        [HttpGet]
        [Route("api/JournalReportHead/{account}/{project}")]
        public IHttpActionResult JournalReportHead(string account, string project)
        {
            var queryP =
                "select  "+
                "\r\noa.\"FormatCode\" as  \"cuenta\"," +
                "\r\noa.\"AcctName\" as \"nombre_cuenta\"," +
                "\r\n j1.\"Project\" as  \"codigo_proyecto\"," +
                "\r\n op.\"PrjName\" as \"nombre_proyecto\"" +
                "\r\n from ucatolica.\"OJDT\" j0" +
                "\r\n inner join ucatolica.\"JDT1\" j1 " +
                "\r\n on j1.\"TransId\" = j0. \"TransId\" " +
                "\r\n inner join ucatolica.\"OACT\" oa" +
                "\r\n on oa.\"AcctCode\" = j1.\"Account\"" +
                "\r\n inner join ucatolica.\"OPRJ\" op" +
                "\r\n on op.\"PrjCode\" = j1.\"Project\"" +
                "\r\n where j1.\"Account\" = '"+account+"' " +
                "and j1. \"Project\" = '"+project+"'  " +
                "\r\ngroup by \r\noa.\"FormatCode\"," +
                "\r\noa.\"AcctName\", " +
                "\r\n j1.\"Project\",\r\n op.\"PrjName\"";

            var rawres = _context.Database.SqlQuery<JournalReportHead>(queryP).ToList();
            var formData = rawres.Select(x => new
            {
                x.codigo_proyecto,
                x.nombre_proyecto,
                x.cuenta,
                x.uo,
                x.nombre_cuenta,
                fechaInicio = x.fechaInicio.ToString("dd/MM/yyyy"),
                fechaFin = x.fechaFin.ToString("dd/MM/yyyy"),
            });
            return Ok(formData);
        }

    }
}