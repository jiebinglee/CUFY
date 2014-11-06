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
    public class ChannelDictionaryMap : EntityTypeConfiguration<ChannelDictionary>
    {
        public ChannelDictionaryMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("CM_DICTIONARY");
            this.Property(t => t.Id).HasColumnName("DICTIONARY_ID");
            this.Property(t => t.DictionaryTable).HasColumnName("DICTIONARY_TABLE");
            this.Property(t => t.DictionaryKey).HasColumnName("DICTIONARY_KEY");
            this.Property(t => t.DictionaryValue).HasColumnName("DICTIONARY_VALUE");
            this.Property(t => t.Flag).HasColumnName("FLAG");
        }
    }
}
