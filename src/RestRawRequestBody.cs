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
    /// <summary>
    /// Class representing the raw binary data to be sent to the server.
    /// The raw data is directly written into the request stream, without any formatting.
    /// </summary>
    public class RestRawRequestBody : RestRequestBody
    {
        public RestRawRequestBody(byte[] content)
        {
            Content = content;
        }

        public override string ToString(ContentType contentType)
        {
            //A raw binary data can be represented into ASCII string in most of the system.
            return Encoding.ASCII.GetString((byte[])Content);
        }

        public override byte[] ToByteArray(ContentType contentType)
        {
            //Plain text body should already be in the string serialized form.
            //So, default logic to convert to string needs to be overriden.
            return (byte[])Content;
        }
    }
}
