// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

    using System;

namespace Microsoft.AspNet.Mvc
{
    public interface IWrapperProvider
    {
        bool TryGetWrappingTypeForSerialization(Type originalType, out Type wrappingType);

        bool TryGetWrappingTypeForDeserialization(Type originalType, out Type wrappingType);

        object Wrap(Type type, object original);

        object Unwrap(Type type, object wrapped);
    }
}