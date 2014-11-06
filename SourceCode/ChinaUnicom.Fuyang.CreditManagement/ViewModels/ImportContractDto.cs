using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.CreditManagement.ViewModels
{
    public class ImportContractDto
    {
        public string ChannelCode { get; set; }
        public int ContractCount { get; set; }       
        public int ContractYear { get; private set; }
        public int ContractMonth { get; private set; }
       
        private string _contractYearAndMonth;
        public string ContractYearAndMonth
        {
            get
            {
                return _contractYearAndMonth;
            }
            set
            {
                _contractYearAndMonth = value;
                ContractYear = int.Parse(value.Substring(0, 4));
                ContractMonth = int.Parse(value.Substring(4, 2));
            }
        }

        public string ImportStatus { get; set; }
    }
}
