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
    /// The Registry that holds a mapping for content type and the associated serializer.
    /// The serializer helps converting the RestRequestBody into their string representation.
    /// </summary>
    public class RestSerializerRegistry
    {
        private Dictionary<string, IRestRequestBodySerializer> _serializerMap = new Dictionary<string, IRestRequestBodySerializer>();

        /// <summary>
        /// Registers a serializer for given content type. If the content type already has serializer associated, then overrides the old value.
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="serializer"></param>
        public void Register(string contentType, IRestRequestBodySerializer serializer)
        {
            if (!_serializerMap.ContainsKey(contentType))
            {
                _serializerMap.Add(contentType, serializer);
            }
            else
            {
                _serializerMap[contentType] = serializer;
            }
        }

        /// <summary>
        /// Retrieves the serializer registered using Register() method.
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public IRestRequestBodySerializer GetSerializer(string contentType)
        {
            if (_serializerMap.ContainsKey(contentType))
                return _serializerMap[contentType];
            else
                return null;
        }
    }
}
