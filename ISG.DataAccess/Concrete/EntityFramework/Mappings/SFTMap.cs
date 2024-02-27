using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;




namespace  ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class SFTMap : EntityTypeConfiguration<SFT>
    {
        public SFTMap()
        {
            // Primary Key
            HasKey(t => t.Sft_Id);

            // Properties
            Property(t => t.Sft_Id)
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.MuayeneTuru)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("SFTleri");
            Property(t => t.Sft_Id).HasColumnName("Sft_Id");
            Property(t => t.FVC).HasColumnName("FVC");
            Property(t => t.FEV1).HasColumnName("FEV1");
            Property(t => t.Fev1Fvc).HasColumnName("Fev1Fvc");
            Property(t => t.VC).HasColumnName("VC");
            Property(t => t.RV).HasColumnName("RV");
            Property(t => t.TLC).HasColumnName("TLC");
            Property(t => t.Fev2575).HasColumnName("Fev2575");
            Property(t => t.FEV50).HasColumnName("FEV50");
            Property(t => t.MVV).HasColumnName("MVV");
            Property(t => t.Protokol).HasColumnName("Protokol");
            Property(t => t.Sonuc).HasColumnName("Sonuc");
            Property(t => t.MuayeneTuru).HasColumnName("MuayeneTuru");
            Property(t => t.RevirIslem_Id).HasColumnName("RevirIslem_Id");
            Property(t => t.Tarih).HasColumnName("Tarih");
            Property(t => t.UserId).IsFixedLength().HasMaxLength(40).HasColumnName("UserId");
            // Relationships
            HasRequired(t => t.RevirIslem)
                .WithMany(t => t.SFTleri)
                .HasForeignKey(d => d.RevirIslem_Id).WillCascadeOnDelete(true);
            HasRequired(t => t.Personel)
           .WithMany(t => t.SFTleri)
           .HasForeignKey(d => d.Personel_Id).WillCascadeOnDelete(true);
        }
    }
}
