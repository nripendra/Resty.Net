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

namespace Resty.Net.Configuration
{
    using Serialization;

    /// <summary>
    /// The Global configuration class that holds REST related configurations such as the deserializer registry and serializer registry
    /// </summary>
    public static class RestConfiguration
    {
        static RestConfiguration()
        {
            DeSerializers = new RestDeSerializerRegistry();

            DeSerializers.Register(ContentType.ApplicationJson.ToString(), new JsonDeserializer());
            DeSerializers.Register(ContentType.ApplicationXJson.ToString(), new JsonDeserializer());
            DeSerializers.Register(ContentType.TextJson.ToString(), new JsonDeserializer());
            
            DeSerializers.Register(ContentType.ApplicationXml.ToString(), new XmlDeserializer());
            DeSerializers.Register(ContentType.TextXml.ToString(), new XmlDeserializer());
            
            Serializers = new RestSerializerRegistry();

            Serializers.Register(ContentType.ApplicationJson.ToString(), new RestRequestBodyToJsonSerializer());
            Serializers.Register(ContentType.ApplicationXJson.ToString(), new RestRequestBodyToJsonSerializer());
            Serializers.Register(ContentType.TextJson.ToString(), new RestRequestBodyToJsonSerializer());

            Serializers.Register(ContentType.ApplicationX_WWW_Form_UrlEncoded.ToString(), new RestRequestBodyToFormSerializer());
        }

        /// <summary>
        /// The deserializer registry that holds mapping between content type and the deserializer.
        /// </summary>
        public static RestDeSerializerRegistry DeSerializers { get; private set; }

        /// <summary>
        /// The serializer registry that holds mapping between content type and the serializer.
        /// </summary>
        public static RestSerializerRegistry Serializers { get; private set; }
    }
}
