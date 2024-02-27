using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using ISG.Entities.Concrete;


namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class PersonelMap : EntityTypeConfiguration<Personel>
    {
        public PersonelMap()
        {
            // Primary Key
            this.HasKey(t => t.Personel_Id);

            this.Property(t => t.Personel_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t=>t.PerGuid).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            // entityde guid olarak tanmlandý. .Identity olarak tanýmlaynca veritaban guid olarak tanmlyor.
            this.Property(t => t.Adi)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(50);
        //    .HasColumnAnnotation(
        //"Index",//index atamak için yazýldý
        //new IndexAnnotation(new IndexAttribute("Adi")));

            this.Property(t => t.Soyadi)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.TcNo)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(11);

            this.Property(t => t.KanGrubu)
                .IsFixedLength()
                .HasMaxLength(20);

            this.Property(t => t.SgkNo)
                .IsFixedLength()
                .HasMaxLength(30);

            this.Property(t => t.SicilNo)
                .IsFixedLength()
                .HasMaxLength(20);

            this.Property(t => t.KadroDurumu)
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.Photo)
                .IsFixedLength()
                .HasMaxLength(100);

            this.Property(t => t.Mail)
                .IsFixedLength()
                .HasMaxLength(55);

            this.Property(t => t.Gorevi)
                .IsFixedLength()
                .HasMaxLength(55);

            this.Property(t => t.Telefon)
                .IsFixedLength()
                .HasMaxLength(30);

            this.Property(t => t.rowVersion)
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            this.Property(t => t.UserId).
         IsFixedLength().
         HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("Personel");
           // this.ToTable("Personel").HasIndex(p => new { p.Adi, p.Soyadi });//ikili index oluþturma
            this.Property(t => t.Personel_Id).HasColumnName("Personel_Id");
            this.Property(t => t.PerGuid).HasColumnName("PerGuid_Id");
            this.Property(t => t.Adi).HasColumnName("Adi");
            this.Property(t => t.Soyadi).HasColumnName("Soyadi");
            this.Property(t => t.TcNo).HasColumnName("TcNo");
            this.Property(t => t.KanGrubu).HasColumnName("KanGrubu");
            this.Property(t => t.Sirket_Id).HasColumnName("Sirket_Id");
            this.Property(t => t.Bolum_Id).HasColumnName("Bolum_Id");
            this.Property(t => t.SicilNo).HasColumnName("SicilNo");
            this.Property(t => t.KadroDurumu).HasColumnName("KadroDurumu");
            this.Property(t => t.SgkNo).HasColumnName("SgkNo");
            this.Property(t => t.Photo).HasColumnName("Photo");
            this.Property(t => t.Mail).HasColumnName("Mail");
            this.Property(t => t.Gorevi).HasColumnName("Gorevi");
            this.Property(t => t.Telefon).HasColumnName("Telefon");
            this.Property(t => t.Status).HasColumnName("Durumu").HasColumnType("bit");
            this.Property(t => t.rowVersion).HasColumnName("rowVersion");
            this.Property(t => t.UserId).HasColumnName("UserId");
            // Relationships
            this.HasOptional(t => t.SirketBolumleri)//personel class da  public virtual Sirket Sirketler { get; set; }
                .WithMany(t => t.Personeller)//Sirket Classda   public virtual ICollection<Personel> Personeller { get; set; }
                .HasForeignKey(d => d.Bolum_Id);// baglayýcý olarak bolum ýd verildi...
            this.HasOptional(t => t.Sirketler)
                .WithMany(t => t.Personeller)
                .HasForeignKey(d => d.Sirket_Id);

        }
    }
}
