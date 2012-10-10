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
using System.Threading.Tasks;

namespace Resty.Net.Extras
{
    using System.Net;
    using Extensions;

    public class RestInvoker
    {
        private Uri _baseUrl;
        private CookieCollection _cookies;
        private IDictionary<object, object> _headers;

        public RestInvoker()
        {
            _cookies = new CookieCollection() ;
            _headers = new Dictionary<object, object>();
        }

        public RestInvoker(string baseUrl)
            : this()
        {
            _baseUrl = new Uri(baseUrl, UriKind.Absolute);
        }

        public void AddCookie(string name, string value)
        {
            _cookies.Add(new Cookie(name, value));
        }

        public void AddCookie(Cookie cookie)
        {
            _cookies.Add(cookie);
        }

        public void AddHeader(string name, string value)
        {
            _headers.Add(name, value);
        }

        public void AddHeader(HttpRequestHeader name, string value)
        {
            _headers.Add(name, value);
        }

        public Task<RestResponse> InvokeAsync(RestRequest request)
        {
            return request.GetResponseAsync();
        }

        public RestResponse Invoke(RestRequest request)
        {
            return request.GetResponse();
        }

        public Task<RestResponse<T>> InvokeAsync<T>(RestRequest request)
        {
            return request.GetResponseAsync<T>();
        }

        public RestResponse<T> Invoke<T>(RestRequest request)
        {
            return request.GetResponse<T>();
        }

        public Task<RestResponse> GetAsync(RestUri uri)
        {
            return InvokeAsync(CreateRestRequest(HttpMethod.GET, uri));
        }

        public Task<RestResponse> GetAsync(string resourceUri)
        {
            return GetAsync(CreateRestUri(_baseUrl, resourceUri, new { }));
        }

        public Task<RestResponse> GetAsync(string resourceUri, object parameters)
        {
            return GetAsync(CreateRestUri(_baseUrl, resourceUri, parameters));
        }

        public RestResponse Get(string resourceUri)
        {
            return GetAsync(resourceUri).Result;
        }

        public RestResponse Get(string resourceUri, object parameters)
        {
            return GetAsync(resourceUri, parameters).Result;
        }

        public RestResponse Get(RestUri uri)
        {
            return GetAsync(uri).Result;
        }

        public Task<RestResponse> PostAsync(RestUri uri, RestRequestBody requestBody, ContentType contentType)//1
        {
            RestRequest request = CreateRestRequest(HttpMethod.POST, uri);
            request.ContentType = contentType;
            request.Body = requestBody;
            return InvokeAsync(request);
        }

        public Task<RestResponse> PostAsync(RestUri uri, RestRequestBody requestBody)//2
        {
            return PostAsync(uri, requestBody, ContentType.ApplicationX_WWW_Form_UrlEncoded);
        }

        public Task<RestResponse> PostAsync(string resourceUri, object parameters, RestRequestBody body, ContentType contentType)//3
        {
            return PostAsync(CreateRestUri(_baseUrl, resourceUri, parameters), body, contentType);
        }

        public Task<RestResponse> PostAsync(string resourceUri, object parameters, RestRequestBody body)//4
        {
            return PostAsync(CreateRestUri(_baseUrl, resourceUri, parameters), body, ContentType.ApplicationX_WWW_Form_UrlEncoded);
        }

        public Task<RestResponse> PostAsync(string resourceUri, RestRequestBody body)//5
        {
            return PostAsync(CreateRestUri(_baseUrl, resourceUri, new { }), body, ContentType.ApplicationX_WWW_Form_UrlEncoded);
        }

        public RestResponse Post(RestUri uri, RestRequestBody requestBody, ContentType contentType)
        {
            return PostAsync(uri, requestBody, contentType).Result;
        }

        public RestResponse Post(RestUri uri, RestRequestBody requestBody)
        {
            return PostAsync(uri, requestBody).Result;
        }

