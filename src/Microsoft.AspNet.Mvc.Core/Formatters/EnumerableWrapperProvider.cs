// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.AspNet.Mvc
{
    public class EnumerableWrapperProvider : IWrapperProvider
    {
        private static readonly ConcurrentDictionary<Type, Type> _delegatingEnumerableCache = new ConcurrentDictionary<Type, Type>();
        private static ConcurrentDictionary<Type, ConstructorInfo> _delegatingEnumerableConstructorCache = new ConcurrentDictionary<Type, ConstructorInfo>();
        private readonly IEnumerable<IWrapperProvider> _wrapperProviders;

        public EnumerableWrapperProvider(IEnumerable<IWrapperProvider> wrapperProviders)
        {
            _wrapperProviders = wrapperProviders;
        }

        public bool TryGetWrappingTypeForDeserialization(Type originalType, out Type wrappingType)
        {
            wrappingType = null;

            return false;
        }

        // Examples: 
        // a. IEnumerable<SerializableError> => DelegatingEnumerable<SerializableErrorWrapper>
        // b. IEnumerable<Person> => DelegatingEnumerable<Person>
        public bool TryGetWrappingTypeForSerialization(Type originalType, out Type wrappingType)
        {
            return TryGetDelegatingType(originalType, out wrappingType);
        }

        /// <inheritdoc />
        public object Unwrap(Type declaredType, object obj)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public object Wrap(Type declaredType, object original)
        {
            Type delegatingType;
            if (TryGetDelegatingType(declaredType, out delegatingType))
            {
                // get the wrapper provider for the T itself
                var elementType = declaredType.GetGenericArguments()[0];

                var wrapperInfo = GetWrapperInfo(elementType);
                elementType = wrapperInfo.WrappingType ?? wrapperInfo.OriginalType;

                return GetTypeRemappingConstructor(delegatingType).Invoke(new object[] 
                    {
                        elementType,
                        original,
                        wrapperInfo.WrapperProvider
                    });
            }

            return original;
        }

        private bool TryGetDelegatingType(Type originalType, out Type wrappingType)
        {
            wrappingType = null;
            if (originalType != null && originalType.IsInterface() && originalType.IsGenericType())
            {
                // check if we can get a enumerable generic type. Example: IEnumerable<SerializableError>
                var genericType = originalType.ExtractGenericInterface(typeof(IEnumerable<>));
                if (genericType != null)
                {
                    wrappingType = GetOrAddDelegatingType(originalType, genericType);
                    return true;
                }
            }

            return false;
        }

        private Type GetOrAddDelegatingType(Type originalType, Type genericType)
        {
            return _delegatingEnumerableCache.GetOrAdd(
                originalType,
                (typeToRemap) =>
                {
                    // This retrieves the T type of the IEnumerable<T> interface.
                    var elementType = genericType.GetGenericArguments()[0];

                    var wrapperInfo = GetWrapperInfo(elementType);
                    elementType = wrapperInfo.WrappingType ?? wrapperInfo.OriginalType;
                    
                    var delegatingType = typeof(DelegatingEnumerable<>).MakeGenericType(elementType);
                    var delegatingConstructor = delegatingType.GetConstructor(new []
                    {
                        typeof(Type),
                        typeof(IEnumerable<object>),
                        typeof(IWrapperProvider)
                    });

                    _delegatingEnumerableConstructorCache.TryAdd(delegatingType, delegatingConstructor);

                    return delegatingType;
                });
        }

        private ConstructorInfo GetTypeRemappingConstructor(Type type)
        {
            ConstructorInfo constructorInfo;
            _delegatingEnumerableConstructorCache.TryGetValue(type, out constructorInfo);
            return constructorInfo;
        }

        private WrapperInfo GetWrapperInfo(Type originalType)
        {
            Type wrappingType;
            foreach (var wrapperProvider in _wrapperProviders)
            {
                if (wrapperProvider.TryGetWrappingTypeForSerialization(originalType, out wrappingType))
                {
                    return new WrapperInfo()
                    {
                        OriginalType = originalType,
                        WrapperProvider = wrapperProvider,
                        WrappingType = wrappingType
                    };
                }
            }

            return new WrapperInfo() { OriginalType = originalType };
        }
    }
}