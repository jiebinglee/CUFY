using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChinaUnicom.Fuyang.Framework.Data;

namespace ChinaUnicom.Fuyang.CreditManagement.Models
{
    public class Channel : Entity
    {
        public Channel()
        { 
        }

        public Guid ChannelGUID { get; set; }
        public string ChannelCode { get; set; }
        public string ChannelName { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public int ChannelLevel { get; set; }
        public string Desc { get; set; }
        public int ChannelStatus { get; set; }
        public int UserId { get; set; }
        public int JoinYear { get; set; }
        public int JoinMonth { get; set; }
        public int BuildYear { get; set; }
        public int BuildMonth { get; set; }

        public virtual CreditTotal CreditTotal { get; set; }

        private List<Development> _developments;
        public virtual ICollection<Development> Developments
        {
            get
            {
                if (_developments == null)
                    _developments = new List<Development>();
                return _developments;
            }
            set
            {
                _developments = new List<Development>(value);
            }
        }

        private List<Contract> _contracts;
        public virtual ICollection<Contract> Contracts
        {
            get
            {
                if (_contracts == null)
                    _contracts = new List<Contract>();
                return _contracts;
            }
            set
            {
                _contracts = new List<Contract>(value);
            }
        }

        private List<CreditExchange> _creditExchanges;
        public virtual ICollection<CreditExchange> CreditExchanges
        {
            get
            {
                if (_creditExchanges == null)
                    _creditExchanges = new List<CreditExchange>();
                return _creditExchanges;
            }
            set
            {
                _creditExchanges = new List<CreditExchange>(value);
            }
        }
    }
}
