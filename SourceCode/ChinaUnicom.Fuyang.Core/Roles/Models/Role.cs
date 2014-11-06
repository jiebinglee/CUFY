using ChinaUnicom.Fuyang.Framework.Data;
using System;

namespace ChinaUnicom.Fuyang.Core.Roles.Models
{
    public class Role : Entity
    {
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }
        public string MDL_DCTNRY { get; set; }
        public Nullable<short> IsAdmin { get; set; }
    }
}
