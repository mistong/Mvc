// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNet.Mvc
{
    /// <summary>
    /// Helper class to serialize <see cref="IEnumerable{T}"/> types by delegating them through a concrete implementation."/>.
    /// </summary>
    /// <typeparam name="T">The interface implementing <see cref="IEnumerable{T}"/> to proxy.</typeparam>
    public sealed class DelegatingEnumerable<T> : IEnumerable<T>
    {
        private IEnumerable<object> _source;
        private readonly IWrapperProvider _wrapperProvider;
        private readonly Type _wrappedEnumerableElementType;

        /// <summary>
        /// Initialize a DelegatingEnumerable. This constructor is necessary for 
        /// <see cref="System.Runtime.Serialization.DataContractSerializer"/> to work.
        /// </summary>
        public DelegatingEnumerable()
        {
            _source = Enumerable.Empty<object>();
        }

        /// <summary>
        /// Initialize a DelegatingEnumerable with an <see cref="IEnumerable{T}"/>. This is a helper class to proxy <see cref="IEnumerable{T}"/> interfaces for <see cref="System.Xml.Serialization.XmlSerializer"/>.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{T}"/> instance to get the enumerator from.</param>
        /// <param name="wrapperProvider">The wrapper for wrapping individual items</param>
        public DelegatingEnumerable(Type wrappedEnumerableElementType, IEnumerable<object> source, IWrapperProvider wrapperProvider)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            _source = source;
            _wrapperProvider = wrapperProvider;
            _wrappedEnumerableElementType = wrappedEnumerableElementType;
        }

        /// <summary>
        /// Get the enumerator of the associated <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <returns>The enumerator of the <see cref="IEnumerable{T}"/> source.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new DelegatingEnumerator<T>(_wrappedEnumerableElementType, _source.GetEnumerator(), _wrapperProvider);
        }

        /// <summary>
        /// This method is not implemented but is required method for serialization to work. Do not use.
        /// </summary>
        /// <param name="item">The item to add. Unused.</param>
        public void Add(object item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the enumerator of the associated <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <returns>The enumerator of the <see cref="IEnumerable{T}"/> source.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new DelegatingEnumerator<T>(_wrappedEnumerableElementType, _source.GetEnumerator(), _wrapperProvider);
        }
    }
}