using ISG.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class AdresMap : EntityTypeConfiguration<Adres>
    {
        public AdresMap()
        {
            // Primary Key
            this.HasKey(t => t.Adres_Id);

            // Properties
            this.Property(t => t.Adres_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Adres_Turu)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.MapLokasyonu)
                .IsFixedLength()
                .HasMaxLength(100);

            this.Property(t => t.UserId).IsFixedLength().HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("Adresler");
            this.Property(t => t.Adres_Id).HasColumnName("Adres_Id");
            this.Property(t => t.Adres_Turu).HasColumnName("Adres_Turu");
            this.Property(t => t.GenelAdresBilgisi).HasColumnName("GenelAdresBilgisi");
            this.Property(t => t.EkAdresBilgisi).HasColumnName("EkAdresBilgisi");
            this.Property(t => t.MapLokasyonu).HasColumnName("MapLokasyonu");
            this.Property(t => t.Personel_Id).HasColumnName("Personel_Id");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.UserId).HasColumnName("UserId");

            // Relationships
            this.HasRequired(t => t.Personel)
                .WithMany(t => t.Adresler)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);

        }
    }
}
