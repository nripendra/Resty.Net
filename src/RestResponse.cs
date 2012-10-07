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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resty.Net
{
    public class RestResponse : IDisposable
    {
        /// <summary>
        /// Gets or sets the WebResponse instance.
        /// </summary>
        protected HttpWebResponse WebResponse { get; set; }

        /// <summary>
        /// Gets the character set of the response.
        /// </summary>
        public string CharacterSet { get; private set; }

        /// <summary>
        /// Gets the method that is used to encode the body of the response.
        /// </summary>
        public string ContentEncoding { get; private set; }

        /// <summary>
        /// Gets the length of the content returned by the request.
        /// </summary>
        public long ContentLength { get; private set; }

        /// <summary>
        /// Gets the content type of the response.
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// Gets or sets the cookies that are associated with this response.
        /// </summary>
        public CookieCollection Cookies { get; private set; }

        public RestException Error { get; private set; }

        /// <summary>
        /// Gets the headers that are associated with this response from the server.
        /// </summary>
        public WebHeaderCollection Headers { get; private set; }

        public bool IsFromCache { get; private set; }

        /// <summary>
        /// Gets a System.Boolean value that indicates whether both client and server were authenticated.
        /// </summary>
        public bool IsMutuallyAuthenticated { get; private set; }

        /// <summary>
        /// Gets a System.Boolean value that indicates whether the Response Status Code indicates success.
        /// </summary>
        public bool IsSuccessStatusCode
        {
            get
            {
                int sc = (int)StatusCode;
                return sc >= 200 && sc < 300;
            }
        }

        /// <summary>
        /// Gets the last date and time that the contents of the response were modified.
        /// </summary>
        public DateTime LastModified { get; private set; }

        /// <summary>
        /// Gets the method that is used to return the response.
        /// </summary>
        public string Method { get; private set; }

        /// <summary>
        /// Gets the version of the HTTP protocol that is used in the response.
        /// </summary>
        public Version ProtocolVersion { get; private set; }

        /// <summary>
        /// Gets the instance of the Request for which server returned this response.
        /// </summary>
        public RestRequest Request { get; private set; }

        /// <summary>
        /// Gets the instace of <see cref="RestResponseBody"/>, which is responsible for accessing the response stream content.
        /// </summary>
        public RestResponseBody Body { get; private set; }

        /// <summary>
        /// Gets the URI of the Internet resource that responded to the request.
        /// </summary>
        public Uri ResponseUri { get; private set; }

        /// <summary>
        /// Gets the name of the server that sent the response.
        /// </summary>
        public string Server { get; private set; }

        /// <summary>
        /// Gets the status of the response.
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Gets the status description returned with the response.
        /// </summary>
        public string StatusDescription { get; private set; }


        public RestResponse(RestRequest request, HttpWebResponse webResponse, RestException responseError)
        {
            Request = request;
            WebResponse = webResponse;
            Error = responseError;

            if (WebResponse != null)
            {
                CharacterSet = WebResponse.CharacterSet;
                ContentEncoding = WebResponse.ContentEncoding;
                ContentLength = WebResponse.ContentLength;
                ContentType = WebResponse.ContentType;
                Cookies = WebResponse.Cookies;
                Headers = WebResponse.Headers;
                IsFromCache = WebResponse.IsFromCache;
                IsMutuallyAuthenticated = WebResponse.IsMutuallyAuthenticated;
                LastModified = WebResponse.LastModified;
                Method = WebResponse.Method;
                ProtocolVersion = WebResponse.ProtocolVersion;
                ResponseUri = WebResponse.ResponseUri;
                Server = WebResponse.Server;
                StatusCode = WebResponse.StatusCode;
                StatusDescription = WebResponse.StatusDescription;

                Body = new RestResponseBody(WebResponse.GetResponseStream(), CharacterSet);

                if (!IsSuccessStatusCode)
                {
                    Error = new RestException(StatusCode, StatusDescription, Body);
                }
            }
        }

        /// <summary>
        /// Gets the Cookie value of given name. Returns null if such name doesn't exist in Cookies collection.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual string GetCookieValue(string name)
        {
            var cookie = Cookies[name];

            if (cookie != null)
            {
                return cookie.Value;
            }

            return null;
        }

        /// <summary>
        /// Gets the response header value of given name.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string GetHeaderValue(string key)
        {
            if (Headers.AllKeys.Contains(key))
            {
                return Headers[key];
            }

            return null;
        }

        /// <summary>
        /// Ensure that IsSuccessStatusCode is true. If not then throw RestException.
        /// </summary>
        /// <returns>Instance of current RestResponse</returns>
        /// <exception cref="RestException"></exception>
        public virtual RestResponse EnsureSuccessStatusCode()
        {
            if (!IsSuccessStatusCode)
            {
                throw new RestException(StatusCode, StatusDescription, Body);
            }

            return this;
        }

        /// <summary>
        /// Dispose all the underlying disposable resources
        /// </summary>
        public virtual void Dispose()
        {
            if (Body != null)
            {
                Body.Dispose();
            }
        }

    }
}
