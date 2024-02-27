using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;



namespace  ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class RadyolojiMap : EntityTypeConfiguration<Radyoloji>
    {
        public RadyolojiMap()
        {
            // Primary Key
            HasKey(t => t.Radyoloji_Id);

            // Properties
            Property(t => t.Radyoloji_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.MuayeneTuru)
                .HasMaxLength(100);

            // Table & Column Mappings
            ToTable("Radyolojileri");
            Property(t => t.Radyoloji_Id).HasColumnName("Radyoloji_Id");
            Property(t => t.RadyolojikTip).IsFixedLength().HasMaxLength(100).HasColumnName("RadyolojikTip");
            Property(t => t.IslemTarihi).HasColumnName("IslemTarihi");
            Property(t => t.Protokol).HasColumnName("Protokol");
            Property(t => t.RadyolojikSonuc).HasColumnName("Sonuc");
            Property(t => t.MuayeneTuru).HasColumnName("MuayeneTuru");
            Property(t => t.RevirIslem_Id).HasColumnName("RevirIslem_Id");
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.UserId).IsFixedLength().HasMaxLength(40).HasColumnName("UserId");

            // Relationships
            HasRequired(t => t.RevirIslem)
                .WithMany(t => t.Radyolojileri)
                .HasForeignKey(d => d.RevirIslem_Id).WillCascadeOnDelete(true);
            HasRequired(t => t.Personel)
                .WithMany(t => t.Radyolojileri)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);
        }
    }
}
