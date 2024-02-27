using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class TanimMap : EntityTypeConfiguration<Tanim>
    {
        public TanimMap()
        {
            this.HasKey(t => t.tanim_Id);

            // Properties
            this.Property(t => t.tanim_Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.tanimAdi)
                .HasMaxLength(50);
            this.Property(t => t.tanimKisaltmasi)
                .IsFixedLength()
                .HasMaxLength(10);
            this.Property(t => t.ifade)
                .IsFixedLength();
            this.Property(t => t.ifadeBagimliligi)
                .IsFixedLength()
                .HasMaxLength(50).HasMaxLength(70);
            this.Property(t => t.aciklama)
                .IsFixedLength();
            this.Property(t => t.RowVersion)
              .IsFixedLength()
              .HasMaxLength(8)
              .IsRowVersion();

            this.ToTable("Tanimlari");
            this.Property(t => t.tanim_Id).HasColumnName("tanim_Id");
            this.Property(t => t.tanimAdi).HasColumnName("tanimAdi");
            this.Property(t => t.tanimKisaltmasi).HasColumnName("tanimKisaltmasi");
            this.Property(t => t.ifade).HasColumnName("ifade");
            this.Property(t => t.ifadeBagimliligi).HasColumnName("ifadeBagimliligi");
            this.Property(t => t.aciklama).HasColumnName("aciklama");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");

        }
    }
}
