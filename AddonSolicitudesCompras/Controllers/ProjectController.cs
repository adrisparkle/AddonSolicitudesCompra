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
            FiltroUser us  = new FiltroUser(user);
            //convertir precio a float o double y cantidad a int!!
            var queryProduct =
                "select\r\np.\"PrjCode\" as \"codigo_proyecto\"," +
                "\r\np.\"PrjName\" as \"nombre_proyecto\"," +
                "\r\np.\"U_Sucursal\" as \"regional\"," +
                "\r\np.\"U_PEI_PO\" as \"pei_po\"," +
                "\r\np.\"U_UORGANIZA\" as \"unidad_organizacional\"," +
                "\r\np.\"ValidTo\" as \"valido_hasta\"," +
                "\r\np.\"ValidFrom\" as \"valido_desde\"" +
                "\r\nfrom " + dbName + ".oprj p" +
                "\r\nwhere p.\"Active\" = 'Y'" +
                "\r\nand p.\"ValidTo\" >= current_date" +
                "\r\n group by\r\np.\"PrjCode\"," +
                "\r\np.\"PrjName\",\r\np.\"U_Sucursal\"," +
                "\r\np.\"U_PEI_PO\",\r\np.\"U_UORGANIZA\"," +
                "\r\np.\"ValidTo\",\r\np.\"ValidFrom\"" +
                "\r\norder by \r\np.\"PrjCode\"," +
                "\r\np.\"PrjName\"";

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
        [Route("api/ProjectInfo/{id}")]
        public IHttpActionResult ProjectInfo(string id)
        {
            //convertir precio a float o double y cantidad a int!!
           var query4 = "SELECT\r\nA.FORMATCODE,\r\nA.ACCTCODE,\r\nA.SUCURSAL,\r\nA.ACCTNAME,\r\nA.DIM1,\r\nA.DIM2,\r\nSUM(A.TOTAL_DIM) TOTAL_DIM,\r\nSUM(A.SOLICITADO) SOLICITADO,\r\nSUM(A.COMPROMETIDO) COMPROMETIDO,\r\nSUM(A.EJECUTADO)EJECUTADO,\r\nSUM(A.TOTAL_CUENTA)TOTAL_CUENTA\r\nFROM (\r\n\tSELECT\r\n\tTO_VARCHAR(T2.\"PrjCode\") as \"PROYECTO\",\r\n\tOP.\"PrjName\" as \"NOMBRE\",\r\n\tT5.\"FormatCode\" as \"FORMATCODE\",\r\n\tT5.\"AcctCode\" as \"ACCTCODE\",\r\n\tT5.\"AcctName\" as \"ACCTNAME\",\r\n\tT4.\"U_OCRCODE1\" AS DIM1,\r\n\tT4.\"U_OCRCODE2\" AS DIM2,\r\n\tIFNULL(SUM(T0.\"U_DEBELOC\"-T0.\"U_CREDLOC\"),0) AS TOTAL_CUENTA,\r\n\tIFNULL(SUM(T4.\"U_DEBELOC\"-T4.\"U_CREDLOC\"),0) AS TOTAL_DIM,\r\n\t0 SOLICITADO,\r\n\t0 COMPROMETIDO,\r\n\t0 EJECUTADO,\r\n\tRIGHT(T5.\"FormatCode\",3) \"SUCURSAL\"\r\n\tFROM ucatolica.\"@PR_PRESUP\" T0 \r\n\tleft JOIN ucatolica.\"OBGS\" T2 \r\n\tON T0.\"U_INSTANCE\" = T2.\"AbsId\" \r\n\tleft JOIN ucatolica.\"@PR_PRESUP_NR\" T4 \r\n\tON T4.\"U_PRESUP_ID\"=T0.\"Code\" \r\n\tand T4.\"U_INSTANCE\"=T0.\"U_INSTANCE\"\r\n\tleft JOIN ucatolica.\"OACT\" T5 \r\n\tON T5.\"AcctCode\"=T0.\"U_ACCTCODE\" \r\n\tleft join  ucatolica.\"OPRJ\" OP\r\n\ton T2.\"PrjCode\" = OP.\"PrjCode\"\r\n\tGROUP BY \r\n\tT0.\"U_FINANCYEAR\", \r\n\tT0.\"U_ACCTCODE\", \r\n\tT2.\"FinancYear\",\r\n\tT4.\"U_OCRCODE1\",\r\n\tT4.\"U_OCRCODE2\",\r\n\tT5.\"AcctCode\",\r\n\tT5.\"FormatCode\",\r\n\tT5.\"AcctName\",\r\n\tT2.\"PrjCode\",\r\n\tOP.\"PrjName\"\r\n\tUNION\r\n\tSELECT \r\n\tTO_VARCHAR(PJ.\"PrjCode\") as \"PROYECTO\",\r\n\tPJ.\"PrjName\" as \"NOMBRE\",\r\n\tT0.\"FormatCode\",\r\n\tT1.\"ACCTCODE\",\r\n\tT0.\"AcctName\",\r\n\tT1.\"DIM1\",\r\n\tT1.\"DIM2\",\r\n\t0,\r\n\t0,\r\n\tSUM(T1.\"SOLICITADO\"),\r\n\t0,\r\n\t0,\r\n\tRIGHT(T0.\"FormatCode\",3) \"SUCURSAL\"\r\n    FROM ucatolica.SOLICITADO T1\r\n    INNER JOIN ucatolica.\"OACT\" T0 \r\n    ON T0.\"AcctCode\"=T1.\"ACCTCODE\"\r\n    left JOIN ucatolica.\"JDT1\" J1\r\n    on J1.\"Account\" = T0.\"AcctCode\"\r\n    left JOIN ucatolica.\"OJDT\" J0\r\n    on J0.\"TransId\" = J1.\"TransId\"\r\n    left JOIN ucatolica.\"OPRJ\" PJ\r\n    on TO_VARCHAR(PJ.\"PrjCode\") = TO_VARCHAR(J1.\"Project\")\r\n    where TO_VARCHAR(PJ.\"PrjCode\") is not null \r\n\tand TO_VARCHAR(PJ.\"PrjCode\") != ''\r\n    GROUP BY \r\n    T0.\"FormatCode\",\r\n    T1.\"ACCTCODE\",\r\n    T0.\"AcctName\",\r\n    T1.\"DIM1\",\r\n    T1.\"DIM2\",\r\n   TO_VARCHAR(PJ.\"PrjCode\"),\r\n   PJ.\"PrjName\"\r\n\tUNION\r\n\tSELECT \r\n\tTO_VARCHAR(PJ.\"PrjCode\") as \"PROYECTO\",\r\n\tPJ.\"PrjName\" as \"NOMBRE\",\r\n\tT0.\"FormatCode\",\r\n\tT1.\"ACCTCODE\",\r\n\tT0.\"AcctName\",\r\n\tT1.\"DIM1\",\r\n\tT1.\"DIM2\",\r\n\t0,\r\n\t0,\r\n\t0,\r\n\tSUM(T1.COMPROMETIDO),\r\n\t0,\r\n\tRIGHT(T0.\"FormatCode\",3) \"SUCURSAL\"\r\n    FROM ucatolica.COMPROMETIDO T1\r\n    INNER JOIN ucatolica.\"OACT\" T0 \r\n    ON T0.\"AcctCode\"=T1.\"ACCTCODE\"\r\n    left JOIN ucatolica.\"JDT1\" J1\r\n    on J1.\"Account\" = T0.\"AcctCode\"\r\n    left JOIN ucatolica.\"OJDT\" J0\r\n    on J0.\"TransId\" = J1.\"TransId\"\r\n    left JOIN ucatolica.\"OPRJ\" PJ\r\n    on TO_VARCHAR(PJ.\"PrjCode\") = TO_VARCHAR(J1.\"Project\")\r\n    where TO_VARCHAR(PJ.\"PrjCode\") is not null \r\n\tand TO_VARCHAR(PJ.\"PrjCode\") != ''\r\n    GROUP BY \r\n    T0.\"FormatCode\",\r\n    T1.\"ACCTCODE\",\r\n    T0.\"AcctName\",\r\n    T1.\"DIM1\",\r\n    T1.\"DIM2\",\r\n    TO_VARCHAR(PJ.\"PrjCode\"),\r\n    PJ.\"PrjName\"\r\n    UNION\r\n\tSELECT \r\n\tTO_VARCHAR(PJ.\"PrjCode\") as \"PROYECTO\",\r\n\tPJ.\"PrjName\" as \"NOMBRE\",\r\n\tT0.\"FormatCode\",\r\n\tT1.\"ACCTCODE\",\r\n\tT0.\"AcctName\",\r\n\tT1.\"DIM1\",\r\n\tT1.\"DIM2\",\r\n\t0,\r\n\t0,\r\n\t0,\r\n\t0,\r\n\tSUM(T1.EJECUTADO),\r\n\tRIGHT(T0.\"FormatCode\",3) \"SUCURSAL\"\r\n    FROM ucatolica.EJECUTADO T1\r\n    INNER JOIN ucatolica.\"OACT\" T0 \r\n    ON T0.\"AcctCode\"=T1.\"ACCTCODE\"\r\n    left JOIN ucatolica.\"JDT1\" J1\r\n    on J1.\"Account\" = T0.\"AcctCode\"\r\n    left JOIN ucatolica.\"OJDT\" J0\r\n    on J0.\"TransId\" = J1.\"TransId\"\r\n    left JOIN ucatolica.\"OPRJ\" PJ\r\n    on TO_VARCHAR(PJ.\"PrjCode\") = TO_VARCHAR(J1.\"Project\")\r\n    WHERE T1.\"DIM1\" NOT IN (select \"PrcCode\" from ucatolica.\"OPRC\" WHERE \"DimCode\"=1 and \"U_ELIMINAR_REP\"='Y')\r\n\tAND T1.\"DIM2\" NOT IN (select \"PrcCode\" from ucatolica.\"OPRC\" WHERE \"DimCode\"=2 and \"U_ELIMINAR_REP\"='Y')\r\n\tand TO_VARCHAR(PJ.\"PrjCode\") is not null \r\n\tand TO_VARCHAR(PJ.\"PrjCode\") != ''\r\n    GROUP BY \r\n    T0.\"FormatCode\",\r\n    T1.\"ACCTCODE\",\r\n    T0.\"AcctName\",\r\n    T1.\"DIM1\",\r\n    T1.\"DIM2\",\r\n    TO_VARCHAR(PJ.\"PrjCode\"),\r\n    PJ.\"PrjName\") A\r\n    WHERE (A.TOTAL_CUENTA + A.TOTAL_DIM + A.SOLICITADO + A.COMPROMETIDO + A.EJECUTADO)<>0\r\n    AND (A.PROYECTO != NULL OR A.PROYECTO != '')\r\n    AND A.PROYECTO = '"+id+"'\r\n    GROUP BY A.PROYECTO, A.NOMBRE, A.FORMATCODE,A.ACCTCODE,A.DIM1, A.DIM2,A.ACCTNAME,A.SUCURSAL\r\n\tORDER BY 1,4,5;";
            var rawresult1 = _context.Database.SqlQuery<ProjectInfo>(query4).ToList();
                var formatedData = rawresult1.Select(x => new
                {
                    x.formatcode,
                    x.acctname,
                    x.acctcode,
                    x.dim1,
                    x.dim2,
                    total_dim = Convert.ToSingle(x.total_dim),
                    ejecutado = Convert.ToSingle(x.ejecutado),
                    solicitado = Convert.ToSingle(x.solicitado),
                    comprometido = Convert.ToSingle(x.comprometido),
                    total_cuenta = Convert.ToSingle(x.total_cuenta),

                });

            return Ok(formatedData);
        }
        [HttpGet]
        [Route("api/ProjectName/{id}")]
        public IHttpActionResult ProjectName(string id)
        {
            var queryP =
                "select op.\"PrjCode\" as \"PROYECTO_CODIGO\", op.\"PrjName\" as " +
                "\"proyecto_nombre\", op.\"ValidFrom\" as \"valido_desde\",\r\nop.\"ValidTo\" as " +
                "\"valido_hasta\", op.\"U_UORGANIZA\" as \"unidad_organizacional\", op.\"U_PEI_PO\" " +
                "as \"pei_po\"\r\nfrom \"UCATOLICA\".\"OPRJ\" op where op.\"PrjCode\" = '"+id+"'";

            var rawres = _context.Database.SqlQuery<ProjectName>(queryP).ToList();
            var formData = rawres.Select(x => new
                {
                    x.PROYECTO_CODIGO,
                    x.proyecto_nombre,
                    x.unidad_organizacional,
                    x.pei_po,
                    valido_hasta = x.valido_hasta.ToString("dd/MM/yyyy"),
                    valido_desde = x.valido_desde.ToString("dd/MM/yyyy"),
                });
            return Ok(formData);
        }

}

}

