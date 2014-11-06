using ChinaUnicom.Fuyang.CreditManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.CreditManagement.Mappings
{
    public class CreditExchangeMap : EntityTypeConfiguration<CreditExchange>
    {
        public CreditExchangeMap()
        {
            this.HasKey(t => t.Id);            

            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("CM_CREDIT_EXCHANGE");
            this.Property(t => t.Id).HasColumnName("EXCHANGE_ID");
            this.Property(t => t.ChannelGUID).HasColumnName("CHANNEL_GUID");
            this.Property(t => t.ExchangeCredit).HasColumnName("EXCHANGE_CREDIT");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Flag).HasColumnName("FLAG");
            this.Property(t => t.ExchangeDateTime).HasColumnName("EXCHANGE_DATETIME");
            this.Property(t => t.ExchangeUser).HasColumnName("EXCHANGE_USER");
            this.Property(t => t.ApprovalDateTime).HasColumnName("APPROVAL_DATETIME");
            this.Property(t => t.ApprovalUser).HasColumnName("APPROVAL_USER");
        }
    }
}
