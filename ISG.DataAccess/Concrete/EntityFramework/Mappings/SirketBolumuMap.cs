using ISG.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class SirketBolumuMap : EntityTypeConfiguration<SirketBolumu>
    {
         public  SirketBolumuMap()
         {
         
           this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.idRef)
                .IsRequired();

            this.Property(t => t.Sirket_id);

            this.Property(t => t.bolumAdi)
            .IsRequired();

            this.Property(t => t.status)
          .IsRequired();


            this.ToTable("SirketBolumleri");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.idRef).HasColumnName("referans");
            this.Property(t => t.bolumAdi).HasColumnName("BolumAdi");
            this.Property(t => t.Sirket_id).HasColumnName("Sirket_id");
            this.Property(t => t.status).HasColumnName("durumu").HasColumnType("bit");

            this.HasRequired(t => t.Sirket)
              .WithMany(t => t.SirketBolumleri)
              .HasForeignKey(d => d.Sirket_id);

            //this.HasOptional<Sirket>(s => s.Sirket)
            //        .WithMany(s => s.SirketBolumleri)
            //        .HasForeignKey(s => s.Sirket_id);


         }
    }
}
