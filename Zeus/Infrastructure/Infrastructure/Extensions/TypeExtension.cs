using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using System.ComponentModel;
using Employment.Web.Mvc.Infrastructure.Helpers;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="System.Type"/>.
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// Gets the default value of this type.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns>The default value.</returns>
        public static object GetDefaultValue(this Type type)
        {
            if (type == typeof(string) || type.GetConstructor(Type.EmptyTypes) == null)
            {
                return null;
            }
            
            return DelegateHelper.CreateConstructorDelegate(type)();
        }

        /// <summary>
        /// Get the underlying non-nullable type.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns>Non-nullable type.</returns>
        public static Type GetNonNullableType(this Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);

            return underlyingType ?? type;
        }
            internal static readonly HashSet<Type> numericTypes = new HashSet<Type>
                                   {
                                       typeof(decimal),
                                       typeof(decimal?),
                                       typeof(float),
                                       typeof(float?),
                                       typeof(double),
                                       typeof(double?),
                                       typeof(byte),
                                       typeof(byte?),
                                       typeof(sbyte),
                                       typeof(sbyte?),
                                       typeof(short),
                                       typeof(short?),
                                       typeof(ushort),
                                       typeof(ushort?),
                                       typeof(int),
                                       typeof(int?),
                                       typeof(uint),
                                       typeof(uint?),
                                       typeof(long),
                                       typeof(long?),
                                       typeof(ulong),
                                       typeof(ulong?)
                                   };

        /// <summary>
        /// Returns whether the type is a numeric type.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns><c>true</c> if type is numeric otherwise, <c>false</c>.</returns>
        public static bool IsNumeric(this Type type)
        {

            return numericTypes.Contains(type);
        }

        /// <summary>
        /// Get the description of an enum value from the <see cref="DescriptionAttribute" /> if present; otherwise, the enum value as a string.
        /// </summary>
        /// <param name="type">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <returns>The enum description.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="type" /> is not a type of <see cref="Enum" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is not a type of <see cref="Enum" />.</exception>
        public static string GetEnumDescription(this Type type, object value)
        {
            if (!type.GetNonNullableType().IsEnum)
            {
                throw new ArgumentException("type must be an enum", "type");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (!value.GetType().GetNonNullableType().IsEnum)
            {
                throw new ArgumentException("value must be an enum", "type");
            }

            var attributes = type.GetNonNullableType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            var attribute = (attributes != null) ? attributes.FirstOrDefault() : null;

            return (attribute != null) ? attribute.Description : value.ToString();
        }

        /// <summary>
        /// Gets type attribute.
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute.</typeparam>
        /// <param name="type">Target type.</param>
        /// <returns>Attribute value</returns>
        public static TAttribute GetAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            return type.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault() as TAttribute;
        }

        /// <summary>
        /// Gets type attribute.
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute.</typeparam>
        /// <param name="type">Target type.</param>
        /// <param name="predicate">Filters attribute based on the predicate.</param>
        /// <returns>Attribute value</returns>
        public static TAttribute GetAttribute<TAttribute>(this Type type, Func<TAttribute, bool> predicate) where TAttribute : Attribute
        {
            return type.GetCustomAttributes(typeof(TAttribute), false).Cast<TAttribute>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// Gets type attributes.
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute.</typeparam>
        /// <param name="type">Target type.</param>
        /// <returns>Attribute values</returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this Type type) where TAttribute : Attribute
        {
            return type.GetCustomAttributes(typeof(TAttribute), false).Cast<TAttribute>();
        }

        /// <summary>
        /// Gets type attributes.
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute.</typeparam>
        /// <param name="type">Target type.</param>
        /// <param name="predicate">Filters attributes based on the predicate.</param>
        /// <returns>Attribute values</returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this Type type, Func<TAttribute, bool> predicate) where TAttribute : Attribute
        {
            return type.GetCustomAttributes(typeof(TAttribute), false).Cast<TAttribute>().Where(predicate);
        }

        /// <summary>
        /// Build Manager.
        /// </summary>
        private static IBuildManager BuildManager
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                if (containerProvider != null)
                {
                    return containerProvider.GetService<IBuildManager>();
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the public types that implement the specified type.
        /// </summary>
        /// <param name="type">The type to get the public implementations for.</param>
        /// <returns>The public types implementing the specified type.</returns>
        public static IEnumerable<Type> GetPublicTypesImplementing(this Type type)
        {
            return BuildManager.PublicTypes.Where(type.IsAssignableFrom);
        }

        /// <summary>
        /// Gets the concrete types that implement the specified type.
        /// </summary>
        /// <param name="type">The type to get the concrete implementations for.</param>
        /// <returns>The concrete types implementing the specified type.</returns>
        public static IEnumerable<Type> GetConcreteTypesImplementing(this Type type)
        {
            return BuildManager.ConcreteTypes.Where(type.IsAssignableFrom);
        }

        /// <summary>
        /// Determines whether the type is nullable.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns><c>true</c> if the type is nullable; otherwise, <c>false</c>.</returns>
        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        /// <summary>
        /// Determines whether the type is a collection.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns><c>true</c> if the type is a collection; otherwise, <c>false</c>.</returns>
        public static bool IsCollectionType(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>))
            {
                return true;
            }

            IEnumerable<Type> genericInterfaces = type.GetInterfaces().Where(t => t.IsGenericType);
            IEnumerable<Type> baseDefinitions = genericInterfaces.Select(t => t.GetGenericTypeDefinition());

            var isCollectionType = baseDefinitions.Any(t => t == typeof(ICollection<>));

            return isCollectionType;
        }

        /// <summary>
        /// Determines whether the type is enumerable.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns><c>true</c> if the type is enumerable; otherwise, <c>false</c>.</returns>
        public static bool IsEnumerableType(this Type type)
        {
	        Type type1 = typeof(IEnumerable);
	        foreach (Type @interface in type.GetInterfaces())
		        if (Equals(@interface, type1)) return true;
	        return false;
        }

        /// <summary>
        /// Determines whether the type is a list.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns><c>true</c> if the type is a list; otherwise, <c>false</c>.</returns>
        public static bool IsListType(this Type type)
        {
	        Type type1 = typeof(IList);
	        foreach (Type @interface in type.GetInterfaces())
		        if (Equals(@interface, type1)) return true;
	        return false;
        }

	    /// <summary>
        /// Determines whether the type is a list or dictionary.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns><c>true</c> if the type is a list or dictionary; otherwise, <c>false</c>.</returns>
        public static bool IsListOrDictionaryType(this Type type)
        {
            return type.IsListType() || type.IsDictionaryType();
        }

        /// <summary>
        /// Determines whether the type is a dictionary.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns><c>true</c> if the type is a dictionary; otherwise, <c>false</c>.</returns>
        public static bool IsDictionaryType(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
            {
                return true;
            }

            var genericInterfaces = type.GetInterfaces().Where(t => t.IsGenericType);
            var baseDefinitions = genericInterfaces.Select(t => t.GetGenericTypeDefinition());

            return baseDefinitions.Any(t => t == typeof(IDictionary<,>));
        }

        /// <summary>
        /// Get element type.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns>Inner element type if there is one; otherwse, current type.</returns>
        public static Type GetUnderlyingType(this Type type)
        {
            if (type.IsGenericType)
            {
                return type.HasElementType ? type.GetElementType() : type.GetGenericArguments().FirstOrDefault();
            }
            
            return type;
        }

        /// <summary>
        /// Find a property within the type, including a nested property (delimited by dot).
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <param name="propertyName">The property name to find.</param>
        /// <returns>If found, the <see cref="PropertyDescriptor" /> of the property; otherwise, null.</returns>
        public static PropertyDescriptor Find(this Type type, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return null;
            }

            var properties = TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>().ToList();

            // Check if property is nested
            if (propertyName.Contains('.'))
            {
                // Split nested property name into its nested segments
                var propertyNameSegments = propertyName.Split('.');

                PropertyDescriptor property = null;

                // Drill down to find the property
                foreach (var propertyNameSegment in propertyNameSegments)
                {
                    // Get matching property for current segment
                    property = properties.FirstOrDefault(p => p.Name.Equals(propertyNameSegment, StringComparison.InvariantCultureIgnoreCase));

                    if (property != null)
                    {
                        properties = property.GetChildProperties().Cast<PropertyDescriptor>().ToList();
                    }
                }

                // Will be populated if final segment (actual property) was found, otherwise null
                return property;
            }

            // Find matching property name, will be populated if found, otherwise null
            return properties.FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Find a property within the type, including a nested property (delimited by dot), and set it with a value.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <param name="propertyName">The property name to find.</param>
        /// <param name="obj">The target object.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>If found, the <see cref="PropertyDescriptor" /> of the property; otherwise, null.</returns>
        public static PropertyDescriptor FindAndSet(this Type type, string propertyName, object obj, object value)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName))
            {
                return null;
            }

            var properties = TypeDescriptor.GetProperties(obj).Cast<PropertyDescriptor>().ToList();
            PropertyDescriptor property = null;

            // Check if property is nested
            if (propertyName.Contains('.'))
            {
                // Split nested property name into its nested segments
                var propertyNameSegments = propertyName.Split('.');

                // Drill down to find the property
                foreach (var propertyNameSegment in propertyNameSegments)
                {
                    if (property != null && obj != null)
                    {
                        obj = property.GetValue(obj);
                    }

                    // Find matching property name for current segment, will be populated if found, otherwise null
                    property = properties.FirstOrDefault(p => p.Name.Equals(propertyNameSegment, StringComparison.InvariantCultureIgnoreCase));

                    if (property != null)
                    {
                        properties = property.GetChildProperties().Cast<PropertyDescriptor>().ToList();
                    }
                }
            }
            else
            {
                // Find matching property name, will be populated if found, otherwise null
                property = properties.FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
            }
            
            if (property != null && obj != null)
            {
                property.SetValue(obj, value);
            }

            return property;
        }
    }
}
