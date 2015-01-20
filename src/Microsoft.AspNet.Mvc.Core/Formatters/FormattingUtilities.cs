// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
#if ASPNET50
using System.Runtime.Serialization;
#endif
using System.Xml;

namespace Microsoft.AspNet.Mvc
{
    /// <summary>
    /// Contains methods which are used by input formatters.
    /// </summary>
    public static class FormattingUtilities
    {
        public static readonly int DefaultMaxDepth = 32;

#if ASPNET50
        public static readonly XsdDataContractExporter XsdDataContractExporter = new XsdDataContractExporter();
#endif

        /// <summary>
        /// Gets the default Reader Quotas for XmlReader.
        /// </summary>
        /// <returns>XmlReaderQuotas with default values</returns>
        public static XmlDictionaryReaderQuotas GetDefaultXmlReaderQuotas()
        {
            return new XmlDictionaryReaderQuotas()
            {
                MaxArrayLength = int.MaxValue,
                MaxBytesPerRead = int.MaxValue,
                MaxDepth = DefaultMaxDepth,
                MaxNameTableCharCount = int.MaxValue,
                MaxStringContentLength = int.MaxValue
            };
        }

        /// <summary>
        /// Gets the default XmlWriterSettings.
        /// </summary>
        /// <returns>Default <see cref="XmlWriterSettings"/></returns>
        public static XmlWriterSettings GetDefaultXmlWriterSettings()
        {
            return new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                CloseOutput = false,
                CheckCharacters = false
            };
        }

        public static WrapperInfo GetWrapperInformation(
            IEnumerable<IWrapperProvider> wrapperProviders,
            Type originalType,
            bool serialization)
        {
            IWrapperProvider wrappingProvider = null;
            Type wrappingType = null;
            if (serialization)
            {
                foreach (var wrapperProvider in wrapperProviders)
                {
                    if (wrapperProvider.TryGetWrappingTypeForSerialization(originalType, out wrappingType))
                    {
                        wrappingProvider = wrapperProvider;
                        break;
                    }
                }
            }
            else
            {
                foreach (var wrapperProvider in wrapperProviders)
                {
                    if (wrapperProvider.TryGetWrappingTypeForDeserialization(originalType, out wrappingType))
                    {
                        wrappingProvider = wrapperProvider;
                        break;
                    }
                }
            }

            return new WrapperInfo()
            {
                OriginalType = originalType,
                WrappingType = wrappingType,
                WrapperProvider = wrappingProvider
            };
        }
    }
}
