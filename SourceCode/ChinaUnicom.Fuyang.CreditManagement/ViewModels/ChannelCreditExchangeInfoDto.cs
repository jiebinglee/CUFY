using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.CreditManagement.ViewModels
{
    public class ChannelCreditExchangeInfoDto : ChannelInfoDto
    {
        public int ExchangeId { get; set; }
        public int ExchangeCredit { get; set; }
        public int Status { get; set; }
        public DateTime ExchangeDateTime { get; set; }
        public int ExchangeUser { get; set; }
        public DateTime? ApprovalDateTime { get; set; }
        public int? ApprovalUser { get; set; }
    }
}
