using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaUnicom.Fuyang.Framework.Adapter
{
    public static class TypeAdapterFactory
    {
        private static ITypeAdapterFactory _currentTypeAdapterFactory = null;

        public static void SetCurrent(ITypeAdapterFactory adapterFactory)
        {
            _currentTypeAdapterFactory = adapterFactory;
        }

        public static ITypeAdapter CreateAdapter()
        {
            return _currentTypeAdapterFactory.Create();
        }
    }
}
