﻿using System;
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
    public class CustomUser
    {
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
        public static int GetNextId(ApplicationDbContext _context)
        {
            return _context.Database.SqlQuery<int>("SELECT \"" + CustomSchema.Schema + "\".\"rrhh_User_sqs\".nextval FROM DUMMY;").ToList()[0];
        }
        
    }

}