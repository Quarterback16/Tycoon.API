using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Helpers;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Mappers;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System.Collections;

namespace Employment.Web.Mvc.Infrastructure.Services.Profiled
{
    /// <summary>
    /// Defines a service for interacting with data stored in a Sql database.
    /// </summary>
    /// <remarks>
    /// Profiled version of <see cref="Services.SqlService" />.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public class SqlService : Services.SqlService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlService" /> class.
        /// </summary>
        /// <param name="configurationManager">Configuration manager for interacting with Connection Strings.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configurationManager"/> is <c>null</c>.</exception>
        public SqlService(IConfigurationManager configurationManager) : base(configurationManager) { }
        
        ///// <summary>
        ///// Executes the Sql command.
        ///// </summary>
        ///// <remarks>
        ///// Identical to <see cref="SqlService" /> method but using <see cref="ProfiledDbConnection" /> instead to allow profiling.
        ///// </remarks>
        ///// <typeparam name="T">Type of item.</typeparam>
        ///// <param name="connectionName">Name of the connection string in the Web.config.</param>
        ///// <param name="commandName">Name of the command to execute.</param>
        ///// <param name="parameters">The parameters to use with the command.</param>
        ///// <param name="commandType">The type of command. Default is <see cref="CommandType.StoredProcedure" />.</param>
        ///// <returns>The number of records affected.</returns>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName"/> is <c>null</c>.</exception>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName"/> is <c>null</c>.</exception>
        //public override int ExecuteNonQuery<T>(string connectionName, string commandName, IEnumerable<SqlParameter> parameters, CommandType commandType)
        //{
        //    if (string.IsNullOrEmpty(connectionName))
        //    {
        //        throw new ArgumentNullException("connectionName");
        //    }

        //    if (string.IsNullOrEmpty(commandName))
        //    {
        //        throw new ArgumentNullException("commandName");
        //    }

        //    // Data to return
        //    int recordsAffected;

        //    using (DbConnection connection = new ProfiledDbConnection(new SqlConnection(ConfigurationManager.ConnectionStrings(connectionName)), MiniProfiler.Current))
        //    {
        //        using (DbCommand command = connection.CreateCommand())
        //        {
        //            command.CommandText = commandName;
        //            command.CommandType = commandType;

        //            // Add parameters
        //            command.Parameters.AddRange(parameters.ToArray());

        //            connection.Open();

        //            recordsAffected = command.ExecuteNonQuery();

        //            command.Parameters.Clear();
        //        }
        //    }

        //    return recordsAffected;
        //}

        ///// <summary>
        ///// Executes the Sql command and returns the results.
        ///// </summary>
        ///// <remarks>
        ///// Identical to <see cref="SqlService" /> method but using <see cref="ProfiledDbConnection" /> instead to allow profiling.
        ///// </remarks>
        ///// <typeparam name="T">Type of item.</typeparam>
        ///// <param name="connectionName">Name of the connection string in the Web.config.</param>
        ///// <param name="commandName">Name of the command to execute.</param>
        ///// <param name="parameters">The parameters to use with the command.</param>
        ///// <param name="commandType">The type of command. Default is <see cref="CommandType.StoredProcedure" />.</param>
        ///// <returns>The result as the specified type.</returns>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName"/> is <c>null</c>.</exception>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName"/> is <c>null</c>.</exception>
        //public override IEnumerable<T> Execute<T>(string connectionName, string commandName, IEnumerable<SqlParameter> parameters, CommandType commandType)
        //{
        //    if (string.IsNullOrEmpty(connectionName))
        //    {
        //        throw new ArgumentNullException("connectionName");
        //    }

        //    if (string.IsNullOrEmpty(commandName))
        //    {
        //        throw new ArgumentNullException("commandName");
        //    }

        //    // Data to return
        //    List<T> data = new List<T>();

        //    using (DbConnection connection = new ProfiledDbConnection(new SqlConnection(ConfigurationManager.ConnectionStrings(connectionName)), MiniProfiler.Current))
        //    {
        //        using (DbCommand command = connection.CreateCommand())
        //        {
        //            command.CommandText = commandName;
        //            command.CommandType = commandType;

        //            // Add parameters 
        //            command.Parameters.AddRange(parameters.ToArray());

        //            connection.Open();

        //            // Read result
        //            using (DbDataReader reader = command.ExecuteReader())
        //            {
        //                if (reader.HasRows)
        //                {
        //                    var properties = TypeDescriptor.GetProperties(typeof(T)).Cast<PropertyDescriptor>();

        //                    while (reader.Read())
        //                    {
        //                        var model = (T)DelegateHelper.CreateConstructorDelegate(typeof(T))();

        //                        foreach (var property in properties)
        //                        {
        //                            var alias = property.Name;

        //                            var aliasAttribute = property.Attributes.OfType<AliasAttribute>().FirstOrDefault();

        //                            if (aliasAttribute != null)
        //                            {
        //                                alias = aliasAttribute.Name;
        //                            }

