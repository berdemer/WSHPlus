using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;


namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class AllerjiMap : EntityTypeConfiguration<Allerji>
    {
        public AllerjiMap()
        {
            // Primary Key
            this.HasKey(t => t.Allerji_Id);

            // Properties
            this.Property(t => t.Allerji_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.AllerjiOykusu)
                .IsFixedLength()
                .HasMaxLength(350);

            this.Property(t => t.UserId).
             IsFixedLength().
             HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("Allerjiler");
            this.Property(t => t.Allerji_Id).HasColumnName("Allerji_Id");
            this.Property(t => t.AllerjiOykusu).HasColumnName("Oykusu");
            this.Property(t => t.AllerjiCesiti).HasColumnName("Cesiti");
            this.Property(t => t.AllerjiEtkeni).HasColumnName("Etken");
            this.Property(t => t.AllerjiSuresi).HasColumnName("SureYil");
            this.Property(t => t.Personel_Id).HasColumnName("Personel_Id");
            this.Property(t => t.UserId).HasColumnName("UserId");

            // Relationships
            this.HasRequired(t => t.Personel)
                .WithMany(t => t.Allerjiler)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);

        }
    }
}
