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
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Resty.Net
{
    using Extensions;

    public class RestRequest
    {
        protected HttpWebRequest HttpWebRequest { get; set; }
        protected System.Threading.CancellationTokenSource CancellationTokenSource { get; set; }
        protected System.Threading.CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// Gets or sets the RestUri of the Request. RestUri with resource Uri combines to become the full Uri pointing to the resource.
        /// </summary>
        public RestUri RestUri { get; protected set; }

        /// <summary>
        /// Gets or sets the collection of header name/value pairs associated with the request.
        /// </summary>
        public WebHeaderCollection Headers { get; protected set; }

        /// <summary>
        /// Gets or sets the value to be sent to server as part of Request Stream.
        /// </summary>
        public RestRequestBody Body { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the request should follow redirection responses.
        /// </summary>
        public bool AllowAutoRedirect { get; set; }

        /// <summary>
        ///  Gets or sets values indicating the level of authentication and impersonation used for this request.
        /// </summary>
        public AuthenticationLevel AuthenticationLevel { get; set; }

        /// <summary>
        /// Gets or sets the type of decompression that is used.
        /// </summary>
        public DecompressionMethods AutomaticDecompression { get; set; }

        /// <summary>
        /// Gets or sets the cache policy for this request.
        /// </summary>
        public System.Net.Cache.RequestCachePolicy CachePolicy { get; set; }

        /// <summary>
        /// Gets or sets the collection of security certificates that are associated with this request.
        /// </summary>
        public System.Security.Cryptography.X509Certificates.X509CertificateCollection ClientCertificates { get; set; }

        /// <summary>
        /// Gets or sets the value indicating the content-type (MIME type) of request body.
        /// </summary>
        public ContentType ContentType { get; set; }

        /// <summary>
        /// Gets or sets the cookies associated with the request.
        /// </summary>
        public CookieContainer CookieContainer { get; set; }

        /// <summary>
        ///  Gets or sets authentication information for the request.
        /// </summary>
        public ICredentials Credentials { get; set; }

        /// <summary>
        ///  Gets HttpMethod header for the request.
        /// </summary>
        public HttpMethod Method { get; protected set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to make a persistent connection to the Internet resource.
        /// </summary>
        public bool KeepAlive { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of redirects that the request follows.
        /// </summary>
        public int MaximumAutomaticRedirections { get; set; }

        /// <summary>
        /// Gets or sets the value of the Referer HTTP header.
        /// </summary>
        public string Referer { get; set; }

        /// <summary>
        /// Gets or sets the time-out value in milliseconds for the GetResponse() method.
        /// </summary>
        public TimeSpan TimeOut { get; set; }

        /// <summary>
        /// Gets or sets TCP keep-alive option. If set to true, then the TCP keep-alive option on a TCP connection will be enabled. Default is false.
        /// </summary>
        public bool TcpKeepAlive { get; set; }

        /// <summary>
        /// Gets or sets the timeout period, with no activity until the first keep-alive packet is sent. The value must be greater than 0. 
        /// If a value of less than or equal to zero is passed an System.ArgumentOutOfRangeException is thrown.
        /// Default value is 2 hours.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public TimeSpan TcpKeepAliveTimeOut { get; set; }

        /// <summary>
        /// Gets or sets the value of the User-agent HTTP header.
        /// </summary>
        public string UserAgent { get; set; }

        public RestRequest(HttpMethod method, RestUri uri)
        {
            RestUri = uri;
            Headers = new WebHeaderCollection();

            AllowAutoRedirect = true;
            AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
            AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            CookieContainer = new System.Net.CookieContainer();
            ContentType = ContentType.ApplicationX_WWW_Form_UrlEncoded;
            MaximumAutomaticRedirections = 50;
            TimeOut = TimeSpan.FromMinutes(6);
            TcpKeepAliveTimeOut = TimeSpan.FromHours(2);
            UserAgent = "Resty.Net-RestRequest";
            Method = method;
        }

        /// <summary>
        /// Abort the http request. Note that the request may still reach to server and be processed on the server but the response from server is not processed.
        /// <para>N.B:</para>
        /// <para>- Abort Works only on async operations.</para>
        /// <para>- Abort should be called after GetResponseAsync() method or it's overloads. Calling it before causes exception.</para> 
        /// </summary>
        public virtual void Abort()
        {
            if (HttpWebRequest != null)
            {
                HttpWebRequest.Abort();
            }

            if (CancellationTokenSource != null)
            {
                CancellationTokenSource.Cancel();
            }
        }

        /// <summary>
        /// Inserts specified header with the specified value into Headers collection.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddHeader(HttpRequestHeader name, string value)
        {
            Headers.Add(name, value);
        }

        /// <summary>
        /// Inserts specified header with the specified value into Headers collection.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddHeader(string name, string value)
        {
            Headers.Add(name, value);
        }

        /// <summary>
        /// Inserts specified headers with the respective values into Headers collection.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddHeader(object headers)
        {
            var dictionary = headers.ToDictionary();

            foreach (var property in dictionary)
            {
                string value = property.Value.ToString();
                HttpRequestHeader key;
                if (Enum.TryParse(property.Key, true, out key))
                {
                    AddHeader(key, value);
                }
                else
                {
                    AddHeader(property.Key, value);
                }
            }
        }

        /// <summary>
        /// Add a cookie to be sent to server.
        /// </summary>
        /// <param name="cookie"></param>
        public void AddCookie(Cookie cookie)
        {
            if (this.CookieContainer != null)
            {
                if (cookie != null)
                {
                    this.CookieContainer.Add(cookie);
                }
            }
        }

        /// <summary>
        /// Add a cookie to be sent to server.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddCookie(string name, string value)
        {
            this.AddCookie(new Cookie(name, value));
        }

        /// <summary>
        /// Returns a response from a REST resource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpMethod"></param>
        /// <param name="resource"></param>
        /// <returns>RestResponse<T></returns>
        public virtual RestResponse<T> GetResponse<T>()
        {
            RestResponse<T> restResponse = (RestResponse<T>)GetResponseInternal(typeof(T));
            return restResponse;
        }

        /// <summary>
        /// Returns a response from a REST resource
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="resource"></param>
        /// <returns>RestResponse</returns>
        public virtual RestResponse GetResponse()
        {
            RestResponse restResponse = GetResponseInternal(null);
            return restResponse;
        }

        /// <summary>
        /// Returns a response from a REST resource, in async fashion
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="resource"></param>
        /// <returns>Task<RestResponse></returns>
        public virtual Task<RestResponse> GetResponseAsync()
        {
            return GetResponseInternalAsync(null).ContinueWith(t =>
            {
                if (t.IsCanceled)
                {
                    return new RestResponse(this, null, new RestException(0, "Request canceled", null));
                }
                return t.Result;
            });
        }

        /// <summary>
        /// Returns a response from a REST resource, in async fashion
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpMethod"></param>
        /// <param name="resource"></param>
        /// <returns>Task<RestResponse<T>></returns>
        public virtual Task<RestResponse<T>> GetResponseAsync<T>()
        {
            return GetResponseInternalAsync(typeof(T)).ContinueWith(t =>
            {
                if (t.IsCanceled)
                {
                    return new RestResponse<T>(this, null, new RestException(0, "Request canceled", null));
                }
                return (RestResponse<T>)t.Result;
            });
        }

        /// <summary>
        /// Actual logic for actually making synchronous webrequest and retrieving the response for the server.
        /// </summary>
        /// <param name="typeArg"></param>
        /// <param name="httpMethod"></param>
        /// <param name="resource"></param>
        /// <returns>RestResponse</returns>
        protected virtual RestResponse GetResponseInternal(Type typeArg)
        {
            HttpWebRequest = CreateHttpWebRequest();
            WriteRequestStream(HttpWebRequest);

            HttpWebResponse webResponse = null;
            RestException responseError = null;
            try
            {
                webResponse = (HttpWebResponse)HttpWebRequest.GetResponse();
            }
            catch(Exception ex)
            {
                responseError = new RestException(0, "An exception has occured, get more detail in inner-exception", null, ex);
            }

            return CreateResponse(typeArg, webResponse, responseError);
        }

        /// <summary>
        /// Actual logic for actually making asynchronous webrequest and retrieving the response for the server.
        /// </summary>
        /// <param name="typeArg"></param>
        /// <param name="httpMethod"></param>
        /// <param name="resource"></param>
        /// <returns>A task of RestResponse.</returns>
        protected virtual Task<RestResponse> GetResponseInternalAsync(Type typeArg)
        {
            HttpWebRequest = CreateHttpWebRequest();
            WriteRequestStream(HttpWebRequest);

            CancellationTokenSource = new System.Threading.CancellationTokenSource();
            CancellationToken = CancellationTokenSource.Token;

            Task<WebResponse> result = Task.Factory.FromAsync(HttpWebRequest.BeginGetResponse, asyncResult => HttpWebRequest.EndGetResponse(asyncResult), (object)HttpWebRequest);
            ThreadPool.RegisterWaitForSingleObject((result as IAsyncResult).AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), HttpWebRequest, TimeOut, true);

            return result.ContinueWith((task) =>
            {
                HttpWebResponse webResponse = null;
                RestException responseError = null;

                if (task.IsFaulted)
                {
                    responseError = new RestException(0, "An exception has occured, get more detail in inner-exception", null, task.Exception.Flatten());
                }
                else if (task.IsCanceled)
                {
                    responseError = new RestException(0, "Request Canceled", null);
                }
                else
                {
                    webResponse = (HttpWebResponse)task.Result;
                }

                return CreateResponse(typeArg, webResponse, responseError);
            }, CancellationToken);
        }

        /// <summary>
        /// Write the Body into the Request stream, to send the body to server.
        /// </summary>
        /// <param name="webRequest"></param>
        protected virtual void WriteRequestStream(HttpWebRequest webRequest)
        {
            if (Body != null)
            {
                byte[] byteArray = Body.ToByteArray(ContentType);

                webRequest.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = webRequest.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();

                dataStream = null;
                byteArray = null;
            }
        }

        /// <summary>
        /// Factory method for creating the RestResponse
        /// </summary>
        /// <param name="typeArg"></param>
        /// <param name="webResponse"></param>
        /// <param name="responseError"></param>
        /// <returns>RestResponse</returns>
        protected virtual RestResponse CreateResponse(Type typeArg, HttpWebResponse webResponse, RestException responseError)
        {
            RestResponse restResponse = null;

            OnWebResponseReceived(webResponse);

            if (typeArg != null)
            {
                Type restResponseType = typeof(RestResponse<>);
                Type genericType = restResponseType.MakeGenericType(new Type[] { typeArg });
                restResponse = (RestResponse)Activator.CreateInstance(genericType, this, webResponse, responseError);
            }
            else
            {
                restResponse = new RestResponse(this, webResponse, responseError);
            }

            return restResponse;
        }

        /// <summary>
        /// Factory for creating the HttpWebRequest.
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="resource"></param>
        /// <returns>HttpWebRequest</returns>
        protected virtual HttpWebRequest CreateHttpWebRequest()
        {
            var webRequest = (HttpWebRequest)System.Net.WebRequest.Create(RestUri.ToString());

            webRequest.AllowAutoRedirect = AllowAutoRedirect;
            webRequest.AuthenticationLevel = AuthenticationLevel;
            webRequest.AutomaticDecompression = AutomaticDecompression;
            webRequest.ContentType = ContentType.ToString();
            webRequest.CachePolicy = CachePolicy;
            webRequest.CookieContainer = CookieContainer;
            webRequest.Credentials = Credentials;
            webRequest.KeepAlive = KeepAlive;
            webRequest.MaximumAutomaticRedirections = MaximumAutomaticRedirections;
            webRequest.Method = Method.ToString();
            webRequest.Referer = Referer;
            webRequest.UserAgent = UserAgent;

            foreach (var cert in ClientCertificates)
            {
                webRequest.ClientCertificates.Add(cert);
            }

            if (TimeOut != TimeSpan.MinValue && TimeOut > TimeSpan.FromSeconds(0))
            {
                webRequest.ReadWriteTimeout = (int)TimeOut.TotalMilliseconds;
                webRequest.Timeout = (int)TimeOut.TotalMilliseconds;
            }

            if (TcpKeepAlive)
            {
                webRequest.ServicePoint.SetTcpKeepAlive(true, (int)TcpKeepAliveTimeOut.TotalMilliseconds, 1000);
            }

            foreach (var key in Headers.AllKeys)
            {
                if (WebHeaderCollection.IsRestricted(key))
                {
                    SetRestrictedHeaderProperty(webRequest, key, Headers[key]);
                }
                else
                {
                    webRequest.Headers.Add(key, Headers[key]);
                }
            }

            OnWebRequestCreated(webRequest);

            return webRequest;
        }

        /// <summary>
        /// Set properties for restricted headers like accept, connection etc.
        /// </summary>
        /// <param name="webRequest"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        protected virtual void SetRestrictedHeaderProperty(HttpWebRequest webRequest, string propertyName, string value)
        {
            switch (propertyName.ToLower())
            {
                case "accept":
                    webRequest.Accept = value;
                    break;
                case "connection":
                    webRequest.Connection = value;
                    break;
                case "content-length":
                    webRequest.ContentLength = long.Parse(value);
                    break;
                case "content-type":
                    webRequest.ContentType = value;
                    break;
                case "date":
                    webRequest.Date = DateTime.Parse(value);
                    break;
                case "expect":
                    webRequest.Expect = value;
                    break;
                case "host":
                    webRequest.Host = value;
                    break;
                case "if-modified-since":
                    webRequest.IfModifiedSince = DateTime.Parse(value);
                    break;
                case "referer":
                    webRequest.Referer = value;
                    break;
                case "transfer-encoding":
                    webRequest.TransferEncoding = value;
                    break;
                case "user-agent":
                    webRequest.UserAgent = value;
                    break;
            }
        }

        /// <summary>
        /// Needs to be overidden by sub class if one needs to get access over the underlying WebRequest instance
        /// </summary>
        /// <param name="request"></param>
        protected virtual void OnWebRequestCreated(WebRequest request)
        {
        }

        /// <summary>
        /// Needs to be overidden by sub class if one needs to get access over the underlying WebResponse instance
        /// </summary>
        /// <param name="request"></param>
        protected virtual void OnWebResponseReceived(WebResponse response)
        {
        }

        // Abort the request if the timer fires. 
        private void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                if (CancellationTokenSource != null)
                {
                    CancellationTokenSource.Cancel();
                }

                HttpWebRequest request = state as HttpWebRequest;
                if (request != null)
                {
                    request.Abort();
                }
            }
        }
    }
}
