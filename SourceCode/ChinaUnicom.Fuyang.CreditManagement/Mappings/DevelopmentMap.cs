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
    public class DevelopmentMap : EntityTypeConfiguration<Development>
    {
        public DevelopmentMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("CM_DEVELOPMENT");
            this.Property(t => t.Id).HasColumnName("DEV_ID");
            this.Property(t => t.ChannelGUID).HasColumnName("CHANNEL_GUID");
            this.Property(t => t.DevType).HasColumnName("DEV_TYPE");
            this.Property(t => t.DevCount).HasColumnName("DEV_COUNT");
            this.Property(t => t.DevYear).HasColumnName("DEV_YEAR");
            this.Property(t => t.DevMonth).HasColumnName("DEV_MONTH");
            this.Property(t => t.DevCount2).HasColumnName("DEV_COUNT_2");
            this.Property(t => t.CreditAmount).HasColumnName("CREDIT_AMOUNT");
            this.Property(t => t.Flag).HasColumnName("FLAG");
        }
    }
}
