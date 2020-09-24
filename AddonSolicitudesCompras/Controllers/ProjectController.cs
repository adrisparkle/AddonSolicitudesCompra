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
            var query4 = "SELECT A.\"SUCURSAL\", A.\"FORMATCODE\", A.\"ACCTCODE\", A.\"ACCTNAME\", A.\"GroupMask\"" +
                         ", A.DIM1, A.DIM2, OPRJ.\"PrjName\",A.\"PRJCODE\" \"PrjCode\",\r\n\r\nSUM(A.TOTAL_CUENTA) \"TOTAL_CUENTA\", SUM(A.TOTAL_DIM) \"TOTAL_DIM\"" +
                         ", " +
                         "SUM(A.EJECUTADO) \"EJECUTADO\"\r\n\r\nFROM\r\n\r\n(\r\n\r\n(SELECT RIGHT(T5.\"FormatCode\",3) \"SUCURSAL\",T5.\"FormatCode\"" +
                         " \"FORMATCODE\",T5.\"AcctCode\" \"ACCTCODE\",T5.\"AcctName\" \"ACCTNAME\",T5.\"GroupMask\",T4.\"U_OCRCODE1\"" +
                         " AS DIM1,\r\n\r\nT4.\"U_OCRCODE2\" AS DIM2,T2.\"PrjCode\" \"PRJCODE\",IFNULL(MAP(T5.\"GroupMask\",4,SUM(T0.\"U_CREDLOC\"" +
                         " - T0.\"U_DEBELOC\"),SUM(T0.\"U_DEBELOC\"- T0.\"U_CREDLOC\")),0) AS TOTAL_CUENTA,\r\n\r\nIFNULL(MAP(T5.\"GroupMask\",4,SUM(T4.\"U_CREDLOC\"" +
                         " - T4.\"U_DEBELOC\"),SUM(T4.\"U_DEBELOC\" -T4.\"U_CREDLOC\")),0) AS TOTAL_DIM,0 EJECUTADO\r\n\r\nFROM \"UCATOLICA\".\"@PR_PRESUP\"" +
                         " T0\r\n\r\nINNER JOIN \"UCATOLICA\".\"OBGS\" T2 ON T0.\"U_INSTANCE\" = T2.\"AbsId\"\r\n\r\nINNER JOIN \"UCATOLICA\".\"@PR_PRESUP_NR\"" +
                         " T4 ON T4.\"U_PRESUP_ID\"=T0.\"Code\"\r\n\r\nAND T4.\"U_INSTANCE\"=T0.\"U_INSTANCE\"\r\n\r\nINNER JOIN \"UCATOLICA\".\"OACT\" T5 " +
                         "ON T5.\"AcctCode\"=T0.\"U_ACCTCODE\"\r\n\r\nAND T5.\"Budget\"='Y'\r\n\r\nWHERE YEAR(T2.\"FinancYear\")= YEAR(TO_DATE('" + initDate + "','YYYY-MM-DD'))" +
                         "\r\n\r\nAND T2.\"IsMain\"='N'\r\n\r\nGROUP BY RIGHT(T5.\"FormatCode\",3), T5.\"FormatCode\",T5.\"AcctCode\", T5.\"AcctName\" , T5.\"GroupMask\", T4.\"U_OCRCODE1\"" +
                         " ,T4.\"U_OCRCODE2\",T2.\"PrjCode\")\r\n\r\nUNION\r\n\r\n(SELECT RIGHT(T1.\"FormatCode\",3) \"SUCURSAL\", T1.\"FormatCode\", T0.\"Account\" \"ACCTCODE\",  T1.\"AcctName\"" +
                         " \"ACCTNAME\", T1.\"GroupMask\", T0.\"ProfitCode\" \"DIM1\",\r\n\r\nT0.\"OcrCode2\" \"DIM2\", T0.\"Project\" \"PRJCODE\", 0 TOTAL_CUENTA, 0 TOTAL_DIM," +
                         "\r\n\r\nIFNULL( MAP(T1.\"GroupMask\",4,SUM(T0.\"Credit\"-T0.\"Debit\"),SUM(T0.\"Debit\"-T0.\"Credit\")),0) AS \"EJECUTADO\"\r\n\r\nFROM \"UCATOLICA\".\"JDT1\" T0" +
                         "\r\n\r\nINNER JOIN \"UCATOLICA\".\"OACT\" T1 ON T1.\"AcctCode\" = T0.\"Account\"\r\n\r\nAND T1.\"Budget\"='Y'\r\n\r\nWHERE IFNULL(T0.\"ProfitCode\", '') LIKE '%'" +
                         "\r\n\r\nAND IFNULL(T0.\"OcrCode2\", '') LIKE '%'\r\n\r\nGROUP BY RIGHT(T1.\"FormatCode\",3), T1.\"FormatCode\",T0.\"Account\", T1.\"AcctName\", T1.\"GroupMask\", T0.\"ProfitCode\", " +
                         "T0.\"OcrCode2\",T0.\"Project\")\r\n\r\n) A\r\n\r\nINNER JOIN UCATOLICA.\"OPRJ\" OPRJ ON OPRJ.\"PrjCode\" = A.\"PRJCODE\"" +
                         "\r\n\r\nWHERE (A.\"PRJCODE\" is not null and A.\"PRJCODE\" != '')" +
                         "\r\n\r\nAND A.\"PRJCODE\" = '" + id +"'"+
                         "\r\n\r\nGROUP BY A.\"SUCURSAL\", A.\"FORMATCODE\", A.\"ACCTCODE\", A.\"ACCTNAME\", A.\"GroupMask\", A.DIM1, A.DIM2,OPRJ.\"PrjName\",A.\"PRJCODE\"" +
                         "\r\n\r\nORDER BY A.\"SUCURSAL\", A.PRJCODE,A.\"FORMATCODE\"," +
                         " A.DIM1, A.DIM2;";
            var query = "call ucatolica.SP_PRS_RPTANUALACUMULADO_PROY('"+id+"','"+initDate+"','Todos')";
            //var rawresult1 = _context.Database.SqlQuery<ProjectInfo>(query).ToList();
            var rawresult1 = _context.Database.SqlQuery<ProyPrueba>(query4).ToList();
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