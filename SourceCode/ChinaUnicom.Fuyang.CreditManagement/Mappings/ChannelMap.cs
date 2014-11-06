using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChinaUnicom.Fuyang.CreditManagement.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChinaUnicom.Fuyang.CreditManagement.Mappings
{
    public class ChannelMap : EntityTypeConfiguration<Channel>
    {
        public ChannelMap()
        {
            this.HasKey(t => t.ChannelGUID);

            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("CM_CHANNEL");
            this.Property(t => t.Id).HasColumnName("CHANNEL_ID");
            this.Property(t => t.ChannelGUID).HasColumnName("CHANNEL_GUID");
            this.Property(t => t.ChannelCode).HasColumnName("CHANNEL_CODE");
            this.Property(t => t.ChannelName).HasColumnName("CHANNEL_NAME");
            this.Property(t => t.AreaCode).HasColumnName("AREA_CODE");
            this.Property(t => t.AreaName).HasColumnName("AREA_NAME");
            this.Property(t => t.ChannelLevel).HasColumnName("CHANNEL_LEVEL");
            this.Property(t => t.Desc).HasColumnName("CHANNEL_DESC");
            this.Property(t => t.ChannelStatus).HasColumnName("CHANNEL_STATUS");
            this.Property(t => t.Flag).HasColumnName("FLAG");
            this.Property(t => t.UserId).HasColumnName("USER_ID");
            this.Property(t => t.JoinYear).HasColumnName("JOIN_YEAR");
            this.Property(t => t.JoinMonth).HasColumnName("JOIN_MONTH");
            this.Property(t => t.BuildYear).HasColumnName("BUILD_YEAR");
            this.Property(t => t.BuildMonth).HasColumnName("BUILD_MONTH");

            this.HasRequired(t => t.CreditTotal)
                .WithOptional();

            this.HasMany(t => t.Developments)
                .WithRequired()
                .HasForeignKey(d => d.ChannelGUID);

            this.HasMany(t => t.Contracts)
                .WithRequired()
                .HasForeignKey(d => d.ChannelGUID);

            this.HasMany(t => t.CreditExchanges)
                .WithRequired()
                .HasForeignKey(d => d.ChannelGUID);
        }
    }
}
