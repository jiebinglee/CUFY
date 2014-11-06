using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ChinaUnicom.Fuyang.Framework
{
    public interface IModuleManager
    {
        TransBox ProcessTask(TransBox data, string user);
    }
}
