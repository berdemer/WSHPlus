using ISG.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class KronikHastalikMap : EntityTypeConfiguration<KronikHastalik>
    {
        public KronikHastalikMap()
        {
            HasKey(t => t.KronikHastalik_Id);

            Property(t => t.KronikHastalik_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.UserId).
           IsFixedLength().
           HasMaxLength(40);

            ToTable("KronikHastaliklar");
            Property(t => t.KronikHastalik_Id).HasColumnName("KronikHastalik_Id");
            Property(t => t.Personel_Id).HasColumnName("Personel_Id");
            Property(t => t.HastalikAdi).HasColumnName("HastalikAdi");
            Property(t => t.KullandigiIlaclar).HasColumnName("KullandigiIlaclar");
            Property(t => t.HastalikYilSuresi).HasColumnName("HastalikYilSuresi");
            Property(t => t.HastalikOzurDurumu).HasColumnName("HastalikOzurDurumu");
            Property(t => t.IsKazasi).HasColumnName("IsKazasi");
            Property(t => t.AmeliyatVarmi).HasColumnName("AmeliyatVarmi");
            Property(t => t.UserId).HasColumnName("UserId");
            Property(t => t.Tarih).HasColumnName("Tarih");
            HasRequired(t => t.Personel)
             .WithMany(t => t.KronikHastaliklar)
            .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);

        }
    }
}



