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
    public class ImageUploadMap : EntityTypeConfiguration<imageUpload>
    {
        public ImageUploadMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.IdGuid)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.MimeType)
             .IsRequired()
             .HasMaxLength(100);

            this.Property(t => t.GenericName)
             .IsRequired();

            this.Property(t => t.FileLenght)
             .IsRequired();

            this.Property(t => t.Folder)
             .IsRequired()
             .HasMaxLength(50);

            this.Property(t => t.UserId).
           IsFixedLength().
           HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("ImageUploads");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.FileLenght).HasColumnName("FileLenght");
            this.Property(t => t.Folder).HasColumnName("Folder");
            this.Property(t => t.GenericName).HasColumnName("GenericName");
            this.Property(t => t.MimeType).HasColumnName("MimeType");
            this.Property(t => t.Protokol).HasColumnName("Protokol");
            this.Property(t => t.UserId).HasColumnName("UserId");
        }


    }
}