        public RestResponse Post(string resourceUri, object parameters, RestRequestBody body, ContentType contentType)
        {
            return PostAsync(resourceUri, parameters, body, contentType).Result;
        }

        public RestResponse Post(string resourceUri, RestRequestBody body)
        {
            return PostAsync(resourceUri, body).Result;
        }

        public RestResponse Post(string resourceUri, object parameters, RestRequestBody body)
        {
            return PostAsync(resourceUri, parameters, body).Result;
        }

        public Task<RestResponse> PutAsync(RestUri uri, RestRequestBody requestBody, ContentType contentType)//1
        {
            RestRequest request = CreateRestRequest(HttpMethod.PUT, uri);
            request.Body = requestBody;
            request.ContentType = contentType;
            return InvokeAsync(request);
        }

        public Task<RestResponse> PutAsync(RestUri uri, RestRequestBody requestBody)//2
        {
            return PutAsync(uri, requestBody, ContentType.ApplicationX_WWW_Form_UrlEncoded);
        }

        public Task<RestResponse> PutAsync(string resourceUri, object parameters, RestRequestBody body, ContentType contentType)//3
        {
            return PutAsync(CreateRestUri(_baseUrl, resourceUri, parameters), body, contentType);
        }

        public Task<RestResponse> PutAsync(string resourceUri, object parameters, RestRequestBody body)//4
        {
            return PutAsync(CreateRestUri(_baseUrl, resourceUri, parameters), body, ContentType.ApplicationX_WWW_Form_UrlEncoded);
        }

        public Task<RestResponse> PutAsync(string resourceUri, RestRequestBody body)//5
        {
            return PutAsync(CreateRestUri(_baseUrl, resourceUri, new { }), body, ContentType.ApplicationX_WWW_Form_UrlEncoded);
        }

        public RestResponse Put(RestUri uri, RestRequestBody requestBody, ContentType contentType)
        {
            return PutAsync(uri, requestBody, contentType).Result;
        }

        public RestResponse Put(RestUri uri, RestRequestBody requestBody)
        {
            return PutAsync(uri, requestBody).Result;
        }

        public RestResponse Put(string resourceUri, object parameters, RestRequestBody body, ContentType contentType)
        {
            return PutAsync(resourceUri, parameters, body, contentType).Result;
        }

        public RestResponse Put(string resourceUri, RestRequestBody body)
        {
            return PutAsync(resourceUri, body).Result;
        }

        public RestResponse Put(string resourceUri, object parameters, RestRequestBody body)
        {
            return PutAsync(resourceUri, parameters, body).Result;
        }

        public Task<RestResponse> PatchAsync(RestUri uri, RestRequestBody requestBody, ContentType contentType)
        {
            RestRequest request = CreateRestRequest(HttpMethod.PATCH, uri);
            request.Body = requestBody;
            request.ContentType = contentType;
            return InvokeAsync(request);
        }

        public Task<RestResponse> PatchAsync(RestUri uri, RestRequestBody requestBody)
        {
            return PatchAsync(uri, requestBody, ContentType.ApplicationX_WWW_Form_UrlEncoded);
        }

        public Task<RestResponse> PatchAsync(string resourceUri, object parameters, RestRequestBody body, ContentType contentType)
        {
            return PatchAsync(CreateRestUri(_baseUrl, resourceUri, parameters), body, contentType);
        }

        public Task<RestResponse> PatchAsync(string resourceUri, RestRequestBody body)
        {
            return PatchAsync(CreateRestUri(_baseUrl, resourceUri, new { }), body, ContentType.ApplicationX_WWW_Form_UrlEncoded);
        }

        public Task<RestResponse> PatchAsync(string resourceUri, object parameters, RestRequestBody body)
        {
            return PatchAsync(CreateRestUri(_baseUrl, resourceUri, parameters), body, ContentType.ApplicationX_WWW_Form_UrlEncoded);
        }

