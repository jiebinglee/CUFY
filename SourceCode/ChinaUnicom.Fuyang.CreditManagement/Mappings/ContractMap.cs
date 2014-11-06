using ChinaUnicom.Fuyang.CreditManagement.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ChinaUnicom.Fuyang.CreditManagement.Mappings
{
    public class ContractMap : EntityTypeConfiguration<Contract>
    {
        public ContractMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("CM_CONTRACT");
            this.Property(t => t.Id).HasColumnName("CONTRACT_ID");
            this.Property(t => t.ChannelGUID).HasColumnName("CHANNEL_GUID");
            this.Property(t => t.ContractCount).HasColumnName("CONTRACT_COUNT");
            this.Property(t => t.ContractYear).HasColumnName("CONTRACT_YEAR");
            this.Property(t => t.ContractMonth).HasColumnName("CONTRACT_MONTH");
            this.Property(t => t.ContractCount2).HasColumnName("CONTRACT_COUNT_2");
            this.Property(t => t.CreditAmount).HasColumnName("CREDIT_AMOUNT");
            this.Property(t => t.Flag).HasColumnName("FLAG");
        }
    }
}
