using ISG.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class ICDSablonuMap : EntityTypeConfiguration<ICDSablonu>
    {
        public ICDSablonuMap()
        {
            // Primary Key
            HasKey(t => t.ICDSablonu_Id);

            // Properties
            Property(t => t.ICDSablonu_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            // Table & Column Mappings
            ToTable("ICDSablonlari");
            Property(t => t.ICDSablonu_Id).HasColumnName("ICDSablonu_Id");
            Property(t => t.ICDSablonuJson).HasColumnName("ICDSablonuJson").IsRequired();
            Property(t => t.ICDkod).HasColumnName("ICDkod").IsMaxLength().HasMaxLength(5).IsRequired();
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.Status).HasColumnName("Status");
            Property(t => t.UserId).HasColumnName("UserId");



        }
    }
}
