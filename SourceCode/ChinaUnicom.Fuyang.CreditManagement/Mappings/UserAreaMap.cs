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
    public class UserAreaMap : EntityTypeConfiguration<UserArea>
    {
        public UserAreaMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("CM_USER_AREA");
            this.Property(t => t.Id).HasColumnName("USER_AREA_ID");
            this.Property(t => t.UserId).HasColumnName("USER_ID");
            this.Property(t => t.AreaCode).HasColumnName("AREA_CODE");
            this.Property(t => t.Flag).HasColumnName("FLAG");
            this.Property(t => t.AreaName).HasColumnName("AREA_NAME");
        }
    }
}
