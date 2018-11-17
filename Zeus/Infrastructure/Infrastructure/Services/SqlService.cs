using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.Interfaces;
#if DEBUG
using StackExchange.Profiling;
#endif

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// Defines a service for interacting with data stored in a Sql database.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SqlService : ISqlService
    {
        /// <summary>
        /// Configuration manager for interacting with Connection Strings.
        /// </summary>
        protected readonly IConfigurationManager ConfigurationManager;


        /// <summary>
        /// Initializes a new instance of the <see cref="SqlService" /> class.
        /// </summary>
        /// <param name="configurationManager">Configuration manager for interacting with Connection Strings.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configurationManager"/> is <c>null</c>.</exception>
        public SqlService(IConfigurationManager configurationManager)
        {
            if (configurationManager == null)
            {
                throw new ArgumentNullException("configurationManager");
            }

            ConfigurationManager = configurationManager;

        }

        /// <summary>
        /// Executes the Sql command.
        /// </summary>
        /// <param name="connectionName">Name of the connection string in the Web.config.</param>
        /// <param name="commandName">Name of the command to execute.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        /// <returns>The number of records affected.</returns>
        public int ExecuteNonQuery(string connectionName, string commandName)
        {
            return ExecuteNonQuery(connectionName, commandName, Enumerable.Empty<SqlParameter>(), CommandType.StoredProcedure);
        }

        /// <summary>
        /// Executes the Sql command.
        /// </summary>
        /// <param name="connectionName">Name of the connection string in the Web.config.</param>
        /// <param name="commandName">Name of the command to execute.</param>
        /// <param name="commandType">The type of command. Default is <see cref="CommandType.StoredProcedure" />.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        /// <returns>The number of records affected.</returns>
        public int ExecuteNonQuery(string connectionName, string commandName, CommandType commandType)
        {
            return ExecuteNonQuery(connectionName, commandName, Enumerable.Empty<SqlParameter>(), commandType);
        }

        /// <summary>
        /// Executes the Sql command.
        /// </summary>
        /// <param name="connectionName">Name of the connection string in the Web.config.</param>
        /// <param name="commandName">Name of the command to execute.</param>
        /// <param name="parameters">The parameters to use with the command.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        /// <returns>The number of records affected.</returns>
        public int ExecuteNonQuery(string connectionName, string commandName, IEnumerable<SqlParameter> parameters)
        {
            return ExecuteNonQuery(connectionName, commandName, parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Executes the Sql command.
        /// </summary>
        /// <param name="connectionName">Name of the connection string in the Web.config.</param>
        /// <param name="commandName">Name of the command to execute.</param>
        /// <param name="parameters">The parameters to use with the command.</param>
        /// <param name="commandType">The type of command. Default is <see cref="CommandType.StoredProcedure" />.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        /// <returns>The number of records affected.</returns>
        public virtual int ExecuteNonQuery(string connectionName, string commandName, IEnumerable<SqlParameter> parameters, CommandType commandType)
        {
#if DEBUG
                var step = MiniProfiler.Current.Step("SqlService.ExecuteNonQuery: "+commandName);

            try
            {
#endif			
            if (string.IsNullOrEmpty(connectionName))
            {
                throw new ArgumentNullException("connectionName");
            }

            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }

            // Data to return
            int recordsAffected;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings(connectionName)))
            {
                using (SqlCommand command = new SqlCommand(commandName, connection))
                {
                    command.CommandType = commandType;

                    // Add parameters
                    command.Parameters.AddRange(parameters.ToArray());

                    connection.Open();

                    recordsAffected = command.ExecuteNonQuery();

                    command.Parameters.Clear();
                }
            }

            return recordsAffected;

#if DEBUG
            }
            finally
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }
#endif
        }

        /// <summary>
        /// Executes the Sql command and returns the results.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="connectionName">Name of the connection string in the Web.config.</param>
        /// <param name="commandName">Name of the command to execute.</param>
        /// <param name="mapping">The mapping.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        /// <returns>The result as the specified type.</returns>
        public IEnumerable<TModel> Execute<TModel>(string connectionName, string commandName, Func<IDataReader, TModel> mapping)
        {
            return Execute(connectionName, commandName, Enumerable.Empty<SqlParameter>(), CommandType.StoredProcedure, mapping);
        }

        /// <summary>
        /// Executes the Sql command and returns the results.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="connectionName">Name of the connection string in the Web.config.</param>
        /// <param name="commandName">Name of the command to execute.</param>
        /// <param name="commandType">The type of command. Default is <see cref="CommandType.StoredProcedure" />.</param>
        /// <param name="mapping"></param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        /// <returns>The result as the specified type.</returns>
        public IEnumerable<TModel> Execute<TModel>(string connectionName, string commandName, CommandType commandType, Func<IDataReader, TModel> mapping)
        {
            return Execute(connectionName, commandName, Enumerable.Empty<SqlParameter>(), commandType, mapping);
        }

        /// <summary>
        /// Executes the Sql command and returns the results.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="connectionName">Name of the connection string in the Web.config.</param>
        /// <param name="commandName">Name of the command to execute.</param>
        /// <param name="parameters">The parameters to use with the command.</param>
        /// <param name="mapping">The mapping.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        /// <returns>The result as the specified type.</returns>
        public IEnumerable<TModel> Execute<TModel>(string connectionName, string commandName, IEnumerable<SqlParameter> parameters, Func<IDataReader, TModel> mapping)
        {
            return Execute(connectionName, commandName, parameters, CommandType.StoredProcedure, mapping);
        }

        /// <summary>
        /// Executes the Sql command and returns the results.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="connectionName">Name of the connection string in the Web.config.</param>
        /// <param name="commandName">Name of the command to execute.</param>
        /// <param name="parameters">The parameters to use with the command.</param>
        /// <param name="commandType">The type of command. Default is <see cref="CommandType.StoredProcedure" />.</param>
        /// <param name="mapping">The mapping.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        /// <returns>The result as the specified type.</returns>
        public virtual IEnumerable<TModel> Execute<TModel>(string connectionName, string commandName, IEnumerable<SqlParameter> parameters, CommandType commandType, Func<IDataReader, TModel> mapping)
        {
#if DEBUG
            var step = MiniProfiler.Current.Step("SqlService.Execute: " + commandName);

            try
            {
#endif			

            if (string.IsNullOrEmpty(connectionName))
            {
                throw new ArgumentNullException("connectionName");
            }

            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }

            List<TModel> res = new List<TModel>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings(connectionName)))
            {
                using (SqlCommand command = new SqlCommand(commandName, connection))
                {
                    command.CommandType = commandType;

                    // Add parameters
                    command.Parameters.AddRange(parameters.ToArray());

                    connection.Open();

                    // Read result
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            res.Add( mapping(reader));
                        }
                    }
                }
            }

            return res;
