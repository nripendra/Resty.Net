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
    public class HttpMethod
    {
        private string _httpMethod;


        public static HttpMethod ACL { get { return new HttpMethod("ACL"); } }

        public static HttpMethod CONNECT { get { return new HttpMethod("CONNECT"); } }

        public static HttpMethod DELETE { get { return new HttpMethod("DELETE"); } }

        public static HttpMethod GET { get { return new HttpMethod("GET"); } }

        public static HttpMethod HEAD { get { return new HttpMethod("HEAD"); } }

        public static HttpMethod OPTIONS { get { return new HttpMethod("OPTIONS"); } }

        public static HttpMethod PATCH { get { return new HttpMethod("PATCH"); } }

        public static HttpMethod POST { get { return new HttpMethod("POST"); } }

        public static HttpMethod PUT { get { return new HttpMethod("PUT"); } }

        public static HttpMethod PROPPATCH { get { return new HttpMethod("PROPPATCH"); } }

        public static HttpMethod TRACE { get { return new HttpMethod("TRACE"); } }

        public static HttpMethod ORDERPATCH { get { return new HttpMethod("ORDERPATCH"); } }

        public static HttpMethod SEARCH { get { return new HttpMethod("SEARCH"); } }

        private HttpMethod(string httpMethod)
        {
            if (httpMethod == null)
                throw new ArgumentNullException();

            if (string.IsNullOrWhiteSpace(httpMethod))
                throw new ArgumentException();

            _httpMethod = httpMethod;
        }

        /// <summary>
        /// Create a custom http method.
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <returns></returns>
        public static HttpMethod Create(string httpMethod)
        {
            return new HttpMethod(httpMethod);
        }

        /// <summary>
        /// Returns a System.String that represents the current HttpMethod.
        /// </summary>
        /// <returns>A System.String that represents the current HttpMethod.</returns>
        public override string ToString()
        {
            return _httpMethod;
        }
    }
}
