using ISG.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class IsgTopluEgitimiMap : EntityTypeConfiguration<IsgTopluEgitimi>
    {
        public IsgTopluEgitimiMap()
        {
            // Primary Key
            this.HasKey(t => t.IsgTopluEgitimi_Id);

            // Properties
            this.Property(t => t.IsgTopluEgitimi_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            this.Property(t => t.UserId).IsFixedLength().HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("IsgTopluEgitimleri");
            this.Property(t => t.IsgTopluEgitimi_Id).HasColumnName("IsgTopluEgitimi_Id");
            this.Property(t => t.belgeTipi).HasColumnName("belgeTipi");
            this.Property(t => t.nitelikDurumu).HasColumnName("nitelikDurumu");
            this.Property(t => t.isgProfTckNo).HasColumnName("isgProfTckNo");
            this.Property(t => t.egitimObjects).HasColumnName("egitimObjects");
            this.Property(t => t.calisanObjects).HasColumnName("calisanObjects");
            this.Property(t => t.egitimTarihi).HasColumnName("egitimTarihi");
            this.Property(t => t.egitimYer).HasColumnName("egitimYer");
            this.Property(t => t.egitimtur).HasColumnName("egitimtur");
            this.Property(t => t.sgkTescilNo).HasColumnName("sgkTescilNo");
            this.Property(t => t.egiticiTckNo).HasColumnName("egiticiTckNo");
            this.Property(t => t.imzalýDeger).HasColumnName("imzalýDeger");
            this.Property(t => t.firmaKodu).HasColumnName("firmaKodu");
            this.Property(t => t.sorguNo).HasColumnName("sorguNo");
            this.Property(t => t.Sirket_id).HasColumnName("Sirket_id");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.Tarih).HasColumnName("Tarih");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.IsgTopluEgitimiJson).HasColumnName("EgitimJson");
            // Relationships
            this.HasRequired(t => t.Sirket)
                .WithMany(t => t.IsgTopluEgitimleri)
                .HasForeignKey(d => d.Sirket_id).WillCascadeOnDelete(true);

        }
    }
}
