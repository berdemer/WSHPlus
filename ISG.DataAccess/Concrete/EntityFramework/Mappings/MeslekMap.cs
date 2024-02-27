using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class MeslekMap : EntityTypeConfiguration<Meslek>
    {
        public MeslekMap()
        {
            HasKey(t => t.Meslek_id);

           Property(t => t.Meslek_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

           this.Property(t => t.MeslekAdi)
              .IsRequired()
              .HasMaxLength(50).IsConcurrencyToken();


           this.ToTable("Meslekler");
           this.Property(t => t.Meslek_id).HasColumnName("Meslek_id");
           this.Property(t => t.MeslekAdi).HasColumnName("MeslekAdi");

        }
    }
    
}
