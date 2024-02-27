using ISG.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class Mail_OnerileriMap : EntityTypeConfiguration<Mail_Onerileri>
    {
        public Mail_OnerileriMap()
        {
            // Primary Key
            HasKey(t => t.Mail_Onerileri_Id);

            Property(t => t.Sirket_Id);
            Property(t => t.Bolum_Id);
            // Properties
            Property(t => t.Mail_Onerileri_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.MailAdresi)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(150);

            Property(t => t.OneriTanimi)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(150);
            Property(t => t.UserId).IsFixedLength().HasMaxLength(40);

            // Table & Column Mappings
            ToTable("Mail_Onerileri");
            Property(t => t.Mail_Onerileri_Id).HasColumnName("Id");
            Property(t => t.Sirket_Id).HasColumnName("Sirket_Id");
            Property(t => t.Bolum_Id).HasColumnName("Bolum_Id");
            Property(t => t.MailAdresi).HasColumnName("Adresi");
            Property(t => t.MailAdiVeSoyadi).HasColumnName("AdiVeSoyadi");
            Property(t => t.UserId).HasColumnName("UserId");
            Property(t => t.TumSirketteOneriListesinde).HasColumnName("TumSirketteOneriListesinde");
            Property(t => t.gonderimSekli).HasColumnName("gonderimSekli").IsFixedLength().HasMaxLength(5);
            HasRequired(t => t.Sirket)
              .WithMany(t => t.Mail_Onerileri)
              .HasForeignKey(d => d.Sirket_Id);
            HasRequired(t => t.SirketBolumu)
              .WithMany(t => t.Mail_Onerileri)
              .HasForeignKey(d => d.Bolum_Id).WillCascadeOnDelete(false);


        }
    }
}
