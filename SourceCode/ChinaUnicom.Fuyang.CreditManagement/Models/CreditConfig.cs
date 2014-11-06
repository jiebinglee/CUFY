using ChinaUnicom.Fuyang.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.CreditManagement.Models
{
    public class CreditConfig : Entity
    {
        public CreditConfig()
        { 
        }

        public int ChannelLevelId { get; set; }
        public int DevTypeId { get; set; }
        public decimal CreditBase { get; set; }
        public decimal CreditRatio { get; set; }
    }
}