        public RestResponse Patch(RestUri uri, RestRequestBody requestBody, ContentType contentType)
        {
            return PatchAsync(uri, requestBody, contentType).Result;
        }

        public RestResponse Patch(RestUri uri, RestRequestBody requestBody)
        {
            return PatchAsync(uri, requestBody).Result;
        }

        public RestResponse Patch(string resourceUri, object parameters, RestRequestBody body, ContentType contentType)
        {
            return PatchAsync(resourceUri, parameters, body, contentType).Result;
        }

        public RestResponse Patch(string resourceUri, RestRequestBody body)
        {
            return PatchAsync(resourceUri, body).Result;
        }

        public RestResponse Patch(string resourceUri, object parameters, RestRequestBody body)
        {
            return PatchAsync(resourceUri, parameters, body).Result;
        }

        public Task<RestResponse> DeleteAsync(RestUri uri)
        {
            return InvokeAsync(CreateRestRequest(HttpMethod.DELETE, uri));
        }

        public Task<RestResponse> DeleteAsync(string resourceUri)
        {
            return DeleteAsync(new RestUri(_baseUrl, resourceUri));
        }

        public Task<RestResponse> DeleteAsync(string resourceUri, object parameters)
        {
            return DeleteAsync(CreateRestUri(_baseUrl, resourceUri, parameters));
        }

        public RestResponse Delete(string resourceUri)
        {
            return DeleteAsync(resourceUri).Result;
        }

        public RestResponse Delete(string resourceUri, object parameters)
        {
            return DeleteAsync(resourceUri, parameters).Result;
        }

        public RestResponse Delete(RestUri uri)
        {
            return DeleteAsync(uri).Result;
        }

        public Task<RestResponse<T>> GetAsync<T>(RestUri uri)
        {
            return InvokeAsync<T>(CreateRestRequest(HttpMethod.GET, uri));
        }

        public Task<RestResponse<T>> GetAsync<T>(string resourceUri)
        {
            return GetAsync<T>(CreateRestUri(_baseUrl, resourceUri, new { }));
        }

        public Task<RestResponse<T>> GetAsync<T>(string resourceUri, object parameters)
        {
            return GetAsync<T>(CreateRestUri(_baseUrl, resourceUri, parameters));
        }

        public RestResponse<T> Get<T>(string resourceUri)
        {
            return GetAsync<T>(resourceUri).Result;
        }

        public RestResponse<T> Get<T>(string resourceUri, object parameters)
        {
            return GetAsync<T>(resourceUri, parameters).Result;
        }

        public RestResponse<T> Get<T>(RestUri uri)
        {
            return GetAsync<T>(uri).Result;
        }

        public Task<RestResponse<T>> PostAsync<T>(RestUri uri, RestRequestBody requestBody, ContentType contentType)
        {
            RestRequest request = CreateRestRequest(HttpMethod.POST, uri);
            request.ContentType = contentType;
            request.Body = requestBody;
            return InvokeAsync<T>(request);
        }

        public Task<RestResponse<T>> PostAsync<T>(RestUri uri, RestRequestBody requestBody)
        {
            return PostAsync<T>(uri, requestBody, ContentType.ApplicationX_WWW_Form_UrlEncoded);
        }

        public Task<RestResponse<T>> PostAsync<T>(string resourceUri, object parameters, RestRequestBody body, ContentType contentType)
        {
            return PostAsync<T>(CreateRestUri(_baseUrl, resourceUri, parameters), body, contentType);
        }

        public Task<RestResponse<T>> PostAsync<T>(string resourceUri, RestRequestBody body)
        {
            return PostAsync<T>(CreateRestUri(_baseUrl, resourceUri, new { }), body, ContentType.ApplicationX_WWW_Form_UrlEncoded);
        }

        public Task<RestResponse<T>> PostAsync<T>(string resourceUri, object parameters, RestRequestBody body)
        {
            return PostAsync<T>(CreateRestUri(_baseUrl, resourceUri, parameters), body, ContentType.ApplicationX_WWW_Form_UrlEncoded);
        }

