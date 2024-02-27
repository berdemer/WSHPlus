using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;


namespace  ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class PsikolojikTestMap : EntityTypeConfiguration<PsikolojikTest>
    {
        public PsikolojikTestMap()
        {
            // Primary Key
            HasKey(t => t.PsikolojikTest_Id);

            // Properties
            Property(t => t.PsikolojikTest_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            Property(t => t.UserId)
                .IsFixedLength()
                .HasMaxLength(40);

            // Table & Column Mappings
            ToTable("PsikolojikTestleri");
            Property(t => t.PsikolojikTest_Id).HasColumnName("PsikolojikTest_Id");
            Property(t => t.TestAdi).HasColumnName("TestAdi");
            Property(t => t.TestJson).HasColumnName("TestJson");
            Property(t => t.Protokol).HasColumnName("Protokol");
            Property(t => t.Sonuc).HasColumnName("Sonuc");
            Property(t => t.MuayeneTuru).HasColumnName("MuayeneTuru");
            Property(t => t.RevirIslem_Id).HasColumnName("RevirIslem_Id");
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.UserId).IsFixedLength().HasMaxLength(40).HasColumnName("UserId");

            // Relationships
            HasOptional(t => t.RevirIslem)
                .WithMany(t => t.PsikolojikTestler)
                .HasForeignKey(d => d.RevirIslem_Id).WillCascadeOnDelete(true);
            HasRequired(t => t.Personel)
                .WithMany(t => t.PsikolojikTestler)
                .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);

        }
    }
}
