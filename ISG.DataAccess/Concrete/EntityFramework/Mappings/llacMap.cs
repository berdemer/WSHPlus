using ISG.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Infrastructure.Annotations;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class IlacMap : EntityTypeConfiguration<Ilac>
    {
        public IlacMap()
        {
            // Primary Key
            this.HasKey(t => t.IlacBarkodu);

            // Properties
            this.Property(t => t.IlacBarkodu).HasColumnName("IlacBarkodu").IsRequired().IsFixedLength().
                HasMaxLength(20).HasUniqueIndexAnnotation("UK_Unique_Ilac_Barkodu", 0);
            // HasUniqueIndexAnnotation harici bir extension metoddur.
            this.Property(t => t.AtcAdi).HasColumnName("IlacAdi");

            this.Property(t => t.AtcKodu).HasColumnName("AtcKodu")
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.FirmaAdi).HasColumnName("FirmaAdi")
                .IsFixedLength()
                .HasMaxLength(150);

            this.Property(t => t.AtcAdi).HasColumnName("AtcAdi");

            this.Property(t => t.Aski).HasColumnName("Aski");

            this.Property(t => t.ReceteTuru).HasColumnName("ReceteTuru")
                .IsFixedLength()
                .HasMaxLength(30);

            //Property(t => t.Fiyat).HasColumnName("Fiyat").HasPrecision(10,2);

            this.Property(t => t.Fiyat).HasColumnName("Fiyat").HasColumnType("Money");

            this.Property(t => t.Status).HasColumnName("Status");

            this.Property(t => t.SystemStatus).HasColumnName("SystemStatus");

            // Table & Column Mappings
            this.ToTable("Ilaclar");

        }
    }
}