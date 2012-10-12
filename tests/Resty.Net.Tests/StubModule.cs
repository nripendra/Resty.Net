using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Nancy;
using Nancy.Diagnostics;
using Nancy.ModelBinding;

namespace Resty.Net.Tests
{
    public class StubModule : NancyModule
    {
        public static TimeSpan HaltProcessing;
        public static List<Person> TestHarness;
        public static IDictionary<string, string> Cookie;
        public static RequestHeaders RequestHeaders;
        public static bool GetPerson = false;
        public static bool PostPerson = false;
        public static bool PutPerson = false;
        public static bool PatchPerson = false;
        public static bool DeletePerson = false;

        public StubModule()
        {
            Get["/Person/{id}"] = p =>
            {
                GetPerson = true;

                Cookie = Request.Cookies;
                RequestHeaders = Request.Headers;

                if (HaltProcessing != null)
                {
                    Thread.Sleep(HaltProcessing);
                }

                var person = TestHarness.Where(x => x.Id == p.id).FirstOrDefault();
                if (person != null)
                {
                    return person;
                }
                return 404;
            };

            Post["/Person"] = _ =>
            {
                PostPerson = true;
                Cookie = Request.Cookies;
                RequestHeaders = Request.Headers;
                Person person = this.Bind<Person>();
                TestHarness.Add(person);
                return 200;
            };

            Put["/Person/{id}"] = (p) =>
            {
                PutPerson = true;
                Cookie = Request.Cookies;
                RequestHeaders = Request.Headers;
                Person updatedPerson = this.Bind<Person>();
                var existingPerson = TestHarness.Where(x => x.Id == p.id).FirstOrDefault();
                existingPerson.Email = updatedPerson.Email;
                return 200;
            };

            Patch["/Person/{id}"] = (p) =>
            {
                PatchPerson = true;
                Cookie = Request.Cookies;
                RequestHeaders = Request.Headers;
                Person updatedPerson = this.Bind<Person>();
                var existingPerson = TestHarness.Where(x => x.Id == p.id).FirstOrDefault();
                existingPerson.Email = updatedPerson.Email;
                return 200;
            };

            Delete["/Person/{id}"] = (p) =>
            {
                DeletePerson = true;
                Cookie = Request.Cookies;
                RequestHeaders = Request.Headers;
                var existingPerson = TestHarness.Where(x => x.Id == p.id).FirstOrDefault();
                TestHarness.Remove(existingPerson);
                return 200;
            };
        }

    }
}
