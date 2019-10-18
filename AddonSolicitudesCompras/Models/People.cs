using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Data.Entity;
using System.Security.Cryptography;
using Microsoft.Ajax.Utilities;
using AddonSolicitudesCompras.Models;

namespace AddonSolicitudesCompras.Models
{
    [CustomSchema("People")]
    public class People
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [MaxLength(10, ErrorMessage = "Cadena de texto muy grande")]
        public string CUNI { get; set; }

        [MaxLength(15, ErrorMessage = "Cadena de texto muy grande")]
        //[Required]
        public string TypeDocument { get; set; }

        [MaxLength(15, ErrorMessage = "Cadena de texto muy grande")]
        [Required]
        public string Document { get; set; }

        [MaxLength(5, ErrorMessage = "Cadena de texto muy grande")]
        //[Required]
        public string Ext { get; set; }

        [MaxLength(200, ErrorMessage = "Cadena de texto muy grande")]
        [Required]
        public string Names { get; set; }

        [MaxLength(100, ErrorMessage = "Cadena de texto muy grande")]
        [Required]
        public string FirstSurName { get; set; }

        [MaxLength(100, ErrorMessage = "Cadena de texto muy grande")]
        public string SecondSurName { get; set; }

        [MaxLength(100, ErrorMessage = "Cadena de texto muy grande")]
        public string MariedSurName { get; set; }

        [Column(TypeName = "date")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Required]
        public DateTime BirthDate { get; set; }

        [MaxLength(1, ErrorMessage = "Cadena de texto muy grande")]
        [Required]
        public string Gender { get; set; }

        [MaxLength(20, ErrorMessage = "Cadena de texto muy grande")]
        //[Required]
        public string Nationality { get; set; }

        [MaxLength(250, ErrorMessage = "Cadena de texto muy grande")]
        public string Photo { get; set; }

        [MaxLength(15, ErrorMessage = "Cadena de texto muy grande")]
        public string PhoneNumber { get; set; }

        [MaxLength(30, ErrorMessage = "Cadena de texto muy grande")]
        public string PersonalEmail { get; set; }

        [MaxLength(30, ErrorMessage = "Cadena de texto muy grande")]
        public string UcbEmail { get; set; }

        [MaxLength(15, ErrorMessage = "Cadena de texto muy grande")]
        public string OfficePhoneNumber { get; set; }

        [MaxLength(15, ErrorMessage = "Cadena de texto muy grande")]
        public string OfficePhoneNumberExt { get; set; }

        [MaxLength(200, ErrorMessage = "Cadena de texto muy grande")]
        public string HomeAddress { get; set; }

        [MaxLength(20, ErrorMessage = "Cadena de texto muy grande")]
        public string AFP { get; set; }

        [MaxLength(30, ErrorMessage = "Cadena de texto muy grande")]
        public string NUA { get; set; }

        [MaxLength(50, ErrorMessage = "Cadena de texto muy grande")]
        public string Insurance { get; set; }

        [MaxLength(20, ErrorMessage = "Cadena de texto muy grande")]
        public string InsuranceNumber { get; set; }

        [MaxLength(250, ErrorMessage = "Cadena de texto muy grande")]
        public string DocPath { get; set; }

        public int? SAPCodeRRHH { get; set; }

        public int UseMariedSurName { get; set; }

        public int UseSecondSurName { get; set; }

        public bool Pending { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public string GetFullName()
        {
            return this.FirstSurName + " " +
                   (this.UseSecondSurName == 1 ? (this.SecondSurName + " ") : "") +
                   (this.UseMariedSurName == 1 ? (this.MariedSurName + " ") : "") +
                   this.Names;
        }

        public static int GetNextId(ApplicationDbContext _context)
        {
            return _context.Database.SqlQuery<int>("SELECT \"" + CustomSchema.Schema + "\".\"rrhh_People_sqs\".nextval FROM DUMMY;").ToList()[0];
        }


    }
}