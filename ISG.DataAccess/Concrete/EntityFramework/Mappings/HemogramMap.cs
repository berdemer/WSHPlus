using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;



namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class HemogramMap : EntityTypeConfiguration<Hemogram>
    {
        public HemogramMap()
        {
            // Primary Key
            HasKey(t => t.Hemogram_Id);

            // Properties
            Property(t => t.Hemogram_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.MuayeneTuru)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Hemogramlar");
            Property(t => t.Hemogram_Id).HasColumnName("Hemogram_Id");
            Property(t => t.Eritrosit).IsFixedLength().HasMaxLength(10).HasColumnName("Eritrosit");
            Property(t => t.Hematokrit).IsFixedLength().HasMaxLength(10).HasColumnName("Hematokrit");
            Property(t => t.Hemoglobin).IsFixedLength().HasMaxLength(10).HasColumnName("Hemoglobin");
            Property(t => t.MCV).IsFixedLength().HasMaxLength(10).HasColumnName("MCV");
            Property(t => t.MCH).IsFixedLength().HasMaxLength(10).HasColumnName("MCH");
            Property(t => t.MCHC).IsFixedLength().HasMaxLength(10).HasColumnName("MCHC");
            Property(t => t.RDW).IsFixedLength().HasMaxLength(10).HasColumnName("RDW");
            Property(t => t.Lokosit).IsFixedLength().HasMaxLength(10).HasColumnName("Lokosit");
            Property(t => t.Lenfosit_Yuzde).IsFixedLength().HasMaxLength(10).HasColumnName("Lenfosit_Yuzde");
            Property(t => t.Monosit_Yuzde).IsFixedLength().HasMaxLength(10).HasColumnName("Monosit_Yuzde");
            Property(t => t.Granülosit_Yuzde).IsFixedLength().HasMaxLength(10).HasColumnName("Granülosit_Yuzde");
            Property(t => t.Notrofil_Yuzde).IsFixedLength().HasMaxLength(10).HasColumnName("Notrofil_Yuzde");
            Property(t => t.Eoznofil_Yuzde).IsFixedLength().HasMaxLength(10).HasColumnName("Eoznofil_Yuzde");
            Property(t => t.Bazofil_Yuzde).IsFixedLength().HasMaxLength(10).HasColumnName("Bazofil_Yuzde");
            Property(t => t.Trombosit).IsFixedLength().HasMaxLength(10).HasColumnName("Trombosit");
            Property(t => t.MeanPlateletVolume).IsFixedLength().HasMaxLength(10).HasColumnName("MeanPlateletVolume");
            Property(t => t.Platekrit).IsFixedLength().HasMaxLength(10).HasColumnName("Platekrit");
            Property(t => t.PDW).IsFixedLength().HasMaxLength(10).HasColumnName("PDW");
            Property(t => t.Protokol).HasColumnName("Protokol");
            Property(t => t.Sonuc).HasColumnName("Sonuc");
            Property(t => t.MuayeneTuru).HasColumnName("MuayeneTuru");
            Property(t => t.RevirIslem_Id).HasColumnName("RevirIslem_Id");
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.UserId).IsFixedLength().HasMaxLength(40).HasColumnName("UserId");

            // Relationships
            HasRequired(t => t.RevirIslem)
                .WithMany(t => t.Hemogramlar)
                .HasForeignKey(d => d.RevirIslem_Id).WillCascadeOnDelete(true);
            HasRequired(t => t.Personel)
                .WithMany(t => t.Hemogramlar)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);
        }
    }
}
