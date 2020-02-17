using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using AddonSolicitudesCompras.Models;
using  AddonSolicitudesCompras.Logic;
using AddonSolicitudesCompras.Models.Auth;
using System.Data.Entity;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace AddonSolicitudesCompras.Logic
{
    public class ADClass
    {
        // first install directory services dll
        //Install-Package System.DirectoryServices -Version 4.5.0
        //Install-Package System.DirectoryServices.AccountManagement -Version 4.5.0

        public string sDomain = "UCB.BO";
        public string Domain = "192.168.18.62";
        //public string Domain = "UCB.BO";
        private HanaValidator hanaval;

        public ADClass()
        {
            hanaval = new HanaValidator();
        }
        public string getSamAcoutName(People person)
        {

            var _context = new ApplicationDbContext();
            var personuser = _context.CustomUsers.FirstOrDefault(x => x.PeopleId == person.Id);


            if (personuser != null)
            {
                return personuser.UserPrincipalName.Split('@')[0];
            }
            // First attempt
            var SAN = hanaval.CleanText(person.Names).ToCharArray()[0].ToString() + "."
                      + hanaval.CleanText(person.FirstSurName)
                      + (!person.SecondSurName.IsNullOrWhiteSpace() ? ("." + hanaval.CleanText(person.SecondSurName).ToCharArray()[0].ToString()) : "");
            SAN = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(SAN.Replace(" ", "")));
            var UPN = SAN + "@" + sDomain;
            var search = _context.CustomUsers.Where(x => x.UserPrincipalName == UPN).ToList();
            if (search.Any())
            {
                // Second attempt
                SAN = hanaval.CleanText(person.Names).ToCharArray()[0].ToString() + hanaval.CleanText(person.Names).ToCharArray()[1].ToString() + "."
                        + hanaval.CleanText(person.FirstSurName)
                        + (!person.SecondSurName.IsNullOrWhiteSpace() ? ("." + hanaval.CleanText(person.SecondSurName).ToCharArray()[0].ToString()) : "");
                SAN = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(SAN.Replace(" ", "")));

                UPN = SAN + "@" + sDomain;
                search = _context.CustomUsers.Where(x => x.UserPrincipalName == UPN).ToList();
                if (search.Any())
                {
                    // Third attempt
                    SAN = hanaval.CleanText(person.Names).ToCharArray()[0].ToString() + hanaval.CleanText(person.Names).ToCharArray()[1].ToString() + "."
                          + hanaval.CleanText(person.FirstSurName)
                          + (!person.SecondSurName.IsNullOrWhiteSpace() ? ("." + hanaval.CleanText(person.SecondSurName).ToCharArray()[0].ToString() + hanaval.CleanText(person.SecondSurName).ToCharArray()[1].ToString()) : "");
                    SAN = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(SAN.Replace(" ", "")));

                    UPN = SAN + "@" + sDomain;
                    search = _context.CustomUsers.Where(x => x.UserPrincipalName == UPN).ToList();
                    if (search.Any())
                    {
                        // Fourth attempt
                        SAN = hanaval.CleanText(person.Names).ToCharArray()[0].ToString() + "."
                              + hanaval.CleanText(person.FirstSurName)
                              + (!person.SecondSurName.IsNullOrWhiteSpace() ? ("." + hanaval.CleanText(person.SecondSurName).ToCharArray()[0].ToString()) : "")
                              + (person.BirthDate.Day).ToString().PadLeft(2, '0');
                        SAN = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(SAN.Replace(" ", "")));

                        UPN = SAN + "@" + sDomain;
                        Random rnd = new Random();
                        while (_context.CustomUsers.Where(x => x.UserPrincipalName == UPN).ToList().Any())
                        {
                            // Last and final attempt

                            SAN = hanaval.CleanText(person.Names).ToCharArray()[0].ToString() + "."
                                                                + hanaval.CleanText(person.FirstSurName)
                                                                + (!person.SecondSurName.IsNullOrWhiteSpace() ? ("." + hanaval.CleanText(person.SecondSurName).ToCharArray()[0].ToString()) : "")
                                                                + (rnd.Next(1, 100)).ToString().PadLeft(2, '0');
                            SAN = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(SAN.Replace(" ", "")));
                        }
                    }
                }
            }

            //person.UserPrincipalName = UPN;

            var newUser = new CustomUser();
            newUser.Id = CustomUser.GetNextId(_context);
            newUser.PeopleId = person.Id;
            newUser.UserPrincipalName = UPN;
            _context.CustomUsers.Add(newUser);
            _context.SaveChanges();
            return SAN;
        }
        public bool memberOf(CustomUser user, string groupName)
        {
            using (PrincipalContext ouContex = new PrincipalContext(ContextType.Domain,
                Domain,
                "ADMNALRRHH@UCB.BO",
                "Rrhh1234"))
            {
                ouContex.ValidateCredentials("ADMNALRRHH", "Rrhh1234");
                var u = findUser(user.People);
                GroupPrincipal group = GroupPrincipal.FindByIdentity(ouContex, groupName);
                if (group != null)
                {
                    return (u.IsMemberOf(group));
                }
            }

            return false;
        }
        public Principal findUser(People person)
        {
            Principal user;
            PrincipalContext ouContex = new PrincipalContext(ContextType.Domain,
                Domain,
                "ADMNALRRHH@UCB.BO",
                "Rrhh1234");
            ouContex.ValidateCredentials("ADMNALRRHH", "Rrhh1234");
            UserPrincipal up = new UserPrincipal(ouContex);
            up.EmployeeId = person.CUNI;
            PrincipalSearcher ps = new PrincipalSearcher(up);
            user = (UserPrincipal)ps.FindOne();
            return user;
        }
        public List<dynamic> FiltrarRegional(FiltroUser user, IQueryable<dynamic> list)
        {
            var ubranches = getBranches(user).Select(x => x.Abr);
            var filtered =
                from Lc in list.ToList()
                join branches in ubranches on Lc.regional equals branches
                select Lc;
            return filtered.ToList();
        }
        public List<Branches> getBranches(FiltroUser customUser)
        {
            List<Branches> roles = new List<Branches>();
            DirectoryEntry obEntry = new DirectoryEntry("LDAP://UCB.BO",
                "ADMNALRRHH",
                "Rrhh1234");
            DirectorySearcher srch = new DirectorySearcher(obEntry,
                "(sAMAccountName=" + customUser.UserPrincipalName.Split('@')[0] + ")");

            SearchResult res = srch.FindOne();

            if (res != null)
            {
                DirectoryEntry obUser = new DirectoryEntry(res.Path, "ADMNALRRHH",
                    "Rrhh1234");
                object obGroups = obUser.Invoke("Groups");
                List<string> grps = new List<string>();

                foreach (var group in obUser.Properties["memberOf"])
                {
                    var ss = "{'" + group.ToString().Replace("=", "':'").Replace(",", "','") + "'}";
                    var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(ss);
                    grps.Add(dic["CN"]);
                }

                var _context = new ApplicationDbContext();
                roles = _context.Branch.ToList().Where(x => grps.Contains(x.ADGroupName)).ToList();

            }

            return roles;
        }
        public List<Branches> getUserBranches(CustomUser customUser)
        {
            List<Branches> roles = new List<Branches>();
            DirectoryEntry obEntry = new DirectoryEntry("LDAP://UCB.BO",
                "ADMNALRRHH",
                "Rrhh1234");
            DirectorySearcher srch = new DirectorySearcher(obEntry,
                "(sAMAccountName=" + customUser.UserPrincipalName.Split('@')[0] + ")");

            SearchResult res = srch.FindOne();

            if (res != null)
            {
                DirectoryEntry obUser = new DirectoryEntry(res.Path, "ADMNALRRHH",
                    "Rrhh1234");
                object obGroups = obUser.Invoke("Groups");
                List<string> grps = new List<string>();

                foreach (var group in obUser.Properties["memberOf"])
                {
                    var ss = "{'" + group.ToString().Replace("=", "':'").Replace(",", "','") + "'}";
                    var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(ss);
                    grps.Add(dic["CN"]);
                }

                var _context = new ApplicationDbContext();
                roles = _context.Branch.ToList().Where(x => grps.Contains(x.ADGroupName)).ToList();

            }

            return roles;
        }
        public List<Rol> getUserRols(CustomUser customUser)
        {
            List<Rol> roles = new List<Rol>();
            DirectoryEntry obEntry = new DirectoryEntry("LDAP://UCB.BO",
                "ADMNALRRHH@UCB.BO",
                "Rrhh1234");
            // obEntry.Username = "ADMNALRRHH";
            // obEntry.Password = "Rrhh1234";
            DirectorySearcher srch = new DirectorySearcher(obEntry,
                "(sAMAccountName=" + customUser.UserPrincipalName.Split('@')[0] + ")");

            SearchResult res = srch.FindOne();

            if (res != null)
            {
                DirectoryEntry obUser = new DirectoryEntry(res.Path, "ADMNALRRHH",
                    "Rrhh1234");
                object obGroups = obUser.Invoke("Groups");
                List<string> grps = new List<string>();

                foreach (var group in obUser.Properties["memberOf"])
                {
                    var ss = "{'" + group.ToString().Replace("=", "':'").Replace(",", "','") + "'}";
                    var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(ss);
                    grps.Add(dic["CN"]);
                }

                var _context = new ApplicationDbContext();
                roles = _context.Rols.Include(x => x.Resource).ToList().Where(x => grps.Contains(x.ADGroupName)).ToList();

            }

            return roles;
        }
        public bool ActiveDirectoryAuthenticate(string username, string password)
        {

            PrincipalContext ouContex = new PrincipalContext(ContextType.Domain,
                Domain,
                "ADMNALRRHH@UCB.BO",
                "Rrhh1234");
            bool Valid = ouContex.ValidateCredentials(username, password);
            if (Valid)
            {
                ouContex.ValidateCredentials("ADMNALRRHH", "Rrhh1234");
                UserPrincipal up = new UserPrincipal(ouContex);
                up.UserPrincipalName = username;
                PrincipalSearcher ps = new PrincipalSearcher(up);

                var user = (UserPrincipal)ps.FindOne();

                return user == null ? false : user.Enabled.Value;

            }

            return false;

        }
        public List<string> getGroups()
        {
            List<string> res = new List<string>();
            PrincipalContext ouContex = new PrincipalContext(ContextType.Domain,
                Domain,
                "ADMNALRRHH@UCB.BO",
                "Rrhh1234");
            ouContex.ValidateCredentials("ADMNALRRHH", "Rrhh1234");
            GroupPrincipal qbeGroup = new GroupPrincipal(ouContex);

            PrincipalSearcher srch = new PrincipalSearcher(qbeGroup);



            foreach (var group in srch.FindAll())
            {
                res.Add(((GroupPrincipal)group).Name);
            }

            return res;

        }
        
    }
}