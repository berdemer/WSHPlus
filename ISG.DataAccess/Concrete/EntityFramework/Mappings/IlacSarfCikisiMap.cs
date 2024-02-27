using ISG.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class IlacSarfCikisiMap : EntityTypeConfiguration<IlacSarfCikisi>
    {
        public IlacSarfCikisiMap()
        {
            // Primary Key
            HasKey(t => t.IlacSarfCikisi_Id);
            Property(t => t.IlacSarfCikisi_Id);
            // Properties
            Property(t => t.IlacSarfCikisi_Id).HasColumnName("IlacSarfCikisi_Id");
            Property(t => t.StokId).HasColumnName("StokId");
            Property(t => t.SaglikBirimi_Id).HasColumnName("SaglikBirimi_Id");
            Property(t => t.RevirTedavi_Id).HasColumnName("RevirTedavi_Id");
            Property(t => t.SarfMiktari).HasColumnName("SarfMiktari");
            Property(t => t.Status).HasColumnName("Status");
            Property(t => t.UserId).IsFixedLength().HasMaxLength(40).HasColumnName("UserId");
            Property(t => t.Tarih).HasColumnName("Tarih");
            ToTable("IlacSarfCikislari");
      
            HasRequired(t => t.IlacStoklari)
                .WithMany(t => t.IlacSarfCikislari)
                .HasForeignKey(d => d.StokId).WillCascadeOnDelete(true);
            HasRequired(t => t.RevirTedavileri)
                .WithMany(t => t.IlacSarfCikislari)
                .HasForeignKey(d => d.RevirTedavi_Id).WillCascadeOnDelete(true);
            HasRequired(t => t.SaglikBirimleri)
               .WithMany(t => t.IlacSarfCikislari)
               .HasForeignKey(d => d.SaglikBirimi_Id).WillCascadeOnDelete(true);

        }
    }
}