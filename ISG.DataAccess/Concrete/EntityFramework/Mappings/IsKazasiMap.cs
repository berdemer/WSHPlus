using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;



namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class IsKazasiMap : EntityTypeConfiguration<IsKazasi>
    {
        public IsKazasiMap()
        {
            // Primary Key
            HasKey(t => t.IsKazasi_Id);

            // Properties
            Property(t => t.IsKazasi_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.MuayeneTuru)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("IsKazalari");
            Property(t => t.IsKazasi_Id).HasColumnName("IsKazasi_Id");
            Property(t => t.Protokol).HasColumnName("Protokol");
            Property(t => t.PMJson).HasColumnName("PM");
            Property(t => t.MuayeneTuru).HasColumnName("MuayeneTuru");
            Property(t => t.RevirIslem_Id).HasColumnName("RevirIslem_Id");
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.UserId).IsFixedLength().HasMaxLength(40).HasColumnName("UserId");

            // Relationships
            HasRequired(t => t.RevirIslem)
                .WithMany(t => t.IsKazalari)
                .HasForeignKey(d => d.RevirIslem_Id).WillCascadeOnDelete(true);
            HasRequired(t => t.Personel)
                .WithMany(t => t.IsKazalari)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);
        }
    }
}
