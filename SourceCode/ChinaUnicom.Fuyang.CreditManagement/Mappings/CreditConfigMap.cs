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
    public class CreditConfigMap : EntityTypeConfiguration<CreditConfig>
    {
        public CreditConfigMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("CM_CREDIT_CONFIG");
            this.Property(t => t.Id).HasColumnName("CONFIG_ID");
            this.Property(t => t.ChannelLevelId).HasColumnName("CHANNEL_LEVEL_ID");
            this.Property(t => t.DevTypeId).HasColumnName("DEV_TYPE_ID");
            this.Property(t => t.CreditBase).HasColumnName("CREDIT_BASE");
            this.Property(t => t.CreditRatio).HasColumnName("CREDIT_RATIO");
            this.Property(t => t.Flag).HasColumnName("FLAG");
        }
    }
}
