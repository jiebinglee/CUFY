using ChinaUnicom.Fuyang.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.CreditManagement.Models
{
    public class ChannelDictionary : Entity
    {
        public ChannelDictionary()
        {
        }

        public string DictionaryTable { get; set; }
        public int DictionaryKey { get; set; }
        public string DictionaryValue { get; set; }
    }
}
