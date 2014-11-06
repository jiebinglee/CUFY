using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChinaUnicom.Fuyang.CreditManagement.Models;
using ChinaUnicom.Fuyang.CreditManagement.ViewModels;

namespace ChinaUnicom.Fuyang.CreditManagement.AutoMapperProfiles
{
    class ChannelProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Channel, ChannelInfoDto>()
                .ForMember(dto => dto.ChannelId, mc => mc.MapFrom(e => e.Id))
                //.ForMember(dto => dto.ChannelLevelDesc, mc => mc.ResolveUsing<ChannelLevelResolver>().FromMember(x => x.ChannelLevel))
                .ForMember(dto => dto.ChannelContractCredit, mc => mc.MapFrom(e => e.CreditTotal.ContractCredit))
                .ForMember(dto => dto.ChannelDevelopmentCredit, mc => mc.MapFrom(e => e.CreditTotal.DevCredit))
                .ForMember(dto => dto.ChannelYearBonus, mc => mc.MapFrom(e => e.CreditTotal.YearBonus))
                .ForMember(dto => dto.ChannelTotalAmount, mc => mc.MapFrom(e => e.CreditTotal.TotalAmount))
                .ForMember(dto => dto.ChannelExchangedCredit, mc => mc.MapFrom(e => e.CreditTotal.ExchangedCredit))
                .ForMember(dto => dto.ChannelRemainingTotalAmount, mc => mc.MapFrom(e => e.CreditTotal.RemainingTotalAmount))
                .ForMember(dto => dto.ChannelExchangeableCredit, mc => mc.MapFrom(e => e.CreditTotal.RemainingTotalAmount - GetExangeableCredit(e.CreditExchanges)));
                
        }

        private int GetExangeableCredit(ICollection<CreditExchange> creditExchanges)
        {
            var result = 0;
            var temp = creditExchanges.ToList();

            foreach (var t in temp)
            {
                if (t.Flag == 1 && t.Status == 0)
                {
                    result += t.ExchangeCredit;
                }
            }

            return result;
        }
    }
}
