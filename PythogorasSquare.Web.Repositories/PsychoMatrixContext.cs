using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using PythogorasSquare.Repositories.Interfaces;
using PythogorasSquare.Web.DomainModel;
using PythogorasSquare.Web.Repositories.Migrations;

namespace PythogorasSquare.Web.Repositories
{
    public class PsychoMatrixContext : DbContext, IDbContext
    {
        private const string DatabaseConnectionString = "DefaultConnection";


        public DbSet<Quality> Qualities { get; set; }

        public DbSet<QualityDescription> QualityDescriptions { get; set; }

        public DbSet<QualityDetailedInfo> QualityDetailedInfos { get; set; }


        static PsychoMatrixContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PsychoMatrixContext, PsychoMatrixConfiguration>(DatabaseConnectionString));
        }

        public PsychoMatrixContext()
            : base(DatabaseConnectionString)
        {

        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Quality>().Property(q => q.Name).IsRequired();
            modelBuilder.Entity<Quality>().Property(q => q.AssociatedDigit).IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_QualityAssociatedDigit") { IsUnique = true }));

            modelBuilder.Entity<QualityDescription>().Property(qd => qd.Description).IsRequired();

            modelBuilder.Entity<QualityDetailedInfo>().HasKey(qualityDetailedInfo => new { qualityDetailedInfo.QualityPower, qualityDetailedInfo.QualityId });
            modelBuilder.Entity<QualityDetailedInfo>().Property(qdi => qdi.QualityPower).IsRequired();
            modelBuilder.Entity<QualityDetailedInfo>().Property(qdi => qdi.QualityId).IsRequired();
            modelBuilder.Entity<QualityDetailedInfo>().Property(qdi => qdi.QualityDescriptionId).IsRequired();
        }
    }
}