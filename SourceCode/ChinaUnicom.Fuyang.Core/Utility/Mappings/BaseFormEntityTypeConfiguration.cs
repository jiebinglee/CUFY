using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ChinaUnicom.Fuyang.Core.Utility.Models;

namespace ChinaUnicom.Fuyang.Core.Utility.Mappings
{
    class BaseFormEntityTypeConfiguration : EntityTypeConfiguration<BaseForm>
    {
        public BaseFormEntityTypeConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FormNo)
                .IsRequired()
                .HasMaxLength(40);

            //this.Ignore(t => t.TableName);

            // Table & Column Mappings
            this.ToTable("CWF_OWNER.WF_BSNS_FRM");
            this.Property(t => t.Id).HasColumnName("WF_BSNS_FRM_ID");
            this.Property(t => t.FormTypeId).HasColumnName("FRM_TYP_ID");
            this.Property(t => t.FormNo).HasColumnName("FRM_NBR");
            this.Property(t => t.RequestorId).HasColumnName("RQSTR_ID");
            this.Property(t => t.RequestDate).HasColumnName("RQSTR_DT");
            this.Property(t => t.RequestStatus).HasColumnName("RQSTR_ST");
            this.Property(t => t.PrevApproval).HasColumnName("PST_APRVL");
            this.Property(t => t.NextApproval).HasColumnName("NXT_APRVL");
            this.Property(t => t.CompleteDate).HasColumnName("CMPLT_DT");

            // Relationships
            this.HasRequired(t => t.FormType)
                .WithMany()
                .HasForeignKey(d => d.FormTypeId);

            this.HasRequired(t => t.User)
                .WithMany()
                .HasForeignKey(d => d.RequestorId);            
        }
    }
}
