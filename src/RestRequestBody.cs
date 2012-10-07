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

namespace Resty.Net
{
    using Configuration;

    /// <summary>
    /// Abstract class representing the data that must be serialized into request stream, in order to send content to the server.
    /// </summary>
    public abstract class RestRequestBody
    {
        /// <summary>
        /// The content that must be written into request stream in order to send content to the server.
        /// </summary>
        public object Content { get; protected set; }

        /// <summary>
        /// Serialize the content into string using the serializer registerd for given content type.
        /// </summary>
        /// <param name="contentType">The content type (MIME type) into which the request body must be serialized.</param>
        /// <returns>String representation of the request body, formatted into the provided content type.</returns>
        public virtual string ToString(ContentType contentType)
        {
            var serializer = RestConfiguration.Serializers.GetSerializer(contentType.ToString());
            return serializer.Serialize(this);
        }

        /// <summary>
        /// Converts the request body into array of bytes(byte[]).
        /// </summary>
        /// <param name="contentType">The content type (MIME type) into which the request body must be serialized.</param>
        /// <returns>Binary representation of request body that can directly be written into the request stream.</returns>
        public virtual byte[] ToByteArray(ContentType contentType)
        {
            //c# strings are UTF8 encoded
            return Encoding.UTF8.GetBytes(ToString(contentType));
        }
    }
}
