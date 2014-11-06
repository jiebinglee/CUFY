using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.CreditManagement.ViewModels
{
    public class DevelopmentInfoDto
    {
        public int DevType { get; set; }
        public string DevTypeDesc { get; set; }
        public int DevCount { get; set; }
        public int CreditAmount { get; set; }
        public decimal CreditBase { get; set; }
        public decimal CreditRatio { get; set; }
    }
}
