using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using AddonSolicitudesCompras.Logic;
using AddonSolicitudesCompras.Models;
using AddonSolicitudesCompras.Models.Auth;

namespace AddonSolicitudesCompras.Controllers
{
    public class CustomUserController : ApiController
    {
        private ApplicationDbContext _context;
        private ValidateToken validator;
        private ADClass activeDirectory;

        public CustomUserController()
        {
            _context = new ApplicationDbContext();
            validator = new ValidateToken();
            activeDirectory = new ADClass();
        }

        // GET api/user
        [Route("api/user/")]
        public IHttpActionResult Get()
        {
            /*var userlist = _context.CustomUsers.Include(x=>x.People)
                .ToList().
                Select(x => new
                {
                    x.Id,
                    x.UserPrincipalName,
                    person = x.People.GetFullName(),
                    x.PeopleId,
                    x.AutoGenPass,
                    x.TipoLicenciaSAP,
                    x.CajaChica,
                    x.SolicitanteCompras,
                    x.AutorizadorCompras,
                    x.Rendiciones
                });*/
            var query = "select u.\"Id\", p.\"SAPCodeRRHH\", p.cuni, p.\"Document\", c.\"FullName\", " +
            " case when u.\"AutorizadorCompras\" = true then 'APROBADOR' when u.\"SolicitanteCompras\" = true then 'SOLICITANTE' when u.\"CajaChica\" = true then 'CAJA CHICA' when u.\"Rendiciones\" = true then 'RENDICIONES' ELSE 'SIN ROL' END AS \"Rol\"," +
            " case when auth.\"FullName\" is null then null else 'Las solicitudes que usted realice deberan ser aprobadas por: ' end as \"MensajeAprobacion\"," +
            " p.\"UcbEmail\",p.\"PersonalEmail\",coalesce(u.\"UserPrincipalName\",'Sin Usuario') as \"UserPrincipalName\", c.\"DependencyCod\", c.\"Dependency\", ou.\"Cod\" as \"OUCod\", " +
            " ou.\"Name\" as \"OUName\", c.\"Positions\", " +
            " auth.\"FullName\" as \"AuthFullName\", br.\"Name\" as \"Branches\", u.\"AutoGenPass\", " +
            " case when (c.\"Active\" = false and c.\"EndDate\" < current_date) then 'INACTIVO' else 'ACTIVO' end as \"State\" " +
            " from \"ADMNALRRHH\".lastcontracts c " +
            " inner join \"ADMNALRRHH\".\"Branches\" br " +
            "    on c.\"BranchesId\" = br.\"Id\" " +
            " left join \"ADMNALRRHH\".\"User\" u " +
            "    on c.\"PeopleId\" = u.\"PeopleId\" " +
            " inner join \"ADMNALRRHH\".\"People\" p " +
            "    on c.\"PeopleId\" = p.\"Id\" " +
            " inner join \"ADMNALRRHH\".\"OrganizationalUnit\" ou " +
            "    on c.\"OUId\" = ou.\"Id\" " +
            " left join \"ADMNALRRHH\".lastcontracts auth " +
            "   on u.\"AuthPeopleId\" = auth.\"PeopleId\" " +
            " left join \"ADMNALRRHH\".\"People\" pauth " +
            "    on auth.\"PeopleId\" = pauth.\"Id\"" +
                //" where c.\"EndDate\" is null or c.\"EndDate\" > current_date" +
            " order by (case when u.\"UserPrincipalName\" is null then 1 else 0 end) asc," +
            "    c.\"FullName\"";
            var rawresult = _context.Database.SqlQuery<UserViewModel>(query).Select(x => new
            {
                x.Id,
                x.CUNI,
                x.Document,
                x.FullName,
                x.Rol,
                x.UcbEmail,
                x.PersonalEmail,
                x.UserPrincipalName,
                x.DependencyCod,
                x.Dependency,
                x.OUName,
                x.OUCod,
                x.Positions,
                x.AuthFullName,
                x.Branches,
                x.AutoGenPass,
                x.MensajeAprobacion,
                x.State

            }).ToList();
            return Ok(rawresult);
        }


        [HttpGet]
        [Route("api/user/Branches/{id}")]
        public IHttpActionResult GetSegments(int id)
        {
            CustomUser userInDB = null;

            userInDB = _context.CustomUsers.Include(x => x.People).FirstOrDefault(d => d.Id == id);

            if (userInDB == null)
                return NotFound();

            var br = activeDirectory.getUserBranches(userInDB).Select(x => new { x.Id, x.Abr, x.Name });

            return Ok(br);
        }


        [HttpGet]
        [Route("api/user/Rol/{id}")]
        public IHttpActionResult GetRols(int id)
        {
            CustomUser userInDB = null;
            userInDB = _context.CustomUsers.Include(x => x.People).FirstOrDefault(d => d.Id == id);

            if (userInDB == null)
                return NotFound();

            var rols = activeDirectory.getUserRols(userInDB).Select(x => new { x.Id, x.Name });
            return Ok(rols);
        }
        
        // GET api/user/5
        [Route("api/user/{id}")]
        public IHttpActionResult Get(int id)
        {
            CustomUser userInDB = null;

            userInDB = _context.CustomUsers.Include(x => x.People).FirstOrDefault(d => d.Id == id);

            if (userInDB == null)
                return NotFound();
            dynamic respose = new JObject();
            respose.Id = userInDB.Id;
            respose.UserPrincipalName = userInDB.UserPrincipalName;
            respose.PeopleId = userInDB.People.Id;
            respose.Name = userInDB.People.GetFullName();
            respose.Gender = userInDB.People.Gender;
            respose.TipoLicenciaSAP = userInDB.TipoLicenciaSAP;
            respose.CajaChica = userInDB.CajaChica;
            respose.SolicitanteCompras = userInDB.SolicitanteCompras;
            respose.AutorizadorCompras = userInDB.AutorizadorCompras;
            respose.Rendiciones = userInDB.Rendiciones;
            respose.RendicionesDolares = userInDB.RendicionesDolares;
            respose.AuthPeopleId = userInDB.AuthPeopleId;
            respose.UcbEmail = userInDB.People.UcbEmail;

            return Ok(respose);
        }
        
      // DELETE api/user/5
        [HttpPost]
        [Route("api/user/ChangeStatus")]
        public IHttpActionResult ChangeStatus(int id)
        {
            var userInDB = _context.CustomUsers.FirstOrDefault(d => d.Id == id);
            if (userInDB == null)
                return NotFound();

            //_context.CustomUsers.Remove(userInDB);
            _context.SaveChanges();
            return Ok();
        }
    }
}
