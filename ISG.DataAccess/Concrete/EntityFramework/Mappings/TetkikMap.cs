using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;


namespace  ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class TetkikMap : EntityTypeConfiguration<Tetkik>
    {
        public TetkikMap()
        {
            // Primary Key
            this.HasKey(t => t.Tetkik_Id);

            // Properties
            this.Property(t => t.Tetkik_Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.MuayeneTuru)
                .HasMaxLength(50);

            this.Property(t => t.TetkikTanimi)
                .HasMaxLength(50);

            this.Property(t => t.TetkikSonucu)
                .HasMaxLength(50);

            this.Property(t => t.HekimAdi)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Tetkikleri");
            this.Property(t => t.Tetkik_Id).HasColumnName("Tetkik_Id");
            this.Property(t => t.MuayeneTuru).HasColumnName("MuayeneTuru");
            this.Property(t => t.TetkikTanimi).HasColumnName("TetkikTanimi");
            this.Property(t => t.TetkikSonucu).HasColumnName("TetkikSonucu");
            this.Property(t => t.HekimAdi).HasColumnName("HekimAdi");
            this.Property(t => t.Sikayeti).HasColumnName("Sikayeti");
            this.Property(t => t.RevirIslem_Id).HasColumnName("RevirIslem_Id");

            // Relationships
            this.HasRequired(t => t.RevirIslem)
                .WithMany(t => t.Tetkikleri)
                .HasForeignKey(d => d.RevirIslem_Id).WillCascadeOnDelete(true);

        }
    }
}
