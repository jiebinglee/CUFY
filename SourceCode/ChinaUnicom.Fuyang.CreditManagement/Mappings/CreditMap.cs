using ChinaUnicom.Fuyang.CreditManagement.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ChinaUnicom.Fuyang.CreditManagement.Mappings
{
    public class CreditMap : EntityTypeConfiguration<Credit>
    {
        public CreditMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("CM_CREDIT");
            this.Property(t => t.Id).HasColumnName("CREDIT_ID");
            this.Property(t => t.ChannelId).HasColumnName("CHANNEL_ID");
            this.Property(t => t.CreditYear).HasColumnName("CREDIT_YEAR");
            this.Property(t => t.CreditMonth).HasColumnName("CREDIT_MONTH");
            this.Property(t => t.CreditAmount).HasColumnName("CREDIT_AMOUNT");
            this.Property(t => t.Flag).HasColumnName("FLAG");
        }
    }
}
