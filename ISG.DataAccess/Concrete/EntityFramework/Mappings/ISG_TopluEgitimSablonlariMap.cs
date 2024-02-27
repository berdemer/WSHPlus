using ISG.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class ISG_TopluEgitimSablonlariMap : EntityTypeConfiguration<ISG_TopluEgitimSablonlari>
    {
        public ISG_TopluEgitimSablonlariMap()
        {
            // Primary Key
            this.HasKey(t => t.ISG_TopluEgitimSablonlariId);

            // Properties
            this.Property(t => t.ISG_TopluEgitimSablonlariId)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            // Table & Column Mappings
            this.ToTable("ISGTopluEgitimSablonlari");
            this.Property(t => t.ISG_TopluEgitimSablonlariId).HasColumnName("ISG_TopluEgitimSablonlariId");
            this.Property(t => t.Egitim_Turu).HasColumnName("Egitim_Turu").HasMaxLength(50).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IDX_ConfigurationId_Name", 0) { IsUnique = true }));
            this.Property(t => t.Egitim_Turu).HasColumnName("EgitimJson");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.UserId).HasColumnName("UserId");

        }
    }
}
