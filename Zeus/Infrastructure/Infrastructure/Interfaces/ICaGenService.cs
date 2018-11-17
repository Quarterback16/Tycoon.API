using System;
using System.Collections.Generic;
using Employment.Esc.Shared.Contracts.Execution;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines the methods and properties that are required for a CaGen Service.
    /// </summary>
    public interface ICaGenService
    {
        ///// <summary>
        ///// Executes the specified request.
        ///// </summary>
        ///// <typeparam name="TRequest">The type of the request.</typeparam>
        ///// <typeparam name="TResponse">The type of the response.</typeparam>
        ///// <param name="request">The request.</param>
        ///// <param name="wrapperName">Name of the wrapper.</param>
        ///// <param name="action">The action.</param>
        ///// <param name="throwFaultOnError">if set to <c>true</c>, throw a fault exception on error.</param>
        ///// <returns></returns>
        //TResponse Execute<TRequest, TResponse>(TRequest request, string wrapperName, string action, bool throwFaultOnError) where TResponse : IResponseWithExecutionResult;

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="wrapperName">Name of the wrapper.</param>
        /// <param name="action">The action.</param>
        /// <returns>The response type</returns>
        TResponse Execute<TRequest, TResponse>(TRequest request, string wrapperName, string action);

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="wrapperName">Name of the wrapper.</param>
        /// <param name="action">The action.</param>
        /// <param name="executionResult">The execution result.</param>
        /// <returns>The response type</returns>
        TResponse Execute<TRequest, TResponse>(TRequest request, string wrapperName, string action, out CaGenExecutionResult executionResult);

        ///// <summary>
        ///// Executes the specified wrapper.
        ///// </summary>
        ///// <param name="wrapperName">Name of the wrapper.</param>
        ///// <param name="cvsRequest">The CVS request.</param>
        ///// <returns>The CVS response</returns>
        //string Execute(string wrapperName, string cvsRequest);
    }

    /// <summary>
    /// The Gen execution result
    /// </summary>
    public class CaGenExecutionResult {
        /// <summary> Gets or sets the type of the message. </summary>
        public CaGenExecuteStatus MessageType { get; set; }
        /// <summary> Gets or sets the message. </summary>
        public string Message { get; set; }
        /// <summary> Gets or sets the error code. </summary>
        public string ErrorCode { get; set; }
        /// <summary> Gets or sets the exit status number. </summary>
        public string ExitStatusNumber { get; set; }
        /// <summary> Gets or sets the rollback status. </summary>
        public CaGenRollbackStatus RollbackStatus { get; set; }
        /// <summary> Gets or sets the unmatched properties. </summary>
        public List<string> UnMatchedProperties { get; set; }
        /// <summary> Gets or sets the Gen application error. </summary>
        public CaGenApplicationError GenApplicationError { get; set; }
        /// <summary> Gets or sets the Gen system error. </summary>
        public CaGenSystemError GenSystemError { get; set; }
    }
    /// <summary>
    /// The rollback status
    /// </summary>
    public enum CaGenRollbackStatus {
        /// <summary>
        /// Committed status
        /// </summary>
        Committed = 0,
        /// <summary>
        /// Rolled back status
        /// </summary>
        RolledBack = 1,
        /// <summary>
        /// Aborted status
        /// </summary>
        Aborted = 2
    }
    /// <summary>
    /// The execution status
    /// </summary>
    public enum CaGenExecuteStatus {
        /// <summary>
        /// Normal status - success
        /// </summary>
        Normal = 0,
        /// <summary>
        /// Information status
        /// </summary>
        Information = 1,
        /// <summary>
        /// Warning status
        /// </summary>
        Warning = 2,
        /// <summary>
        /// Error status
        /// </summary>
        Error = 3
    }
    /// <summary>
    /// A CaGen application error
    /// </summary>
    public class CaGenApplicationError {
        /// <summary> Gets or sets the text. </summary>
        public string Text { get; set; }
        /// <summary> Gets or sets the attribute. </summary>
        public string Attribute { get; set; }
        /// <summary> Gets or sets the date. </summary>
        public DateTime Date { get; set; }
        /// <summary> Gets or sets the text value. </summary>
        public string TextValue { get; set; }
        /// <summary> Gets or sets the number. </summary>
        public long Number { get; set; }
    }
    /// <summary>
    /// A CaGen system error
    /// </summary>
    public class CaGenSystemError {
        /// <summary> Gets or sets the user identifier. </summary>
        public string UserId { get; set; }
        /// <summary> Gets or sets the type. </summary>
        public string Type { get; set; }
        /// <summary> Gets or sets the Exit State. </summary>
        public string ExitState { get; set; }
        /// <summary> Gets or sets the statement number. </summary>
        public int StatementNumber { get; set; }
        /// <summary> Gets or sets the name of the action block. </summary>
        public string ActionBlockName { get; set; }
    }

}
