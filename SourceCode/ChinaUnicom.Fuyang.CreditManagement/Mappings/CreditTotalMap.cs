using ChinaUnicom.Fuyang.CreditManagement.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ChinaUnicom.Fuyang.CreditManagement.Mappings
{
    public class CreditTotalMap : EntityTypeConfiguration<CreditTotal>
    {
        public CreditTotalMap()
        {
            this.HasKey(t => t.ChannelGUID);            

            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.ToTable("CM_CREDIT_TOTAL");
            this.Property(t => t.Id).HasColumnName("CHANNEL_ID");
            this.Property(t => t.ChannelGUID).HasColumnName("CHANNEL_GUID");
            this.Property(t => t.TotalAmount).HasColumnName("CREDIT_TOTAL_AMOUNT");
            this.Property(t => t.LastUpdateDate).HasColumnName("LAST_UPDATE_DATE");
            this.Property(t => t.Flag).HasColumnName("FLAG");
            this.Property(t => t.DevCredit).HasColumnName("DEV_CREDIT");
            this.Property(t => t.ContractCredit).HasColumnName("CONTRACT_CREDIT");
            this.Property(t => t.YearBonus).HasColumnName("YEAR_BONUS");

            this.Property(t => t.ExchangedCredit).HasColumnName("EXCHANGED_CREDIT");
            this.Property(t => t.RemainingTotalAmount).HasColumnName("REMAINING_TOTAL_AMOUNT");

        }
    }
}
