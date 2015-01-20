using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;

namespace XmlSerializerWebSite.Controllers
{
    public class WrapperController : Controller
    {
        public IEnumerable<string> IEnumerableOfNonWrappedTypes()
        {
            return new[] { "value1", "value2" };
        }

        public IEnumerable<Person> IEnumerableOfWrappedTypes()
        {
            return new[] {
                new Person() { Id = 10, Name = "Mike" },
                new Person() { Id = 11, Name = "Jimmy" }
            };
        }

        // ---------------------

        public IEnumerable<IEnumerable<string>> NestedIEnumerableOfNonWrappedTypes()
        {
            return new[] { new[] {
                "A",
                "B"
            },
            new[] {
                "C",
                "D"
            }};
        }

        public IEnumerable<IEnumerable<Person>> NestedIEnumerableOfWrappedTypes()
        {
            return new[] { new[] {
                new Person() { Id = 10, Name = "Mikey" },
                new Person() { Id = 11, Name = "Jimmy" }
            },
            new[] {
                new Person() { Id = 12, Name = "Johnny" },
                new Person() { Id = 13, Name = "Timmy" }
            }};
        }

        // --------------------

        public IQueryable<string> IQueryableOfNonWrappedTypes()
        {
            return Enumerable.Range(1, 2).Select(i => "value" + i).AsQueryable();
        }

        public IQueryable<Person> IQueryableOfWrappedTypes()
        {
            return new[] {
                new Person() { Id = 10, Name = "Mike" },
                new Person() { Id = 11, Name = "Jimmy" }
            }.AsQueryable();
        }

        // --------------------

        [HttpGet]
        public SerializableError SerializableError()
        {
            var error1 = new SerializableError();
            error1.Add("key1", "key1-error");
            error1.Add("key2", "key2-error");

            return error1;
        }

        [HttpGet]
        public IEnumerable<SerializableError> IEnumerableOfSerializableErrors()
        {
            List<SerializableError> errors = new List<SerializableError>();
            var error1 = new SerializableError();
            error1.Add("key1", "key1-error");
            error1.Add("key2", "key2-error");

            var error2 = new SerializableError();
            error2.Add("key3", "key1-error");
            error2.Add("key4", "key2-error");
            errors.Add(error1);
            errors.Add(error2);
            return errors;
        }

        [HttpPost]
        public IActionResult LogSerializableError([FromBody] SerializableError error)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            return new ObjectResult(error);
        }
    }
}