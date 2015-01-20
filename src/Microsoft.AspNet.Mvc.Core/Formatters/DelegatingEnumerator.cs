// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.AspNet.Mvc
{
    public class DelegatingEnumerator<T> : IEnumerator<T>
    {
        private readonly IEnumerator<object> _inner;
        private readonly Type _originalType;
        private readonly IWrapperProvider _wrapperProvider;

        public DelegatingEnumerator(Type originalType, IEnumerator<object> inner, IWrapperProvider wrapperProvider)
        {
            _inner = inner;
            _wrapperProvider = wrapperProvider;
            _originalType = originalType;
        }

        public T Current
        {
            get
            {
                var obj = _inner.Current;
                if (_wrapperProvider == null)
                {
                    return (T)obj;
                }

                return (T)_wrapperProvider.Wrap(_originalType, obj);
            }
        }

        object IEnumerator.Current
        {
            get
            {
                var obj = _inner.Current;
                if (_wrapperProvider == null)
                {
                    return obj;
                }

               return _wrapperProvider.Wrap(_originalType, obj); 
            }
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public bool MoveNext()
        {
            return _inner.MoveNext();
        }

        public void Reset()
        {
            _inner.Reset();
        }
    }
}