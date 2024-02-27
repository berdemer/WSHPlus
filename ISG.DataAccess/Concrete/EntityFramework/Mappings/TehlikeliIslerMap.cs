using ISG.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class TehlikeliIslerMap : EntityTypeConfiguration<TehlikeliIsler>
    {
        public TehlikeliIslerMap()
        {
            // Primary Key
            this.HasKey(t => t.TehlikeliIsler_Id);

            // Properties
            this.Property(t => t.TehlikeliIsler_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            // Table & Column Mappings
            this.ToTable("TehlikeliIsler");
            this.Property(t => t.TehlikeliIsler_Id).HasColumnName("TehlikeliIsler_Id");
            this.Property(t => t.meslek).HasColumnName("meslek");
            this.Property(t => t.grubu).HasColumnName("grubu");
            this.Property(t => t.sure).HasColumnName("sure").IsFixedLength().HasMaxLength(20);

        }
    }
}
