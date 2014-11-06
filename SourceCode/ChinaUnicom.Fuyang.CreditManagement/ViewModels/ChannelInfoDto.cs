using AutoMapper;
using ChinaUnicom.Fuyang.CreditManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.CreditManagement.ViewModels
{
    public class ChannelInfoDto
    {        
        public int ChannelId { get; set; }
        public Guid ChannelGUID { get; set; }
        public string ChannelCode { get; set; }
        public string ChannelName { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public int ChannelLevel { get; set; }
        public string ChannelLevelDesc { get; set; }
        public string Desc { get; set; }
        public int ChannelContractCredit { get; set; }
        public int ChannelDevelopmentCredit { get; set; }
        public int ChannelYearBonus { get; set; }
        public int ChannelTotalAmount { get; set; }

        public int ChannelExchangedCredit { get; set; }
        public int ChannelRemainingTotalAmount { get; set; }
        public int ChannelExchangeableCredit { get; set; }
    }

    public class ChannelLevelResolver : ValueResolver<int, string>
    {
        protected override string ResolveCore(int source)
        {
            string str = "";
            switch (source)
            {
                case 1:
                    str = "合作厅";
                    break;
                case 2:
                    str = "一级专营店";
                    break;
                case 3:
                    str = "二级级专营店";
                    break;
                case 4:
                    str = "社会渠道";
                    break;
            }

            return str;
        }

    }
}
