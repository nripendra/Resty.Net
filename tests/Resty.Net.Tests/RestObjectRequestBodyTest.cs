using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace Resty.Net.Tests
{
    public class RestObjectRequestBodyTest
    {
        [Fact]
        public void SerializeToJSONWithDefaults()
        {
            string expected = "{\"Id\":0,\"UID\":\"00000000-0000-0000-0000-000000000000\",\"Email\":null,\"NoOfSiblings\":0,\"DOB\":\"0001-01-01T00:00:00Z\",\"IsActive\":false,\"Salary\":0}";

            RestObjectRequestBody<Person> target = new RestObjectRequestBody<Person>(new Person());

            string actual = target.ToString(ContentType.ApplicationJson);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SerializeToJSONWithValues()
        {
            string expected = "{\"Id\":1,\"UID\":\"e4d2c5c2-2e7a-4117-90c2-28214879d3b7\",\"Email\":\"abc@abc.com\",\"NoOfSiblings\":2,\"DOB\":\"2010-01-01T00:00:00Z\",\"IsActive\":true,\"Salary\":100}";

            RestObjectRequestBody<Person> target = new RestObjectRequestBody<Person>(new Person { Id = 1, UID = new Guid("E4D2C5C2-2E7A-4117-90C2-28214879D3B7"), Email = "abc@abc.com", NoOfSiblings = 2, DOB = new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Salary = 100 });

            string actual = target.ToString(ContentType.ApplicationJson);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SerializeToNameValueWithDefaults()
        {
            string expected = "Id=0&UID=00000000-0000-0000-0000-000000000000&Email=&NoOfSiblings=0&DOB=1%2F1%2F0001%2012%3A00%3A00%20AM&IsActive=False&Salary=0";

            RestObjectRequestBody<Person> target = new RestObjectRequestBody<Person>(new Person());

            string actual = target.ToString(ContentType.ApplicationX_WWW_Form_UrlEncoded);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SerializeToNameValueWithValues()
        {
            string expected = "Id=1&UID=e4d2c5c2-2e7a-4117-90c2-28214879d3b7&Email=abc%40abc.com&NoOfSiblings=2&DOB=1%2F1%2F2010%2012%3A00%3A00%20AM&IsActive=True&Salary=100";

            RestObjectRequestBody<Person> target = new RestObjectRequestBody<Person>(new Person { Id = 1, UID = new Guid("E4D2C5C2-2E7A-4117-90C2-28214879D3B7"), Email = "abc@abc.com", NoOfSiblings = 2, DOB = new DateTime(2010, 1, 1), IsActive = true, Salary = 100 });

            string actual = target.ToString(ContentType.ApplicationX_WWW_Form_UrlEncoded);

            Assert.Equal(expected, actual);
        }
    }
}
