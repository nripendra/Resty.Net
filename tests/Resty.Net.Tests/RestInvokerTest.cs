using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Resty.Net.Extras;
using Xunit;
using Xunit.Extensions;

namespace Resty.Net.Tests
{
    public class RestInvokerTest
    {
        Uri _MyUri;
        NancyHostHelper _nancyHostHelper;

        public RestInvokerTest()
        {
            _nancyHostHelper = new NancyHostHelper();
            _MyUri = _nancyHostHelper.Start();
        }

        ~RestInvokerTest()
        {
            _nancyHostHelper.Stop();
        }

        [Fact]
        public void InvokeMethodWithGetRequest()
        {
            //Arrange
            RestInvoker target = new RestInvoker();
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            RestRequest request = new RestRequest(HttpMethod.GET, new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1"));

            //Act
            using (RestResponse actual = target.Invoke(request))
            {
                //Assert
                Assert.True(StubModule.GetPerson);
                Assert.True(actual.IsSuccessStatusCode);
                Assert.NotNull(actual);

                string content = actual.Body.ReadAsString();

                Assert.Equal("{\"Id\":1,\"UID\":\"00000000-0000-0000-0000-000000000000\",\"Email\":\"abc@abc.com\",\"NoOfSiblings\":0,\"DOB\":\"\\/Date(-59011459200000)\\/\",\"IsActive\":false,\"Salary\":0}", content);
            }
        }

        [Fact]
        public void InvokeAsyncMethodWithGetRequest()
        {
            //Arrange
            RestInvoker target = new RestInvoker();
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            RestRequest request = new RestRequest(HttpMethod.GET, new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1"));

            //Act
            target.InvokeAsync(request).ContinueWith(task =>
            {
                using (var actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.GetPerson);
                    Assert.True(actual.IsSuccessStatusCode);
                    Assert.NotNull(actual);

                    string content = actual.Body.ReadAsString();

                    Assert.Equal("{\"Id\":1,\"UID\":\"00000000-0000-0000-0000-000000000000\",\"Email\":\"abc@abc.com\",\"NoOfSiblings\":0,\"DOB\":\"\\/Date(-59011459200000)\\/\",\"IsActive\":false,\"Salary\":0}", content);
                }

            }).Wait();

        }

        [Fact]
        public void InvokeOfT_MethodWithGetRequest()
        {
            //Arrange
            RestInvoker target = new RestInvoker();
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            RestRequest request = new RestRequest(HttpMethod.GET, new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1"));

            //Act
            using (RestResponse<Person> actual = target.Invoke<Person>(request))
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
        public void InvokeAsyncOfT_MethodWithGetRequest()
        {
            //Arrange
            RestInvoker target = new RestInvoker();
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            RestRequest request = new RestRequest(HttpMethod.GET, new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1"));

            //Act
            target.InvokeAsync<Person>(request).ContinueWith(task =>
            {
                using (RestResponse<Person> actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.GetPerson);
                    Assert.NotNull(actual);
                    Assert.NotNull(actual.Data);
                    Assert.True(actual.IsSuccessStatusCode);

                    Person person = actual.Data;

                    Assert.Equal("abc@abc.com", person.Email);
                }

            }).Wait();
        }

        [Fact]
        public void GetAsync_overload1()
        {
            //Arrange
            RestInvoker target = new RestInvoker();
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            target.GetAsync(new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1")).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.GetPerson);
                    Assert.True(actual.IsSuccessStatusCode);
                    Assert.NotNull(actual);

                    string content = actual.Body.ReadAsString();

                    Assert.Equal("{\"Id\":1,\"UID\":\"00000000-0000-0000-0000-000000000000\",\"Email\":\"abc@abc.com\",\"NoOfSiblings\":0,\"DOB\":\"\\/Date(-59011459200000)\\/\",\"IsActive\":false,\"Salary\":0}", content);
                }

            }).Wait();

        }

        [Fact]
        public void GetAsync_overload2()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            target.GetAsync("/Person/1").ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.GetPerson);
                    Assert.True(actual.IsSuccessStatusCode);
                    Assert.NotNull(actual);

                    string content = actual.Body.ReadAsString();

