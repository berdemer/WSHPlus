using ISG.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class KkdMap : EntityTypeConfiguration<Kkd>
    {
        public KkdMap()
        {
            // Primary Key
            this.HasKey(t => t.Kkd_Id);

            // Properties
            this.Property(t => t.Kkd_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Kkd_Tanimi)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(150);


            this.Property(t => t.Aciklama)
                .IsFixedLength()
                .HasMaxLength(200);

            this.Property(t => t.UserId).
            IsFixedLength().
            HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("Kkdleri");
            this.Property(t => t.Kkd_Id).HasColumnName("Kkd_Id");
            this.Property(t => t.Kkd_Tanimi).HasColumnName("Kkd_Tanimi");
            this.Property(t => t.Alinma_Tarihi).HasColumnName("Alinma_Tarihi");
            this.Property(t => t.Maruziyet).HasColumnName("Maruziyet");
            this.Property(t => t.Guncelleme_Suresi_Ay).HasColumnName("Guncelleme_Suresi_Ay");
            this.Property(t => t.Aciklama).HasColumnName("Aciklama");
            this.Property(t => t.Personel_Id).HasColumnName("Personel_Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            // Relationships
            this.HasRequired(t => t.Personel)
                .WithMany(t => t.KkdLeri)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);

        }
    }
}
