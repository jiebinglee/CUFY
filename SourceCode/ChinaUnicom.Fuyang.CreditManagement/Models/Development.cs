using System;
using ChinaUnicom.Fuyang.Framework.Data;

namespace ChinaUnicom.Fuyang.CreditManagement.Models
{
    public class Development : Entity
    {
        public Guid ChannelGUID { get; set; }
        public int DevType { get; set; }
        public int DevCount { get; set; }
        public int DevYear { get; set; }
        public int DevMonth { get; set; }
        public int DevCount2 { get; set; }
        public int CreditAmount { get; set; }
    }
}
