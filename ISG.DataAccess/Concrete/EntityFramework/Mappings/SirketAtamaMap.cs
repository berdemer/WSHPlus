using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace ISG.DataAccess.Concrete.EntityFramework.Context
{
    public class SirketAtamaMap : EntityTypeConfiguration<SirketAtama>
    {
        public  SirketAtamaMap()
         {

             this.HasKey(t => t.SirketAtama_id);

            // Properties
             this.Property(t => t.SirketAtama_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

             this.Property(t => t.uzmanPersonelId)
                .IsRequired().HasMaxLength(50);

            this.Property(t => t.Sirket_id);


            this.ToTable("SirketAtamalari");
            this.Property(t => t.SirketAtama_id).HasColumnName("id");
            this.Property(t => t.uzmanPersonelId).HasColumnName("uzmanPersonelId");
            this.Property(t => t.Sirket_id).HasColumnName("Sirket_id");

            this.HasRequired(t => t.Sirket)
              .WithMany(t => t.SirketAtamalari)
              .HasForeignKey(d => d.Sirket_id);

         }
    }
}