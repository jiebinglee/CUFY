using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaUnicom.Fuyang.Framework.Data
{
    [Serializable]
    public class Entity
    {
        int _id;
        int _flag;

        protected Entity()
        { 
        }

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual int Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }
    }
}