        public RestResponse<T> Post<T>(RestUri uri, RestRequestBody requestBody, ContentType contentType)
        {
            return PostAsync<T>(uri, requestBody, contentType).Result;
        }

        public RestResponse<T> Post<T>(RestUri uri, RestRequestBody requestBody)
        {
            return PostAsync<T>(uri, requestBody).Result;
        }

        public RestResponse<T> Post<T>(string resourceUri, object parameters, RestRequestBody body, ContentType contentType)
        {
            return PostAsync<T>(resourceUri, parameters, body, contentType).Result;
        }

        public RestResponse<T> Post<T>(string resourceUri, RestRequestBody body)
        {
            return PostAsync<T>(resourceUri, body).Result;
        }

        public RestResponse<T> Post<T>(string resourceUri, object parameters, RestRequestBody body)
        {
            return PostAsync<T>(resourceUri, parameters, body).Result;
        }

        public Task<RestResponse<T>> PutAsync<T>(RestUri uri, RestRequestBody requestBody, ContentType contentType)
        {
            RestRequest request = CreateRestRequest(HttpMethod.PUT, uri);
            request.Body = requestBody;
            request.ContentType = contentType;
            return InvokeAsync<T>(request);
        }

        public Task<RestResponse<T>> PutAsync<T>(RestUri uri, RestRequestBody requestBody)
        {
            return PutAsync<T>(uri, requestBody, ContentType.ApplicationX_WWW_Form_UrlEncoded);
        }

        public Task<RestResponse<T>> PutAsync<T>(string resourceUri, object parameters, RestRequestBody body, ContentType contentType)
        {
            return PutAsync<T>(CreateRestUri(_baseUrl, resourceUri, parameters), body, contentType);
        }

        public Task<RestResponse<T>> PutAsync<T>(string resourceUri, RestRequestBody body)
        {
            return PutAsync<T>(CreateRestUri(_baseUrl, resourceUri, new { }), body, ContentType.ApplicationX_WWW_Form_UrlEncoded);
        }

        public Task<RestResponse<T>> PutAsync<T>(string resourceUri, object parameters, RestRequestBody body)
        {
            return PutAsync<T>(CreateRestUri(_baseUrl, resourceUri, parameters), body, ContentType.ApplicationX_WWW_Form_UrlEncoded);
        }

        public RestResponse<T> Put<T>(RestUri uri, RestRequestBody requestBody, ContentType contentType)
        {
            return PutAsync<T>(uri, requestBody, contentType).Result;
        }

        public RestResponse<T> Put<T>(RestUri uri, RestRequestBody requestBody)
        {
            return PutAsync<T>(uri, requestBody).Result;
        }

        public RestResponse<T> Put<T>(string resourceUri, object parameters, RestRequestBody body, ContentType contentType)
        {
            return PutAsync<T>(resourceUri, parameters, body, contentType).Result;
        }

        public RestResponse<T> Put<T>(string resourceUri, RestRequestBody body)
        {
            return PutAsync<T>(resourceUri, body).Result;
        }

        public RestResponse<T> Put<T>(string resourceUri, object parameters, RestRequestBody body)
        {
            return PutAsync<T>(resourceUri, parameters, body).Result;
        }

        public Task<RestResponse<T>> PatchAsync<T>(RestUri uri, RestRequestBody requestBody, ContentType contentType)
        {
            RestRequest request = CreateRestRequest(HttpMethod.PATCH, uri);
            request.Body = requestBody;
            request.ContentType = contentType;
            return InvokeAsync<T>(request);
        }

        public Task<RestResponse<T>> PatchAsync<T>(RestUri uri, RestRequestBody requestBody)
        {
            return PatchAsync<T>(uri, requestBody, ContentType.ApplicationX_WWW_Form_UrlEncoded);
        }

