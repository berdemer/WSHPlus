using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;


namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class AliskanlikMap : EntityTypeConfiguration<Aliskanlik>
    {
        public AliskanlikMap()
        {
            // Primary Key
            this.HasKey(t => t.Aliskanlik_Id);

            // Properties
            this.Property(t => t.Aliskanlik_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Madde)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.SiklikDurumu)
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.UserId).
                IsFixedLength().
                HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("Aliskanliklar");
            this.Property(t => t.Aliskanlik_Id).HasColumnName("Aliskanlik_Id");
            this.Property(t => t.Madde).HasColumnName("Madde");
            this.Property(t => t.BaslamaTarihi).HasColumnName("BaslamaTarihi");
            this.Property(t => t.BitisTarihi).HasColumnName("BitisTarihi");
            this.Property(t => t.SiklikDurumu).HasColumnName("SiklikDurumu");
            this.Property(t => t.Aciklama).HasColumnName("Aciklama");
            this.Property(t => t.Personel_Id).HasColumnName("Personel_Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            // Relationships
            this.HasRequired(t => t.Personel)
                .WithMany(t => t.Aliskanliklar)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);

        }
    }
}
