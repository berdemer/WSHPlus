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
    public class SirketUploadMap : EntityTypeConfiguration<SirketUpload>
    {
        public SirketUploadMap()
        {
            // Primary Key
            this.HasKey(t => t.SirketUpload_Id);

            // Properties
            this.Property(t => t.SirketUpload_Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Sirket_Id)
            .IsRequired();

            this.Property(t => t.MimeType)
             .IsRequired()
             .HasMaxLength(100);

            this.Property(t => t.GenericName)
             .IsRequired();

            this.Property(t => t.FileLenght)
             .IsRequired();

            this.Property(t => t.DosyaTipi).
           IsFixedLength().
           HasMaxLength(100);
            
            this.Property(t => t.Hazırlayan).
           IsFixedLength().
           HasMaxLength(100);


            this.Property(t => t.UserId).
           IsFixedLength().
           HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("SirketUploadlari");
            this.Property(t => t.SirketUpload_Id).HasColumnName("id");
            this.Property(t => t.Sirket_Id).HasColumnName("Sirket_Id");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.FileLenght).HasColumnName("FileLenght");
            this.Property(t => t.GenericName).HasColumnName("GenericName");
            this.Property(t => t.MimeType).HasColumnName("MimeType");
            this.Property(t => t.DosyaTipi).HasColumnName("DosyaTipi");
            this.Property(t => t.DosyaTipiID).HasColumnName("DosyaTipiID");
            this.Property(t => t.Konu).HasColumnName("Konu");
            this.Property(t => t.Hazırlayan).HasColumnName("Hazırlayan");
            this.Property(t => t.UserId).HasColumnName("UserId");
          
            this.HasRequired(t => t.Sirket)
            .WithMany(t => t.SirketUploadlari)
            .HasForeignKey(d => d.Sirket_Id).WillCascadeOnDelete(true);
        }
    }
}
