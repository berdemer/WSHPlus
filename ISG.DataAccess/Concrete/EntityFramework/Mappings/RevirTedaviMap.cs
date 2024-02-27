using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;


namespace  ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class RevirTedaviMap : EntityTypeConfiguration<RevirTedavi>
    {
        public RevirTedaviMap()
        {
            // Primary Key
            HasKey(t => t.RevirTedavi_Id);

            // Properties
            Property(t => t.RevirTedavi_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            Property(t => t.UserId)
                .IsFixedLength()
                .HasMaxLength(40);

            // Table & Column Mappings
            ToTable("RevirTedavileri");
            Property(t => t.RevirTedavi_Id).HasColumnName("RevirTedavi_Id");
            Property(t => t.Sikayeti).HasColumnName("Sikayeti");
            Property(t => t.Tani).HasColumnName("Tani");
            Property(t => t.HastaninIlaclari).HasColumnName("HastaninIlaclari");
            Property(t => t.Protokol).HasColumnName("Protokol");
            Property(t => t.Sonuc).HasColumnName("Sonuc");
            Property(t => t.MuayeneTuru).HasColumnName("MuayeneTuru");
            Property(t => t.RevirIslem_Id).HasColumnName("RevirIslem_Id");
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.UserId).IsFixedLength().HasMaxLength(40).HasColumnName("UserId");
            HasOptional(t => t.RevirIslem)
                    .WithMany(t => t.RevirTedavileri)
                    .HasForeignKey(d => d.RevirIslem_Id).WillCascadeOnDelete(true);

        }
    }
}