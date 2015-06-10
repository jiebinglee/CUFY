using System;
using ChinaUnicom.Fuyang.Framework.Data;
using ChinaUnicom.Fuyang.CreditManagement.Models;
using ChinaUnicom.Fuyang.Core.Users.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ChinaUnicom.Fuyang.CreditManagement.ViewModels;
using ChinaUnicom.Fuyang.Framework.Adapter;
using System.Reflection;

namespace ChinaUnicom.Fuyang.CreditManagement.Services
{
    public class CMService : ICMService
    {
        readonly IRepository<Channel> _channelRepository;
        readonly IRepository<Development> _developmentRepository;
        readonly IRepository<Import> _importRepository;
        readonly IRepository<CreditTotal> _creditTotalRepository;
        readonly IRepository<User> _userRepository;
        readonly IRepository<ChannelDictionary> _channelDictionaryRepository;
        readonly IRepository<CreditConfig> _creditConfigRepository;
        readonly IRepository<UserArea> _userAreaRepository;
        readonly IRepository<Contract> _contractRepository;
        readonly IRepository<CreditExchange> _creditExchangeRepository;

        public CMService(IRepository<Channel> channelRepository,
            IRepository<Development> developmentRepository,
            IRepository<Import> importRepository,
            IRepository<CreditTotal> creditTotalRepository,
            IRepository<User> userRepository,
            IRepository<ChannelDictionary> channelDictionaryRepository,
            IRepository<CreditConfig> creditConfigRepository,
            IRepository<UserArea> userAreaRepository,
            IRepository<Contract> contractRepository,
            IRepository<CreditExchange> creditExchangeRepository
            )
        {
            _channelRepository = channelRepository;
            _developmentRepository = developmentRepository;
            _importRepository = importRepository;
            _creditTotalRepository = creditTotalRepository;
            _userRepository = userRepository;
            _channelDictionaryRepository = channelDictionaryRepository;
            _creditConfigRepository = creditConfigRepository;
            _userAreaRepository = userAreaRepository;
            _contractRepository = contractRepository;
            _creditExchangeRepository = creditExchangeRepository;
        }

        public Channel GetChannel(int userId)
        {
            Channel model;

            model = _channelRepository.Get(t => t.UserId == userId && t.ChannelStatus == 1 && t.Flag == 1);

            return model;
        }

        public Channel GetChannel(Guid channelGuid)
        {
            return _channelRepository.Get(t => t.ChannelGUID == channelGuid && t.ChannelStatus == 1 && t.Flag == 1);
        }

        public Channel InsertChannel(Channel channel)
        {
            return _channelRepository.Create(channel);
        }

        public Import InsertImport(Import import)
        {
            return _importRepository.Create(import);
        }

        public CreditTotal InsertCreditTotal(CreditTotal creditTotal)
        {
            return _creditTotalRepository.Create(creditTotal);
        }

        public int UpdateCreditTotal(List<CreditTotal> creditTotals)
        {
            return _creditTotalRepository.Update(creditTotals);
        }

        public List<Development> InsertDevelopment(List<Development> developments)
        {
            return _developmentRepository.Create(developments).ToList();
        }

        public List<Contract> InsertContract(List<Contract> contracts)
        {
            return _contractRepository.Create(contracts).ToList();
        }

        public List<ChannelDictionary> GetChannelDictionary()
        {
            return _channelDictionaryRepository.Fetch(t => t.Flag == 1).ToList();
        }

        public List<CreditConfig> GetCreditConfig()
        {
            return _creditConfigRepository.Fetch(t => t.Flag == 1).ToList();
        }

        public List<Channel> GetChannelList()
        {
            var channel = _channelRepository.Table;

            return channel.Where(t => t.ChannelStatus == 1 && t.Flag == 1).ToList();
        }

