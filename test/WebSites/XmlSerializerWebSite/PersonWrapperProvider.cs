using System;
using Microsoft.AspNet.Mvc;

namespace XmlSerializerWebSite
{
    public class PersonWrapperProvider : IWrapperProvider
    {
        public bool TryGetWrappingTypeForDeserialization(Type originalType, out Type wrappingType)
        {
            return TryGetWrappingType(originalType, out wrappingType);
        }

        public bool TryGetWrappingTypeForSerialization(Type originalType, out Type wrappingType)
        {
            return TryGetWrappingType(originalType, out wrappingType);
        }

        public object Unwrap(Type declaredType, object obj)
        {
            var personWrapper = obj as PersonWrapper;

            if (personWrapper == null)
            {
                return obj;
            }

            return new Person() { Id = personWrapper.Id, Name = personWrapper.Name };
        }

        public object Wrap(Type declaredType, object obj)
        {
            var person = obj as Person;

            if (person == null)
            {
                return obj;
            }

            return new PersonWrapper(person);
        }

        private static bool TryGetWrappingType(Type originalType, out Type wrappingType)
        {
            wrappingType = null;
            if (originalType == typeof(Person))
            {
                wrappingType = typeof(PersonWrapper);
                return true;
            }

            return false;
        }
    }
}