using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using AddonSolicitudesCompras.Logic;
using AddonSolicitudesCompras.Models;
using AddonSolicitudesCompras.Models.Auth;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace AddonSolicitudesCompras.Controllers
{
    public class AuthController : ApiController
    {
        private ApplicationDbContext _context;
        private ValidateToken validator;
        private ValidateAuth validateauth;
        private ADClass activeDirectory;

        public AuthController()
        {
            _context = new ApplicationDbContext();
            validator = new ValidateToken();
            validateauth = new ValidateAuth();
            activeDirectory = new ADClass();
        }
        // POST: /api/auth/gettoken/
        [HttpPost]
        [Route("api/auth/GetToken")]
        public IHttpActionResult GetToken([FromBody]JObject credentials)
        {
            if (credentials["username"] == null || credentials["password"] == null)
                return BadRequest();

            string username = credentials["username"].ToString().ToUpper();
            string password = credentials["password"].ToString();
            CustomUser user = _context.CustomUsers.FirstOrDefault(u => u.UserPrincipalName == username);
            if (!activeDirectory.ActiveDirectoryAuthenticate(username, password))
                return Unauthorized();
            user.Token = validator.getToken(user);
            user.TokenCreatedAt = DateTime.Now;
            user.RefreshToken = validator.getRefreshToken(user);
            user.RefreshTokenCreatedAt = DateTime.Now;
           _context.SaveChanges();
           HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.Add("Id", user.Id.ToString());
            response.Headers.Add("Token", user.Token);
            response.Headers.Add("RefreshToken", user.RefreshToken);
            response.Headers.Add("name", user.UserPrincipalName);
            dynamic respose = new JObject();
            respose.Id = user.Id;
            respose.Token = user.Token;
            respose.RefreshToken = user.RefreshToken;
            respose.name = user.UserPrincipalName;
            respose.ExpiresIn = validateauth.tokenLife;
            respose.RefreshExpiresIn = validateauth.refeshtokenLife;
            return Ok(respose);
        }

        // POST: /api/auth/RefreshToken/
        [HttpPost]
        [Route("api/auth/RefreshToken/")]
        public IHttpActionResult RefreshToken(JObject data)
        {

            IEnumerable<string> idlist;
            if (!Request.Headers.TryGetValues("id", out idlist))
                return BadRequest();
            if (data["RefreshToken"] == null)
                return BadRequest();

            int userid = 0;
            if (!Int32.TryParse(idlist.First(), out userid))
                return Unauthorized();
            string rt = data["RefreshToken"].ToString();
            CustomUser user = _context.CustomUsers.FirstOrDefault(u => u.Id == userid && u.RefreshToken == rt);
            if (user == null)
                return Unauthorized();
            if (user.RefreshTokenCreatedAt == null)
                return Unauthorized();

            int seconds = (int)DateTime.Now.Subtract(user.RefreshTokenCreatedAt.Value).TotalSeconds;

            if (seconds > validateauth.refeshtokenLife)
                return Unauthorized();

            user.Token = validator.getToken(user);
            user.TokenCreatedAt = DateTime.Now;

            _context.SaveChanges();

            dynamic respose = new JObject();
            respose.Token = user.Token;
            respose.ExpiresIn = validateauth.tokenLife;
            respose.RefreshExpiresIn = validateauth.refeshtokenLife - ((int)DateTime.Now.Subtract(user.RefreshTokenCreatedAt.Value).TotalSeconds);

            return Ok(respose);
        }

        [HttpGet]
        [Route("api/auth/Logout/")]
        public IHttpActionResult Logout()
        {
            IEnumerable<string> tokenlist;
            IEnumerable<string> idlist;
            if (!Request.Headers.TryGetValues("token", out tokenlist) || !Request.Headers.TryGetValues("id", out idlist))
                return Unauthorized();
            int userid = 0;

            if (!Int32.TryParse(idlist.First(), out userid))
                return Unauthorized();

            string token = tokenlist.First();
            CustomUser user = _context.CustomUsers.FirstOrDefault(u => u.Id == userid && u.Token == token);
            if (user == null)
                return Unauthorized();

            user.Token = null;
            user.TokenCreatedAt = null;
            user.RefreshToken = null;
            user.RefreshTokenCreatedAt = null;
            _context.SaveChanges();

            return Ok();
        }
    }
}
