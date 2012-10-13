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
    public class MultiPartFormData : ContentType
    {
        /// <summary>
        /// Gets the boundary pattern that seperates the multiple parts of the request body stream.
        /// </summary>
        public string Boundary { get; private set; }

        protected MultiPartFormData()
            : base("multipart/form-data")
        {
        }

        protected MultiPartFormData(string boundary)
            : base("multipart/form-data")
        {
            Boundary = boundary;
        }

        /// <summary>
        /// Sets the boundary value and returns the current instance of MultiPartFormData
        /// </summary>
        /// <param name="boundary">The boundary pattern that seperates the multiple parts of the request body stream.</param>
        /// <returns>The current instance of MultiPartFormData</returns>
        public MultiPartFormData WithBoundary(string boundary)
        {
            Boundary = boundary;
            return this;
        }

        public static MultiPartFormData Create()
        {
            return new MultiPartFormData();
        }

        public new static MultiPartFormData Create(string boundary)
        {
            return new MultiPartFormData(boundary);
        }

        public override string ToString()
        {
            return string.Format("multipart/form-data; boundary={0}", this.Boundary);
        }
    }
}
