using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;



namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class EgitimHayatiMap : EntityTypeConfiguration<EgitimHayati>
    {
        public EgitimHayatiMap()
        {
            // Primary Key
            this.HasKey(t => t.EgitimHayati_Id);

            // Properties
            this.Property(t => t.EgitimHayati_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Egitim_seviyesi)
                .IsFixedLength()
                .HasMaxLength(20);

            this.Property(t => t.Okul_Adi)
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.Meslek_Tanimi)
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.UserId).
            IsFixedLength().
            HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("EgitimHayatlari");
            this.Property(t => t.EgitimHayati_Id).HasColumnName("EgitimHayati_Id");
            this.Property(t => t.Egitim_seviyesi).HasColumnName("Egitim_seviyesi");
            this.Property(t => t.Okul_Adi).HasColumnName("Okul_Adi");
            this.Property(t => t.Baslama_Tarihi).HasColumnName("Baslama_Tarihi");
            this.Property(t => t.Bitis_Tarihi).HasColumnName("Bitis_Tarihi");
            this.Property(t => t.Meslek_Tanimi).HasColumnName("Meslek_Tanimi");
            this.Property(t => t.Personel_Id).HasColumnName("Personel_Id");
            this.Property(t => t.UserId).HasColumnName("UserId");

            // Relationships
            this.HasRequired(t => t.Personel)
                .WithMany(t => t.EgitimHayatlari)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);

        }
    }
}
