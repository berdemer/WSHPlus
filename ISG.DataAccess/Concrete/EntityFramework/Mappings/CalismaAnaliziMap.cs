using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;



namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class CalismaAnalizMap : EntityTypeConfiguration<CalismaAnalizi>
    {
        public CalismaAnalizMap()
        {
            // Primary Key
            HasKey(t => t.CalismaAnalizi_Id);

            // Properties
            Property(t => t.CalismaAnalizi_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

          

            // Table & Column Mappings
            ToTable("CalismaAnalizleri");
            Property(t => t.CalismaAnalizi_Id).HasColumnName("CalismaAnalizi_Id");
            Property(t => t.CAJson).HasColumnName("BR");
            Property(t => t.Sirket_Id).HasColumnName("Sirket_Id");
            Property(t => t.Bolum_Id).HasColumnName("Bolum_Id");
            Property(t => t.MeslekAdi).HasColumnName("MeslekAdi");
            Property(t => t.Meslek_Kodu).HasColumnName("Meslek_Kodu");
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.UserId).IsFixedLength().HasMaxLength(40).HasColumnName("UserId");
        }
    }
}
