using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.CreditManagement.ViewModels
{
    public class AreaUserInfoDto
    {
        public int UserAreaId { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
