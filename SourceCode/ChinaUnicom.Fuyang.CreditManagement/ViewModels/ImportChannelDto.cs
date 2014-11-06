using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.CreditManagement.ViewModels
{
    public class ImportChannelDto
    {
        public int ChannelId { get; set; }
        public string ChannelCode { get; set; }
        public string ChannelName { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string ChannelLevelDesc { get; set; }
        public string Desc { get; set; }
        public string UserName { get; set; }

        private string _joinYearAndMonth;
        public string JoinYearAndMonth
        {
            get
            {
                return _joinYearAndMonth;
            }
            set
            {
                _joinYearAndMonth = value;
                JoinYear = int.Parse(value.Substring(0, 4));
                JoinMonth = int.Parse(value.Substring(4, 2));
            }
        }
        public int JoinYear { get; set; }
        public int JoinMonth { get; set; }

        private string _buildYearAndMonth;
        public string BuildYearAndMonth
        {
            get
            {
                return _buildYearAndMonth;
            }
            set
            {
                _buildYearAndMonth = value;
                BuildYear = int.Parse(value.Substring(0, 4));
                BuildMonth = int.Parse(value.Substring(4, 2));
            }
        }       
        public int BuildYear { get; set; }
        public int BuildMonth { get; set; }

        public string ImportStatus { get; set; }
    }
}
