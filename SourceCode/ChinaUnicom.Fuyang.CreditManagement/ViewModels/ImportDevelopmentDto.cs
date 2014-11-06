using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.CreditManagement.ViewModels
{
    public class ImportDevelopmentDto
    {
        public string ChannelCode { get; set; }

        private string _devType1;
        public string DevType1
        {
            get
            {
                return _devType1;
            }
            set
            {
                var t = value.Trim() == string.Empty ? "0" : value.Trim();
                if (_devCount == null)
                    _devCount = new int[4];
                _devCount.SetValue(int.Parse(t), 0);
                _devType1 = t;
            }
        }

        private string _devType2;
        public string DevType2
        {
            get
            {
                return _devType2;
            }
            set
            {
                var t = value.Trim() == string.Empty ? "0" : value.Trim();
                if (_devCount == null)
                    _devCount = new int[4];
                _devCount.SetValue(int.Parse(t), 1);

                _devType2 = t;
            }
        }

        private string _devType3;
        public string DevType3
        {
            get
            {
                return _devType3;
            }
            set
            {
                var t = value.Trim() == string.Empty ? "0" : value.Trim();
                if (_devCount == null)
                    _devCount = new int[4];
                _devCount.SetValue(int.Parse(t), 2);

                _devType3 = t;
            }
        }

        private string _devType4;
        public string DevType4
        {
            get
            {
                return _devType4;
            }
            set
            {
                var t = value.Trim() == string.Empty ? "0" : value.Trim();
                if (_devCount == null)
                    _devCount = new int[4];
                _devCount.SetValue(int.Parse(t), 3);

                _devType4 = t;
            }
        }

        public string[] DevTypeStatus { get; set; }

        public int DevYear { get; private set; }
        public int DevMonth { get; private set; }


        private int[] _devCount;
        public int[] DevCount
        {
            get
            {
                return _devCount;
            }
        }        

        private string _devYearAndMonth;
        public string DevYearAndMonth
        {
            get
            {
                return _devYearAndMonth;
            }
            set
            {
                _devYearAndMonth = value;
                DevYear = int.Parse(value.Substring(0, 4));
                DevMonth = int.Parse(value.Substring(4, 2));
            }
        }

        public string ImportStatus { get; set; }
    }
}
