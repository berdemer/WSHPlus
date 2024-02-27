using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;



namespace  ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class BoyKiloMap : EntityTypeConfiguration<BoyKilo>
    {
        public BoyKiloMap()
        {
            // Primary Key
            HasKey(t => t.BoyKilo_Id);

            // Properties
            Property(t => t.BoyKilo_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.MuayeneTuru)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("BoyKilolari");
            Property(t => t.BoyKilo_Id).HasColumnName("BoyKilo_Id");
            Property(t => t.Boy).HasColumnName("Boy");
            Property(t => t.Kilo).HasColumnName("Kilo");
            Property(t => t.Bel).HasColumnName("Bel");
            Property(t => t.Kalca).HasColumnName("Kalca");
            Property(t => t.BKI).HasColumnName("BKI");
            Property(t => t.BKO).HasColumnName("BKO");
            Property(t => t.Protokol).HasColumnName("Protokol");
            Property(t => t.Sonuc).HasColumnName("Sonuc");
            Property(t => t.MuayeneTuru).HasColumnName("MuayeneTuru");
            Property(t => t.RevirIslem_Id).HasColumnName("RevirIslem_Id");
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.UserId).IsFixedLength().HasMaxLength(40).HasColumnName("UserId");

            // Relationships
            HasRequired(t => t.RevirIslem)
                .WithMany(t => t.BoyKilolari)
                .HasForeignKey(d => d.RevirIslem_Id).WillCascadeOnDelete(true);
            HasRequired(t => t.Personel)
                .WithMany(t => t.BoyKilolari)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);
        }
    }
}
