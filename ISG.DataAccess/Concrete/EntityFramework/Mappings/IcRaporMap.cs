using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;


namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class IcRaporMap : EntityTypeConfiguration<IcRapor>
    {
        public IcRaporMap()
        {
            // Primary Key
            this.HasKey(t => t.IcRapor_Id);

            // Properties
            this.Property(t => t.IcRapor_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.MuayeneTuru)
                .HasMaxLength(50);

            this.Property(t => t.RaporTuru)
                .IsFixedLength()
                .HasMaxLength(20);

            this.Property(t => t.Doktor_Adi)
                .IsFixedLength()
                .HasMaxLength(100);

            this.Property(t => t.UserId)
                .IsFixedLength()
                .HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("IcRaporlari");
            this.Property(t => t.IcRapor_Id).HasColumnName("IcRapor_Id");
            this.Property(t => t.MuayeneTuru).HasColumnName("MuayeneTuru");
            this.Property(t => t.Tani).HasColumnName("Tani");
            this.Property(t => t.RaporTuru).HasColumnName("RaporTuru");
            this.Property(t => t.BaslangicTarihi).HasColumnName("BaslangicTarihi");
            this.Property(t => t.BitisTarihi).HasColumnName("BitisTarihi");
            this.Property(t => t.SureGun).HasColumnName("SureGun");
            this.Property(t => t.Doktor_Adi).HasColumnName("Doktor_Adi");
            this.Property(t => t.Tarih).HasColumnName("Tarih");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Personel_Id).HasColumnName("Personel_Id");
            this.Property(t => t.Revir_Id).HasColumnName("Revir_Id");
            this.Property(t => t.Sirket_Id).HasColumnName("Sirket_Id");
            this.Property(t => t.Bolum_Id).HasColumnName("Bolum_Id");
            Property(t => t.RevirIslem_Id).HasColumnName("RevirIslem_Id");
            Property(t => t.Protokol).HasColumnName("Protokol");
            // Relationships
            this.HasRequired(t => t.Personel)
                .WithMany(t => t.IcRaporlari)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);

        }
    }
}
