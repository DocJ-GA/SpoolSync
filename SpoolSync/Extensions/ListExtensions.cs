using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpoolSync.Extensions
{
    public static class ListExtensions
    {
        public static List<T> CAddRange<T>(this List<T> list, IEnumerable<T>? items)
        {
            if (items == null)
                return list;

            foreach (var item in items)
                list.Add(item);

            return list;
        }
    }
}
