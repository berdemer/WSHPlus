using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;


namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class Calisma_GecmisiMap : EntityTypeConfiguration<Calisma_Gecmisi>
    {
        public Calisma_GecmisiMap()
        {
            // Primary Key
            this.HasKey(t => t.Calisma_Gecmisi_Id);

            // Properties
            this.Property(t => t.Calisma_Gecmisi_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Calistigi_Yer_Adi)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(150);

            this.Property(t => t.Gorevi)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(100);

            this.Property(t => t.Unvani)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(100);

            this.Property(t => t.UserId).
            IsFixedLength().
            HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("Calisma_Gecmisi");
            this.Property(t => t.Calisma_Gecmisi_Id).HasColumnName("Calisma_Gecmisi_Id");
            this.Property(t => t.Calistigi_Yer_Adi).HasColumnName("Calistigi_Yer_Adi");
            this.Property(t => t.Ise_Baslama_Tarihi).HasColumnName("Ise_Baslama_Tarihi");
            this.Property(t => t.Isden_Cikis_Tarihi).HasColumnName("Isden_Cikis_Tarihi");
            this.Property(t => t.Gorevi).HasColumnName("Gorevi");
            this.Property(t => t.Unvani).HasColumnName("Unvani");
            this.Property(t => t.Personel_Id).HasColumnName("Personel_Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            // Relationships
            this.HasRequired(t => t.Personel)
                .WithMany(t => t.Calisma_Gecmisi)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);

        }
    }
}
