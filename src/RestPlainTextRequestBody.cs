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
    /// Class representing the plain text data to be sent to the server.
    /// The plain text data can be hand formatted into JSON fromat or name-value format, 
    /// or any other suitable format. No automatic serialization occurs.
    /// </summary>
    public class RestPlainTextRequestBody : RestRequestBody
    {
        public RestPlainTextRequestBody(string content)
        {
            Content = content;
        }

        /// <summary>
        /// Returns the content of request body as is. Ignores the ContentType parameter.
        /// The request body must be pre formatted into desired content type by the user of this class.
        /// </summary>
        /// <param name="contentType">Ignored</param>
        /// <returns></returns>
        public override string ToString(ContentType contentType)
        {
            //Plain text body should already be in the string serialized form.
            //So, default logic to convert to string needs to be overriden.
            return (string)Content;
        }
    }
}
