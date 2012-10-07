using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;
using Nancy.Hosting.Self;
using System.Net;

namespace Resty.Net.Tests
{
    public class RestRequestTest
    {
        //Idea for REST client testing shamelessly copied from
        //http://www.csharpfritz.com/post/26765731081/restful-client-unit-testing-with-nancyfx.

        private NancyHost _Nancy;
        private static int port = 50001;
        protected Uri _MyUri = new Uri("http://localhost:50001");

        public RestRequestTest()
        {
            bool nancyStarted = false;
            // Need to retry in order to ensure that we properly startup after any failures
            for (var i = 0; i < 3; i++)
            {
                _Nancy = new NancyHost(_MyUri);

                try
                {
                    _Nancy.Start();
                    nancyStarted = true;
                    break;
                }
                catch (HttpListenerException)
                {
                    UriBuilder ub = new UriBuilder(_MyUri);
                    ub.Port = ++port;
                    _MyUri = ub.Uri;
                }
                catch
                {
                    try
                    {
                        _Nancy.Stop();
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            if (!nancyStarted)
            {
                //Don't allow to run the tests if Nancy not started.
                throw new Exception();
            }
        }

        ~RestRequestTest()
        {
            try
            {
                _Nancy.Stop();
                _Nancy = null;
            }
            catch { }
        }

        [Fact]
        public void Get()
        {
            //Arrange
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
        public void GetWeaklyTypedResponse()
        {
            //Arrange
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
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };
            StubModule.PutPerson = false;

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
        public void Delete()
        {
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
