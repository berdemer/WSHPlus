using ISG.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class SaglikBirimiMap : EntityTypeConfiguration<SaglikBirimi>
    {
        public SaglikBirimiMap()
        {
            // Primary Key
            HasKey(t => t.SaglikBirimi_Id);

            // Properties
            Property(t => t.SaglikBirimi_Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Adi)
                .IsRequired()
                .HasMaxLength(150);

            Property(t => t.StiId);

            Property(t => t.Protokol);

            Property(t => t.Year);

            Property(t => t.Status);

            // Table & Column Mappings
            ToTable("SaglikBirimleri");
            Property(t => t.SaglikBirimi_Id).HasColumnName("SaglikBirimi_Id");
            Property(t => t.Adi).HasColumnName("Adi");
            Property(t => t.StiId).HasColumnName("StiId");
            Property(t => t.Protokol).HasColumnName("Protokol");
            Property(t => t.Year).HasColumnName("Yil");
            Property(t => t.Status).HasColumnName("Durumu").HasColumnType("bit");
            Property(t => t.MailPort).HasColumnName("MailPort");
            Property(t => t.MailHost).HasColumnName("MailHost");
            Property(t => t.MailUserName).HasColumnName("MailUserName");
            Property(t => t.MailPassword).HasColumnName("MailPassword");
            Property(t => t.domain).HasColumnName("Domain");
            Property(t => t.mailSekli).HasColumnName("mailSekli");
            Property(t => t.mailfromAddress).HasColumnName("mailfromAddress");
            Property(t => t.EnableSsl).HasColumnName("EnableSsl").HasColumnType("bit");
            Property(t => t.UseDefaultCredentials).HasColumnName("UseDefaultCredentials").HasColumnType("bit");
        }


    }
}
