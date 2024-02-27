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
    public class KtubKtMap : EntityTypeConfiguration<KtubKt>
    {
        public KtubKtMap()
        {
            // Primary Key
            HasKey(t => t.KtubKt_Id);

            // Properties
            Property(t => t.KtubKt_Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            ToTable("KtubKtListesi");
            Property(t => t.KtubKt_Id).HasColumnName("KtubKt_Id");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Element).HasColumnName("Element");
            Property(t => t.FirmName).HasColumnName("FirmName");
            Property(t => t.ConfirmationDateKub).HasColumnName("ConfirmationDateKub");
            Property(t => t.ConfirmationDateKt).HasColumnName("ConfirmationDateKt");
            Property(t => t.DocumentPathKub).HasColumnName("DocumentPathKub");
            Property(t => t.DocumentPathKt).HasColumnName("DocumentPathKt");
        }
    }
}
