using ISG.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class MeslekHastaliklariMap : EntityTypeConfiguration<MeslekHastaliklari>
    {
        public MeslekHastaliklariMap()
        {
            // Primary Key
            this.HasKey(t => t.MeslekHastaliklari_Id);

            // Properties
            this.Property(t => t.MeslekHastaliklari_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            // Table & Column Mappings
            this.ToTable("MeslekHastaliklari");
            this.Property(t => t.MeslekHastaliklari_Id).HasColumnName("MeslekHastaliklari_Id");
            this.Property(t => t.meslekHastalik).HasColumnName("meslekHastalik");
            this.Property(t => t.grubu).HasColumnName("grubu");
            this.Property(t => t.sure).HasColumnName("sure").IsFixedLength().HasMaxLength(20);
        }
    }
}
