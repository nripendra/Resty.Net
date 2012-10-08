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
using System.Net.Security;
using System.Text;

namespace Resty.Net.Extras
{
    public class RestRequestBuilder
    {
        private RestRequest _request;

        public RestRequestBuilder(HttpMethod method, RestUri uri)
        {
            _request = new RestRequest(method, uri);
        }

        public static RestRequestBuilder CreateRequest(HttpMethod method, RestUri uri)
        {
            return new RestRequestBuilder(method, uri);
        }

        public RestRequestBuilder WithHeader(string headerName, string headerValue)
        {
            _request.AddHeader(headerName, headerValue);
            return this;
        }

        public RestRequestBuilder WithHeader(HttpRequestHeader headerName, string headerValue)
        {
            _request.AddHeader(headerName, headerValue);
            return this;
        }

        public RestRequestBuilder WithHeader(object headerParams)
        {
            _request.AddHeader(headerParams);
            return this;
        }

        public RestRequestBuilder WithCookie(string cookieName, string cookieValue)
        {
            _request.AddCookie(cookieName, cookieValue);
            return this;
        }

        public RestRequestBuilder WithCookie(Cookie cookie)
        {
            _request.AddCookie(cookie);
            return this;
        }

        public RestRequestBuilder WithBody(RestRequestBody body)
        {
            _request.Body = body;
            return this;
        }

        public RestRequestBuilder WithAutoRedirect()
        {
            _request.AllowAutoRedirect = true;
            return this;
        }

        public RestRequestBuilder WithAuthenticationLevel(AuthenticationLevel authenticationLevel)
        {
            _request.AuthenticationLevel = authenticationLevel;
            return this;
        }

        public RestRequestBuilder WithAutomaticDecompression(DecompressionMethods automaticDecompression)
        {
            _request.AutomaticDecompression = automaticDecompression;
            return this;
        }

        public RestRequestBuilder WithCachePolicy(System.Net.Cache.RequestCachePolicy cachePolicy)
        {
            _request.CachePolicy = cachePolicy;
            return this;
        }

        public RestRequestBuilder WithClientCertificates(System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates)
        {
            _request.ClientCertificates = clientCertificates;
            return this;
        }

        public RestRequestBuilder AddClientCertificate(System.Security.Cryptography.X509Certificates.X509Certificate clientCertificate)
        {
            _request.ClientCertificates.Add(clientCertificate);
            return this;
        }

        public RestRequestBuilder WithContentType(ContentType contentType)
        {
            _request.ContentType = contentType;
            return this;
        }

        public RestRequestBuilder WithCredentials(ICredentials credentials)
        {
            _request.Credentials = credentials;
            return this;
        }

        public RestRequestBuilder KeepAlive()
        {
            _request.KeepAlive = true;
            return this;
        }

        public RestRequestBuilder WithMaximumAutomaticRedirections(int value)
        {
            _request.MaximumAutomaticRedirections = value;
            return this;
        }

        public RestRequestBuilder WithReferer(string referer)
        {
            _request.Referer = referer;
            return this;
        }

        public RestRequestBuilder WithTimeOut(TimeSpan timeout)
        {
            _request.TimeOut = timeout;
            return this;
        }

        public RestRequestBuilder TcpKeepAlive()
        {
            _request.TcpKeepAlive = true;
            return this;
        }

        public RestRequestBuilder WithTcpKeepAliveTimeOut(TimeSpan tcpKeepAliveTimeOut)
        {
            _request.TcpKeepAliveTimeOut = tcpKeepAliveTimeOut;
            return this;
        }

        public RestRequestBuilder WithUserAgent(string userAgent)
        {
            _request.UserAgent = userAgent;
            return this;
        }

        public static implicit operator RestRequest(RestRequestBuilder builder)
        {
            return builder.ToRestRequest();
        }

        public virtual RestRequest ToRestRequest()
        {
            return _request;
        }
    }
}
