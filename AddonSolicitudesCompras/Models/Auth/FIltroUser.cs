using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Web;
using AddonSolicitudesCompras.Models;

namespace AddonSolicitudesCompras.Models.Auth
{
    [CustomSchema("User")]
    public class FiltroUser
    {
        public FiltroUser(int Id, ApplicationDbContext _context = null)
        {
            _context = _context == null ? new ApplicationDbContext() : _context;
            FiltroUser userInDb = _context.Database.SqlQuery<FiltroUser>("select u.\"Id\",u.\"UserPrincipalName\", f.\"FullName\", p.\"UcbEmail\", p.\"PersonalEmail\", p.CUNI, p.\"Gender\", p.\"BirthDate\", u.\"PeopleId\"" +
                                                                         " from admnalrrhh.\"User\" u " +
                                                                         " inner join admnalrrhh.\"FullName\" f " +
                                                                         "  on u.\"PeopleId\" = f.\"PeopleId\" " +
                                                                         " inner join admnalrrhh.\"People\" p" +
                                                                         "  on p.\"Id\" = f.\"PeopleId\"" +
                                                                         " where u.\"Id\" = " + Id).FirstOrDefault();
            this.Id = userInDb.Id;
            this.UserPrincipalName = userInDb.UserPrincipalName;
            this.FullName = userInDb.FullName;
            this.UcbEmail = userInDb.UcbEmail;
            this.PersonalEmail = userInDb.PersonalEmail;
            this.CUNI = userInDb.CUNI;
            this.Gender = userInDb.Gender;
            this.BirthDate = userInDb.BirthDate;
            this.PeopleId = userInDb.PeopleId;
        }
        public FiltroUser() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public string UserPrincipalName { get; set; }
        public People People { get; set; }
        public int PeopleId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? TokenCreatedAt { get; set; }
        public DateTime? RefreshTokenCreatedAt { get; set; }
        public string AutoGenPass { get; set; }
        public string TipoLicenciaSAP { get; set; }
        public bool? CajaChica { get; set; }
        public bool? SolicitanteCompras { get; set; }
        public bool? AutorizadorCompras { get; set; }
        public bool? Rendiciones { get; set; }
        public bool? RendicionesDolares { get; set; }
        [ForeignKey("AuthPeopleId")]
        public People Auth { get; set; }
        public int? AuthPeopleId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string CUNI { get; set; }
        public string Document { get; set; }
        public DateTime BirthDate { get; set; }
        public string UcbEmail { get; set; }
        public string PersonalEmail { get; set; }

        public static int GetNextId(ApplicationDbContext _context)
        {
            return _context.Database.SqlQuery<int>("SELECT \"" + CustomSchema.Schema + "\".\"rrhh_User_sqs\".nextval FROM DUMMY;").ToList()[0];
        }
        
    }

}