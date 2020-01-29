using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using AddonSolicitudesCompras.Models.Auth;

namespace AddonSolicitudesCompras.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<People> Person { get; set; }
        public DbSet<Branches> Branch { get; set; }
        public DbSet<AccessLogs> AccessLogses { get; set; }
        //auth models
        public DbSet<Access> Accesses { get; set; }
        public DbSet<Rol> Rols { get; set; }
        public DbSet<RolhasAccess> RolshaAccesses { get; set; }
        public DbSet<CustomUser> CustomUsers { get; set; }



        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("ADMNALRRHH");
            // modelBuilder.Ignore<People>();
        }
    }

    public class SapContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<People> Person { get; set; }
        public DbSet<Branches> Branch { get; set; }
        public DbSet<AccessLogs> AccessLogses { get; set; }
        public DbSet<Access> Accesses { get; set; }
        public DbSet<Rol> Rols { get; set; }
        public DbSet<RolhasAccess> RolshaAccesses { get; set; }
        public DbSet<ProyPrueba> ProyPrueba { get; set; }



        static SapContext()
        {
            Database.SetInitializer<SapContext>(null);
        }

        public SapContext()
            : base("desarrollo", throwIfV1Schema: false)
        {
        }

        public static SapContext Create()
        {
            return new SapContext();
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("UCATOLICA");
            // modelBuilder.Ignore<People>();
        }
    }
}