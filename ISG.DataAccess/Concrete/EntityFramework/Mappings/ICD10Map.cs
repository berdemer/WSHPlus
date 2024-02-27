using ISG.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Infrastructure.Annotations;

namespace ISG.DataAccess.Concrete.EntityFramework.Mappings
{
    public class ICD10Map : EntityTypeConfiguration<ICD10>
    {
        public ICD10Map()
        {
            // Primary Key
            this.HasKey(t => t.ICD10_Id);

            // Properties
            this.Property(t => t.ICD10_Id).HasColumnName("id").IsRequired();

            this.Property(t => t.IdRef).HasColumnName("referans");

            this.Property(t => t.ICDTanimi).HasColumnName("Tanimi");

            this.Property(t => t.AramaOnceligi).HasColumnName("oncelik");

            // Table & Column Mappings
            this.ToTable("ICD10");

        }
    }
}