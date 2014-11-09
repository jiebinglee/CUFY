using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChinaUnicom.Fuyang.Framework;
using ChinaUnicom.Fuyang.CreditManagement.Services;
using ChinaUnicom.Fuyang.Framework.Adapter;
using ChinaUnicom.Fuyang.CreditManagement.Models;
using ChinaUnicom.Fuyang.CreditManagement.ViewModels;
using ChinaUnicom.Fuyang.Core.Users.Services;
using ChinaUnicom.Fuyang.Framework.FileSystems;
using System.Data;
using ChinaUnicom.Fuyang.Core.Users.Models;
using System.Transactions;
using ChinaUnicom.Fuyang.Framework.Data;
using System.Security.Cryptography;

namespace ChinaUnicom.Fuyang.CreditManagement
{
    public class CMManager : ModuleManager
    {
        readonly ICMService _cmService;
        readonly IUserService _userService;

        public CMManager(
            ICMService cmService,
            IUserService userService
            )
        {
            _cmService = cmService;
            _userService = userService;
        }

        #region Property

        private List<ChannelDictionary> _channelDictionary;
        private List<ChannelDictionary> ChannelDictionary
        {
            get
            {
                if (_channelDictionary == null)
                {
                    _channelDictionary = _cmService.GetChannelDictionary();
                }

                return _channelDictionary;
            }
        }

        private List<CreditConfig> _creditConfig;
        private List<CreditConfig> CreditConfig
        {
            get
            {
                if (_creditConfig == null)
                {
                    _creditConfig = _cmService.GetCreditConfig();
                }

                return _creditConfig;
            }
        }

        #endregion

        #region Ajax

        protected override void TaskBinding(ref Dictionary<string, ProcessFunc> tasks)
        {
            tasks.Add("Login", Login);
            tasks.Add("CheckUser", CheckUser);
            tasks.Add("GetChannel", GetChannel);
            tasks.Add("SearchChannel", SearchChannel);
            tasks.Add("Import", Import);
            tasks.Add("SearchChannelList", GetChannelList);
            tasks.Add("ExchangeCredit", ExchangeCredit);
            tasks.Add("GetCreditExchangeApprovalList", GetCreditExchangeApprovalList);
            tasks.Add("ApprovalExchangeCredit", ApprovalExchangeCredit);
            tasks.Add("GetAreaUser", GetAreaUser);    
        }

        #endregion

        #region Private Method

        #region Login

        private bool Login(TransBox data, string user, out object content, ref StringBuilder messager)
        {
            bool flagSuccess = false;
            content = null;

            var paramsFromClient = data.GetContent<Dictionary<string, string>>();

            if (paramsFromClient != null)
            {
                content = Login(paramsFromClient["UserName"], paramsFromClient["Password"]);
                flagSuccess = true;
            }

            return flagSuccess;
        }

        private Dictionary<string, object> Login(string userName, string password)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            var user = _userService.GetUser(userName, EncryptPassword(password));

            if (user != null)
            {
                result.Add("Token", TokenManager.Instance.GenerateLoginToken(decimal.ToInt32(user.Id), user.UserName));
                result.Add("UserType", user.UserType);
            }

            return result;
        }

        private bool CheckUser(TransBox data, string user, out object content, ref StringBuilder messager)
        {
            bool flagSuccess = false;
            content = null;

            var paramsFromClient = data.GetContent<Dictionary<string, string>>();

            if (paramsFromClient != null)
            {
                content = CheckUser(int.Parse(user), int.Parse(paramsFromClient["UserType"]));
                flagSuccess = true;
            }

            return flagSuccess;
        }

        private bool CheckUser(int userId, int userType)
        {
            bool result = false;

            var user = _userService.GetUser(userId);

            if (user != null)
            {
                result = user.UserType == userType;
            }

            return result;
        }

        #endregion

        #region Import

        private bool Import(TransBox data, string user, out object content, ref StringBuilder messager)
        {
            bool flagSuccess = false;
            content = null;

            var paramsFromClient = data.GetContent<Dictionary<string, string>>();

            if (paramsFromClient != null)
            {
                content = Import(user, int.Parse(paramsFromClient["ImportType"]), paramsFromClient["FileName"]);
                flagSuccess = true;
            }

            return flagSuccess;
        }

        private Dictionary<string, object> Import(string user, int importType, string fileName)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (importType == 1)
            {
                result.Add("ImportResult", ImportChannel(user, fileName));
            }
            else if (importType == 2)
            {
                result.Add("ImportResult", ImportDevelopment(user, fileName));
            }
            else if (importType == 3)
            {
                result.Add("ImportResult", ImportContract(user, fileName));
            }
            
