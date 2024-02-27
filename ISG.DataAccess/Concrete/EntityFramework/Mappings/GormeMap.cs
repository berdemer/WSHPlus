using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;



namespace  ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class GormeMap : EntityTypeConfiguration<Gorme>
    {
        public GormeMap()
        {
            // Primary Key
            HasKey(t => t.Gorme_Id);

            // Properties
            Property(t => t.Gorme_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.MuayeneTuru)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Gormeleri");
            Property(t => t.Gorme_Id).HasColumnName("Gorme_Id");
            Property(t => t.GozKapagi).IsFixedLength().HasMaxLength(30).HasColumnName("GozKapagi");
            Property(t => t.GozKuresi).IsFixedLength().HasMaxLength(30).HasColumnName("GozKuresi");
            Property(t => t.GozKaymasi).IsFixedLength().HasMaxLength(30).HasColumnName("GozKaymasi");
            Property(t => t.GozdeKizariklik).IsFixedLength().HasMaxLength(30).HasColumnName("GozdeKizariklik");
            Property(t => t.PupillaninDurumu).IsFixedLength().HasMaxLength(30).HasColumnName("PupillaninDurumu");
            Property(t => t.IsikRefleksi).IsFixedLength().HasMaxLength(30).HasColumnName("IsikRefleksi");
            Property(t => t.GormeAlani).IsFixedLength().HasMaxLength(30).HasColumnName("GormeAlani");
            Property(t => t.GozTonusu).IsFixedLength().HasMaxLength(30).HasColumnName("GozTonusu");
            Property(t => t.Fundoskopi).IsFixedLength().HasColumnName("Fundoskopi");
            Property(t => t.GormeKeskinligi).IsFixedLength().HasMaxLength(30).HasColumnName("GormeKeskinligi");
            Property(t => t.DerinlikDuyusu).IsFixedLength().HasMaxLength(30).HasColumnName("DerinlikDuyusu");
            Property(t => t.RenkKorlugu).IsFixedLength().HasMaxLength(20).HasColumnName("RenkKorlugu");
            Property(t => t.Protokol).HasColumnName("Protokol");
            Property(t => t.Sonuc).HasColumnName("Sonuc");
            Property(t => t.MuayeneTuru).HasColumnName("MuayeneTuru");
            Property(t => t.RevirIslem_Id).HasColumnName("RevirIslem_Id");
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.UserId).IsFixedLength().HasMaxLength(40).HasColumnName("UserId");

            // Relationships
            HasRequired(t => t.RevirIslem)
                .WithMany(t => t.Gormeleri)
                .HasForeignKey(d => d.RevirIslem_Id).WillCascadeOnDelete(true);
            HasRequired(t => t.Personel)
                .WithMany(t => t.Gormeleri)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);
        }
    }
}
