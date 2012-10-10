﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Nancy.Hosting.Self;
using Xunit;
using Xunit.Extensions;

namespace Resty.Net.Tests
{
    public class RestRequestTest
    {
        //Idea for REST client testing shamelessly copied from
        //http://www.csharpfritz.com/post/26765731081/restful-client-unit-testing-with-nancyfx.

        Uri _MyUri;
        NancyHostHelper _nancyHostHelper;

        public RestRequestTest()
        {
            _nancyHostHelper = new NancyHostHelper();
            _MyUri = _nancyHostHelper.Start();
        }

        ~RestRequestTest()
        {
            _nancyHostHelper.Stop();
        }

        [Fact]
        public void Get()
        {
            //Arrange
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            RestRequest target = new RestRequest(HttpMethod.GET, new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1"));

            //Act
            using (RestResponse<Person> actual = target.GetResponse<Person>())
            {
                //Assert
                Assert.True(StubModule.GetPerson);
                Assert.NotNull(actual);
                Assert.NotNull(actual.Data);
                Assert.True(actual.IsSuccessStatusCode);

                Person person = actual.Data;

                Assert.Equal("abc@abc.com", person.Email);
            }
        }

        [Fact]
        public void GetAborted()
        {
            //Arrange
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            RestRequest target = new RestRequest(HttpMethod.GET, new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1"));

            //Act
            var response = target.GetResponseAsync();
            target.Abort();

            //Assert
            Assert.NotNull(response.Result);
            Assert.NotNull(response.Result.Error);
            Assert.Equal(0, (int)response.Result.Error.StatusCode);
        }

        [Fact]
        public void GetWithTimeout()
        {
            //Arrange
            StubModule.HaltProcessing = TimeSpan.FromSeconds(10);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            RestRequest target = new RestRequest(HttpMethod.GET, new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1"));
            target.TimeOut = TimeSpan.FromSeconds(2);

            //Act
            using (RestResponse<Person> actual = target.GetResponse<Person>())
            {
                //Assert
                Assert.True(StubModule.GetPerson);
                Assert.NotNull(actual);
                Assert.NotNull(actual.Error);
                Assert.Equal("request canceled", actual.Error.Message.ToLower());
            }
        }

        [Fact]
        public void GetWithoutServer()
        {
            //Arrange
            StubModule.HaltProcessing = TimeSpan.FromSeconds(10);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            RestRequest target = new RestRequest(HttpMethod.GET, new RestUri(new Uri("http://localhost:60001/api"), "/Person/{id}").SetParameter("id", "1"));

            //Act
            using (RestResponse<Person> actual = target.GetResponse<Person>())
            {
                //Assert
                Assert.NotNull(actual);
                Assert.NotNull(actual.Error);
                Assert.NotNull(actual.Error.InnerException);
                Assert.Equal("unable to connect to the remote server", actual.Error.InnerException.Message.ToLower());
            }
        }

        [Fact]
        public void GetWithoutResolvableDomainName()
        {
            //Arrange
            StubModule.HaltProcessing = TimeSpan.FromSeconds(10);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            RestRequest target = new RestRequest(HttpMethod.GET, new RestUri(new Uri("http://localhost.nripendraiscool:60001/api"), "/Person/{id}").SetParameter("id", "1"));

            //Act
            using (RestResponse<Person> actual = target.GetResponse<Person>())
            {
                //Assert
                Assert.NotNull(actual);
                Assert.NotNull(actual.Error);
                Assert.NotNull(actual.Error.InnerException);
                Assert.Equal("the remote name could not be resolved: 'localhost.nripendraiscool'", actual.Error.InnerException.Message.ToLower());
            }
        }

        [Fact]
        public void GetWeaklyTypedResponse()
        {
            //Arrange
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            RestRequest target = new RestRequest(HttpMethod.GET, new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1"));

            //Act
            using (RestResponse actual = target.GetResponse())
            {
                //Assert
                Assert.True(actual.IsSuccessStatusCode);
                Assert.True(StubModule.GetPerson);
                Assert.NotNull(actual);
                string content = actual.Body.ReadAsString();
                Assert.Equal("{\"Id\":1,\"UID\":\"00000000-0000-0000-0000-000000000000\",\"Email\":\"abc@abc.com\",\"NoOfSiblings\":0,\"DOB\":\"\\/Date(-59011459200000)\\/\",\"IsActive\":false,\"Salary\":0}", content);
            }
        }

        [Fact]
        public void PostWithNormalContentType()
        {
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PostPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            RestRequest target = new RestRequest(HttpMethod.POST, new RestUri(_MyUri, "/Person"));
            target.ContentType = ContentType.ApplicationX_WWW_Form_UrlEncoded;
            target.Body = new RestObjectRequestBody<Person>(new Person { Id = 2, Email = "xyz@abc.com" });

            using (RestResponse actual = target.GetResponse())
            {
                Assert.True(StubModule.PostPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);
                var person = StubModule.TestHarness.Where(x => x.Id == 2).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("xyz@abc.com", person.Email);
            }
        }

        [Fact]
        public void PostWithJsonContentType()
        {
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PostPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            RestRequest target = new RestRequest(HttpMethod.POST, new RestUri(_MyUri, "/Person"));
            target.ContentType = ContentType.ApplicationJson;
            target.Body = new RestObjectRequestBody<Person>(new Person { Id = 3, Email = "xyz123@abc.com" });

            using (RestResponse actual = target.GetResponse())
            {
                Assert.True(StubModule.PostPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);
                var person = StubModule.TestHarness.Where(x => x.Id == 3).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("xyz123@abc.com", person.Email);
            }
        }

        [Fact]
        public void PutWithNormalContentType()
        {
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PutPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            RestRequest target = new RestRequest(HttpMethod.PUT, new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1"));
            target.ContentType = ContentType.ApplicationX_WWW_Form_UrlEncoded;
            target.Body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });


            using (RestResponse actual = target.GetResponse())
            {
                Assert.True(StubModule.PutPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);

                var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("bcd@abc.com", person.Email);
            }
        }

        [Fact]
        public void PutWithJsonContentType()
        {
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PutPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            RestRequest target = new RestRequest(HttpMethod.PUT, new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1"));
            target.ContentType = ContentType.ApplicationJson;
            target.Body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });


            using (RestResponse actual = target.GetResponse())
            {
                Assert.True(StubModule.PutPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);

                var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("bcd@abc.com", person.Email);
            }
        }

        [Fact]
        public void PatchWithNormalContentType()
        {
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PatchPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            RestRequest target = new RestRequest(HttpMethod.PATCH, new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1"));
            target.ContentType = ContentType.ApplicationX_WWW_Form_UrlEncoded;
            target.Body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });


            using (RestResponse actual = target.GetResponse())
            {
                Assert.True(StubModule.PutPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);

                var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("bcd@abc.com", person.Email);
            }
        }

        [Fact]
        public void PatchWithJsonContentType()
        {
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PatchPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            RestRequest target = new RestRequest(HttpMethod.PATCH, new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1"));
            target.ContentType = ContentType.ApplicationJson;
            target.Body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });


            using (RestResponse actual = target.GetResponse())
            {
                Assert.True(StubModule.PutPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);

                var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("bcd@abc.com", person.Email);
            }
        }

        [Fact]
        public void Delete()
        {
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.DeletePerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            RestRequest target = new RestRequest(HttpMethod.DELETE, new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1"));

            using (RestResponse actual = target.GetResponse())
            {
                Assert.True(StubModule.DeletePerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);
                var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                Assert.Null(person);
            }
        }
    }
}
