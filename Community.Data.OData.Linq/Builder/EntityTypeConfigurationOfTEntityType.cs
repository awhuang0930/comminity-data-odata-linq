﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace Community.OData.Linq.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using System.Reflection;

    using Microsoft.OData.Edm;

    /// <summary>
    /// Represents an <see cref="IEdmEntityType"/> that can be built using <see cref="ODataModelBuilder"/>.
    /// </summary>
    /// <typeparam name="TEntityType">The backing CLR type for this <see cref="IEdmEntityType"/>.</typeparam>
    public class EntityTypeConfiguration<TEntityType> : StructuralTypeConfiguration<TEntityType> where TEntityType : class
    {
        private EntityTypeConfiguration _configuration;
        private EntityCollectionConfiguration<TEntityType> _collection;
        private ODataModelBuilder _modelBuilder;

        /// <summary>
        /// Initializes a new instance of <see cref="EntityTypeConfiguration"/>.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ODataModelBuilder"/> being used.</param>
        internal EntityTypeConfiguration(ODataModelBuilder modelBuilder)
            : this(modelBuilder, new EntityTypeConfiguration(modelBuilder, typeof(TEntityType)))
        {
        }

        internal EntityTypeConfiguration(ODataModelBuilder modelBuilder, EntityTypeConfiguration configuration)
            : base(configuration)
        {
            Contract.Assert(modelBuilder != null);
            Contract.Assert(configuration != null);

            this._modelBuilder = modelBuilder;
            this._configuration = configuration;
            this._collection = new EntityCollectionConfiguration<TEntityType>(configuration);
        }

        /// <summary>
        /// Gets the base type of this entity type.
        /// </summary>
        public EntityTypeConfiguration BaseType
        {
            get
            {
                return this._configuration.BaseType;
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="NavigationPropertyConfiguration"/> of this entity type.
        /// </summary>
        public IEnumerable<NavigationPropertyConfiguration> NavigationProperties
        {
            get { return this._configuration.NavigationProperties; }
        }

        /// <summary>
        /// Used to access a Collection of Entities through which you can configure
        /// actions and functions that are bindable to EntityCollections.
        /// </summary>
        public EntityCollectionConfiguration<TEntityType> Collection
        {
            get { return this._collection; }
        }

        /// <summary>
        /// Marks this entity type as abstract.
        /// </summary>
        /// <returns>Returns itself so that multiple calls can be chained.</returns>
        public EntityTypeConfiguration<TEntityType> Abstract()
        {
            this._configuration.IsAbstract = true;
            return this;
        }

        /// <summary>
        /// Marks this entity type as media type.
        /// </summary>
        /// <returns>Returns itself so that multiple calls can be chained.</returns>
        public EntityTypeConfiguration<TEntityType> MediaType()
        {
            this._configuration.HasStream = true;
            return this;
        }

        /// <summary>
        /// Sets the base type of this entity type to <c>null</c> meaning that this entity type 
        /// does not derive from anything.
        /// </summary>
        /// <returns>Returns itself so that multiple calls can be chained.</returns>
        public EntityTypeConfiguration<TEntityType> DerivesFromNothing()
        {
            this._configuration.DerivesFromNothing();
            return this;
        }

        /// <summary>
        /// Sets the base type of this entity type.
        /// </summary>
        /// <typeparam name="TBaseType">The base entity type.</typeparam>
        /// <returns>Returns itself so that multiple calls can be chained.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "typeof(TBaseType) is used and getting it as a generic argument is cleaner")]
        public EntityTypeConfiguration<TEntityType> DerivesFrom<TBaseType>() where TBaseType : class
        {
            EntityTypeConfiguration<TBaseType> baseEntityType = this._modelBuilder.EntityType<TBaseType>();
            this._configuration.DerivesFrom(baseEntityType._configuration);
            return this;
        }

        /// <summary>
        /// Configures the key property(s) for this entity type.
        /// </summary>
        /// <typeparam name="TKey">The type of key.</typeparam>
        /// <param name="keyDefinitionExpression">A lambda expression representing the property to be used as the primary key. For example, in C# t => t.Id and in Visual Basic .Net Function(t) t.Id.</param>
        /// <returns>Returns itself so that multiple calls can be chained.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nested generic appropriate here")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Explicit Expression generic type is more clear")]
        public EntityTypeConfiguration<TEntityType> HasKey<TKey>(Expression<Func<TEntityType, TKey>> keyDefinitionExpression)
        {
            ICollection<PropertyInfo> properties = PropertySelectorVisitor.GetSelectedProperties(keyDefinitionExpression);
            foreach (PropertyInfo property in properties)
            {
                this._configuration.HasKey(property);
            }
            return this;
        }
    }
}
