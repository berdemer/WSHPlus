using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;


namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class IsgEgitimiMap : EntityTypeConfiguration<IsgEgitimi>
    {
        public IsgEgitimiMap()
        {
            // Primary Key
            this.HasKey(t => t.Egitim_Id);

            // Properties
            this.Property(t => t.Egitim_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Tanimi)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(150);

            this.Property(t => t.UserId).
            IsFixedLength().
            HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("IsgEgitimleri");
            this.Property(t => t.Egitim_Id).HasColumnName("Egitim_Id");
            this.Property(t => t.Egitim_Turu).HasColumnName("Egitim_Turu");
            this.Property(t => t.Tanimi).HasColumnName("Tanimi");
            Property(t => t.Tarih).HasColumnName("Tarih") ;
            this.Property(t => t.VerildigiTarih).HasColumnName("VerildigiTarih");
            this.Property(t => t.Egitim_Suresi).HasColumnName("Egitim_Suresi");
            this.Property(t => t.IsgEgitimiVerenPersonel).HasColumnName("IsgEgitimiVerenPersonel");
            this.Property(t => t.Personel_Id).HasColumnName("Personel_Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.IsgTopluEgitimi_Id).HasColumnName("IsgTopluEgitimi_Id");
            this.Property(t => t.DersTipi).HasColumnName("DersTipi");
            // Relationships
            this.HasRequired(t => t.Personel)
                .WithMany(t => t.IsgEgitimleri)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);


            this.HasRequired(t => t.IsgTopluEgitimi)
                .WithMany(t => t.IsgEgitimleri)
                .HasForeignKey(d => d.IsgTopluEgitimi_Id).WillCascadeOnDelete(true);

        }
    }
}
