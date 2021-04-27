using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace solve.Models
{
    public class ApplicationContext :DbContext
    {
        public DbSet<RequestModel> request { get; set; }
        public DbSet<ip_adress> ip_adresses { get; set; }
        public DbSet<file_path> file_pathes { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
           
            Database.EnsureCreated();
    
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("DataSource=DESKTOP-SULJS9R;IntegratedSecurity=True;ConnectTimeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<RequestModel>()
                .HasOne(p => p.request)
                .WithMany(t => t.Users)
                .HasForeignKey(p => p.CompanyInfoKey);
*/        }
    }
}


