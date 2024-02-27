using ISG.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class AsiMap : EntityTypeConfiguration<Asi>
    {
        public AsiMap()
        {
            // Primary Key
            this.HasKey(t => t.Asi_Id);

            // Properties
            this.Property(t => t.Asi_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Asi_Tanimi)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(150);

            this.Property(t => t.Dozu)
                .IsFixedLength()
                .HasMaxLength(30);

            this.Property(t => t.Aciklama)
                .IsFixedLength()
                .HasMaxLength(200);

            this.Property(t => t.UserId).
            IsFixedLength().
            HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("Asilar");
            this.Property(t => t.Asi_Id).HasColumnName("Asi_Id");
            this.Property(t => t.Asi_Tanimi).HasColumnName("Asi_Tanimi");
            this.Property(t => t.Yapilma_Tarihi).HasColumnName("Yapilma_Tarihi");
            this.Property(t => t.Dozu).HasColumnName("Dozu");
            this.Property(t => t.Guncelleme_Suresi_Ay).HasColumnName("Guncelleme_Suresi_Ay");
            this.Property(t => t.Muhtamel_Tarih).HasColumnName("Muhtamel_Tarih");
            this.Property(t => t.Aciklama).HasColumnName("Aciklama");
            this.Property(t => t.StokHarcamasi).HasColumnName("StokHarcamasi");
            this.Property(t => t.Personel_Id).HasColumnName("Personel_Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            // Relationships
            this.HasRequired(t => t.Personel)
                .WithMany(t => t.Asilar)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);

        }
    }
}
