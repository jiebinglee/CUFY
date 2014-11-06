using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaUnicom.Fuyang.Framework.Adapter
{
    public interface ITypeAdapterFactory
    {
        ITypeAdapter Create();
    }
}
