using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;


namespace  ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class PansumanMap : EntityTypeConfiguration<Pansuman>
    {
        public PansumanMap()
        {
            // Primary Key
            HasKey(t => t.Pansuman_Id);

            // Properties
            Property(t => t.Pansuman_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            // Table & Column Mappings
            ToTable("Pansumanlari");
            Property(t => t.Pansuman_Id).HasColumnName("Pansuman_Id");
            Property(t => t.PansumanTuru).IsFixedLength().HasMaxLength(30).HasColumnName("PansumanTuru");
            Property(t => t.IsKazasi).HasColumnName("IsKazasi");
            Property(t => t.YaraCesidi).HasColumnName("YaraCesidi").IsFixedLength().HasMaxLength(30);
            Property(t => t.YaraYeri).HasColumnName("YaraYeri");
            Property(t => t.PansumaninAmaci).HasColumnName("PansumaninAmaci").IsFixedLength().HasMaxLength(100);
            Property(t => t.PansumanTuru).HasColumnName("PansumanTuru").IsFixedLength().HasMaxLength(30);
            Property(t => t.SuturBicimi).HasColumnName("SuturBicimi").IsFixedLength().HasMaxLength(30);
            Property(t => t.RevirIslem_Id).HasColumnName("RevirIslem_Id");
            Property(t => t.Protokol).HasColumnName("Protokol");
            Property(t => t.Sonuc).HasColumnName("Sonuc");
            Property(t => t.MuayeneTuru).HasColumnName("MuayeneTuru");
            Property(t => t.RevirIslem_Id).HasColumnName("RevirIslem_Id");
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.UserId).IsFixedLength().HasMaxLength(40).HasColumnName("UserId");
            HasRequired(t => t.Personel)
                .WithMany(t => t.Pansumanlari)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);
            HasRequired(t => t.RevirIslem)
                .WithMany(t => t.Pansumanlari)
                .HasForeignKey(d => d.RevirIslem_Id).WillCascadeOnDelete(true);

        }
    }
}
