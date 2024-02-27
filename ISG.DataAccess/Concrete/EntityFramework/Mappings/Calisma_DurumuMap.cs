using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;


namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class Calisma_DurumuMap : EntityTypeConfiguration<Calisma_Durumu>
    {
        public Calisma_DurumuMap()
        {
            // Primary Key
            this.HasKey(t => t.Calisma_Durumu_Id);


            this.Property(t => t.Calisma_Durumu_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Properties
            this.Property(t => t.Sirket)
                .IsFixedLength()
                .HasMaxLength(150);

            this.Property(t => t.Bolum)
                .IsFixedLength()
                .HasMaxLength(150);

            this.Property(t => t.Calisma_Duzeni)
                .IsFixedLength()
                .HasMaxLength(35);

            this.Property(t => t.SicilNo)
                .IsFixedLength()
                .HasMaxLength(35);

            this.Property(t => t.KadroDurumu)
                .IsFixedLength()
                .HasMaxLength(35);

            this.Property(t => t.Gorevi)
               .IsFixedLength()
               .HasMaxLength(55);

            this.Property(t => t.UserId).
            IsFixedLength().
            HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("Calisma_Durumu");
            this.Property(t => t.Calisma_Durumu_Id).HasColumnName("Calisma_Durumu_Id");
            this.Property(t => t.Sirket).HasColumnName("Sirket");
            this.Property(t => t.Sirket_Id).HasColumnName("Sirket_Id");
            this.Property(t => t.Bolum).HasColumnName("Bolum");
            this.Property(t => t.Bolum_Id).HasColumnName("Bolum_Id");
            this.Property(t => t.Baslama_Tarihi).HasColumnName("Baslama_Tarihi");
            this.Property(t => t.Bitis_Tarihi).HasColumnName("Bitis_Tarihi");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Aciklama).HasColumnName("Aciklama");
            this.Property(t => t.Personel_Id).HasColumnName("Personel_Id");
            this.Property(t => t.Calisma_Duzeni).HasColumnName("Calisma_Duzeni");
            this.Property(t => t.SicilNo).HasColumnName("SicilNo");
            this.Property(t => t.Gorevi).HasColumnName("Gorevi");
            this.Property(t => t.KadroDurumu).HasColumnName("KadroDurumu");
            this.Property(t => t.UserId).HasColumnName("UserId");
            // Relationships
            this.HasRequired(t => t.Personel)
                .WithMany(t => t.Calisma_Durumu)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);

        }
    }
}
