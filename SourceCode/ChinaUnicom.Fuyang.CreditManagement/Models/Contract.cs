using ChinaUnicom.Fuyang.Framework.Data;
using System;

namespace ChinaUnicom.Fuyang.CreditManagement.Models
{
    public class Contract : Entity
    {
        public Guid ChannelGUID { get; set; }
        public int ContractCount { get; set; }
        public int ContractYear { get; set; }
        public int ContractMonth { get; set; }
        public int ContractCount2 { get; set; }
        public int CreditAmount { get; set; }
    }
}
