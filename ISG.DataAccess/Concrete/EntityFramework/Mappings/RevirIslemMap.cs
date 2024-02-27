using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;


namespace  ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class RevirIslemMap : EntityTypeConfiguration<RevirIslem>
    {
        public RevirIslemMap()
        {
            // Primary Key
            HasKey(t => t.RevirIslem_Id);

            // Properties
            Property(t => t.RevirIslem_Id)
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.IslemTuru)
                .HasMaxLength(150);

            Property(t => t.UserId)
                .IsFixedLength()
                .HasMaxLength(40);

            // Table & Column Mappings
            ToTable("RevirIslemleri");
            Property(t => t.RevirIslem_Id).HasColumnName("RevirIslem_Id");
            Property(t => t.SaglikBirimi_Id).HasColumnName("SaglikBirimi_Id");
            Property(t => t.IslemDetayi).HasColumnName("IslemDetayi");
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.UserId).HasColumnName("UserId");
            Property(t => t.Protokol).HasColumnName("Protokol");
            Property(t => t.Status).HasColumnName("Status");
        }
    }
}
