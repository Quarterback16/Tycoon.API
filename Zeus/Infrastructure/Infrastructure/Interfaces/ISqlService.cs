using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines the methods and properties that are required for a Sql Service.
    /// </summary>
    public interface ISqlService
    {
        /// <summary>
        /// Executes the Sql command.
        /// </summary>
        /// <param name="connectionName">Name of the connection string in the Web.config.</param>
        /// <param name="commandName">Name of the command to execute.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        /// <returns>The number of records affected.</returns>
        int ExecuteNonQuery(string connectionName, string commandName);

        /// <summary>
        /// Executes the Sql command.
        /// </summary>
        /// <param name="connectionName">Name of the connection string in the Web.config.</param>
        /// <param name="commandName">Name of the command to execute.</param>
        /// <param name="commandType">The type of command. Default is <see cref="CommandType.StoredProcedure" />.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        /// <returns>The number of records affected.</returns>
        int ExecuteNonQuery(string connectionName, string commandName, CommandType commandType);

        /// <summary>
        /// Executes the Sql command.
        /// </summary>
        /// <param name="connectionName">Name of the connection string in the Web.config.</param>
        /// <param name="commandName">Name of the command to execute.</param>
        /// <param name="parameters">The parameters to use with the command.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        /// <returns>The number of records affected.</returns>
        int ExecuteNonQuery(string connectionName, string commandName, IEnumerable<SqlParameter> parameters);

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
        int ExecuteNonQuery(string connectionName, string commandName, IEnumerable<SqlParameter> parameters, CommandType commandType);

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
        IEnumerable<TModel> Execute<TModel>(string connectionName, string commandName, Func<IDataReader, TModel> mapping);

        /// <summary>
        /// Executes the Sql command and returns the results.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="connectionName">Name of the connection string in the Web.config.</param>
        /// <param name="commandName">Name of the command to execute.</param>
        /// <param name="commandType">The type of command. Default is <see cref="CommandType.StoredProcedure" />.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns>
        /// The result as the specified type.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        IEnumerable<TModel> Execute<TModel>(string connectionName, string commandName, CommandType commandType, Func<IDataReader, TModel> mapping);

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
        IEnumerable<TModel> Execute<TModel>(string connectionName, string commandName, IEnumerable<SqlParameter> parameters, Func<IDataReader, TModel> mapping);

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
        IEnumerable<TModel> Execute<TModel>(string connectionName, string commandName, IEnumerable<SqlParameter> parameters, CommandType commandType, Func<IDataReader, TModel> mapping);

        ///// <summary>
        ///// Executes the Sql command and returns the multiple result sets.
        ///// </summary>
        ///// <typeparam name="TModel">The type of the T model.</typeparam>
        ///// <param name="connectionName">Name of the connection string in the Web.config.</param>
        ///// <param name="commandName">Name of the command to execute.</param>
        ///// <param name="mapping">The mapping.</param>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        ///// <returns>The result as the specified type.</returns>
        //IEnumerable<TModel> ExecuteMany<TModel>(string connectionName, string commandName, Func<IDataReader, TModel> mapping);

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
        //IEnumerable<TModel> ExecuteMany<TModel>(string connectionName, string commandName, Func<IDataReader, TModel> mapping, Action<TModel, IDataReader> postProcessing);

        ///// <summary>
        ///// Executes the Sql command and returns the multiple result sets.
        ///// </summary>
        ///// <typeparam name="TModel">The type of the T model.</typeparam>
        ///// <param name="connectionName">Name of the connection string in the Web.config.</param>
        ///// <param name="commandName">Name of the command to execute.</param>
        ///// <param name="commandType">The type of command. Default is <see cref="CommandType.StoredProcedure" />.</param>
        ///// <param name="mapping">The mapping.</param>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        ///// <returns>The result as the specified type.</returns>
        //IEnumerable<TModel> ExecuteMany<TModel>(string connectionName, string commandName, CommandType commandType, Func<IDataReader, TModel> mapping);

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
        //IEnumerable<TModel> ExecuteMany<TModel>(string connectionName, string commandName, CommandType commandType, Func<IDataReader, TModel> mapping, Action<TModel, IDataReader> postProcessing);

        ///// <summary>
        ///// Executes the Sql command and returns the multiple result sets.
        ///// </summary>
        ///// <typeparam name="TModel">The type of the T model.</typeparam>
        ///// <param name="connectionName">Name of the connection string in the Web.config.</param>
        ///// <param name="commandName">Name of the command to execute.</param>
        ///// <param name="parameters">The parameters to use with the command.</param>
        ///// <param name="mapping">The mapping delegate.</param>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        ///// <returns>The result as the specified type.</returns>
        //IEnumerable<TModel> ExecuteMany<TModel>(string connectionName, string commandName, IEnumerable<SqlParameter> parameters, Func<IDataReader, TModel> mapping);

        ///// <summary>
        ///// Executes the Sql command and returns the multiple result sets.
        ///// </summary>
        ///// <typeparam name="TModel">The type of the T model.</typeparam>
        ///// <param name="connectionName">Name of the connection string in the Web.config.</param>
        ///// <param name="commandName">Name of the command to execute.</param>
        ///// <param name="parameters">The parameters to use with the command.</param>
        ///// <param name="commandType">Type of the command.</param>
        ///// <param name="mapping">The mapping delegate.</param>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        ///// <returns>The result as the specified type.</returns>
        //IEnumerable<TModel> ExecuteMany<TModel>(string connectionName, string commandName, IEnumerable<SqlParameter> parameters, CommandType commandType, Func<IDataReader, TModel> mapping);

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
        //IEnumerable<TModel> ExecuteMany<TModel>(string connectionName, string commandName, IEnumerable<SqlParameter> parameters, Func<IDataReader, TModel> mapping, Action<TModel, IDataReader> postProcessing);

        ///// <summary>
        ///// Executes the Sql command and returns the multiple result sets.
        ///// </summary>
        ///// <typeparam name="TModel">The type of the T model.</typeparam>
        ///// <param name="connectionName">Name of the connection string in the Web.config.</param>
        ///// <param name="commandName">Name of the command to execute.</param>
        ///// <param name="parameters">The parameters to use with the command.</param>
        ///// <param name="commandType">The type of command. Default is <see cref="CommandType.StoredProcedure" />.</param>
        ///// <param name="mapping">The mapping delegate.</param>
        ///// <param name="postProcessing">Delegate for execution per row in result set</param>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionName" /> is <c>null</c>.</exception>
        ///// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName" /> is <c>null</c>.</exception>
        ///// <returns>The result as the specified type.</returns>
        //IEnumerable<TModel> ExecuteMany<TModel>(string connectionName, string commandName, IEnumerable<SqlParameter> parameters, CommandType commandType, Func<IDataReader, TModel> mapping, Action<TModel, IDataReader> postProcessing);
  
    }
}
