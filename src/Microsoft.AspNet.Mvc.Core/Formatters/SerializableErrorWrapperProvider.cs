using System;

namespace Microsoft.AspNet.Mvc
{
    public class SerializableErrorWrapperProvider : IWrapperProvider
    {
        public bool TryGetWrappingTypeForDeserialization(Type originalType, out Type wrappingType)
        {
            return TryGetWrappingType(originalType, out wrappingType);   
        }

        public bool TryGetWrappingTypeForSerialization(Type originalType, out Type wrappingType)
        {
            return TryGetWrappingType(originalType, out wrappingType);
        }

        public object Unwrap(Type declaredType, object wrapped)
        {
            var errorWrapper = wrapped as SerializableErrorWrapper;
            if (errorWrapper == null)
            {
                return wrapped;
            }

            return errorWrapper.SerializableError;
        }

        public object Wrap(Type declaredType, object original)
        {
            var error = original as SerializableError;
            if (error == null)
            {
                return original;
            }

            return new SerializableErrorWrapper(error);
        }

        private static bool TryGetWrappingType(Type originalType, out Type wrappingType)
        {
            wrappingType = null;
            if (originalType == typeof(SerializableError))
            {
                wrappingType = typeof(SerializableErrorWrapper);
                return true;
            }

            return false;
        }
    }
}