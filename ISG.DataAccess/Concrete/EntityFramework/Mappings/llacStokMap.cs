using ISG.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class IlacStokMap : EntityTypeConfiguration<IlacStok>
    {
        public IlacStokMap()
        {
            //// Primary Key
            HasKey(t => t.StokId);

            //// Properties
            Property(t => t.StokId).HasColumnName("StokId");

            Property(t => t.SaglikBirimi_Id).HasColumnName("SaglikBirimi_Id");

            Property(t => t.IlacAdi).HasColumnName("IlacAdi")
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(250);
            Property(t => t.StokTuru).HasColumnName("StokTuru").IsRequired();
            Property(t => t.StokMiktari).HasColumnName("StokMiktari");
            Property(t => t.KritikStokMiktari).HasColumnName("KritikStokMiktari");
            Property(t => t.StokMiktarBirimi).HasColumnName("StokMiktarBirimi")
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(15);
            Property(t => t.Status).HasColumnName("Status");

            //// Table & Column Mappings
            ToTable("IlacStoklari");
        }
    }
}
