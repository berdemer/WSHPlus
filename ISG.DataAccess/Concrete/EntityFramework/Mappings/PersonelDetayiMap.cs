using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;


namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class PersonelDetayiMap : EntityTypeConfiguration<PersonelDetayi>
    {
        public PersonelDetayiMap()
        {
            // Primary Key
            this.HasKey(t => t.PersonelDetay_Id);

            this.Property(t => t.PersonelDetay_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Properties
            this.Property(t => t.DogumYeri)
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.Uyruk)
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.EgitimSeviyesi)
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.AskerlikDurumu)
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.MedeniHali)
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.anne_adi)
                .IsFixedLength()
                .HasMaxLength(40);

            this.Property(t => t.baba_adi)
                .IsFixedLength()
                .HasMaxLength(40);

            this.Property(t => t.rowVersion)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            this.Property(t => t.UserId).
            IsFixedLength().
            HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("PersonelDetayi");
            this.Property(t => t.PersonelDetay_Id).HasColumnName("PersonelDetay_Id");
            this.Property(t => t.DogumTarihi).HasColumnName("DogumTarihi");
            this.Property(t => t.DogumYeri).HasColumnName("DogumYeri");
            this.Property(t => t.Cinsiyet).HasColumnName("Cinsiyet");
            this.Property(t => t.Uyruk).HasColumnName("Uyruk");
            this.Property(t => t.EgitimSeviyesi).HasColumnName("EgitimSeviyesi");
            this.Property(t => t.AskerlikDurumu).HasColumnName("AskerlikDurumu");
            this.Property(t => t.IlkIseBaslamaTarihi).HasColumnName("IlkIseBaslamaTarihi");
            this.Property(t => t.MedeniHali).HasColumnName("MedeniHali");
            this.Property(t => t.CocukSayisi).HasColumnName("CocukSayisi");
            this.Property(t => t.anne_adi).HasColumnName("anne_adi");
            this.Property(t => t.baba_adi).HasColumnName("baba_adi");
            this.Property(t => t.anne_sag).HasColumnName("anne_sag");
            this.Property(t => t.anne_sag_bilgisi).HasColumnName("anne_sag_bilgisi");
            this.Property(t => t.baba_sag).HasColumnName("baba_sag");
            this.Property(t => t.baba_sag_bilgisi).HasColumnName("baba_sag_bilgisi");
            this.Property(t => t.KardesSayisi).HasColumnName("KardesSayisi");
            this.Property(t => t.Kardes_Sag_Bilgisi).HasColumnName("Kardes_Sag_Bilgisi");

            this.HasRequired(t => t.Personel)
                 .WithOptional(t => t.PersonelDetayi).WillCascadeOnDelete(true);
        }
    }
}
