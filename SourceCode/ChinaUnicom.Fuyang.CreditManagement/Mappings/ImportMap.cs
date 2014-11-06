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
    public class ImportMap : EntityTypeConfiguration<Import>
    {
        public ImportMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("CM_CHANNEL_IMPORT");
            this.Property(t => t.Id).HasColumnName("IMPORT_ID");
            this.Property(t => t.ImportYear).HasColumnName("IMPORT_YEAR");
            this.Property(t => t.ImportMonth).HasColumnName("IMPORT_MONTH");
            this.Property(t => t.ImportDate).HasColumnName("IMPORT_DATE");
            this.Property(t => t.OperatorId).HasColumnName("OPERATOR_ID");
            this.Property(t => t.ImportContent).HasColumnName("IMPORT_CONTENT");            
            this.Property(t => t.Flag).HasColumnName("FLAG");
            this.Property(t => t.DataType).HasColumnName("DATA_TYPE");
        }
    }
}
