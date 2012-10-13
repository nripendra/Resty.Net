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
