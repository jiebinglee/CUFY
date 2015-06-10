using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.CreditManagement.ViewModels
{
    public class ChannelCreditDetail
    {
        public int ChannelId { get; set; }
        public Guid ChannelGUID { get; set; }
        public string ChannelCode { get; set; }
        public string ChannelName { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public int ChannelLevel { get; set; }
        public string ChannelLevelDesc { get; set; }
        public int CreditYear { get; set; }
        public int CreditMonth { get; set; }
        public int Dev1Credit { get; set; }
        public int Dev2Credit { get; set; }
        public int Dev3Credit { get; set; }
        public int Dev4Credit { get; set; }
        public int ContractCredit { get; set; }
            
    }

    public class ChannelDevelopmentCreditDetail
    {
        public Guid ChannelGUID { get; set; }
        public int CreditYear { get; set; }
        public int CreditMonth { get; set; }
        public int? Dev1Credit { get; set; }
        public int? Dev2Credit { get; set; }
        public int? Dev3Credit { get; set; }
        public int? Dev4Credit { get; set; }
    }

    public class ChannelContractCreditDetail
    {
        public Guid ChannelGUID { get; set; }
        public int CreditYear { get; set; }
        public int CreditMonth { get; set; }
        public int ContractCredit { get; set; }
    }
}
