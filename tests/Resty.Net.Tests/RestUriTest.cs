using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace Resty.Net.Tests
{
    public class RestUriTest
    {
        [Fact]
        public void Simple()
        {
            RestUri target = new RestUri("http://localhost/", "account");

            var actual = target.ToString();

            Assert.Equal("http://localhost/account", actual);
        }

        [Fact]
        public void SimpleWithNonDefaultPort()
        {
            RestUri target = new RestUri("http://localhost:50001/", "account");

            var actual = target.ToString();

            Assert.Equal("http://localhost:50001/account", actual);
        }

        [Fact]
        public void SimpleWithTrailingAndLeadingSlashesInBaseUriAndResourceUrlRespectively()
        {
            RestUri target = new RestUri("http://localhost/", "/account");

            var actual = target.ToString();

            Assert.Equal("http://localhost/account", actual);
        }

        [Fact]
        public void SimpleWithNoTrailingAndLeadingSlashesInBaseUriAndResourceUrlRespectively()
        {
            RestUri target = new RestUri("http://localhost", "account");

            var actual = target.ToString();

            Assert.Equal("http://localhost/account", actual);
        }

        [Fact]
        public void SimpleWithPathPortionInUrlAndTrailingAndLeadingSlashesInBaseUriAndResourceUrlRespectively()
        {
            RestUri target = new RestUri("http://localhost/api/", "/account");

            var actual = target.ToString();

            Assert.Equal("http://localhost/api/account", actual);
        }

        [Fact]
        public void SimpleWithPathPortionInUrlAndNoTrailingAndLeadingSlashes()
        {
            RestUri target = new RestUri("http://localhost/api", "account");

            var actual = target.ToString();

            Assert.Equal("http://localhost/api/account", actual);
        }

        [Fact]
        public void WithUriTemplateParameter()
        {
            RestUri target = new RestUri("http://localhost/", "account/{id}").SetParameter("id", 1);

            var actual = target.ToString();

            Assert.Equal("http://localhost/account/1", actual);
        }

        [Fact]
        public void WithMultipleUriTemplateParameter()
        {
            RestUri target = new RestUri("http://localhost/", "account/{id}/{uid}/{date}/{email}").SetParameter("id", 1).SetParameter("uid", new Guid("5AAE425D-A603-43B7-86CE-D66EF0AC870C")).SetParameter("date", new DateTime(2010, 1, 1)).SetParameter("email", "abc@abc.com");

            var actual = target.ToString();
            var dateString = Uri.EscapeDataString(new DateTime(2010, 1, 1).ToString());
            var expected = string.Format("http://localhost/account/1/5aae425d-a603-43b7-86ce-d66ef0ac870c/{0}/abc%40abc.com", dateString);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithSingleQueryString()
        {
            RestUri target = new RestUri("http://localhost/", "account").SetQuery("id", 1);

            var actual = target.ToString();

            Assert.Equal("http://localhost/account?id=1", actual);
        }

        [Fact]
        public void WithMultipleQueryStrings()
        {
            RestUri target = new RestUri("http://localhost/", "account").SetQuery("id", 1).SetQuery("date", new DateTime(2010, 1, 1)).SetQuery("uid", new Guid("5AAE425D-A603-43B7-86CE-D66EF0AC870C"));

            var actual = target.ToString();

            var dateString = Uri.EscapeDataString(new DateTime(2010, 1, 1).ToString());
            var expected = string.Format("http://localhost/account?id=1&date={0}&uid=5aae425d-a603-43b7-86ce-d66ef0ac870c", dateString);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithMultipleQueryStringsAndUriTemplateParameters()
        {
            RestUri target = new RestUri("http://localhost/", "account/{id}/{uid}/{date}/{email}")
                .SetParameter("id", 1)
                .SetParameter("date", new DateTime(2010, 1, 1))
                .SetParameter("uid", new Guid("5AAE425D-A603-43B7-86CE-D66EF0AC870C"))
                .SetParameter("email", "abc@abc.com")
                .SetQuery("id", 1)
                .SetQuery("date", new DateTime(2010, 1, 1))
                .SetQuery("uid", new Guid("5AAE425D-A603-43B7-86CE-D66EF0AC870C"))
                .SetQuery("email", "abc@abc.com");

            var actual = target.ToString();

            var dateString = Uri.EscapeDataString(new DateTime(2010, 1, 1).ToString());
            var expected = string.Format("http://localhost/account/1/5aae425d-a603-43b7-86ce-d66ef0ac870c/{0}/abc%40abc.com?id=1&date={0}&uid=5aae425d-a603-43b7-86ce-d66ef0ac870c&email=abc%40abc.com", dateString);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Withonlybaseurl()
        {
            RestUri target = new RestUri("http://localhost/account/{id}/{uid}/{date}/{email}", "")
                .SetParameter("id", 1)
                .SetParameter("date", new DateTime(2010, 1, 1))
                .SetParameter("uid", new Guid("5AAE425D-A603-43B7-86CE-D66EF0AC870C"))
                .SetParameter("email", "abc@abc.com")
                .SetQuery("id", 1)
                .SetQuery("date", new DateTime(2010, 1, 1))
                .SetQuery("uid", new Guid("5AAE425D-A603-43B7-86CE-D66EF0AC870C"))
                .SetQuery("email", "abc@abc.com");

            var actual = target.ToString();

            var dateString = Uri.EscapeDataString(new DateTime(2010, 1, 1).ToString());
            var expected = string.Format("http://localhost/account/1/5aae425d-a603-43b7-86ce-d66ef0ac870c/{0}/abc%40abc.com?id=1&date={0}&uid=5aae425d-a603-43b7-86ce-d66ef0ac870c&email=abc%40abc.com", dateString);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithEmbededUrlInUriTemplate()
        {
            RestUri target = new RestUri("http://localhost/account/{redirectTo}")
                .SetParameter("redirectTo", "http://localhost/login");

            var actual = target.ToString();

            var expected = "http://localhost/account/http%3A%2F%2Flocalhost%2Flogin";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithEmbededUrlWithPortInUriTemplate()
        {
            RestUri target = new RestUri("http://localhost/account/{redirectTo}")
                .SetParameter("redirectTo", "http://localhost:80/login");

            var actual = target.ToString();

            var expected = "http://localhost/account/http%3A%2F%2Flocalhost%3A80%2Flogin";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithEmbededUrlInQueryString()
        {
            RestUri target = new RestUri("http://localhost/account/")
                .SetQuery("redirectTo", "http://localhost/login");

            var actual = target.ToString();

            var expected = "http://localhost/account?redirectTo=http%3A%2F%2Flocalhost%2Flogin";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithQueryStringInBaseUrl()
        {
            RestUri target = new RestUri("http://localhost/account?id=1");

            var actual = target.ToString();

            var expected = "http://localhost/account?id=1";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithOnlyQueryStringInResourceUrl()
        {
            RestUri target = new RestUri("http://localhost/account", "?id=1");

            var actual = target.ToString();

            var expected = "http://localhost/account?id=1";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithQueryStringNotInNameValueFormatOption1()
        {
            RestUri target = new RestUri("http://localhost/", "account").SetQuery(123456);

            var actual = target.ToString();

            var expected = "http://localhost/account?123456";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithQueryStringNotInNameValueFormatOption2()
        {
            RestUri target = new RestUri("http://localhost/", "account").SetQuery("123456", QueryStringParameter.None);

            var actual = target.ToString();

            var expected = "http://localhost/account?123456";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithQueryStringNotInNameValueFormatOption3()
        {
            RestUri target = new RestUri("http://localhost/", "account?123456").SetQuery("id", "1");

            var actual = target.ToString();

            var expected = "http://localhost/account?123456&id=1";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithQueryStringRepeatedInResourceUrlAndUsingSetQuery()
        {
            RestUri target = new RestUri("http://localhost/", "account?id=1").SetQuery("id", 2);

            var actual = target.ToString();

            var expected = "http://localhost/account?id=2";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithQueryStringRepeatedInBaseUrlAndUsingSetQuery()
        {
            RestUri target = new RestUri("http://localhost/account?id=1").SetQuery("id", 2);

            var actual = target.ToString();

            var expected = "http://localhost/account?id=2";
            Assert.Equal(expected, actual);
        }
    }
}
