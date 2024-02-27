using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;


namespace  ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class LaboratuarMap : EntityTypeConfiguration<Laboratuar>
    {
        public LaboratuarMap()
        {
            // Primary Key
            this.HasKey(t => t.Laboratuar_Id);

            // Properties
            this.Property(t => t.Laboratuar_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Grubu)
                .HasMaxLength(150).IsRequired();

            Property(t => t.MuayeneTuru)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Laboratuarlari");
            this.Property(t => t.Laboratuar_Id).HasColumnName("Laboratuar_Id");
            this.Property(t => t.Grubu).HasColumnName("Grubu");
            this.Property(t => t.Sonuc).HasColumnName("Sonuc");
            Property(t => t.Protokol).HasColumnName("Protokol");
            Property(t => t.MuayeneTuru).HasColumnName("MuayeneTuru");
            Property(t => t.RevirIslem_Id).HasColumnName("RevirIslem_Id");
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.UserId).IsFixedLength().HasMaxLength(40).HasColumnName("UserId");
            // Relationships
            HasRequired(t => t.RevirIslem)
                .WithMany(t => t.Laboratuarlari)
                .HasForeignKey(d => d.RevirIslem_Id).WillCascadeOnDelete(true);
            HasRequired(t => t.Personel)
                .WithMany(t => t.Laboratuarlari)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);

        }
    }
}