        //                            if (reader.HasColumn(alias) && reader[alias] != null && reader[alias] != DBNull.Value)
        //                            {
        //                                if (property.PropertyType.GetNonNullableType() == reader[alias].GetType().GetNonNullableType())
        //                                {
        //                                    property.SetValue(model, reader[alias]);
        //                                }
        //                                else
        //                                {
        //                                    object value = StringMapper.ConvertString(reader[alias], reader[alias].GetType(), property.PropertyType);

        //                                    property.SetValue(model, value);
        //                                }
                                        
        //                            }
        //                        }
                                
        //                        data.Add(model);
        //                    }
        //                }
        //            }

        //            command.Parameters.Clear();
        //        }
        //    }

        //    return data;
        //}

        ///// <summary>
        ///// Executes the Sql command and returns the multiple result sets, by placing them into the return object properties based on their assigned order (via <see cref="OrderAttribute" />).
        ///// </summary>
        ///// <typeparam name="T">Type of item.</typeparam>
        ///// <param name="connectionName">Name of the connection string in the Web.config.</param>
        ///// <param name="commandName">Name of the command to execute.</param>
        ///// <param name="parameters">The parameters to use with the command.</param>
        ///// <param name="commandType">The type of command. Default is <see cref="CommandType.StoredProcedure" />.</param>
        ///// <returns>The result as the specified type.</returns>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName"/> is <c>null</c>.</exception>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName"/> is <c>null</c>.</exception>
        //public override T ExecuteMany<T>(string connectionName, string commandName, IEnumerable<SqlParameter> parameters, CommandType commandType)
        //{
        //    if (string.IsNullOrEmpty(connectionName))
        //    {
        //        throw new ArgumentNullException("connectionName");
        //    }

        //    if (string.IsNullOrEmpty(commandName))
        //    {
        //        throw new ArgumentNullException("commandName");
        //    }

        //    // Instantiate base model
        //    var baseModel = (T)DelegateHelper.CreateConstructorDelegate(typeof(T))();

        //    using (DbConnection connection = new ProfiledDbConnection(new SqlConnection(ConfigurationManager.ConnectionStrings(connectionName)), MiniProfiler.Current))
        //    {
        //        using (DbCommand command = connection.CreateCommand())
        //        {
        //            command.CommandText = commandName;
        //            command.CommandType = commandType;

        //            // Add parameters
        //            command.Parameters.AddRange(parameters.ToArray());

        //            connection.Open();

        //            // Read result
        //            using (DbDataReader reader = command.ExecuteReader())
        //            {
        //                if (reader.HasRows)
        //                {
        //                    // Get properties of base model and order based on [Order] for storing each result set
        //                    var baseProperties = TypeDescriptor.GetProperties(typeof(T)).Cast<PropertyDescriptor>().OrderBy(p =>
        //                    {
        //                        var orderAttribute = p.Attributes.OfType<OrderAttribute>().FirstOrDefault();

        //                        return orderAttribute != null ? orderAttribute.PositionInSequence : int.MaxValue;
        //                    });

        //                    foreach (var baseProperty in baseProperties)
        //                    {
        //                        var elementType = baseProperty.PropertyType.GetUnderlyingType();
        //                        var properties = TypeDescriptor.GetProperties(elementType).Cast<PropertyDescriptor>();

        //                        // Instantiate list for current result set
        //                        var data = (IList)DelegateHelper.CreateConstructorDelegate(typeof(List<>).MakeGenericType(new[] { elementType }))();

        //                        while (reader.Read())
        //                        {
        //                            // Instantiate model used by this result set
        //                            var model = DelegateHelper.CreateConstructorDelegate(elementType)();

        //                            foreach (var property in properties)
        //                            {
        //                                var alias = property.Name;

        //                                var aliasAttribute = property.Attributes.OfType<AliasAttribute>().FirstOrDefault();

        //                                if (aliasAttribute != null)
        //                                {
        //                                    alias = aliasAttribute.Name;
        //                                }

        //                                if (reader.HasColumn(alias) && reader[alias] != null && reader[alias] != DBNull.Value)
        //                                {
        //                                    if (property.PropertyType.GetNonNullableType() == reader[alias].GetType().GetNonNullableType())
        //                                    {
        //                                        property.SetValue(model, reader[alias]);
        //                                    }
        //                                    else
        //                                    {
        //                                        object value = StringMapper.ConvertString(reader[alias], reader[alias].GetType(), property.PropertyType);

        //                                        property.SetValue(model, value);
        //                                    }

        //                                }
        //                            }

        //                            data.Add(model);
        //                        }

        //                        // Assign result set to base model
        //                        baseProperty.SetValue(baseModel, data);

        //                        // Continue to next result set
        //                        if (!reader.NextResult())
        //                        {
        //                            // No next result set so exit
        //                            break;
        //                        }
        //                    }
        //                }
        //            }

        //            command.Parameters.Clear();
        //        }
        //    }

        //    return baseModel;
        //}
    }
}