        public Pageable<ChannelInfoDto> GetChannelList(int userId, int pageNumber, int pageSize)
        {
            var channel = _channelRepository.Table;
            var userArea = _userAreaRepository.Table;
            var creditTotal = _creditTotalRepository.Table;
            var channelDictionary = _channelDictionaryRepository.Table;
            var creditExchange = _creditExchangeRepository.Table;

            var exchangeGoury = from ce in creditExchange
                    where ce.Status == 0 && ce.Flag == 1
                    group ce by ce.ChannelGUID into n
                    select new
                    {
                        n.Key,
                        ExchangeableCreditAmount = n.Sum(t => t.ExchangeCredit),
                    };

            if (userId != 0)
            {
                var query = from c in channel
                            join ua in
                                (from u in userArea where u.UserId == userId && u.Flag == 1 select u) on c.AreaCode equals ua.AreaCode
                            join ct in creditTotal on c.ChannelGUID equals ct.ChannelGUID
                            join cf in
                                (from cd in channelDictionary where cd.DictionaryTable == "CHANNEL_LEVEL" && cd.Flag == 1 select cd) on c.ChannelLevel equals cf.DictionaryKey
                            join mm in exchangeGoury on c.ChannelGUID equals mm.Key into mmmm
                            from nnnn in mmmm.DefaultIfEmpty()
                            select new ChannelInfoDto
                            {
                                ChannelId = c.Id,
                                ChannelGUID = c.ChannelGUID,
                                ChannelCode = c.ChannelCode,
                                ChannelName = c.ChannelName,
                                AreaCode = c.AreaCode,
                                AreaName = c.AreaName,
                                ChannelLevel = c.ChannelLevel,
                                ChannelLevelDesc = cf.DictionaryValue,
                                Desc = c.Desc,
                                ChannelContractCredit = ct.ContractCredit,
                                ChannelDevelopmentCredit = ct.DevCredit,
                                ChannelYearBonus = ct.YearBonus,
                                ChannelTotalAmount = ct.TotalAmount,

                                ChannelExchangedCredit = ct.ExchangedCredit,
                                ChannelRemainingTotalAmount = ct.RemainingTotalAmount,
                                ChannelExchangeableCredit = ct.RemainingTotalAmount - (nnnn == null ? 0 : nnnn.ExchangeableCreditAmount)
                            };

                return new Pageable<ChannelInfoDto>(query, t => t.Asc(m => m.ChannelId).Asc(n => n.AreaCode), pageNumber, pageSize);
            }
            else
            {
                var query = from c in channel
                            join ct in creditTotal on c.ChannelGUID equals ct.ChannelGUID
                            join cf in
                                (from cd in channelDictionary where cd.DictionaryTable == "CHANNEL_LEVEL" && cd.Flag == 1 select cd) on c.ChannelLevel equals cf.DictionaryKey
                            join mm in exchangeGoury on c.ChannelGUID equals mm.Key into mmmm
                            from nnnn in mmmm.DefaultIfEmpty()
                            select new ChannelInfoDto
                            {
                                ChannelId = c.Id,
                                ChannelGUID = c.ChannelGUID,
                                ChannelCode = c.ChannelCode,
                                ChannelName = c.ChannelName,
                                AreaCode = c.AreaCode,
                                AreaName = c.AreaName,
                                ChannelLevel = c.ChannelLevel,
                                ChannelLevelDesc = cf.DictionaryValue,
                                Desc = c.Desc,
                                ChannelContractCredit = ct.ContractCredit,
                                ChannelDevelopmentCredit = ct.DevCredit,
                                ChannelYearBonus = ct.YearBonus,
                                ChannelTotalAmount = ct.TotalAmount,

                                ChannelExchangedCredit = ct.ExchangedCredit,
                                ChannelRemainingTotalAmount = ct.RemainingTotalAmount,
                                ChannelExchangeableCredit = ct.RemainingTotalAmount - (nnnn == null ? 0 : nnnn.ExchangeableCreditAmount)
                            };

                return new Pageable<ChannelInfoDto>(query, t => t.Asc(m => m.ChannelId).Asc(n => n.AreaCode), pageNumber, pageSize);
            }
        }

        public CreditExchange InsertCreditExchange(CreditExchange creditExchange)
        {
            return _creditExchangeRepository.Create(creditExchange);
        }

        public Pageable<ChannelCreditExchangeInfoDto> GetCreditExchangeApprovalList(int pageNumber, int pageSize)
        {
            var channel = _channelRepository.Table;
            var creditTotal = _creditTotalRepository.Table;
            var channelDictionary = _channelDictionaryRepository.Table;
            var creditExchange = _creditExchangeRepository.Table;

            var query = from c in channel
                        join ct in creditTotal on c.ChannelGUID equals ct.ChannelGUID
                        join cf in
                            (from cd in channelDictionary where cd.DictionaryTable == "CHANNEL_LEVEL" && cd.Flag == 1 select cd) on c.ChannelLevel equals cf.DictionaryKey
                        join ce in creditExchange on c.ChannelGUID equals ce.ChannelGUID
                        select new ChannelCreditExchangeInfoDto
                        {
                            ChannelId = c.Id,
                            ChannelGUID = c.ChannelGUID,
                            ChannelCode = c.ChannelCode,
                            ChannelName = c.ChannelName,
                            AreaCode = c.AreaCode,
                            AreaName = c.AreaName,
                            ChannelLevel = c.ChannelLevel,
                            ChannelLevelDesc = cf.DictionaryValue,
                            Desc = c.Desc,
                            ChannelContractCredit = ct.ContractCredit,
                            ChannelDevelopmentCredit = ct.DevCredit,
                            ChannelYearBonus = ct.YearBonus,
                            ChannelTotalAmount = ct.TotalAmount,

                            ChannelExchangedCredit = ct.ExchangedCredit,
                            ChannelRemainingTotalAmount = ct.RemainingTotalAmount,

                            ExchangeId = ce.Id,
                            ExchangeCredit = ce.ExchangeCredit,
                            Status = ce.Status,
                            ExchangeDateTime = ce.ExchangeDateTime,
                            ExchangeUser = ce.ExchangeUser,
                            ApprovalDateTime = ce.ApprovalDateTime,
                            ApprovalUser = ce.ApprovalUser
                        };

            return new Pageable<ChannelCreditExchangeInfoDto>(query, t => t.Desc(m => m.ExchangeDateTime), pageNumber, pageSize);
        }

