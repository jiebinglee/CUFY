using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChinaUnicom.Fuyang.Framework.Data;

namespace ChinaUnicom.Fuyang.CreditManagement.Models
{
    public class Credit :Entity
    {
        public int ChannelId { get; set; }
        public int CreditYear { get; set; }
        public int CreditMonth { get; set; }
        public int CreditAmount { get; set; }
        //public int Flag { get; set; }
    }
}
