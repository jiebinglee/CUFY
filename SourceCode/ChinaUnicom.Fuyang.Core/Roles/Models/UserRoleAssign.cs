using ChinaUnicom.Fuyang.Framework.Data;
using System;

namespace ChinaUnicom.Fuyang.Core.Roles.Models
{
    public class UserRoleAssign : Entity
    {
        public Nullable<decimal> RoleId { get; set; }
        public Nullable<decimal> UserId { get; set; }
        public Nullable<short> IS_DT_RL { get; set; }
    }
}
