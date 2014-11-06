using ChinaUnicom.Fuyang.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.CreditManagement.Models
{
    public class CreditTotal : Entity
    {
        public Guid ChannelGUID { get; set; }
        public int TotalAmount { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int DevCredit { get; set; }
        public int ContractCredit { get; set; }
        public int YearBonus { get; set; }

        public int ExchangedCredit { get; set; }
        public int RemainingTotalAmount { get; set; }
    }
}
