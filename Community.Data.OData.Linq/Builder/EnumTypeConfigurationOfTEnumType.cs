﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace Community.OData.Linq.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    using Microsoft.OData.Edm;

    /// <summary>
    /// Represents an <see cref="IEdmEnumType"/> that can be built using <see cref="ODataModelBuilder"/>.
    /// </summary>
    public class EnumTypeConfiguration<TEnumType>
    {
        private EnumTypeConfiguration _configuration;

        internal EnumTypeConfiguration(EnumTypeConfiguration configuration)
        {
            Contract.Assert(configuration != null);
            Contract.Assert(configuration.ClrType == typeof(TEnumType));
            this._configuration = configuration;
        }

        /// <summary>
        /// Gets the collection of EDM enum members that belong to this type.
        /// </summary>
        public IEnumerable<EnumMemberConfiguration> Members
        {
            get { return this._configuration.Members; }
        }

        /// <summary>
        /// Gets the full name of this EDM type.
        /// </summary>
        public string FullName
        {
            get
            {
                return this._configuration.FullName;
            }
        }

        /// <summary>
        /// Gets or sets the namespace of this EDM type.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Namespace", Justification = "Follow StructuralTypeConfiguration's naming")]
        public string Namespace
        {
            get
            {
                return this._configuration.Namespace;
            }
            set
            {
                this._configuration.Namespace = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of this EDM type.
        /// </summary>
        public string Name
        {
            get
            {
                return this._configuration.Name;
            }
            set
            {
                this._configuration.Name = value;
            }
        }

        /// <summary>
        /// Excludes a member from the type.
        /// </summary>
        /// <param name="member">The member being excluded.</param>
        /// <remarks>This method is used to exclude members from the enum type that would have been added by convention during model discovery.</remarks>
        public virtual void RemoveMember(TEnumType member)
        {
            this._configuration.RemoveMember((Enum)(object)member);
        }

        /// <summary>
        /// Adds a required enum member to the EDM type.
        /// </summary>
        /// <param name="enumMember">The member being added.</param>
        /// <returns>A configuration object that can be used to further configure the type.</returns>
        public EnumMemberConfiguration Member(TEnumType enumMember)
        {
            return this._configuration.AddMember((Enum)(object)enumMember);
        }
    }
}