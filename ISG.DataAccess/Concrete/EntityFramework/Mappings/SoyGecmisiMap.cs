using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;


namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class SoyGecmisiMap : EntityTypeConfiguration<SoyGecmisi>
    {
        public SoyGecmisiMap()
        {
            // Primary Key
            HasKey(t => t.SoyGecmisi_Id);

            // Properties
            Property(t => t.SoyGecmisi_Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.HastalikAdi)
                .IsFixedLength()
                .IsRequired()
                .HasMaxLength(70);

            Property(t => t.AkrabalikDurumi)
                .IsFixedLength()
                .IsRequired()
                .HasMaxLength(50).HasColumnName("AkrabalikDurumi");


            Property(t => t.UserId).
           IsFixedLength().
           HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("SoyGecmisleri");
            this.Property(t => t.SoyGecmisi_Id).HasColumnName("SoyGecmisi_Id");
            this.Property(t => t.HastalikAdi).HasColumnName("HastalikAdi");
            this.Property(t => t.AkrabaninYasi).HasColumnName("AkrabaninYasi");
            this.Property(t => t.AkrabalikDurumi).HasColumnName("AkrabalikDurumi");
            this.Property(t => t.AkrabaninHastaOlduguYas).HasColumnName("AkrabaninHastaOlduguYas");
            this.Property(t => t.HastalikSuAnAktifmi).HasColumnName("HastalikSuAnAktifmi");
            this.Property(t => t.ICD10).HasColumnName("ICD10");
            this.Property(t => t.OlumNedeni).HasColumnName("OlumNedeni");
            this.Property(t => t.OlumYasi).HasColumnName("OlumYasi");
            this.Property(t => t.Tarihi).HasColumnName("Tarihi");
            this.Property(t => t.Aciklama).HasColumnName("Aciklama");
            this.Property(t => t.Personel_Id).HasColumnName("Personel_Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            // Relationships
            this.HasRequired(t => t.Personel)
                .WithMany(t => t.SoyGecmisleri)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);

        }
    }
}
