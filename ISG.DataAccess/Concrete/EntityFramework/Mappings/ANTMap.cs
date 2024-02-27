using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;


namespace  ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class ANTMap : EntityTypeConfiguration<ANT>
    {
        public ANTMap()
        {
            // Primary Key
            HasKey(t => t.ANT_Id);

            // Properties
            Property(t => t.ANT_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.TASagKolSistol)
                .HasMaxLength(50);

            Property(t => t.TASagKolDiastol)
                .HasMaxLength(50);

            Property(t => t.TASolKolSistol)
                .HasMaxLength(50);

            Property(t => t.TASolKolDiastol)
                .HasMaxLength(50);

            Property(t => t.Ates)
                .IsFixedLength()
                .HasMaxLength(10);

            Property(t => t.UserId)
                .IsFixedLength()
                .HasMaxLength(40);

            // Table & Column Mappings
            ToTable("ANTlari");
            Property(t => t.ANT_Id).HasColumnName("ANT_Id");
            Property(t => t.TASagKolSistol).HasColumnName("TASagKolSistol");
            Property(t => t.TASagKolDiastol).HasColumnName("TASagKolDiastol");
            Property(t => t.TASolKolSistol).HasColumnName("TASolKolSistol");
            Property(t => t.TASolKolDiastol).HasColumnName("TASolKolDiastol");
            Property(t => t.Nabiz).HasColumnName("Nabiz");
            Property(t => t.Ates).HasColumnName("Ates");
            Property(t => t.NabizRitmi).HasMaxLength(75).HasColumnName("NabizRitmi");
            Property(t => t.Protokol).HasColumnName("Protokol");
            Property(t => t.Sonuc).HasColumnName("Sonuc");
            Property(t => t.MuayeneTuru).HasColumnName("MuayeneTuru");
            Property(t => t.RevirIslem_Id).HasColumnName("RevirIslem_Id");
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.UserId).IsFixedLength().HasMaxLength(40).HasColumnName("UserId");

            // Relationships
            HasOptional(t => t.RevirIslem)
                .WithMany(t => t.ANTlari)
                .HasForeignKey(d => d.RevirIslem_Id).WillCascadeOnDelete(true);
            HasRequired(t => t.Personel)
                .WithMany(t => t.ANTlari)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);

        }
    }
}
