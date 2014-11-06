using ChinaUnicom.Fuyang.CreditManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.CreditManagement.ViewModels
{
    public class DevelopmentSummaryDto
    {
        public int Id { get; set; }
        public string SummaryPeriod { get; set; }
        public List<DevelopmentInfoDto> DevelopmentInfos { get; set; }
    }
}
