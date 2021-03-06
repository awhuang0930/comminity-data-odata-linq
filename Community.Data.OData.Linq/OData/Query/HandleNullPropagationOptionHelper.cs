﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace Community.OData.Linq.OData.Query
{
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Community.OData.Linq.Common;

    internal static class HandleNullPropagationOptionHelper
    {
        private const string EntityFrameworkQueryProviderNamespace = "System.Data.Entity.Internal.Linq";

        private const string ObjectContextQueryProviderNamespaceEF5 = "System.Data.Objects.ELinq";
        private const string ObjectContextQueryProviderNamespaceEF6 = "System.Data.Entity.Core.Objects.ELinq";

        private const string Linq2SqlQueryProviderNamespace = "System.Data.Linq";
        internal const string Linq2ObjectsQueryProviderNamespace = "System.Linq";

        public static bool IsDefined(HandleNullPropagationOption value)
        {
            return value == HandleNullPropagationOption.Default ||
                   value == HandleNullPropagationOption.True ||
                   value == HandleNullPropagationOption.False;
        }

        public static void Validate(HandleNullPropagationOption value, string parameterValue)
        {
            if (!IsDefined(value))
            {
                throw Error.InvalidEnumArgument(parameterValue, (int)value, typeof(HandleNullPropagationOption));
            }
        }

        public static ODataQuerySettings UpdateQuerySettings(this ODataQueryContext context, ODataQuerySettings querySettings, IQueryable query)
        {
            ODataQuerySettings updatedSettings = new ODataQuerySettings();
            updatedSettings.CopyFrom(querySettings);

            if (updatedSettings.HandleNullPropagation == HandleNullPropagationOption.Default)
            {
                updatedSettings.HandleNullPropagation = query != null
                                                            ? HandleNullPropagationOptionHelper.GetDefaultHandleNullPropagationOption(query)
                                                            : HandleNullPropagationOption.True;
            }

            return updatedSettings;
        }

        public static HandleNullPropagationOption GetDefaultHandleNullPropagationOption(IQueryable query)
        {
            Contract.Assert(query != null);

            HandleNullPropagationOption options;

            string queryProviderNamespace = query.Provider.GetType().Namespace;
            switch (queryProviderNamespace)
            {
                case EntityFrameworkQueryProviderNamespace:
                case Linq2SqlQueryProviderNamespace:
                case ObjectContextQueryProviderNamespaceEF5:
                case ObjectContextQueryProviderNamespaceEF6:
                    options = HandleNullPropagationOption.False;
                    break;

                case Linq2ObjectsQueryProviderNamespace:
                default:
                    options = HandleNullPropagationOption.True;
                    break;
            }

            return options;
        }
    }
}