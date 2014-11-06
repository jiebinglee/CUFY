using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Data;

namespace ChinaUnicom.Fuyang.Framework.Adapter
{
    public class AutomapperTypeAdapter : ITypeAdapter
    {
        public TTarget Adapt<TSource, TTarget>(TSource source)
            where TSource : class
            where TTarget : class,new()
        {
            return Mapper.Map<TSource, TTarget>(source);
        }

        public TTarget Adapt<TTarget>(object source)
            where TTarget : class, new()
        {
            if (source.GetType().Equals(typeof(DataTableReader)))
            {
                return Mapper.DynamicMap<IDataReader, TTarget>((IDataReader)source);
            }
            else
            {
                return Mapper.Map<TTarget>(source);
            }
        }
    }
}
