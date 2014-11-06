using ChinaUnicom.Fuyang.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.CreditManagement.Models
{
    public class UserArea : Entity
    {
        public UserArea()
        { 
        }

        public int UserId { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
    }
}
