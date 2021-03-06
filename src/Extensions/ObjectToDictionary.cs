﻿//-------------------------------------------------------------------------------
// <copyright>
//    Copyright (c) 2012 Nripendra Nath Newa (nripendra@uba-solutions.com).
//    Licensed under the MIT License (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.opensource.org/licenses/mit-license.php
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Nripendra Nath Newa (nripendra@uba-solutions.com).</author>
//-------------------------------------------------------------------------------

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
