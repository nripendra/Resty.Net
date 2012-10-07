//-------------------------------------------------------------------------------
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
using System.Text;

namespace Resty.Net.Serialization
{
    /// <summary>
    /// The Registry that holds a mapping for content type and the associated deserializer.
    /// The deserializer helps converting the string representation returned by server into .net types.
    /// </summary>
    public class RestDeSerializerRegistry
    {
        private Dictionary<string, IResponseDeSerializer> _serializerMap = new Dictionary<string, IResponseDeSerializer>();

        /// <summary>
        /// Registers a deserializer for given content type. If the content type already has deserializer associated, then overrides the old value.
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="deserializer"></param>
        public void Register(string contentType, IResponseDeSerializer deserializer)
        {
            if (!_serializerMap.ContainsKey(contentType))
            {
                _serializerMap.Add(contentType, deserializer);
            }
            else
            {
                _serializerMap[contentType] = deserializer;
            }
        }

        /// <summary>
        /// Retrieves the deserializer registered using Register() method.
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public IResponseDeSerializer GetDeSerializer(string contentType)
        {
            if (contentType.IndexOf(';') > -1)
            {
                contentType = contentType.Substring(0, contentType.IndexOf(';'));
            }

            if (_serializerMap.ContainsKey(contentType))
                return _serializerMap[contentType];
            else
                return null;
        }
    }
}
