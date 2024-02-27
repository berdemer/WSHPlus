using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;



namespace  ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class OzurlulukMap : EntityTypeConfiguration<Ozurluluk>
    {
        public OzurlulukMap()
        {
            // Primary Key
            this.HasKey(t => t.Ozurluluk_Id);

            // Properties
            this.Property(t => t.Ozurluluk_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.HastalikTanimi);


            this.Property(t => t.Aciklama);


            // Table & Column Mappings
            this.ToTable("Ozurlulukler");
            this.Property(t => t.Ozurluluk_Id).HasColumnName("Ozurluluk_Id");
            this.Property(t => t.HastalikTanimi).HasColumnName("HastalikTanimi");
            this.Property(t => t.HastahaneAdi).HasColumnName("HastahaneAdi");
            this.Property(t => t.Derecesi).HasColumnName("Derecesi");
            this.Property(t => t.Oran).HasColumnName("Oran");
            this.Property(t => t.Aciklama).HasColumnName("Aciklama");
            this.Property(t => t.Personel_Id).HasColumnName("Personel_Id");

            // Relationships
            this.HasRequired(t => t.Personel)
                .WithMany(t => t.Ozurlulukler)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);

        }
    }
}
