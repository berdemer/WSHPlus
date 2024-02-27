using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;



namespace  ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class PeriyodikMuayeneMap : EntityTypeConfiguration<PeriyodikMuayene>
    {
        public PeriyodikMuayeneMap()
        {
            // Primary Key
            HasKey(t => t.PeriyodikMuayene_Id);

            // Properties
            Property(t => t.PeriyodikMuayene_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.MuayeneTuru)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("PeriyodikMuayeneleri");
            Property(t => t.PeriyodikMuayene_Id).HasColumnName("PeriyodikMuayene_Id");
            Property(t => t.Protokol).HasColumnName("Protokol");
            Property(t => t.PMJson).HasColumnName("PM");
            Property(t => t.MuayeneTuru).HasColumnName("MuayeneTuru");
            Property(t => t.RevirIslem_Id).HasColumnName("RevirIslem_Id");
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.UserId).IsFixedLength().HasMaxLength(40).HasColumnName("UserId");

            // Relationships
            HasRequired(t => t.RevirIslem)
                .WithMany(t => t.PeriyodikMuayeneleri)
                .HasForeignKey(d => d.RevirIslem_Id).WillCascadeOnDelete(true);
            HasRequired(t => t.Personel)
                .WithMany(t => t.PeriyodikMuayeneleri)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);
        }
    }
}
