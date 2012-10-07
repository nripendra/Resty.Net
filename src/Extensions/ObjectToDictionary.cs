using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Resty.Net.Extensions
{
    public static class ObjectToDictionary
    {
        public static IDictionary<string, object> ToDictionary(this object input)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            var attr = BindingFlags.Public | BindingFlags.Instance;

            foreach (var property in input.GetType().GetProperties(attr))
            {
                if (property.CanRead)
                {
                    object value = property.GetValue(input, null);
                    dict.Add(property.Name, value);
                }
            }

            return dict;
        }
    }
}
