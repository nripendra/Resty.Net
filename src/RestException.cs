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
    /// <summary>
    /// REST Exception as retured by server. Any condition that is not a success would be termed as exception.
    /// </summary>
    public class RestException : ApplicationException
    {
        /// <summary>
        /// Gets the HttpStatusCode returned by server while making REST request.
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Gets the content of ResponseStream as returned by the server.
        /// </summary>
        public RestResponseBody Body { get; private set; }

        public RestException(HttpStatusCode statusCode, string statusDescription, RestResponseBody body, Exception innerException)
            : base(statusDescription, innerException)
        {
            StatusCode = StatusCode;
            Body = body;
        }

        public RestException(HttpStatusCode statusCode, string statusDescription, RestResponseBody body)
            : this(statusCode, statusDescription, body, null)
        {
        }
    }
}
