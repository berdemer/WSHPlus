using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;



namespace  ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class OdioMap : EntityTypeConfiguration<Odio>
    {
        public OdioMap()
        {
            // Primary Key
            HasKey(t => t.Odio_Id);

            // Properties
            Property(t => t.Odio_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.MuayeneTuru)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Odiolar");
            Property(t => t.Odio_Id).HasColumnName("Odio_Id");
            Property(t => t.Sag250).HasColumnName("Sag250");
            Property(t => t.Sag500).HasColumnName("Sag500");
            Property(t => t.Sag1000).HasColumnName("Sag1000");
            Property(t => t.Sag2000).HasColumnName("Sag2000");
            Property(t => t.Sag3000).HasColumnName("Sag3000");
            Property(t => t.Sag4000).HasColumnName("Sag4000");
            Property(t => t.Sag5000).HasColumnName("Sag5000");
            Property(t => t.Sag6000).HasColumnName("Sag6000");
            Property(t => t.Sag7000).HasColumnName("Sag7000");
            Property(t => t.Sag8000).HasColumnName("Sag8000");
            Property(t => t.Sol250).HasColumnName("Sol250");
            Property(t => t.Sol500).HasColumnName("Sol500");
            Property(t => t.Sol1000).HasColumnName("Sol1000");
            Property(t => t.Sol2000).HasColumnName("Sol2000");
            Property(t => t.Sol3000).HasColumnName("Sol3000");
            Property(t => t.Sol4000).HasColumnName("Sol4000");
            Property(t => t.Sol5000).HasColumnName("Sol5000");
            Property(t => t.Sol6000).HasColumnName("Sol6000");
            Property(t => t.Sol7000).HasColumnName("Sol7000");
            Property(t => t.Sol8000).HasColumnName("Sol8000");
            Property(t => t.SagOrtalama).HasColumnName("SagOrtalama");
            Property(t => t.SolOrtalama).HasColumnName("SolOrtalama");
            Property(t => t.Protokol).HasColumnName("Protokol");
            Property(t => t.Sonuc).HasColumnName("Sonuc");
            Property(t => t.MuayeneTuru).HasColumnName("MuayeneTuru");
            Property(t => t.RevirIslem_Id).HasColumnName("RevirIslem_Id");
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.UserId).IsFixedLength().HasMaxLength(40).HasColumnName("UserId");

            // Relationships
            HasRequired(t => t.RevirIslem)
                .WithMany(t => t.Odiolar)
                .HasForeignKey(d => d.RevirIslem_Id).WillCascadeOnDelete(true);
            HasRequired(t => t.Personel)
                .WithMany(t => t.Odiolar)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);
        }
    }
}
