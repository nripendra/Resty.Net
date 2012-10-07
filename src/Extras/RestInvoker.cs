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
    public class RestInvoker
    {
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

        public RestResponse Get(RestUri uri)
        {
            return Invoke(CreateRestRequest(HttpMethod.GET, uri));
        }

        public Task<RestResponse> PostAsync(RestUri uri, RestRequestBody requestBody)
        {
            return InvokeAsync(CreateRestRequest(HttpMethod.POST, uri));
        }

        public RestResponse Post(RestUri uri, RestRequestBody requestBody)
        {
            return Invoke(CreateRestRequest(HttpMethod.POST, uri));
        }

        public Task<RestResponse> PutAsync(RestUri uri, RestRequestBody requestBody)
        {
            return InvokeAsync(CreateRestRequest(HttpMethod.PUT, uri));
        }

        public RestResponse Put(RestUri uri, RestRequestBody requestBody)
        {
            return Invoke(CreateRestRequest(HttpMethod.PUT, uri));
        }

        public Task<RestResponse> PatchAsync(RestUri uri, RestRequestBody requestBody)
        {
            return InvokeAsync(CreateRestRequest(HttpMethod.PATCH, uri));
        }

        public RestResponse Patch(RestUri uri, RestRequestBody requestBody)
        {
            return Invoke(CreateRestRequest(HttpMethod.PATCH, uri));
        }

        public Task<RestResponse> DeleteAsync(RestUri uri)
        {
            return InvokeAsync(CreateRestRequest(HttpMethod.DELETE, uri));
        }

        public RestResponse Delete(RestUri uri)
        {
            return Invoke(CreateRestRequest(HttpMethod.DELETE, uri));
        }

        public Task<RestResponse<T>> GetAsync<T>(RestUri uri)
        {
            return InvokeAsync<T>(CreateRestRequest(HttpMethod.GET, uri));
        }

        public RestResponse<T> Get<T>(RestUri uri)
        {
            return Invoke<T>(CreateRestRequest(HttpMethod.GET, uri));
        }

        public Task<RestResponse<T>> PostAsync<T>(RestUri uri, RestRequestBody requestBody)
        {
            return InvokeAsync<T>(CreateRestRequest(HttpMethod.POST, uri));
        }

        public RestResponse<T> Post<T>(RestUri uri, RestRequestBody requestBody)
        {
            return Invoke<T>(CreateRestRequest(HttpMethod.POST, uri));
        }

        public Task<RestResponse<T>> PutAsync<T>(RestUri uri, RestRequestBody requestBody)
        {
            return InvokeAsync<T>(CreateRestRequest(HttpMethod.PUT, uri));
        }

        public RestResponse<T> Put<T>(RestUri uri, RestRequestBody requestBody)
        {
            return Invoke<T>(CreateRestRequest(HttpMethod.PUT, uri));
        }

        public Task<RestResponse<T>> PatchAsync<T>(RestUri uri, RestRequestBody requestBody)
        {
            return InvokeAsync<T>(CreateRestRequest(HttpMethod.PATCH, uri));
        }

        public RestResponse<T> Patch<T>(RestUri uri, RestRequestBody requestBody)
        {
            return Invoke<T>(CreateRestRequest(HttpMethod.PATCH, uri));
        }

        public Task<RestResponse<T>> DeleteAsync<T>(RestUri uri)
        {
            return InvokeAsync<T>(CreateRestRequest(HttpMethod.DELETE, uri));
        }

        public RestResponse<T> Delete<T>(RestUri uri)
        {
            return Invoke<T>(CreateRestRequest(HttpMethod.DELETE, uri));
        }

        private RestRequest CreateRestRequest(HttpMethod method, RestUri uri)
        {
            return new RestRequest(method, uri);
        }

    }
}