                    Assert.Equal("{\"Id\":1,\"UID\":\"00000000-0000-0000-0000-000000000000\",\"Email\":\"abc@abc.com\",\"NoOfSiblings\":0,\"DOB\":\"\\/Date(-59011459200000)\\/\",\"IsActive\":false,\"Salary\":0}", content);
                }

            }).Wait();

        }

        [Fact]
        public void GetAsync_overload3()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            target.GetAsync("/Person/{id}", new { id = 1 }).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.GetPerson);
                    Assert.True(actual.IsSuccessStatusCode);
                    Assert.NotNull(actual);

                    string content = actual.Body.ReadAsString();

                    Assert.Equal("{\"Id\":1,\"UID\":\"00000000-0000-0000-0000-000000000000\",\"Email\":\"abc@abc.com\",\"NoOfSiblings\":0,\"DOB\":\"\\/Date(-59011459200000)\\/\",\"IsActive\":false,\"Salary\":0}", content);
                }

            }).Wait();
        }

        [Fact]
        public void Get_overload1()
        {
            //Arrange
            RestInvoker target = new RestInvoker();
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            using (RestResponse actual = target.Get(new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1")))
            {
                //Assert
                Assert.True(StubModule.GetPerson);
                Assert.True(actual.IsSuccessStatusCode);
                Assert.NotNull(actual);

                string content = actual.Body.ReadAsString();

                Assert.Equal("{\"Id\":1,\"UID\":\"00000000-0000-0000-0000-000000000000\",\"Email\":\"abc@abc.com\",\"NoOfSiblings\":0,\"DOB\":\"\\/Date(-59011459200000)\\/\",\"IsActive\":false,\"Salary\":0}", content);
            }

        }

        [Fact]
        public void Get_overload2()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            using (RestResponse actual = target.Get("/Person/1"))
            {
                //Assert
                Assert.True(StubModule.GetPerson);
                Assert.True(actual.IsSuccessStatusCode);
                Assert.NotNull(actual);

                string content = actual.Body.ReadAsString();

                Assert.Equal("{\"Id\":1,\"UID\":\"00000000-0000-0000-0000-000000000000\",\"Email\":\"abc@abc.com\",\"NoOfSiblings\":0,\"DOB\":\"\\/Date(-59011459200000)\\/\",\"IsActive\":false,\"Salary\":0}", content);
            }

        }

        [Fact]
        public void Get_overload3()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            using (RestResponse actual = target.Get("/Person/{id}", new { id = 1 }))
            {
                //Assert
                Assert.True(StubModule.GetPerson);
                Assert.True(actual.IsSuccessStatusCode);
                Assert.NotNull(actual);

                string content = actual.Body.ReadAsString();

                Assert.Equal("{\"Id\":1,\"UID\":\"00000000-0000-0000-0000-000000000000\",\"Email\":\"abc@abc.com\",\"NoOfSiblings\":0,\"DOB\":\"\\/Date(-59011459200000)\\/\",\"IsActive\":false,\"Salary\":0}", content);
            }
        }

        [Fact]
        public void PostAsync_overload1()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PostPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var uri = new RestUri(_MyUri, "/Person");
            var contentType = ContentType.ApplicationX_WWW_Form_UrlEncoded;
            var body = new RestObjectRequestBody<Person>(new Person { Id = 2, Email = "xyz@abc.com" });

            //Act
            target.PostAsync(uri, body, contentType).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.PostPerson);
                    Assert.NotNull(actual);
                    Assert.True(actual.IsSuccessStatusCode);
                    var person = StubModule.TestHarness.Where(x => x.Id == 2).FirstOrDefault();
                    Assert.NotNull(person);
                    Assert.Equal("xyz@abc.com", person.Email);
                }
            }).Wait();
        }

        [Fact]
        public void PostAsync_overload2()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PostPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var uri = new RestUri(_MyUri, "/Person");
            var body = new RestObjectRequestBody<Person>(new Person { Id = 2, Email = "xyz@abc.com" });

            //Act
            target.PostAsync(uri, body).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.PostPerson);
                    Assert.NotNull(actual);
                    Assert.True(actual.IsSuccessStatusCode);
                    var person = StubModule.TestHarness.Where(x => x.Id == 2).FirstOrDefault();
                    Assert.NotNull(person);
                    Assert.Equal("xyz@abc.com", person.Email);
                }
            }).Wait();
        }

        [Fact]
        public void PostAsync_overload3()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PostPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var body = new RestObjectRequestBody<Person>(new Person { Id = 2, Email = "xyz@abc.com" });

            //Act
            target.PostAsync("/Person", new { mode = "test" }, body).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.Equal("?mode=test", actual.ResponseUri.Query);
                    Assert.True(StubModule.PostPerson);
                    Assert.NotNull(actual);
                    Assert.True(actual.IsSuccessStatusCode);
                    var person = StubModule.TestHarness.Where(x => x.Id == 2).FirstOrDefault();
                    Assert.NotNull(person);
                    Assert.Equal("xyz@abc.com", person.Email);
                }
            }).Wait();
        }

        [Fact]
        public void PostAsync_overload4()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PostPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var body = new RestObjectRequestBody<Person>(new Person { Id = 2, Email = "xyz@abc.com" });

            //Act
            target.PostAsync("/Person", body).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.PostPerson);
                    Assert.NotNull(actual);
                    Assert.True(actual.IsSuccessStatusCode);
                    var person = StubModule.TestHarness.Where(x => x.Id == 2).FirstOrDefault();
                    Assert.NotNull(person);
                    Assert.Equal("xyz@abc.com", person.Email);
                }
            }).Wait();
        }

        [Fact]
        public void PostAsync_overload5()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PostPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var body = new RestObjectRequestBody<Person>(new Person { Id = 2, Email = "xyz@abc.com" });

            //Act
            target.PostAsync("/Person", new { mode = "test" }, body, ContentType.ApplicationX_WWW_Form_UrlEncoded).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.Equal("?mode=test", actual.ResponseUri.Query);
                    Assert.True(StubModule.PostPerson);
                    Assert.NotNull(actual);
                    Assert.True(actual.IsSuccessStatusCode);
                    var person = StubModule.TestHarness.Where(x => x.Id == 2).FirstOrDefault();
                    Assert.NotNull(person);
                    Assert.Equal("xyz@abc.com", person.Email);
                }
            }).Wait();
        }

        [Fact]
        public void Post_overload1()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PostPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var uri = new RestUri(_MyUri, "/Person");
            var contentType = ContentType.ApplicationX_WWW_Form_UrlEncoded;
            var body = new RestObjectRequestBody<Person>(new Person { Id = 2, Email = "xyz@abc.com" });

            //Act
            using (RestResponse actual = target.Post(uri, body, contentType))
            {
                //Assert
                Assert.True(StubModule.PostPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);
                var person = StubModule.TestHarness.Where(x => x.Id == 2).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("xyz@abc.com", person.Email);
            }
        }

        [Fact]
        public void Post_overload2()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PostPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var uri = new RestUri(_MyUri, "/Person");
            var body = new RestObjectRequestBody<Person>(new Person { Id = 2, Email = "xyz@abc.com" });

            //Act
            using (RestResponse actual = target.Post(uri, body))
            {
                //Assert
                Assert.True(StubModule.PostPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);
                var person = StubModule.TestHarness.Where(x => x.Id == 2).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("xyz@abc.com", person.Email);
            }
        }

        [Fact]
        public void Post_overload3()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PostPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var body = new RestObjectRequestBody<Person>(new Person { Id = 2, Email = "xyz@abc.com" });

            //Act
            using (RestResponse actual = target.Post("/Person", new { mode = "test" }, body))
            {
                //Assert
                Assert.Equal("?mode=test", actual.ResponseUri.Query);
                Assert.True(StubModule.PostPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);
                var person = StubModule.TestHarness.Where(x => x.Id == 2).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("xyz@abc.com", person.Email);
            }
        }

        [Fact]
        public void Post_overload4()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PostPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var body = new RestObjectRequestBody<Person>(new Person { Id = 2, Email = "xyz@abc.com" });

            //Act
            using (RestResponse actual = target.Post("/Person", body))
            {
                //Assert
                Assert.True(StubModule.PostPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);
                var person = StubModule.TestHarness.Where(x => x.Id == 2).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("xyz@abc.com", person.Email);
            }
        }

        [Fact]
        public void Post_overload5()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PostPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var body = new RestObjectRequestBody<Person>(new Person { Id = 2, Email = "xyz@abc.com" });

            //Act
            using (RestResponse actual = target.Post("/Person", new { mode = "test" }, body, ContentType.ApplicationX_WWW_Form_UrlEncoded))
            {
                //Assert
                Assert.Equal("?mode=test", actual.ResponseUri.Query);
                Assert.True(StubModule.PostPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);
                var person = StubModule.TestHarness.Where(x => x.Id == 2).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("xyz@abc.com", person.Email);
            }
        }

        [Fact]
        public void PutAsync_overload1()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PutPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var uri = new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1");
            var contentType = ContentType.ApplicationX_WWW_Form_UrlEncoded;
            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            target.PutAsync(uri, body, contentType).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.PutPerson);
                    Assert.NotNull(actual);
                    Assert.True(actual.IsSuccessStatusCode);

                    var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                    Assert.NotNull(person);
                    Assert.Equal("bcd@abc.com", person.Email);
                }
            }).Wait();
        }

        [Fact]
        public void PutAsync_overload2()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PutPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var uri = new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1");
            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            target.PutAsync(uri, body).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.PutPerson);
                    Assert.NotNull(actual);
                    Assert.True(actual.IsSuccessStatusCode);

                    var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                    Assert.NotNull(person);
                    Assert.Equal("bcd@abc.com", person.Email);
                }
            }).Wait();
        }

        [Fact]
        public void PutAsync_overload3()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PutPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            target.PutAsync("/Person/{id}", new { id = 1 }, body).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.PutPerson);
                    Assert.NotNull(actual);
                    Assert.True(actual.IsSuccessStatusCode);

                    var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                    Assert.NotNull(person);
                    Assert.Equal("bcd@abc.com", person.Email);
                }
            }).Wait();
        }

        [Fact]
        public void PutAsync_overload4()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PutPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            target.PutAsync("/Person/1", body).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.PutPerson);
                    Assert.NotNull(actual);
                    Assert.True(actual.IsSuccessStatusCode);

                    var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                    Assert.NotNull(person);
                    Assert.Equal("bcd@abc.com", person.Email);
                }
            }).Wait();
        }

        [Fact]
        public void PutAsync_overload5()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PutPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            target.PutAsync("/Person/{id}", new { id = 1 }, body, ContentType.ApplicationX_WWW_Form_UrlEncoded).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.PutPerson);
                    Assert.NotNull(actual);
                    Assert.True(actual.IsSuccessStatusCode);

                    var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                    Assert.NotNull(person);
                    Assert.Equal("bcd@abc.com", person.Email);
                }
            }).Wait();
        }

        [Fact]
        public void Put_overload1()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PutPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var uri = new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1");
            var contentType = ContentType.ApplicationX_WWW_Form_UrlEncoded;
            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            using (RestResponse actual = target.Put(uri, body, contentType))
            {
                //Assert
                Assert.True(StubModule.PutPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);

                var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("bcd@abc.com", person.Email);
            }
        }

        [Fact]
        public void Put_overload2()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PutPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var uri = new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1");
            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            using (RestResponse actual = target.Put(uri, body))
            {
                //Assert
                Assert.True(StubModule.PutPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);

                var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("bcd@abc.com", person.Email);
            }
        }

        [Fact]
        public void Put_overload3()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PutPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            using (RestResponse actual = target.Put("/Person/{id}", new { id = 1 }, body))
            {
                //Assert
                Assert.True(StubModule.PutPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);

                var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("bcd@abc.com", person.Email);
            }
        }

        [Fact]
        public void Put_overload4()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PutPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            using (RestResponse actual = target.Put("/Person/1", body))
            {
                //Assert
                Assert.True(StubModule.PutPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);

                var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("bcd@abc.com", person.Email);
            }
        }

        [Fact]
        public void Put_overload5()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PutPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            using (RestResponse actual = target.Put("/Person/{id}", new { id = 1 }, body, ContentType.ApplicationX_WWW_Form_UrlEncoded))
            {
                //Assert
                Assert.True(StubModule.PutPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);

                var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("bcd@abc.com", person.Email);
            }
        }

        [Fact]
        public void PatchAsync_overload1()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PatchPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var uri = new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1");
            var contentType = ContentType.ApplicationX_WWW_Form_UrlEncoded;
            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            target.PatchAsync(uri, body, contentType).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.PatchPerson);
                    Assert.NotNull(actual);
                    Assert.True(actual.IsSuccessStatusCode);

                    var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                    Assert.NotNull(person);
                    Assert.Equal("bcd@abc.com", person.Email);
                }
            }).Wait();
        }

        [Fact]
        public void PatchAsync_overload2()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PatchPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var uri = new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1");
            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            target.PatchAsync(uri, body).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.PatchPerson);
                    Assert.NotNull(actual);
                    Assert.True(actual.IsSuccessStatusCode);

                    var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                    Assert.NotNull(person);
                    Assert.Equal("bcd@abc.com", person.Email);
                }
            }).Wait();
        }

        [Fact]
        public void PatchAsync_overload3()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PatchPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            target.PatchAsync("/Person/{id}", new { id = 1 }, body).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.PatchPerson);
                    Assert.NotNull(actual);
                    Assert.True(actual.IsSuccessStatusCode);

                    var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                    Assert.NotNull(person);
                    Assert.Equal("bcd@abc.com", person.Email);
                }
            }).Wait();
        }

        [Fact]
        public void PatchAsync_overload4()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PatchPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            target.PatchAsync("/Person/1", body).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.PatchPerson);
                    Assert.NotNull(actual);
                    Assert.True(actual.IsSuccessStatusCode);

                    var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                    Assert.NotNull(person);
                    Assert.Equal("bcd@abc.com", person.Email);
                }
            }).Wait();
        }

        [Fact]
        public void PatchAsync_overload5()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PatchPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            target.PatchAsync("/Person/{id}", new { id = 1 }, body, ContentType.ApplicationX_WWW_Form_UrlEncoded).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.PatchPerson);
                    Assert.NotNull(actual);
                    Assert.True(actual.IsSuccessStatusCode);

                    var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                    Assert.NotNull(person);
                    Assert.Equal("bcd@abc.com", person.Email);
                }
            }).Wait();
        }

        [Fact]
        public void Patch_overload1()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PatchPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var uri = new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1");
            var contentType = ContentType.ApplicationX_WWW_Form_UrlEncoded;
            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            using (RestResponse actual = target.Patch(uri, body, contentType))
            {
                //Assert
                Assert.True(StubModule.PatchPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);

                var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("bcd@abc.com", person.Email);
            }
        }

        [Fact]
        public void Patch_overload2()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PatchPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var uri = new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1");
            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            using (RestResponse actual = target.Patch(uri, body))
            {
                //Assert
                Assert.True(StubModule.PatchPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);

                var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("bcd@abc.com", person.Email);
            }
        }

        [Fact]
        public void Patch_overload3()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PatchPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            using (RestResponse actual = target.Patch("/Person/{id}", new { id = 1 }, body))
            {
                //Assert
                Assert.True(StubModule.PatchPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);

                var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("bcd@abc.com", person.Email);
            }
        }

        [Fact]
        public void Patch_overload4()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PatchPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            using (RestResponse actual = target.Patch("/Person/1", body))
            {
                //Assert
                Assert.True(StubModule.PatchPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);

                var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("bcd@abc.com", person.Email);
            }
        }

        [Fact]
        public void Patch_overload5()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.PatchPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            var body = new RestObjectRequestBody<Person>(new Person { Id = 1, Email = "bcd@abc.com" });

            //Act
            using (RestResponse actual = target.Patch("/Person/{id}", new { id = 1 }, body, ContentType.ApplicationX_WWW_Form_UrlEncoded))
            {
                //Assert
                Assert.True(StubModule.PatchPerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);

                var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                Assert.NotNull(person);
                Assert.Equal("bcd@abc.com", person.Email);
            }
        }

        [Fact]
        public void DeleteAsync_overload1()
        {
            //Arrange
            RestInvoker target = new RestInvoker();
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.DeletePerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            target.DeleteAsync(new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1")).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.DeletePerson);
                    Assert.NotNull(actual);
                    Assert.True(actual.IsSuccessStatusCode);
                    var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                    Assert.Null(person);
                }

            }).Wait();

        }

        [Fact]
        public void DeleteAsync_overload2()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.DeletePerson = false; ;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            target.DeleteAsync("/Person/1").ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.DeletePerson);
                    Assert.NotNull(actual);
                    Assert.True(actual.IsSuccessStatusCode);
                    var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                    Assert.Null(person);
                }

            }).Wait();

        }

        [Fact]
        public void DeleteAsync_overload3()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.DeletePerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            target.DeleteAsync("/Person/{id}", new { id = 1 }).ContinueWith(task =>
            {
                using (RestResponse actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.DeletePerson);
                    Assert.NotNull(actual);
                    Assert.True(actual.IsSuccessStatusCode);
                    var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                    Assert.Null(person);
                }

            }).Wait();
        }

        [Fact]
        public void Delete_overload1()
        {
            //Arrange
            RestInvoker target = new RestInvoker();
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.DeletePerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            using (RestResponse actual = target.Delete(new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1")))
            {
                //Assert
                Assert.True(StubModule.DeletePerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);
                var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                Assert.Null(person);
            }

        }

        [Fact]
        public void Delete_overload2()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.DeletePerson = false; ;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            using (RestResponse actual = target.Delete("/Person/1"))
            {
                //Assert
                Assert.True(StubModule.DeletePerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);
                var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                Assert.Null(person);
            }

        }

        [Fact]
        public void Delete_overload3()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.DeletePerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            using (RestResponse actual = target.Delete("/Person/{id}", new { id = 1 }))
            {
                //Assert
                Assert.True(StubModule.DeletePerson);
                Assert.NotNull(actual);
                Assert.True(actual.IsSuccessStatusCode);
                var person = StubModule.TestHarness.Where(x => x.Id == 1).FirstOrDefault();
                Assert.Null(person);
            }
        }

        [Fact]
        public void GetAsyncOfT_overload1()
        {
            //Arrange
            RestInvoker target = new RestInvoker();
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            target.GetAsync<Person>(new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1")).ContinueWith(task =>
            {
                using (RestResponse<Person> actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.GetPerson);
                    Assert.True(actual.IsSuccessStatusCode);
                    Assert.NotNull(actual);

                    string content = actual.Body.ReadAsString();

                    Assert.Equal("{\"Id\":1,\"UID\":\"00000000-0000-0000-0000-000000000000\",\"Email\":\"abc@abc.com\",\"NoOfSiblings\":0,\"DOB\":\"\\/Date(-59011459200000)\\/\",\"IsActive\":false,\"Salary\":0}", content);

                    Assert.NotNull(actual.Data);
                    Assert.Equal("abc@abc.com", actual.Data.Email);
                }
            }).Wait();

        }

        [Fact]
        public void GetAsyncOfT_overload2()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            target.GetAsync<Person>("/Person/1").ContinueWith(task =>
            {
                using (RestResponse<Person> actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.GetPerson);
                    Assert.True(actual.IsSuccessStatusCode);
                    Assert.NotNull(actual);

                    string content = actual.Body.ReadAsString();

                    Assert.Equal("{\"Id\":1,\"UID\":\"00000000-0000-0000-0000-000000000000\",\"Email\":\"abc@abc.com\",\"NoOfSiblings\":0,\"DOB\":\"\\/Date(-59011459200000)\\/\",\"IsActive\":false,\"Salary\":0}", content);

                    Assert.NotNull(actual.Data);
                    Assert.Equal("abc@abc.com", actual.Data.Email);
                }

            }).Wait();

        }

        [Fact]
        public void GetAsyncOfT_overload3()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            target.GetAsync<Person>("/Person/{id}", new { id = 1 }).ContinueWith(task =>
            {
                using (RestResponse<Person> actual = task.Result)
                {
                    //Assert
                    Assert.True(StubModule.GetPerson);
                    Assert.True(actual.IsSuccessStatusCode);
                    Assert.NotNull(actual);

                    string content = actual.Body.ReadAsString();

                    Assert.Equal("{\"Id\":1,\"UID\":\"00000000-0000-0000-0000-000000000000\",\"Email\":\"abc@abc.com\",\"NoOfSiblings\":0,\"DOB\":\"\\/Date(-59011459200000)\\/\",\"IsActive\":false,\"Salary\":0}", content);

                    Assert.NotNull(actual.Data);
                    Assert.Equal("abc@abc.com", actual.Data.Email);
                }

            }).Wait();
        }

        [Fact]
        public void GetOfT_overload1()
        {
            //Arrange
            RestInvoker target = new RestInvoker();
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            using (RestResponse<Person> actual = target.Get<Person>(new RestUri(_MyUri, "/Person/{id}").SetParameter("id", "1")))
            {
                //Assert
                Assert.True(StubModule.GetPerson);
                Assert.True(actual.IsSuccessStatusCode);
                Assert.NotNull(actual);

                string content = actual.Body.ReadAsString();

                Assert.Equal("{\"Id\":1,\"UID\":\"00000000-0000-0000-0000-000000000000\",\"Email\":\"abc@abc.com\",\"NoOfSiblings\":0,\"DOB\":\"\\/Date(-59011459200000)\\/\",\"IsActive\":false,\"Salary\":0}", content);

                Assert.NotNull(actual.Data);
                Assert.Equal("abc@abc.com", actual.Data.Email);
            }

        }

        [Fact]
        public void GetOfT_overload2()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            using (RestResponse<Person> actual = target.Get<Person>("/Person/1"))
            {
                //Assert
                Assert.True(StubModule.GetPerson);
                Assert.True(actual.IsSuccessStatusCode);
                Assert.NotNull(actual);

                string content = actual.Body.ReadAsString();

                Assert.Equal("{\"Id\":1,\"UID\":\"00000000-0000-0000-0000-000000000000\",\"Email\":\"abc@abc.com\",\"NoOfSiblings\":0,\"DOB\":\"\\/Date(-59011459200000)\\/\",\"IsActive\":false,\"Salary\":0}", content);

                Assert.NotNull(actual.Data);
                Assert.Equal("abc@abc.com", actual.Data.Email);
            }

        }

        [Fact]
        public void GetOfT_overload3()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            using (RestResponse<Person> actual = target.Get<Person>("/Person/{id}", new { id = 1 }))
            {
                //Assert
                Assert.True(StubModule.GetPerson);
                Assert.True(actual.IsSuccessStatusCode);
                Assert.NotNull(actual);

                string content = actual.Body.ReadAsString();

                Assert.Equal("{\"Id\":1,\"UID\":\"00000000-0000-0000-0000-000000000000\",\"Email\":\"abc@abc.com\",\"NoOfSiblings\":0,\"DOB\":\"\\/Date(-59011459200000)\\/\",\"IsActive\":false,\"Salary\":0}", content);

                Assert.NotNull(actual.Data);
                Assert.Equal("abc@abc.com", actual.Data.Email);
            }
        }

        [Fact]
        public void RestInvokerAddCookie_overload1()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            target.AddCookie("cookie1", "cookieValue1");
            StubModule.Cookie = null;
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            using (RestResponse actual = target.Get("/Person/{id}", new { id = 1 }))
            {
                //Assert
                Assert.NotNull(StubModule.Cookie);
                Assert.Equal("cookieValue1", StubModule.Cookie["cookie1"]);
            }

        }

        [Fact]
        public void RestInvokerAddCookie_overload2()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            target.AddCookie(new System.Net.Cookie("cookie1", "cookieValue1"));
            StubModule.Cookie = null;
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            using (RestResponse actual = target.Get("/Person/{id}", new { id = 1 }))
            {
                //Assert
                Assert.NotNull(StubModule.Cookie);
                Assert.Equal("cookieValue1", StubModule.Cookie["cookie1"]);
            }

        }

        [Fact]
        public void RestInvokerAddHeader_overload1()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            target.AddHeader("header1", "headerValue1");
            StubModule.Cookie = null;
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            using (RestResponse actual = target.Get("/Person/{id}", new { id = 1 }))
            {
                //Assert
                Assert.NotNull(StubModule.RequestHeaders);
                Assert.Equal("headerValue1", StubModule.RequestHeaders["header1"].FirstOrDefault());
            }

        }

        [Fact]
        public void RestInvokerAddHeader_overload2()
        {
            //Arrange
            RestInvoker target = new RestInvoker(_MyUri.OriginalString);
            target.AddHeader(HttpRequestHeader.AcceptCharset, "utf-8");
            StubModule.Cookie = null;
            StubModule.HaltProcessing = TimeSpan.FromSeconds(0);
            StubModule.GetPerson = false;
            StubModule.TestHarness = new List<Person> { new Person { Id = 1, Email = "abc@abc.com" } };

            //Act
            using (RestResponse actual = target.Get("/Person/{id}", new { id = 1 }))
            {
                //Assert
                Assert.NotNull(StubModule.RequestHeaders);
                Assert.Equal("utf-8", StubModule.RequestHeaders["Accept-Charset"].FirstOrDefault());
            }

        }


    }
}

