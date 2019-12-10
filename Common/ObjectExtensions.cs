using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class ObjectExtensions
    {
        public static T ConvertTo<T>(this object obj)
        {
            if (obj is T)
            {
                return (T)obj;
            }
            else
            {
                throw new ArgumentException("传入对象不能转换成指定类型T");
            }
        }
    }
}
