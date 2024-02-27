using ISG.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class IlacStokGirisiMap : EntityTypeConfiguration<IlacStokGirisi>
    {
        public IlacStokGirisiMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasColumnName("IlacStokGirisiId");
            // Properties
            this.Property(t => t.StokId).HasColumnName("StokId");
            Property(t => t.StokEkBilgisi).HasColumnName("StokEkBilgisi");
            this.Property(t => t.SaglikBirimi_Id).HasColumnName("SaglikBirimi_Id");
            this.Property(t => t.KutuIcindekiMiktar).HasColumnName("KutuIcindekiMiktar");
            this.Property(t => t.KutuMiktari).HasColumnName("KutuMiktari");
            this.Property(t => t.ToplamMiktar).HasColumnName("ToplamMiktar");
            this.Property(t => t.MiadTarihi).HasColumnName("MiadTarihi").HasColumnType("datetime2");
            this.Property(t => t.KritikMiadTarihi).HasColumnName("KritikMiadTarihi").HasColumnType("datetime2");
            this.Property(t => t.ArtanMiadTelefMiktari).HasColumnName("ArtanMiadTelefMiktari");
            Property(t => t.Maliyet).HasColumnName("Maliyet").HasPrecision(10, 2);
            Property(t => t.ArtanTelefNedeni).HasColumnName("Nedeni").HasMaxLength(20);
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.UserId).IsFixedLength().HasMaxLength(40).HasColumnName("UserId");
            this.Property(t => t.Tarih).HasColumnName("Tarih").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            //default değer vermek için yazldı şimdiki zamaını AddColumn("dbo.IlacStokGirisleri", "Tarih", c => c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"));
            // Table & Column Mappings
            this.ToTable("IlacStokGirisleri");

            //// Relationships
            this.HasRequired(t => t.IlacStoklari)
                .WithMany(t => t.IlacStokGirisleri)
                .HasForeignKey(d => d.StokId).WillCascadeOnDelete(true);

        }
    }
}