        public CreditExchange GetCreditExchange(int exchangeId)
        {
            return _creditExchangeRepository.Get(exchangeId);
        }

        public int UpdateCreditExchange(CreditExchange creditExchange)
        {
            return _creditExchangeRepository.Update(creditExchange);
        }

        public CreditTotal GetCreditTotal(Guid channelGuid)
        {
            return _creditTotalRepository.Get(t => t.ChannelGUID == channelGuid);
        }

        public int UpdateCreditTotal(CreditTotal creditTotal)
        {
            return _creditTotalRepository.Update(creditTotal);
        }

        public Pageable<AreaUserInfoDto> GetAreaUser(int pageNumber, int pageSize)
        {
            var user = _userRepository.Table;
            var userArea = _userAreaRepository.Table;

            var query = from a in userArea
                        join b in user on a.UserId equals b.Id
                        where a.Flag == 1 && b.Flag == 1
                        select new AreaUserInfoDto
                        {
                            UserAreaId = a.Id,
                            AreaCode = a.AreaCode,
                            AreaName = a.AreaName,
                            UserId = b.Id,
                            UserName = b.UserName,
                            Password = b.Password
                        };

            return new Pageable<AreaUserInfoDto>(query, t => t.Asc(m => m.AreaCode), pageNumber, pageSize);
        }