#if DEBUG
            }
            finally
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }
#endif
        }

        ///// <summary>
        ///// Executes the Sql command and returns the multiple result sets, by placing them into the return object properties based on their assigned order (via ).
        ///// </summary>
        ///// <typeparam name="TModel">The type of the T model.</typeparam>
        ///// <param name="connectionName">Name of the connection string in the Web.config.</param>
        ///// <param name="commandName">Name of the command to execute.</param>
        ///// <param name="mapping">The mapping.</param>
        ///// <returns>
        ///// The result as the specified type.
        ///// </returns>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        
        //public IEnumerable<TModel> ExecuteMany<TModel>(string connectionName, string commandName, Func<IDataReader, TModel> mapping)
        //{
        //    return ExecuteMany<TModel>(connectionName, commandName, Enumerable.Empty<SqlParameter>(), CommandType.StoredProcedure, mapping);
        //}

        ///// <summary>
        ///// Executes the Sql command and returns the multiple result sets.
        ///// </summary>
        ///// <typeparam name="TModel">The type of the T model.</typeparam>
        ///// <param name="connectionName">Name of the connection string in the Web.config.</param>
        ///// <param name="commandName">Name of the command to execute.</param>
        ///// <param name="mapping">The mapping delegate.</param>
        ///// <param name="postProcessing">Delegate for execution per row in result set</param>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        ///// <returns>The result as the specified type.</returns>
        //public IEnumerable<TModel> ExecuteMany<TModel>(string connectionName, string commandName, Func<IDataReader, TModel> mapping, Action<TModel, IDataReader> postProcessing)
        //{
        //    return ExecuteMany<TModel>(connectionName, commandName, Enumerable.Empty<SqlParameter>(), CommandType.StoredProcedure, mapping, postProcessing);
        //}

        ///// <summary>
        ///// Executes the Sql command and returns the multiple result sets, by placing them into the return object properties based on their assigned order (via).
        ///// </summary>
        ///// <typeparam name="TModel">The type of the T model.</typeparam>
        ///// <param name="connectionName">Name of the connection string in the Web.config.</param>
        ///// <param name="commandName">Name of the command to execute.</param>
        ///// <param name="commandType">The type of command. Default is <see cref="CommandType.StoredProcedure" />.</param>
        ///// <param name="mapping">The mapping.</param>
        ///// <returns>
        ///// The result as the specified type.
        ///// </returns>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>

        //public IEnumerable<TModel> ExecuteMany<TModel>(string connectionName, string commandName, CommandType commandType, Func<IDataReader, TModel> mapping)
        //{
        //    return ExecuteMany<TModel>(connectionName, commandName, Enumerable.Empty<SqlParameter>(), commandType, mapping);
        //}

        ///// <summary>
        ///// Executes the Sql command and returns the multiple result sets.
        ///// </summary>
        ///// <typeparam name="TModel">The type of the T model.</typeparam>
        ///// <param name="connectionName">Name of the connection string in the Web.config.</param>
        ///// <param name="commandName">Name of the command to execute.</param>
        ///// <param name="commandType">The type of command. Default is <see cref="CommandType.StoredProcedure" />.</param>
        ///// <param name="mapping">The mapping.</param>
        ///// <param name="postProcessing">Delegate for execution per row in result set</param>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        ///// <returns>The result as the specified type.</returns>
        //public IEnumerable<TModel> ExecuteMany<TModel>(string connectionName, string commandName, CommandType commandType, Func<IDataReader, TModel> mapping, Action<TModel, IDataReader> postProcessing)
        //{
        //    return ExecuteMany<TModel>(connectionName, commandName, Enumerable.Empty<SqlParameter>(), commandType, mapping, null);
        //}

        ///// <summary>
        ///// Executes the Sql command and returns the multiple result sets, by placing them into the return object properties based on their assigned order (via />).
        ///// </summary>
        ///// <typeparam name="TModel">The type of the T model.</typeparam>
        ///// <param name="connectionName">Name of the connection string in the Web.config.</param>
        ///// <param name="commandName">Name of the command to execute.</param>
        ///// <param name="parameters">The parameters to use with the command.</param>
        ///// <param name="mapping">The mapping delegate.</param>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        ///// <returns>The result as the specified type.</returns>
        //public IEnumerable<TModel> ExecuteMany<TModel>(string connectionName, string commandName, IEnumerable<SqlParameter> parameters, Func<IDataReader, TModel> mapping)
        //{
        //    return ExecuteMany<TModel>(connectionName, commandName, parameters, CommandType.StoredProcedure, mapping);
        //}

        ///// <summary>
        ///// Executes the Sql command and returns the multiple result sets, by placing them into the return object properties based on their assigned order (via ).
        ///// </summary>
        ///// <typeparam name="TModel">The type of the T model.</typeparam>
        ///// <param name="connectionName">Name of the connection string in the Web.config.</param>
        ///// <param name="commandName">Name of the command to execute.</param>
        ///// <param name="parameters">The parameters to use with the command.</param>
        ///// <param name="commandType">The type of command. Default is <see cref="CommandType.StoredProcedure" />.</param>
        ///// <param name="mapping">The mapping delegate.</param>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        ///// <returns>The result as the specified type.</returns>
        //public virtual IEnumerable<TModel> ExecuteMany<TModel>(string connectionName, string commandName, IEnumerable<SqlParameter> parameters, CommandType commandType, Func<IDataReader, TModel> mapping)
        //{
        //    return ExecuteMany<TModel>(connectionName, commandName, parameters, commandType, mapping, null);
        //}

        ///// <summary>
        ///// Executes the Sql command and returns the multiple result sets.
        ///// </summary>
        ///// <typeparam name="TModel">The type of the T model.</typeparam>
        ///// <param name="connectionName">Name of the connection string in the Web.config.</param>
        ///// <param name="commandName">Name of the command to execute.</param>
        ///// <param name="parameters">The parameters to use with the command.</param>
        ///// <param name="mapping">The mapping delegate.</param>
        ///// <param name="postProcessing">Delegate for execution per row in result set</param>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        ///// <returns>The result as the specified type.</returns>
        //public IEnumerable<TModel> ExecuteMany<TModel>(string connectionName, string commandName, IEnumerable<SqlParameter> parameters, Func<IDataReader, TModel> mapping, Action<TModel, IDataReader> postProcessing)
        //{
        //    return ExecuteMany<TModel>(connectionName, commandName, parameters, CommandType.StoredProcedure, mapping, null);
        //}

        ///// <summary>
        ///// Executes the Sql command and returns the multiple result sets.
        ///// </summary>
        ///// <typeparam name="TModel">The type of the T model.</typeparam>
        ///// <param name="connectionName">Name of the connection string in the Web.config.</param>
        ///// <param name="commandName">Name of the command to execute.</param>
        ///// <param name="parameters">The parameters to use with the command.</param>
        ///// <param name="commandType">Type of the command.</param>
        ///// <param name="mapping">The mapping delegate.</param>
        ///// <param name="postProcessing">Delegate for execution per row in result set</param>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        ///// <returns>The result as the specified type.</returns>
        //public virtual IEnumerable<TModel> ExecuteMany<TModel>(string connectionName, string commandName, IEnumerable<SqlParameter> parameters, CommandType commandType, Func<IDataReader, TModel> mapping, Action<TModel, IDataReader> postProcessing)
        //{
        //    if (string.IsNullOrEmpty(connectionName))
        //    {
        //        throw new ArgumentNullException("connectionName");
        //    }

        //    if (string.IsNullOrEmpty(commandName))
        //    {
        //        throw new ArgumentNullException("commandName");
        //    }

        //    List<TModel> results = new List<TModel>();
        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings(connectionName)))
        //    {
        //        using (SqlCommand command = new SqlCommand(commandName, connection))
        //        {
        //            command.CommandType = commandType;

        //            // Add parameters
        //            command.Parameters.AddRange(parameters.ToArray());
        //            connection.Open();

        //            // Read result
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    var result = mapping(reader);
        //                    if (postProcessing != null)
        //                        postProcessing(result, reader);

        //                    results.Add(result);
        //                }
        //            }
        //        }
        //    }

        //    return results;
        //}
        
    }
}
