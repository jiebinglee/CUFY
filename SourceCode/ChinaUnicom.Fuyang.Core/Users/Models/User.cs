using ChinaUnicom.Fuyang.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaUnicom.Fuyang.Core.Users.Models
{
    public class User : Entity
    {
        public User() {

        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int UserType { get; set; }
    }
}
