using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;


namespace  ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class DisRaporMap : EntityTypeConfiguration<DisRapor>
    {
        public DisRaporMap()
        {
            // Primary Key
            this.HasKey(t => t.DisRapor_Id);

            // Properties
            this.Property(t => t.DisRapor_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.MuayeneTuru)
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.DoktorAdi)
                .IsFixedLength()
                .HasMaxLength(100);

            this.Property(t => t.RaporuVerenSaglikBirimi)
                .IsFixedLength()
                .HasMaxLength(100);

            this.Property(t => t.User_Id)
                .IsFixedLength()
                .HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("DısRaporlari");
            this.Property(t => t.DisRapor_Id).HasColumnName("DisRapor_Id");
            this.Property(t => t.MuayeneTuru).HasColumnName("MuayeneTuru");
            this.Property(t => t.Tani).HasColumnName("Tani");
            this.Property(t => t.BaslangicTarihi).HasColumnName("BaslangicTarihi");
            this.Property(t => t.BitisTarihi).HasColumnName("BitisTarihi");
            this.Property(t => t.SureGun).HasColumnName("SureGun");
            this.Property(t => t.DoktorAdi).HasColumnName("DoktorAdi");
            this.Property(t => t.RaporuVerenSaglikBirimi).HasColumnName("RaporuVerenSaglikBirimi");
            this.Property(t => t.User_Id).HasColumnName("User_Id");
            this.Property(t => t.Personel_Id).HasColumnName("Personel_Id");
            this.Property(t => t.Revir_Id).HasColumnName("Revir_Id");
            this.Property(t => t.Sirket_Id).HasColumnName("Sirket_Id");
            this.Property(t => t.Bolum_Id).HasColumnName("Bolum_Id");

            // Relationships
            this.HasOptional(t => t.Personel)
                .WithMany(t => t.DisRaporlari)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);

        }
    }
}
