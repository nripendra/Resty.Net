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
using System.Net;
using System.Text;

namespace Resty.Net
{
    using Configuration;

    public class RestResponse<T> : RestResponse
    {
        /// <summary>
        /// The instance of .net type (T) extracted by deserializing the response content.
        /// </summary>
        public T Data
        {
            get
            {
                string content = Body.ReadAsString();
                if (!string.IsNullOrWhiteSpace(content))
                {
                    var deserializer = RestConfiguration.DeSerializers.GetDeSerializer(ContentType);
                    return deserializer.DeSerialize<T>(content);
                }

                return default(T);
            }
        }

        public RestResponse(RestRequest request, HttpWebResponse webResponse, RestException responseError)
            : base(request, webResponse, responseError)
        {
        }
    }
}
