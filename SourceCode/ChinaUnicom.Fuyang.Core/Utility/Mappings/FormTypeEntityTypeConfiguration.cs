using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ChinaUnicom.Fuyang.Core.Utility.Models;

namespace ChinaUnicom.Fuyang.Core.Utility.Mappings
{
    class FormTypeEntityTypeConfiguration : EntityTypeConfiguration<FormType>
    {
        public FormTypeEntityTypeConfiguration() 
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FormName)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.FormShortName)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.FormDesc)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("CWF_OWNER.WF_FRM");
            this.Property(t => t.Id).HasColumnName("WF_FRM_ID");
            this.Property(t => t.FormName).HasColumnName("FRM_NM");
            this.Property(t => t.FormShortName).HasColumnName("FRM_SHRT_NM");
            this.Property(t => t.FormDesc).HasColumnName("FRM_DESC");
        }
    }
}
