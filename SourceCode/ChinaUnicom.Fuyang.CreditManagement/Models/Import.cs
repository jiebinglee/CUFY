using ChinaUnicom.Fuyang.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.CreditManagement.Models
{
    public class Import : Entity
    {
        public Import()
        { 
        }

        public int ImportYear { get; set; }
        public int ImportMonth { get; set; }
        public DateTime ImportDate { get; set; }
        public int OperatorId { get; set; }
        public byte[] ImportContent { get; set; }
        public int DataType { get; set; }
    }
}
