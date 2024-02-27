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
    public class SirketMap : EntityTypeConfiguration<Sirket>
    {
          public SirketMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.idRef)
                .IsRequired();

            this.Property(t => t.status).IsRequired();

            this.ToTable("Sirketler");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.idRef).HasColumnName("referans");
            this.Property(t => t.sirketAdi).HasColumnName("sirketAdi");
            this.Property(t => t.status).HasColumnName("durumu").HasColumnType("bit");
        }
    }
}
