using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class SirketDetayiMap : EntityTypeConfiguration<SirketDetayi>
    {
        public SirketDetayiMap() 
        {
            this.HasKey(t => t.SirketDetayi_Id);

            // Properties
            this.Property(t => t.SirketDetayi_Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SGKSicilNo)
                .HasMaxLength(50);

            this.Property(t => t.Telefon)
                .HasMaxLength(50);

            this.Property(t => t.Faks)
                .HasMaxLength(50);

            this.Property(t => t.Mail)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SirketDetaylari");
            this.Property(t => t.SirketDetayi_Id).HasColumnName("Id");
            this.Property(t => t.SGKSicilNo).HasColumnName("SGKSicilNo");
            this.Property(t => t.Adres).HasColumnName("Adres");
            this.Property(t => t.Telefon).HasColumnName("Telefon");
            this.Property(t => t.Faks).HasColumnName("Faks");
            this.Property(t => t.Mail).HasColumnName("Mail");
            this.Property(t => t.SirketYetkilisi).HasColumnName("SirketYetkilisi");
            this.Property(t => t.SirketYetkilisiTcNo).HasColumnName("SirketYetkilisiTcNo");
            this.Property(t => t.WebUrl).HasColumnName("WebUrl");
            this.Property(t => t.WebUrlApi).HasColumnName("WebUrlApi");
            this.Property(t => t.Nacekodu).HasColumnName("Nacekodu");
            this.Property(t => t.TehlikeGrubu).HasColumnName("TehlikeGrubu");
            // Relationships
            this.HasRequired(t => t.Sirket)
                .WithOptional(t => t.SirketDetayi);//one-to-zero-one

            //this.HasRequired(t => t.Sirket)
            //  .WithRequiredDependent(t => t.SirketDetayi);//one-to-one

        }
    }
}
