using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ChinaUnicom.Fuyang.Core.Users.Models;

namespace ChinaUnicom.Fuyang.Core.Users.Mappings
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
            // Table & Column Mappings
            this.ToTable("CM_USER");
            this.Property(t => t.Id).HasColumnName("USER_ID");
            this.Property(t => t.UserName).HasColumnName("USER_NAME");
            this.Property(t => t.Password).HasColumnName("USER_PASSWORD");
            this.Property(t => t.Phone).HasColumnName("USER_PHONE");
            this.Property(t => t.Email).HasColumnName("USER_EMAIL");
            this.Property(t => t.CreateDateTime).HasColumnName("CREATE_DATETIME");
            this.Property(t => t.Flag).HasColumnName("FLAG");
            this.Property(t => t.UserType).HasColumnName("USER_TYPE");
        }
    }
}
