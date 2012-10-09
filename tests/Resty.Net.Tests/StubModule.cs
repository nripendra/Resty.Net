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
        public static bool GetPerson = false;
        public static bool PostPerson = false;
        public static bool PutPerson = false;
        public static bool DeletePerson = false;

        public StubModule()
        {
            Get["/Person/{id}"] = p =>
            {
                GetPerson = true;
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
                Person person = this.Bind<Person>();
                TestHarness.Add(person);
                return 200;
            };

            Put["/Person/{id}"] = (p) =>
            {
                PutPerson = true;
                Person updatedPerson = this.Bind<Person>();
                var existingPerson = TestHarness.Where(x => x.Id == p.id).FirstOrDefault();
                existingPerson.Email = updatedPerson.Email;
                return 200;
            };

            Delete["/Person/{id}"] = (p) =>
            {
                DeletePerson = true;
                var existingPerson = TestHarness.Where(x => x.Id == p.id).FirstOrDefault();
                TestHarness.Remove(existingPerson);
                return 200;
            };
        }

    }
}
