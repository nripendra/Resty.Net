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
    using Extensions;

    public class RestUri
    {
        private Uri _baseUri;
        private Uri _resourceUri;
        private IDictionary<string, string> _uriTemplateParameters;
        private IDictionary<string, string> _queryString;

        /// <summary>
        /// Instantiate a RestUri.
        /// </summary>
        /// <param name="baseUri">The base Uri, denotes the server and path where the resource are hosted.</param>
        /// <param name="resourceUri">The resource Uri.The resourceUri parameter must compulsarily be relative uri.</param>
        public RestUri(string baseUri) :
            this(new Uri(baseUri), new Uri("", UriKind.Relative))
        {
        }

        /// <summary>
        /// Instantiate a RestUri.
        /// </summary>
        /// <param name="baseUri">The base Uri, denotes the server and path where the resource are hosted.</param>
        /// <param name="resourceUri">The resource Uri.The resourceUri parameter must compulsarily be relative uri.</param>
        public RestUri(string baseUri, string resourceUri) :
            this(new Uri(baseUri), new Uri(resourceUri, UriKind.Relative))
        {
        }

        /// <summary>
        /// Instantiate a RestUri.
        /// </summary>
        /// <param name="baseUri">The base Uri, denotes the server and path where the resource are hosted.</param>
        /// <param name="resourceUri">The resource Uri.The resourceUri parameter must compulsarily be relative uri.</param>
        public RestUri(Uri baseUri, string resourceUri) :
            this(baseUri, new Uri(resourceUri, UriKind.Relative))
        {
        }

        /// <summary>
        /// Instantiate a RestUri.
        /// </summary>
        /// <param name="baseUri">The base Uri, denotes the server and path where the resource are hosted.</param>
        /// <param name="resourceUri">The resource Uri.The resourceUri parameter must compulsarily be relative uri.</param>
        /// <exception cref="System.UriFormatException"></exception>
        public RestUri(Uri baseUri, Uri resourceUri)
        {
            _baseUri = baseUri;
            _resourceUri = resourceUri;
            _uriTemplateParameters = new Dictionary<string, string>();
            _queryString = new Dictionary<string, string>();
        }

        /// <summary>
        /// Add a querystring parameter.
        /// Replace old value if value already set.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public RestUri SetQuery(string name, object value)
        {
            if (_queryString.ContainsKey(name))
            {
                _queryString[name] = value.ToString();
            }
            else
            {
                _queryString.Add(name, value.ToString());
            }
            return this;
        }

        /// <summary>
        /// Add a querystring parameter.
        /// Replace old value if value already set.
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        /// <example>restUri.SetQuery(new { id = 1});</example>
        public RestUri SetQuery(object queryParams)
        {
            var dictionary = queryParams.ToDictionary();
            foreach (var property in dictionary)
            {
                SetQuery(property.Key, property.Value);
            }

            return this;
        }

        /// <summary>
        /// Set the UriTemplate parameter with the value.
        /// Replace old value if value already set.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public RestUri SetParameter(string name, object value)
        {
            if (_uriTemplateParameters.ContainsKey(name))
            {
                _uriTemplateParameters[name] = value.ToString();
            }
            else
            {
                _uriTemplateParameters.Add(name, value.ToString());
            }

            return this;
        }

        /// <summary>
        /// Set the UriTemplate parameter with the value.
        /// Replace old value if value already set.
        /// </summary>
        /// <param name="uriTemplateParams"></param>
        /// <returns></returns>
        public RestUri SetParameter(object uriTemplateParams)
        {
            var dictionary = uriTemplateParams.ToDictionary();
            foreach (var property in dictionary)
            {
                SetParameter(property.Key, property.Value);
            }

            return this;
        }

        /// <summary>
        /// Build uri from the base url and the resource uri template. 
        /// Also processes the uri template parameters and the querystring.
        /// </summary>
        /// <returns>Uri</returns>
        public Uri ToUri()
        {
            Uri fullUri;
            string resourceUri = _resourceUri.OriginalString;
            foreach (var kv in _uriTemplateParameters)
            {
                resourceUri = resourceUri.Replace("{" + kv.Key + "}", kv.Value.ToString());
            }
            Uri.TryCreate(_baseUri, resourceUri, out fullUri);

            UriBuilder uriBuilder = new UriBuilder(fullUri);

            string otherQuerStrings = BuildQueryString();
            if (!string.IsNullOrWhiteSpace(otherQuerStrings))
            {
                if (!string.IsNullOrWhiteSpace(uriBuilder.Query))
                {
                    uriBuilder.Query += "&";
                }
                uriBuilder.Query += otherQuerStrings;
            }

            return uriBuilder.Uri;
        }

        /// <summary>
        /// Build Query string from the query string entries added using AddQuerystring method.
        /// </summary>
        /// <returns>string</returns>
        public virtual string BuildQueryString()
        {
            List<string> qb = new List<string>();
            foreach (var kv in _queryString)
            {
                qb.Add(kv.Key + "=" + kv.Value);
            }
            return string.Join("&", qb.ToArray());
        }

        public override string ToString()
        {
            return ToUri().ToString();
        }
    }
}