        public Task<RestResponse<T>> PatchAsync<T>(string resourceUri, object parameters, RestRequestBody body, ContentType contentType)
        {
            return PatchAsync<T>(CreateRestUri(_baseUrl, resourceUri, parameters), body, contentType);
        }

        public Task<RestResponse<T>> PatchAsync<T>(string resourceUri, RestRequestBody body)
        {
            return PatchAsync<T>(CreateRestUri(_baseUrl, resourceUri, new { }), body, ContentType.ApplicationX_WWW_Form_UrlEncoded);
        }

        public Task<RestResponse<T>> PatchAsync<T>(string resourceUri, object parameters, RestRequestBody body)
        {
            return PatchAsync<T>(CreateRestUri(_baseUrl, resourceUri, parameters), body, ContentType.ApplicationX_WWW_Form_UrlEncoded);
        }

        public RestResponse<T> Patch<T>(RestUri uri, RestRequestBody requestBody, ContentType contentType)
        {
            return PatchAsync<T>(uri, requestBody, contentType).Result;
        }

        public RestResponse<T> Patch<T>(RestUri uri, RestRequestBody requestBody)
        {
            return PatchAsync<T>(uri, requestBody).Result;
        }

        public RestResponse<T> Patch<T>(string resourceUri, object parameters, RestRequestBody body, ContentType contentType)
        {
            return PatchAsync<T>(resourceUri, parameters, body, contentType).Result;
        }

        public RestResponse<T> Patch<T>(string resourceUri, RestRequestBody body)
        {
            return PatchAsync<T>(resourceUri, body).Result;
        }

        public RestResponse<T> Patch<T>(string resourceUri, object parameters, RestRequestBody body)
        {
            return PatchAsync<T>(resourceUri, parameters, body).Result;
        }

        public Task<RestResponse<T>> DeleteAsync<T>(RestUri uri)
        {
            return InvokeAsync<T>(CreateRestRequest(HttpMethod.DELETE, uri));
        }

        public Task<RestResponse<T>> DeleteAsync<T>(string resourceUri)
        {
            return DeleteAsync<T>(new RestUri(_baseUrl, resourceUri));
        }

        public Task<RestResponse<T>> DeleteAsync<T>(string resourceUri, object parameters)
        {
            return DeleteAsync<T>(CreateRestUri(_baseUrl, resourceUri, parameters));
        }

        public RestResponse<T> Delete<T>(string resourceUri)
        {
            return DeleteAsync<T>(resourceUri).Result;
        }

        public RestResponse<T> Delete<T>(string resourceUri, object parameters)
        {
            return DeleteAsync<T>(resourceUri, parameters).Result;
        }

        public RestResponse<T> Delete<T>(RestUri uri)
        {
            return DeleteAsync<T>(uri).Result;
        }

        private RestRequest CreateRestRequest(HttpMethod method, RestUri uri)
        {
            var restRequest = new RestRequest(method, uri);
            
            foreach (Cookie cookie in _cookies)
            {
                restRequest.AddCookie(cookie);
            }

            foreach (var header in _headers)
            {
                if (header.Key is string)
                {
                    restRequest.AddHeader(header.Key.ToString(), header.Value.ToString());
                }
                else if (header.Key is HttpRequestHeader)
                {
                    restRequest.AddHeader((HttpRequestHeader)header.Key, header.Value.ToString());
                }
            }

            return restRequest;
        }

        private RestUri CreateRestUri(Uri baseUri, string resourceUri, object parameters)
        {
            var uri = new RestUri(baseUri, resourceUri);

            var nameValuePairs = parameters.ToDictionary();

            foreach (var nameValue in nameValuePairs)
            {
                if (resourceUri.IndexOf("{" + nameValue.Key + "}") > -1)
                {
                    uri.SetParameter(nameValue.Key, nameValue.Value);
                }
                else
                {
                    uri.SetQuery(nameValue.Key, nameValue.Value);
                }
            }

            return uri;
        }

    }
}
