using System;
using System.Collections.Generic;
using ChinaUnicom.Fuyang.Framework.Data;
using ChinaUnicom.Fuyang.Core.Users.Models;

namespace ChinaUnicom.Fuyang.Core.Utility.Models
{
    public class BaseForm : Entity
    {
        //public decimal WF_BSNS_FRM_ID { get; set; }
        public decimal FormTypeId { get; set; }
        public string FormNo { get; set; }
        public decimal RequestorId { get; set; }
        public DateTime RequestDate { get; set; }
        public short RequestStatus { get; set; }
        public decimal? PrevApproval { get; set; }
        public decimal? NextApproval { get; set; }
        public DateTime? CompleteDate { get; set; }

        public virtual FormType FormType { get; set; }
        public virtual User User { get; set; }        
    }
}
