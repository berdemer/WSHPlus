using ISG.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class IkazMap : EntityTypeConfiguration<Ikaz>
    {
        public IkazMap()
        {
            // Primary Key
            HasKey(t => t.Ikaz_Id);

            // Properties
            Property(t => t.Ikaz_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            Property(t => t.UserId).IsFixedLength().HasMaxLength(40);

            // Table & Column Mappings
            ToTable("Ikazlar");
            Property(t => t.Ikaz_Id).HasColumnName("Ikaz_Id");
            Property(t => t.IkazText).HasColumnName("IkazText");
            Property(t => t.SonucIkazText).HasColumnName("SonucIkazText");
            Property(t => t.Personel_Id).HasColumnName("Personel_Id");
            Property(t => t.Status).HasColumnName("Durumu").HasColumnType("bit");
            Property(t => t.UserId).HasColumnName("UserId");
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.SonTarih).HasColumnName("SonTarih");
            Property(t => t.MuayeneTuru).HasColumnName("MuayeneTuru").HasMaxLength(50);
            // Relationships
            HasRequired(t => t.Personel)
                .WithMany(t => t.Ikazlar)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);

        }
    }
}
