using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;



namespace  ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class PersonelMuayeneMap : EntityTypeConfiguration<PersonelMuayene>
    {
        public PersonelMuayeneMap()
        {
            // Primary Key
            HasKey(t => t.PersonelMuayene_Id);

            // Properties
            Property(t => t.PersonelMuayene_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.MuayeneTuru)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("PersonelMuayeneleri");
            Property(t => t.PersonelMuayene_Id).HasColumnName("PersonelMuayene_Id");
            Property(t => t.Protokol).HasColumnName("Protokol");
            Property(t => t.PMJson).HasColumnName("PM");
            Property(t => t.MuayeneTuru).HasColumnName("MuayeneTuru");
            Property(t => t.RevirIslem_Id).HasColumnName("RevirIslem_Id");
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.UserId).IsFixedLength().HasMaxLength(40).HasColumnName("UserId");

            // Relationships
            HasRequired(t => t.RevirIslem)
                .WithMany(t => t.PersonelMuayeneleri)
                .HasForeignKey(d => d.RevirIslem_Id).WillCascadeOnDelete(true);
            HasRequired(t => t.Personel)
                .WithMany(t => t.PersonelMuayeneleri)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);
        }
    }
}
