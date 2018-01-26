﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace Community.OData.Linq.Builder
{
    using System;

    using Community.OData.Linq.Common;
    using Community.OData.Linq.OData;
    using Community.OData.Linq.OData.Formatter;

    /// <summary>
    /// Represents a parameter to a Operation
    /// </summary>
    public abstract class ParameterConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterConfiguration"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="parameterType">The EDM type of the parameter.</param>
        protected ParameterConfiguration(string name, IEdmTypeConfiguration parameterType)
        {
            if (name == null)
            {
                throw Error.ArgumentNull("name");
            }
            if (parameterType == null)
            {
                throw Error.ArgumentNull("parameterType");
            }

            this.Name = name;
            this.TypeConfiguration = parameterType;

            Type elementType;
            this.OptionalParameter = parameterType.ClrType.IsCollection(out elementType)
                ? EdmLibHelpers.IsNullable(elementType)
                : EdmLibHelpers.IsNullable(parameterType.ClrType);
        }

        /// <summary>
        /// The name of the parameter
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The type of the parameter
        /// </summary>
        public IEdmTypeConfiguration TypeConfiguration { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this parameter is optional or not.
        /// </summary>
        public bool OptionalParameter { get; set; }
    }
}
