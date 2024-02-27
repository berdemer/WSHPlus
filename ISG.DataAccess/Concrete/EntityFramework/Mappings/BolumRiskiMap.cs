using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;



namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class BolumRiskiMap : EntityTypeConfiguration<BolumRiski>
    {
        public BolumRiskiMap()
        {
            // Primary Key
            HasKey(t => t.BolumRiski_Id);

            // Properties
            Property(t => t.BolumRiski_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

          

            // Table & Column Mappings
            ToTable("BolumRiskleri ");
            Property(t => t.BolumRiski_Id).HasColumnName("BolumRiski_Id");
            Property(t => t.PMJson).HasColumnName("BR");
            Property(t => t.Sirket_Id).HasColumnName("Sirket_Id");
            Property(t => t.Bolum_Id).HasColumnName("Bolum_Id");
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.UserId).IsFixedLength().HasMaxLength(40).HasColumnName("UserId");
        }
    }
}
