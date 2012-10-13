using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace Resty.Net.Tests
{
    public class MultiPartFormDataTest
    {
        [Fact]
        public void WithBoundaryTest()
        {
            MultiPartFormData target = MultiPartFormData.Create().WithBoundary("----------xxxx");

            string actual = target.ToString();
            string expected = "multipart/form-data; boundary=----------xxxx";

            Assert.Equal("----------xxxx", target.Boundary);
            Assert.Equal(expected, actual);
        }
    }
}
