using ChinaUnicom.Fuyang.Core.Users.Models;
using ChinaUnicom.Fuyang.CreditManagement.Models;
using ChinaUnicom.Fuyang.CreditManagement.ViewModels;
using ChinaUnicom.Fuyang.Framework;
using ChinaUnicom.Fuyang.Framework.Data;
using System;
using System.Collections.Generic;

namespace ChinaUnicom.Fuyang.CreditManagement.Services
{
    public interface ICMService : IDependency
    {
        Channel GetChannel(int userId);
        Channel GetChannel(Guid channelGuid);
        Channel InsertChannel(Channel channel);

        Import InsertImport(Import import);

        CreditTotal InsertCreditTotal(CreditTotal creditTotal);
        int UpdateCreditTotal(List<CreditTotal> creditTotals);

        List<Development> InsertDevelopment(List<Development> developments);
        List<Contract> InsertContract(List<Contract> contracts);

        List<ChannelDictionary> GetChannelDictionary();
        List<CreditConfig> GetCreditConfig();

        List<Channel> GetChannelList();
        Pageable<ChannelInfoDto> GetChannelList(int userId, int pageNumber, int pageSize);

        CreditExchange InsertCreditExchange(CreditExchange creditExchange);

        Pageable<ChannelCreditExchangeInfoDto> GetCreditExchangeApprovalList(int pageNumber, int pageSize);

        CreditExchange GetCreditExchange(int exchangeId);

        int UpdateCreditExchange(CreditExchange creditExchange);

        CreditTotal GetCreditTotal(Guid channelGuid);

        int UpdateCreditTotal(CreditTotal creditTotal);

        Pageable<AreaUserInfoDto> GetAreaUser(int pageNumber, int pageSize);
    }
}
