using Employment.Esc.Framework.DataTransport.Cics;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
#if DEBUG
using StackExchange.Profiling;
#endif

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// A service for calling CaGen wrappers.
    /// </summary>
    public class CaGenService : ICaGenService
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
        //public TResponse Execute<TRequest, TResponse>(TRequest request, string wrapperName, string action, bool throwFaultOnError) where TResponse : Esc.Shared.Contracts.Execution.IResponseWithExecutionResult
        //{
        //    return CicsClient.Execute<TRequest, TResponse>(request, wrapperName, action, throwFaultOnError);
        //}

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="wrapperName">Name of the wrapper.</param>
        /// <param name="action">The action.</param>
        /// <returns>
        /// The response type
        /// </returns>
        /// <exception cref="CaGenException">is thrown when cics returns error status.</exception>
        public TResponse Execute<TRequest, TResponse>(TRequest request, string wrapperName, string action)
        {
            CaGenExecutionResult executionResult;
            return Execute<TRequest, TResponse>(request, wrapperName, action, out executionResult);
        }


        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="wrapperName">Name of the wrapper.</param>
        /// <param name="action">The action.</param>
        /// <param name="executionResult">The execution result.</param>
        /// <returns>
        /// The response type
        /// </returns>
        /// <exception cref="CaGenException">is thrown when cics returns error status.</exception>
        public TResponse Execute<TRequest, TResponse>(TRequest request, string wrapperName, string action, out CaGenExecutionResult executionResult)
        {
 #if DEBUG
            var step = MiniProfiler.Current.Step("CaGenService.Execute: " + wrapperName + " " + action);

            try
            {
#endif			
           CicsExecutionResult genResult;
            var response= CicsClient.Execute<TRequest, TResponse>(request, wrapperName, action, out genResult);

            executionResult = new CaGenExecutionResult();
            executionResult.ErrorCode = genResult.ErrorCode;
            executionResult.ExitStatusNumber = genResult.ExitStatusNumber;
            executionResult.Message = genResult.Message;
            executionResult.UnMatchedProperties = genResult.UnMatchedProperties;
            if (genResult.CicsApplicationError != null)
            {
                executionResult.GenApplicationError = new CaGenApplicationError();
                executionResult.GenApplicationError.Attribute = genResult.CicsApplicationError.Attribute;
                executionResult.GenApplicationError.Date = genResult.CicsApplicationError.Date;
                executionResult.GenApplicationError.Number = genResult.CicsApplicationError.Number;
                executionResult.GenApplicationError.Text = genResult.CicsApplicationError.Text;
                executionResult.GenApplicationError.TextValue = genResult.CicsApplicationError.TextValue;
            }
            if (genResult.CicsSystemError != null)
            {
                executionResult.GenSystemError = new CaGenSystemError();
                executionResult.GenSystemError.ActionBlockName = genResult.CicsSystemError.ActionBlockName;
                executionResult.GenSystemError.ExitState = genResult.CicsSystemError.ExitState;
                executionResult.GenSystemError.StatementNumber = genResult.CicsSystemError.StatementNumber;
                executionResult.GenSystemError.Type = genResult.CicsSystemError.Type;
                executionResult.GenSystemError.UserId = genResult.CicsSystemError.UserId;
            }
            switch (genResult.MessageType)
            {
                case CicsExecuteStatus.Error:
                    executionResult.MessageType = CaGenExecuteStatus.Error;
                    break;
                case CicsExecuteStatus.Information:
                    executionResult.MessageType = CaGenExecuteStatus.Information;
                    break;
                case CicsExecuteStatus.Normal:
                    executionResult.MessageType = CaGenExecuteStatus.Normal;
                    break;
                case CicsExecuteStatus.Warning:
                    executionResult.MessageType = CaGenExecuteStatus.Warning;
                    break;
            }
            switch (genResult.RollbackStatus)
            {
                case RollbackStatus.Aborted:
                    executionResult.RollbackStatus = CaGenRollbackStatus.Aborted;
                    break;
                case RollbackStatus.Committed:
                    executionResult.RollbackStatus = CaGenRollbackStatus.Committed;
                    break;
                case RollbackStatus.RolledBack:
                    executionResult.RollbackStatus = CaGenRollbackStatus.RolledBack;
                    break;
            }

            if (executionResult.MessageType == CaGenExecuteStatus.Error)
            {
                throw new CaGenException(executionResult);
            }

            return response;
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
    }
}
