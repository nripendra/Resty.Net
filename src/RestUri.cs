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
    using System.Text.RegularExpressions;

    public class RestUri
    {
        private static Regex patternMultipleSlashes = new Regex("/{2,}", RegexOptions.Compiled);

        private string _resourcePath;
        private string _resourceQuery;

        private Uri _baseUri;
        private Uri _resourceUri;
        private IDictionary<string, string> _uriTemplateParameters;
        private IDictionary<string, object> _queryString;

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
            _queryString = new Dictionary<string, object>();

            ParseResourcePathAndQuery();
            SetQueryEntriesFromUriQueries();
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
                _queryString[name] = value;
            }
            else
            {
                _queryString.Add(name, value);
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
            if (queryParams.GetType().IsPrimitive)
            {
                SetQuery(queryParams.ToString(), QueryStringParameter.None);
            }
            else
            {
                var dictionary = queryParams.ToDictionary();
                foreach (var property in dictionary)
                {
                    SetQuery(property.Key, property.Value);
                }
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
        /// Build Query string from the query string entries added using AddQuerystring method.
        /// </summary>
        /// <returns>string</returns>
        public virtual string BuildQueryString()
        {
            List<string> qb = new List<string>();
            foreach (var kv in _queryString)
            {
                if (kv.Value is QueryStringParameter)
                {
                    qb.Add(Uri.EscapeDataString(kv.Key));
                }
                else
                {
                    qb.Add(kv.Key + "=" + Uri.EscapeDataString(kv.Value.ToString()));
                }
            }
            return string.Join("&", qb.ToArray());
        }

        /// <summary>
        /// Converts the RestUri to it's string Url representation.
        /// </summary>
        /// <returns>String Url representation of RestUri.</returns>
        public override string ToString()
        {
            UriBuilder uriBuilder = new UriBuilder(GetBaseUriComponents());

            string resourceUri = _resourcePath;
            foreach (var kv in _uriTemplateParameters)
            {
                resourceUri = resourceUri.Replace("{" + kv.Key + "}", Uri.EscapeDataString(kv.Value.ToString()));
            }

            uriBuilder.Path = resourceUri;

            string finalQuerStrings = BuildQueryString();
            if (!string.IsNullOrWhiteSpace(finalQuerStrings))
            {
                uriBuilder.Query = finalQuerStrings;
            }

            return Regex.Replace(uriBuilder.ToString(), ":80|:443", "");
        }

        private string GetBaseUriComponents()
        {
            UriComponents uriComponents;
            uriComponents = UriComponents.Scheme | UriComponents.Host;

            if (_baseUri.Port != 80 && _baseUri.Port != 443)
            {
                uriComponents |= UriComponents.Port;
            }

            return _baseUri.GetComponents(uriComponents, UriFormat.Unescaped);
        }

        private void ParseResourcePathAndQuery()
        {
            _resourcePath = _resourceUri.OriginalString;
            string pathAndQuery = Uri.UnescapeDataString(_baseUri.PathAndQuery);

            if (pathAndQuery != "/")
            {
                pathAndQuery = pathAndQuery.TrimEnd('/');
                    
                if (string.IsNullOrWhiteSpace(_resourcePath))
                {
                    _resourcePath = pathAndQuery;
                }
                else
                {
                    _resourcePath = _resourcePath.TrimStart('/');
                        
                    if (_resourcePath.StartsWith("?"))
                    {
                        _resourcePath = pathAndQuery + _resourcePath;
                    }
                    else
                    {
                        _resourcePath = pathAndQuery + "/" + _resourcePath;                        
                    }
                }

                _resourcePath = patternMultipleSlashes.Replace(_resourcePath, "/");
            }
            
            int indexOfQueryString = _resourcePath.IndexOf("?");
            if (indexOfQueryString > -1)
            {
                _resourceQuery = _resourcePath.Substring(indexOfQueryString + 1);
                _resourcePath = _resourcePath.Substring(0, indexOfQueryString);
            }
        }

        private void SetQueryEntriesFromUriQueries()
        {
            if (!string.IsNullOrWhiteSpace(_resourceQuery))
            {
                string[] nameValuePairs = _resourceQuery.Split('&');
                foreach (var nameValuePair in nameValuePairs)
                {
                    var nameAndValue = nameValuePair.Split('=');
                    if (nameAndValue.Length > 0)
                    {
                        var name = nameAndValue[0];
                        if (!_queryString.ContainsKey(name))
                        {
                            object value = QueryStringParameter.None;
                            if (nameAndValue.Length > 1)
                            {
                                value = nameAndValue[1];
                            }

                            SetQuery(name, value);
                        }
                    }
                }
            }
        }

    }
}