        public List<ChannelCreditDetail> GetChannelCreditDetail(Guid channelGuid)
        {
            List<ChannelCreditDetail> creditDetailList = new List<ChannelCreditDetail>();
            
            string sqlDev = @"select  a.channel_guid as ChannelGUID,
                                    a.dev_year as CreditYear,
                                    a.dev_month as CreditMonth,
                                    max(case a.dev_type when 1 then a.credit_amount else 0 end) as Dev1Credit,
                                    max(case a.dev_type when 2 then a.credit_amount else 0 end) as Dev2Credit,
                                    max(case a.dev_type when 3 then a.credit_amount else 0 end) as Dev3Credit,
                                    max(case a.dev_type when 4 then a.credit_amount else 0 end) as Dev4Credit
                                    from CM_DEVELOPMENT a 
                                    where a.channel_guid = '{0}'
                                    group by a.channel_guid,a.dev_year,a.dev_month
                                    order by a.dev_year desc,a.dev_month desc";
            sqlDev = string.Format(sqlDev, channelGuid.ToString());

            string sqlContract = @"select   a.channel_guid as ChannelGUID,
                                            a.contract_year as CreditYear,
                                            a.contract_month as CreditMonth,
                                            a.credit_amount as ContractCredit
                                            from CM_CONTRACT a where a.channel_guid = '{0}'
                                            order by a.contract_year desc,a.contract_month desc";
            sqlContract = string.Format(sqlContract, channelGuid.ToString());

            //1.获取积分数据
            var devCredit = _developmentRepository.Fetch(t => t.ChannelGUID == channelGuid);
            var devQuery = from a in devCredit
                           group a by new
                           {
                               a.ChannelGUID,
                               a.DevYear,
                               a.DevMonth
                           }                           
                               into b
                               select new ChannelDevelopmentCreditDetail
                               {
                                   ChannelGUID = b.Key.ChannelGUID,
                                   CreditYear = b.Key.DevYear,
                                   CreditMonth = b.Key.DevMonth,
                                   Dev1Credit = (from c in b where c.DevType == 1 select c.CreditAmount).Max(),
                                   Dev2Credit = (from c in b where c.DevType == 2 select c.CreditAmount).Max(),
                                   Dev3Credit = (from c in b where c.DevType == 3 select c.CreditAmount).Max(),
                                   Dev4Credit = (from c in b where c.DevType == 4 select c.CreditAmount).Max()
                               };
            var dev = devQuery.OrderByDescending(t => t.CreditYear).ThenByDescending(t => t.CreditMonth).ToList();

            var contractCredit = _contractRepository.Fetch(t => t.ChannelGUID == channelGuid);
            var contractQuery = from a in contractCredit
                                select new ChannelContractCreditDetail
                                {
                                    ChannelGUID = a.ChannelGUID,
                                    CreditYear = a.ContractYear,
                                    CreditMonth = a.ContractMonth,
                                    ContractCredit = a.CreditAmount
                                };
            var contract = contractQuery.OrderByDescending(t => t.CreditYear).ThenByDescending(t => t.CreditMonth).ToList();

            //2.计算最大年月和最小年月
            var maxYear = 0;
            var maxMonth = 0;
            var minYear = 0;
            var minMonth = 0;

            var devMaxYear = dev[0].CreditYear;
            var devMaxMonth = dev[0].CreditMonth;
            var devMinYear = dev[dev.Count - 1].CreditYear;
            var devMinMonth = dev[dev.Count - 1].CreditMonth;
            var contractMaxYear = contract[0].CreditYear;
            var contractMaxMonth = contract[0].CreditMonth;
            var contractMinYear = contract[contract.Count - 1].CreditYear;
            var contractMinMonth = contract[contract.Count - 1].CreditMonth;

            if (devMaxYear == contractMaxYear)
            {
                maxYear = devMaxYear;

                if (devMaxMonth > contractMaxMonth)
                {
                    maxMonth = devMaxMonth;
                }
                else
                {
                    maxMonth = contractMaxMonth;
                }
            }
            else if (devMaxYear > contractMaxYear)
            {
                maxYear = devMaxYear;
                maxMonth = devMaxMonth;
            }
            else
            {
                maxYear = contractMaxYear;
                maxMonth = contractMaxMonth;
            }

            if (devMinYear == contractMinYear)
            {
                minYear = devMinYear;

                if (devMinMonth > contractMinMonth)
                {
                    minMonth = contractMinMonth;
                }
                else
                {
                    minMonth = devMinMonth;
                }
            }
            else if (devMinYear > contractMinYear)
            {
                minYear = contractMinYear;
                minMonth = contractMinMonth;
            }
            else
            {
                minYear = devMinYear;
                minMonth = devMinMonth;
            }

            //3.处理积分数据
            var periodYear = maxYear;
            var periodMonth = maxMonth;
            var index = 0;
            ;
            while (!LargerYearAndMonth(minYear, minMonth, periodYear, periodMonth))
            {
                var devPeriods = dev.Where(t => t.CreditYear == periodYear && t.CreditMonth == periodMonth).FirstOrDefault();
                var contractPeriods = contract.Where(t => t.CreditYear == periodYear && t.CreditMonth == periodMonth).FirstOrDefault();

                ChannelCreditDetail creditDetail = new ChannelCreditDetail();

                var channel = GetChannel(channelGuid);
                creditDetail.ChannelGUID = channel.ChannelGUID;
                creditDetail.ChannelName = channel.ChannelName;

                creditDetail.CreditYear = periodYear;
                creditDetail.CreditMonth = periodMonth;
                

                if (devPeriods != null)
                {
                    creditDetail.Dev1Credit = devPeriods.Dev1Credit.HasValue ? devPeriods.Dev1Credit.Value : 0;
                    creditDetail.Dev2Credit = devPeriods.Dev2Credit.HasValue ? devPeriods.Dev2Credit.Value : 0;
                    creditDetail.Dev3Credit = devPeriods.Dev3Credit.HasValue ? devPeriods.Dev3Credit.Value : 0;
                    creditDetail.Dev4Credit = devPeriods.Dev4Credit.HasValue ? devPeriods.Dev4Credit.Value : 0;
                }
                else
                {
                    creditDetail.Dev1Credit = 0;
                    creditDetail.Dev2Credit = 0;
                    creditDetail.Dev3Credit = 0;
                    creditDetail.Dev4Credit = 0;
                }

                if (contractPeriods != null)
                {
                    creditDetail.ContractCredit = contractPeriods.ContractCredit;
                }
                else
                {
                    creditDetail.ContractCredit = 0;
                }

                creditDetailList.Add(creditDetail);

                periodMonth--;
                if (periodMonth == 0)
                {
                    periodMonth = 12;
                    periodYear--;
                }
            }
           

            return creditDetailList;
        }

        private bool LargerYearAndMonth(int beginYear, int beginMonth, int endYear, int endMonth)
        {
            if (beginYear > endYear)
                return true;
            if (beginYear == endYear)
            {
                if (beginMonth > endMonth)
                    return true;
            }

            return false;
        }

    }
}