            return result;
        }

        private object ImportChannel(string user, string fileName)
        {
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            string phyFilePath = path + "Upload\\" + fileName;

            DataTable dt = ExcelHelper.ExcelToDataTable(phyFilePath);

            dt.Columns["区县"].ColumnName = "AreaCode";
            dt.Columns["营服中心"].ColumnName = "AreaName";
            dt.Columns["渠道代码1"].ColumnName = "ChannelCode";
            dt.Columns["渠道代码2"].ColumnName = "UserName";
            dt.Columns["渠道名称"].ColumnName = "ChannelName";
            dt.Columns["渠道等级"].ColumnName = "ChannelLevelDesc";
            dt.Columns["加入积分系统时间"].ColumnName = "JoinYearAndMonth";
            dt.Columns["渠道建立时间"].ColumnName = "BuildYearAndMonth";

            List<ImportChannelDto> importChannelData = new List<ImportChannelDto>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i]["UserName"].ToString()))
                {
                    ImportChannelDto importChannelDto = new ImportChannelDto();
                    importChannelDto.UserName = dt.Rows[i]["UserName"].ToString();
                    importChannelDto.AreaCode = dt.Rows[i]["AreaCode"].ToString();
                    importChannelDto.AreaName = dt.Rows[i]["AreaName"].ToString();
                    importChannelDto.ChannelCode = dt.Rows[i]["ChannelCode"].ToString();
                    importChannelDto.ChannelName = dt.Rows[i]["ChannelName"].ToString();
                    importChannelDto.ChannelLevelDesc = dt.Rows[i]["ChannelLevelDesc"].ToString();
                    importChannelDto.JoinYearAndMonth = dt.Rows[i]["JoinYearAndMonth"].ToString();
                    importChannelDto.BuildYearAndMonth = dt.Rows[i]["BuildYearAndMonth"].ToString();
                    importChannelData.Add(importChannelDto);
                }
                else
                {
                    break;
                }
            }

            using (TransactionScope transaction = new TransactionScope())
            {
                var channelList = _cmService.GetChannelList();

                List<User> userListToDatabase = new List<User>();
                List<Channel> channelListToDatabase = new List<Channel>();
                List<CreditTotal> creditTotalListToDatabase = new List<CreditTotal>();

                for (int i = 0; i < importChannelData.Count; i++)
                {
                    string userName = importChannelData[i].UserName;
                    string channelCode = importChannelData[i].ChannelCode;
                    string channelName = importChannelData[i].ChannelName;
                    string areaCode = importChannelData[i].AreaCode;
                    string areaName = importChannelData[i].AreaName;
                    string channelLevelDesc = importChannelData[i].ChannelLevelDesc;
                    int joinYear = importChannelData[i].JoinYear;
                    int joinMonth = importChannelData[i].JoinMonth;
                    int buildYear = importChannelData[i].BuildYear;
                    int buildMonth = importChannelData[i].BuildMonth;

                    int channelLevel = ChannelDictionary.Where(t => t.DictionaryTable == "CHANNEL_LEVEL" && t.DictionaryValue == channelLevelDesc)
                                .FirstOrDefault()
                                .DictionaryKey;

                    if (channelList.Where(t => t.ChannelCode == channelCode).FirstOrDefault() == null)
                    {
                        var newUser = _userService.Insert(new User()
                        {
                            UserName = userName,
                            Password = EncryptPassword(userName),
                            Phone = null,
                            Email = null,
                            CreateDateTime = DateTime.Now,
                            UserType = (int)EnumUserType.Channel,
                            Flag = 1

                        });

                        var newChannel = _cmService.InsertChannel(new Channel()
                        {
                            ChannelGUID = Guid.NewGuid(),
                            ChannelCode = channelCode,
                            ChannelName = channelName,
                            AreaCode = areaCode,
                            AreaName = areaName,
                            ChannelLevel = channelLevel,
                            Desc = string.Empty,
                            ChannelStatus = 1,
                            Flag = 1,
                            UserId = newUser.Id,
                            JoinYear = joinYear,
                            JoinMonth = joinMonth,
                            BuildYear = buildYear,
                            BuildMonth = buildMonth
                        });

                        importChannelData[i].ImportStatus = "新增";

                        var creditTotal = _cmService.InsertCreditTotal(new CreditTotal()
                        {
                            Id = newChannel.Id,
                            ChannelGUID = newChannel.ChannelGUID,
                            LastUpdateDate = DateTime.Now,
                            TotalAmount = CalculateYearBonus(buildYear, buildMonth),
                            Flag = 1,
                            DevCredit = 0,
                            ContractCredit = 0,
                            YearBonus = CalculateYearBonus(buildYear, buildMonth),
                            ExchangedCredit = 0,
                            RemainingTotalAmount = CalculateYearBonus(buildYear, buildMonth)
                        });
                    }
                    else
                    {
                        importChannelData[i].ImportStatus = "已存在";
                    }
                }

                _cmService.InsertImport(new Import()
                {
                    ImportDate = DateTime.Now,
                    OperatorId = int.Parse(user),
                    Flag = 1,
                    DataType = 1
                });

                transaction.Complete();
            }

            return importChannelData;
        }

        private object ImportDevelopment(string user, string fileName)
        {
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            string phyFilePath = path + "Upload\\" + fileName;

            DataTable dt = ExcelHelper.ExcelToDataTable(phyFilePath);

            dt.Columns["渠道代码"].ColumnName = "ChannelCode";
            dt.Columns["亲情卡、长话卡"].ColumnName = "DevType1";
            dt.Columns["2G单卡、无线公话"].ColumnName = "DevType2";
            dt.Columns["3G/4G合约终端"].ColumnName = "DevType3";
            dt.Columns["3G/4G单卡、宽带"].ColumnName = "DevType4";
            dt.Columns["新增月份"].ColumnName = "DevYearAndMonth";

            List<ImportDevelopmentDto> importDevelopmentData = new List<ImportDevelopmentDto>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i]["ChannelCode"].ToString()))
                {
                    ImportDevelopmentDto importDevelopmentDto = new ImportDevelopmentDto();
                    importDevelopmentDto.ChannelCode = dt.Rows[i]["ChannelCode"].ToString();
                    importDevelopmentDto.DevType1 = dt.Rows[i]["DevType1"].ToString();
                    importDevelopmentDto.DevType2 = dt.Rows[i]["DevType2"].ToString();
                    importDevelopmentDto.DevType3 = dt.Rows[i]["DevType3"].ToString();
                    importDevelopmentDto.DevType4 = dt.Rows[i]["DevType4"].ToString();
                    importDevelopmentDto.DevYearAndMonth = dt.Rows[i]["DevYearAndMonth"].ToString();
                    importDevelopmentData.Add(importDevelopmentDto);
                }
                else
                {
                    break;
                }
            }

            using (TransactionScope transaction = new TransactionScope())
            {
                var channelList = _cmService.GetChannelList();

                List<Development> devListToDatabase = new List<Development>();
                List<CreditTotal> creditTotalListToDatabase = new List<CreditTotal>();

                for (int i = 0; i < importDevelopmentData.Count; i++)
                {
                    var importDevelopment = importDevelopmentData[i];

                    Channel channel = channelList.Where(t => t.ChannelCode == importDevelopment.ChannelCode).FirstOrDefault();

                    if (channel != null)
                    {
                        Guid channelGuid = channel.ChannelGUID;
                        int devYear = importDevelopment.DevYear;
                        int devMonth = importDevelopment.DevMonth;
                        int devCredit = 0;

                        var devTypeStatus = new string[importDevelopment.DevCount.Length];

                        for (int j = 0; j < importDevelopment.DevCount.Length; j++)
                        {
                            var tempDevType = j + 1;

                            if (channel.Developments.Where(t => t.DevType == tempDevType && t.DevYear == devYear
                                && t.DevMonth == devMonth && t.Flag == 1).FirstOrDefault() == null)
                            {
                                var tempDevCount = importDevelopment.DevCount[j];
                                if (tempDevCount != 0)
                                {
                                    var tempCredit = CalculateCredit(channel.ChannelLevel, tempDevType, tempDevCount);

                                    devListToDatabase.Add(new Development()
                                    {
                                        ChannelGUID = channelGuid,
                                        DevType = tempDevType,
                                        DevCount = tempDevCount,
                                        DevYear = devYear,
                                        DevMonth = devMonth,
                                        DevCount2 = 0,
                                        CreditAmount = tempCredit,
                                        Flag = 1
                                    });

                                    devCredit += tempCredit;

                                    devTypeStatus[j] = "导入成功";
                                }
                                else
                                {
                                    devTypeStatus[j] = "未导入";
                                }
                            }
                            else
                            {
                                devTypeStatus[j] = "数据已存在";
                            }
                        }

                        var oldYearBonus = channel.CreditTotal.YearBonus;
                        var newYearBonus = CalculateYearBonus(channel.BuildYear, channel.BuildMonth);

                        var existingCreditTotalList = creditTotalListToDatabase.Where(t => t.ChannelGUID == channel.ChannelGUID);

                        if (existingCreditTotalList.Count() > 0)
                        {
                            var existingCreditTotal = existingCreditTotalList.FirstOrDefault();

                            existingCreditTotal.TotalAmount = existingCreditTotal.DevCredit + devCredit + existingCreditTotal.ContractCredit + newYearBonus;
                            existingCreditTotal.LastUpdateDate = DateTime.Now;
                            existingCreditTotal.DevCredit = existingCreditTotal.DevCredit + devCredit;
                            existingCreditTotal.YearBonus = newYearBonus;
                            existingCreditTotal.RemainingTotalAmount = existingCreditTotal.TotalAmount - existingCreditTotal.ExchangedCredit;
                        }
                        else
                        {
                            if (LargerYearAndMonth(devYear, devMonth, channel.JoinYear, channel.JoinMonth))
                            {
                                creditTotalListToDatabase.Add(new CreditTotal()
                                {
                                    Id = channel.Id,
                                    ChannelGUID = channelGuid,
                                    TotalAmount = channel.CreditTotal.DevCredit + devCredit + channel.CreditTotal.ContractCredit + newYearBonus,
                                    LastUpdateDate = DateTime.Now,
                                    DevCredit = channel.CreditTotal.DevCredit + devCredit,
                                    ContractCredit = channel.CreditTotal.ContractCredit,
                                    YearBonus = newYearBonus,
                                    Flag = 1,
                                    ExchangedCredit = channel.CreditTotal.ExchangedCredit,
                                    RemainingTotalAmount = channel.CreditTotal.DevCredit + devCredit + channel.CreditTotal.ContractCredit + newYearBonus - channel.CreditTotal.ExchangedCredit
                                });
                            }
                        }

                        importDevelopment.DevTypeStatus = devTypeStatus;

                        importDevelopment.ImportStatus = "新增积分：" + (devCredit + newYearBonus - oldYearBonus);
                    }
                    else
                    {
                        importDevelopment.ImportStatus = "渠道商不存在";
                    }
                }

                if (devListToDatabase.Count > 0)
                {
                    var devResults = _cmService.InsertDevelopment(devListToDatabase);
                }

                if (creditTotalListToDatabase.Count > 0)
                {
                    var creditTotalResults = _cmService.UpdateCreditTotal(creditTotalListToDatabase);
                }

                _cmService.InsertImport(new Import()
                {
                    ImportDate = DateTime.Now,
                    OperatorId = int.Parse(user),
                    Flag = 1,
                    DataType = 2
                });

                transaction.Complete();
            }

            return importDevelopmentData;
        }

        private object ImportContract(string user, string fileName)
        {
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            string phyFilePath = path + "Upload\\" + fileName;

            DataTable dt = ExcelHelper.ExcelToDataTable(phyFilePath);

            dt.Columns["渠道代码"].ColumnName = "ChannelCode";
            dt.Columns["发展个数"].ColumnName = "ContractCount";
            dt.Columns["新增月份"].ColumnName = "ContractYearAndMonth";

            List<ImportContractDto> importContractData = new List<ImportContractDto>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i]["ChannelCode"].ToString()))
                {
                    ImportContractDto importContractDto = new ImportContractDto();
                    importContractDto.ChannelCode = dt.Rows[i]["ChannelCode"].ToString();
                    importContractDto.ContractYearAndMonth = dt.Rows[i]["ContractYearAndMonth"].ToString();
                    importContractDto.ContractCount = int.Parse(dt.Rows[i]["ContractCount"].ToString() == string.Empty ? "0" : dt.Rows[i]["ContractCount"].ToString());
                    importContractData.Add(importContractDto);
                }
                else
                {
                    break;
                }
            }

            using (TransactionScope transaction = new TransactionScope())
            {
                var channelList = _cmService.GetChannelList();

                List<Contract> contractListToDatabase = new List<Contract>();
                List<CreditTotal> creditTotalListToDatabase = new List<CreditTotal>();

                for (int i = 0; i < importContractData.Count; i++)
                {
                    var importContract = importContractData[i];

                    Channel channel = channelList.Where(t => t.ChannelCode == importContract.ChannelCode).FirstOrDefault();

                    if (channel != null)
                    {
                        Guid channelGuid = channel.ChannelGUID;
                        int contractCount = importContract.ContractCount;
                        int contractYear = importContract.ContractYear;
                        int contractMonth = importContract.ContractMonth;

                        if (channel.Contracts.Where(t => t.ContractYear == contractYear
                                    && t.ContractMonth == contractMonth && t.Flag == 1).FirstOrDefault() == null)
                        {
                            if (contractCount != 0)
                            {
                                var contractCredit = contractCount * 20;
                                var oldYearBonus = channel.CreditTotal.YearBonus;
                                var newYearBonus = CalculateYearBonus(channel.BuildYear, channel.BuildMonth);

                                contractListToDatabase.Add(new Contract()
                                {
                                    ChannelGUID = channelGuid,
                                    ContractCount = contractCount,
                                    ContractYear = contractYear,
                                    ContractMonth = contractMonth,
                                    ContractCount2 = 0,
                                    CreditAmount = contractCredit,
                                    Flag = 1                                    
                                });

                                var existingCreditTotalList = creditTotalListToDatabase.Where(t => t.ChannelGUID == channel.ChannelGUID);

                                if (existingCreditTotalList.Count() > 0)
                                {
                                    var existingCreditTotal = existingCreditTotalList.FirstOrDefault();

                                    existingCreditTotal.TotalAmount = existingCreditTotal.ContractCredit + contractCredit + existingCreditTotal.DevCredit + newYearBonus;
                                    existingCreditTotal.LastUpdateDate = DateTime.Now;
                                    existingCreditTotal.ContractCredit = existingCreditTotal.ContractCredit + contractCredit;
                                    existingCreditTotal.YearBonus = newYearBonus;
                                    existingCreditTotal.RemainingTotalAmount = existingCreditTotal.TotalAmount - existingCreditTotal.ExchangedCredit;
                                }
                                else
                                {
                                    if (LargerYearAndMonth(contractYear, contractMonth, channel.JoinYear, channel.JoinMonth))
                                    {
                                        creditTotalListToDatabase.Add(new CreditTotal()
                                        {
                                            Id = channel.Id,
                                            ChannelGUID = channelGuid,
                                            TotalAmount = channel.CreditTotal.ContractCredit + contractCredit + channel.CreditTotal.DevCredit + newYearBonus,
                                            LastUpdateDate = DateTime.Now,
                                            DevCredit = channel.CreditTotal.DevCredit,
                                            ContractCredit = channel.CreditTotal.ContractCredit + contractCredit,
                                            YearBonus = newYearBonus,
                                            Flag = 1,
                                            ExchangedCredit = channel.CreditTotal.ExchangedCredit,
                                            RemainingTotalAmount = channel.CreditTotal.ContractCredit + contractCredit + channel.CreditTotal.DevCredit + newYearBonus - channel.CreditTotal.ExchangedCredit
                                        });
                                    }
                                }

                                importContract.ImportStatus = "新增积分：" + (contractCredit + newYearBonus - oldYearBonus);
                            }
                            else
                            {
                                importContract.ImportStatus = "未导入";
                            }
                        }
                        else
                        {
                            importContract.ImportStatus = "数据已存在";
                        }
                    }
                    else
                    {
                        importContract.ImportStatus = "渠道商不存在";
                    }
                }

                if (contractListToDatabase.Count > 0)
                {
                    var contractResults = _cmService.InsertContract(contractListToDatabase);
                }

                if (creditTotalListToDatabase.Count > 0)
                {
                    var creditTotalResults = _cmService.UpdateCreditTotal(creditTotalListToDatabase);
                }

                _cmService.InsertImport(new Import()
                {
                    ImportDate = DateTime.Now,
                    OperatorId = int.Parse(user),
                    Flag = 1,
                    DataType = 3
                });

                transaction.Complete();
            }

            return importContractData;
        }

        private int CalculateCredit(int channelLevel, int devType, int devCount)
        {
            var creditConfig = CreditConfig.Where(t => t.ChannelLevelId == channelLevel && t.DevTypeId == devType).FirstOrDefault();
            return Convert.ToInt32(devCount * creditConfig.CreditBase * creditConfig.CreditRatio);
        }

        private int CalculateYearBonus(int buildYear, int buildMonth)
        {
            var result = 0;
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            var months = 0;

            if (currentYear > buildYear)
            {
                months = (currentYear - buildYear - 1) * 12 + (currentMonth + 12 - buildMonth);
            }

            result = (int)Math.Floor((double)months / 12);

            return result * 1000;
        }

        private bool LargerYearAndMonth(int beginYear, int beginMonth, int endYear, int endMonth)
        {
            if (beginYear > endYear)
                return true;
            if (beginYear == endYear)
            {
                if (beginMonth >= endMonth)
                    return true;
            }

            return false;
        }

        private string EncryptPassword(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            //将要加密的字符串转换为字节数组
            byte[] palindata = Encoding.Default.GetBytes(password);
            //将字符串加密后也转换为字符数组
            byte[] encryptdata = md5.ComputeHash(palindata);
            //将加密后的字节数组转换为加密字符串
            return Convert.ToBase64String(encryptdata);            
        }

        #endregion

        #region ChannelUser

        private bool GetChannel(TransBox data, string user, out object content, ref StringBuilder messager)
        {            
            content = GetChannel(int.Parse(user));

            return content != null;
        }

        private Dictionary<string, object> GetChannel(int userId)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            var channel = _cmService.GetChannel(userId);

            if (channel != null)
            {
                var channelDto = TypeAdapterFactory.CreateAdapter().Adapt<Channel, ChannelInfoDto>(channel);
                channelDto.ChannelLevelDesc = ChannelDictionary.Where(t => t.DictionaryTable == "CHANNEL_LEVEL" && t.DictionaryKey == channelDto.ChannelLevel)
                                .FirstOrDefault()
                                .DictionaryValue;
                result.Add("ChannelInfo", channelDto);
            }

            return result;
        }

        private bool SearchChannel(TransBox data, string user, out object content, ref StringBuilder messager)
        {
            bool flagSuccess = false;
            content = null;

            var paramsFromClient = data.GetContent<Dictionary<string, int>>();

            if (paramsFromClient != null)
            {
                content = SearchChannel(int.Parse(user), paramsFromClient["BeginYear"], paramsFromClient["BeginMonth"], paramsFromClient["EndYear"], paramsFromClient["EndMonth"]);
                flagSuccess = true;
            }

            return flagSuccess;
        }

        private Dictionary<string, object> SearchChannel(int userId, int beginYear, int beginMonth, int endYear, int endMonth)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            //1. 获得渠道商
            var channel = _cmService.GetChannel(userId);
            //2. 获得发展数
            var devList = channel.Developments;

            devList = devList.Where(t =>
            {
                if (t.DevYear < beginYear || t.DevYear > endYear) return false;
                if (t.DevYear == beginYear && t.DevMonth < beginMonth) return false;
                if (t.DevYear == endYear && t.DevMonth > endMonth) return false;

                if (t.DevYear < channel.JoinYear) return false;
                if (t.DevYear == channel.JoinYear && t.DevMonth < channel.JoinMonth) return false;

                return true;
            }).ToList();
            //3. 获得签约放号数
            var contractList = channel.Contracts;
            contractList = contractList.Where(t =>
            {
                if (t.ContractYear < beginYear || t.ContractYear > endYear) return false;
                if (t.ContractYear == beginYear && t.ContractMonth < beginMonth) return false;
                if (t.ContractYear == endYear && t.ContractMonth > endMonth) return false;

                if (t.ContractYear < channel.JoinYear) return false;
                if (t.ContractYear == channel.JoinYear && t.ContractMonth < channel.JoinMonth) return false;

                return true;
            }).ToList();

            //4. 合计 Begin

            List<DevelopmentInfoDto> totalDevInfoList = new List<DevelopmentInfoDto>();


            if (contractList.Count > 0)
            {
                var m = from contract in contractList
                        group contract by contract.ChannelGUID into n
                        select new
                        {
                            n.Key,
                            ContractCountTotal = n.Sum(t => t.ContractCount),
                            CreditAmountTotal = n.Sum(t => t.CreditAmount)
                        };

                var mm = m.FirstOrDefault();

                var totalDevInfo = new DevelopmentInfoDto();
                totalDevInfo.DevType = 0;
                totalDevInfo.DevCount = mm.ContractCountTotal;
                totalDevInfo.CreditAmount = mm.CreditAmountTotal;
                totalDevInfo.DevTypeDesc = "签约放号（发展）";
                totalDevInfo.CreditBase = 20;
                totalDevInfo.CreditRatio = 1;
                totalDevInfoList.Add(totalDevInfo);
            }
            else
            {
                var totalDevInfo = new DevelopmentInfoDto();
                totalDevInfo.DevType = 0;
                totalDevInfo.DevCount = 0;
                totalDevInfo.CreditAmount = 0;
                totalDevInfo.DevTypeDesc = "签约放号（发展）";
                totalDevInfo.CreditBase = 20;
                totalDevInfo.CreditRatio = 1;
                totalDevInfoList.Add(totalDevInfo);
            }

            if (devList.Count > 0)
            {
                var p = from dev in devList
                        group dev by dev.DevType into q
                        select new
                        {
                            q.Key,
                            DevCountTotal = q.Sum(dev => dev.DevCount),
                            CreditAmountTotal = q.Sum(dev => dev.CreditAmount)
                        };

                foreach (var pp in p)
                {
                    DevelopmentInfoDto totalDevInfo = new DevelopmentInfoDto();
                    totalDevInfo.DevType = pp.Key;
                    totalDevInfo.DevCount = pp.DevCountTotal;
                    totalDevInfo.CreditAmount = pp.CreditAmountTotal;

                    FillDevInfo(channel.ChannelLevel, totalDevInfo);

                    totalDevInfoList.Add(totalDevInfo);
                }
            }
            else
            {
                for (int i = 1; i < 5; i++)
                {
                    DevelopmentInfoDto totalDevInfo = new DevelopmentInfoDto();
                    totalDevInfo.DevType = i;
                    totalDevInfo.DevCount = 0;
                    totalDevInfo.CreditAmount = 0;

                    FillDevInfo(channel.ChannelLevel, totalDevInfo);

                    totalDevInfoList.Add(totalDevInfo);
                }
            }

            DevelopmentSummaryDto totalSummary = new DevelopmentSummaryDto();
            totalSummary.Id = 1;
            totalSummary.SummaryPeriod = string.Format("{0}年{1}月 - {2}年{3}月区间", beginYear, beginMonth, endYear, endMonth);
            totalSummary.DevelopmentInfos = CompleteDevelopmentInfo(totalDevInfoList.OrderBy(t => t.DevType).ToList());

            result.Add("DevelopmentSummaryByTotal", totalSummary);

            //4. 合计 End

            //5. 按月 Begin

            List<DevelopmentSummaryDto> periodSummaryList = new List<DevelopmentSummaryDto>();

            var contractPeriodList = contractList.OrderBy(t => t.ContractYear).ThenBy(s => s.ContractMonth).ToList();
            var devPeriodList = devList.OrderBy(t => t.DevYear).ThenBy(s => s.DevMonth).ThenBy(r => r.DevType).ToList();

            var periodYear = beginYear;
            var periodMonth = beginMonth;
            var index = 0;
            ;
            while (LargerYearAndMonth(endYear, endMonth, periodYear, periodMonth))
            {
                var contractPeriods = contractPeriodList.Where(t => t.ContractYear == periodYear && t.ContractMonth == periodMonth).ToList();
                var devPeriods = devPeriodList.Where(t => t.DevYear == periodYear && t.DevMonth == periodMonth).ToList();


                List<DevelopmentInfoDto> periodDevInfoList = new List<DevelopmentInfoDto>();

                if (contractPeriods.Count > 0)
                {

                    var mm = contractPeriods.FirstOrDefault();

                    var periodDevInfo = new DevelopmentInfoDto();
                    periodDevInfo.DevType = 0;
                    periodDevInfo.DevCount = mm.ContractCount;
                    periodDevInfo.CreditAmount = mm.CreditAmount;
                    periodDevInfo.DevTypeDesc = "签约放号（发展）";
                    periodDevInfo.CreditBase = 20;
                    periodDevInfo.CreditRatio = 1;
                    periodDevInfoList.Add(periodDevInfo);
                }
                else
                {
                    var periodDevInfo = new DevelopmentInfoDto();
                    periodDevInfo.DevType = 0;
                    periodDevInfo.DevCount = 0;
                    periodDevInfo.CreditAmount = 0;
                    periodDevInfo.DevTypeDesc = "签约放号（发展）";
                    periodDevInfo.CreditBase = 20;
                    periodDevInfo.CreditRatio = 1;
                    periodDevInfoList.Add(periodDevInfo);
                }

                if (devPeriods.Count > 0)
                {
                    foreach (var pp in devPeriods)
                    {
                        DevelopmentInfoDto periodDevInfo = new DevelopmentInfoDto();
                        periodDevInfo.DevType = pp.DevType;
                        periodDevInfo.DevCount = pp.DevCount;
                        periodDevInfo.CreditAmount = pp.CreditAmount;

                        FillDevInfo(channel.ChannelLevel, periodDevInfo);

                        periodDevInfoList.Add(periodDevInfo);
                    }
                }
                else
                {
                    for (int i = 1; i < 5; i++)
                    {
                        DevelopmentInfoDto periodDevInfo = new DevelopmentInfoDto();
                        periodDevInfo.DevType = i;
                        periodDevInfo.DevCount = 0;
                        periodDevInfo.CreditAmount = 0;

                        FillDevInfo(channel.ChannelLevel, periodDevInfo);

                        periodDevInfoList.Add(periodDevInfo);
                    }
                }

                DevelopmentSummaryDto periodSummary = new DevelopmentSummaryDto();
                periodSummary.Id = index;
                periodSummary.SummaryPeriod = string.Format("{0}年{1}月", periodYear, periodMonth);

                periodSummary.DevelopmentInfos = CompleteDevelopmentInfo(periodDevInfoList);

                periodSummaryList.Add(periodSummary);

                index++;


                periodMonth++;
                if (periodMonth > 12)
                {
                    periodMonth = 1;
                    periodYear++;
                }
            }

            //5. 按月 End

            if (periodSummaryList.Count > 0)
            {
                result.Add("DevelopmentSummaryByPeriod", periodSummaryList);
            }

            return result;
        }

        private void FillDevInfo(int channelLevel, DevelopmentInfoDto devInfo)
        {
            devInfo.DevTypeDesc = ChannelDictionary.Where(t => t.DictionaryTable == "DEV_TYPE" && t.DictionaryKey == devInfo.DevType)
                                .FirstOrDefault()
                                .DictionaryValue + "（在网）";

            var creditConfig = CreditConfig.Where(t => t.ChannelLevelId == channelLevel && t.DevTypeId == devInfo.DevType).FirstOrDefault();
            devInfo.CreditBase = creditConfig.CreditBase;
            devInfo.CreditRatio = creditConfig.CreditRatio;
        }

        private List<DevelopmentInfoDto> CompleteDevelopmentInfo(List<DevelopmentInfoDto> devs)
        {
            var result = devs;
            var creditBase = devs[0].CreditBase;
            var creditRatio = devs[0].CreditRatio;

            if (devs.Where(t => t.DevType == 1).ToList().Count == 0)
            {
                var dev = new DevelopmentInfoDto();
                dev.DevType = 1;
                dev.DevTypeDesc = ChannelDictionary.Where(t => t.DictionaryTable == "DEV_TYPE" && t.DictionaryKey == 1)
                                .FirstOrDefault()
                                .DictionaryValue + "（在网）";
                dev.DevCount = 0;
                dev.CreditBase = creditBase;
                dev.CreditRatio = creditRatio;
                dev.CreditAmount = 0;

                result.Add(dev);
            }

            if (devs.Where(t => t.DevType == 2).ToList().Count == 0)
            {
                var dev = new DevelopmentInfoDto();
                dev.DevType = 2;
                dev.DevTypeDesc = ChannelDictionary.Where(t => t.DictionaryTable == "DEV_TYPE" && t.DictionaryKey == 2)
                                .FirstOrDefault()
                                .DictionaryValue + "（在网）";
                dev.DevCount = 0;
                dev.CreditBase = creditBase;
                dev.CreditRatio = creditRatio;
                dev.CreditAmount = 0;

                result.Add(dev);
            }

            if (devs.Where(t => t.DevType == 3).ToList().Count == 0)
            {
                var dev = new DevelopmentInfoDto();
                dev.DevType = 3;
                dev.DevTypeDesc = ChannelDictionary.Where(t => t.DictionaryTable == "DEV_TYPE" && t.DictionaryKey == 3)
                                .FirstOrDefault()
                                .DictionaryValue + "（在网）";
                dev.DevCount = 0;
                dev.CreditBase = creditBase;
                dev.CreditRatio = creditRatio;
                dev.CreditAmount = 0;

                result.Add(dev);
            }

            if (devs.Where(t => t.DevType == 4).ToList().Count == 0)
            {
                var dev = new DevelopmentInfoDto();
                dev.DevType = 4;
                dev.DevTypeDesc = ChannelDictionary.Where(t => t.DictionaryTable == "DEV_TYPE" && t.DictionaryKey == 4)
                                .FirstOrDefault()
                                .DictionaryValue + "（在网）";
                dev.DevCount = 0;
                dev.CreditBase = creditBase;
                dev.CreditRatio = creditRatio;
                dev.CreditAmount = 0;

                result.Add(dev);
            }

            return result.OrderBy(t => t.DevType).ToList();
        }

        #endregion

        #region AreaUser
        
        private bool GetChannelList(TransBox data, string user, out object content, ref StringBuilder messager)
        {
            bool flagSuccess = false;
            content = null;

            var paramsFromClient = data.GetContent<Dictionary<string, string>>();

            if (paramsFromClient != null)
            {
                var pageNumber = int.Parse(paramsFromClient["PageNumber"]);
                var pageSize = int.Parse(paramsFromClient["PageSize"]);
                content = GetChannelList(int.Parse(user), pageNumber, pageSize);
                flagSuccess = true;
            }

            return flagSuccess;
        }

        private object GetChannelList(int userId, int pageNumber, int pageSize)
        {
            Pageable<ChannelInfoDto> result = null;

            var user = _userService.GetUser(userId);

            if (user.UserType == (int)EnumUserType.Admin)
            {
                result = GetChannelForAdmin(pageNumber, pageSize);
            }

            if (user.UserType == (int)EnumUserType.Area)
            {
                result = GetChannelForArea(userId, pageNumber, pageSize);
            }

            return result;
        }

        private Pageable<ChannelInfoDto> GetChannelForArea(int userId, int pageNumber, int pageSize)
        {
            return _cmService.GetChannelList(userId, pageNumber, pageSize);
        }

        private Pageable<ChannelInfoDto> GetChannelForAdmin(int pageNumber, int pageSize)
        {
            return _cmService.GetChannelList(0, pageNumber, pageSize);
        }

        private Dictionary<string, object> GetChannel(Guid channelGuid)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            var channel = _cmService.GetChannel(channelGuid);

            if (channel != null)
            {
                var channelDto = TypeAdapterFactory.CreateAdapter().Adapt<Channel, ChannelInfoDto>(channel);
                channelDto.ChannelLevelDesc = ChannelDictionary.Where(t => t.DictionaryTable == "CHANNEL_LEVEL" && t.DictionaryKey == channelDto.ChannelLevel)
                                .FirstOrDefault()
                                .DictionaryValue;
                result.Add("ChannelInfo", channelDto);
            }

            return result;
        }

        #endregion

        #region CreditExchange

        private bool ExchangeCredit(TransBox data, string user, out object content, ref StringBuilder messager)
        {
            bool flagSuccess = false;
            content = null;

            var paramsFromClient = data.GetContent<Dictionary<string, string>>();

            if (paramsFromClient != null)
            {
                var channelGuid = Guid.Parse(paramsFromClient["ChannelGuid"]);
                var exchangeCredit = int.Parse(paramsFromClient["ExchangeCredit"]);
                content = ExchangeCredit(int.Parse(user), channelGuid, exchangeCredit);
                flagSuccess = true;
            }

            return flagSuccess;
        }

        private object ExchangeCredit(int userId, Guid channelGuid, int exchangeCredit)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                var newCreditExchange = _cmService.InsertCreditExchange(new CreditExchange()
                {
                    ChannelGUID = channelGuid,
                    ExchangeCredit = exchangeCredit,
                    Status = 0,
                    ExchangeDateTime = DateTime.Now,
                    ExchangeUser = userId,
                    ApprovalDateTime = null,
                    ApprovalUser = null,
                    Flag = 1
                });

                transaction.Complete();
            }

            return GetChannel(channelGuid);
        }

        private bool GetCreditExchangeApprovalList(TransBox data, string user, out object content, ref StringBuilder messager)
        {
            bool flagSuccess = false;
            content = null;

            var paramsFromClient = data.GetContent<Dictionary<string, string>>();

            if (paramsFromClient != null)
            {
                var pageNumber = int.Parse(paramsFromClient["PageNumber"]);
                var pageSize = int.Parse(paramsFromClient["PageSize"]);
                content = GetCreditExchangeApprovalList(pageNumber, pageSize);
                flagSuccess = true;
            }

            return flagSuccess;
        }

        private object GetCreditExchangeApprovalList(int pageNumber, int pageSize)
        {
            Pageable<ChannelCreditExchangeInfoDto> result = null;

            result = _cmService.GetCreditExchangeApprovalList(pageNumber, pageSize);

            return result;
        }

        private bool ApprovalExchangeCredit(TransBox data, string user, out object content, ref StringBuilder messager)
        {
            bool flagSuccess = false;
            content = null;

            var paramsFromClient = data.GetContent<Dictionary<string, string>>();

            if (paramsFromClient != null)
            {
                var exchangeId = int.Parse(paramsFromClient["ExchangeId"]);
                var approvalStatus = int.Parse(paramsFromClient["ApprovalStatus"]);
                content = ApprovalExchangeCredit(int.Parse(user), exchangeId, approvalStatus);
                flagSuccess = true;
            }

            return flagSuccess;
        }

        private object ApprovalExchangeCredit(int userId, int exchangeId, int approvalStatus)
        {
            Guid channelGuid;

            using (TransactionScope transaction = new TransactionScope())
            {
                var creditExchange = _cmService.GetCreditExchange(exchangeId);

                channelGuid = creditExchange.ChannelGUID;

                creditExchange.ApprovalDateTime = DateTime.Now;
                creditExchange.ApprovalUser = userId;
                creditExchange.Status = approvalStatus;

                _cmService.UpdateCreditExchange(creditExchange);

                if (approvalStatus == 1)
                {
                    var creditTotal = _cmService.GetCreditTotal(channelGuid);

                    creditTotal.ExchangedCredit = creditTotal.ExchangedCredit + creditExchange.ExchangeCredit;
                    creditTotal.RemainingTotalAmount = creditTotal.TotalAmount - creditTotal.ExchangedCredit;

                    _cmService.UpdateCreditTotal(creditTotal);
                }

                transaction.Complete();
            }

            if (channelGuid != null)
            {
                return GetChannel(channelGuid);
            }
            else
            {
                return null;
            }
        }

        private bool GetAreaUser(TransBox data, string user, out object content, ref StringBuilder messager)
        {
            bool flagSuccess = false;
            content = null;

            var paramsFromClient = data.GetContent<Dictionary<string, string>>();

            if (paramsFromClient != null)
            {
                var pageNumber = int.Parse(paramsFromClient["PageNumber"]);
                var pageSize = int.Parse(paramsFromClient["PageSize"]);
                content = GetAreaUser(pageNumber, pageSize);
                flagSuccess = true;
            }

            return flagSuccess;
        }

        private object GetAreaUser(int pageNumber, int pageSize)
        {
            Pageable<AreaUserInfoDto> result = null;

            result = _cmService.GetAreaUser(pageNumber, pageSize);

            return result;
        }

        #endregion

        #endregion
    }
}
