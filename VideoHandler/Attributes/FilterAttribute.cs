using VideoHandler.SearchFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VideoHandler.Attributes
{
    class FilterAttribute : Attribute
    {
        public FilterType Enum { get; set; }

        public FilterAttribute(FilterType filterType)
        {
            Enum = filterType;
        }

        internal static IFilter GetFilter(IEnumerable<IFilter> filters, FilterType filter)
        {

            foreach (var f in filters)
            {
                var type = f.GetType();
                var attrib = type.GetCustomAttribute<FilterAttribute>();
                var enumVal = attrib.Enum;
            }
            var result = filters.FirstOrDefault(x => x.GetType().GetCustomAttribute<FilterAttribute>().Enum == filter);


            return result;
        }
    }
}
