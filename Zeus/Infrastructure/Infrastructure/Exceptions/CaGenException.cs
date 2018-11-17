using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.Exceptions
{
    /// <summary>
    /// CaGen execution exception.
    /// </summary>
    public class CaGenException : Exception
    {
        /// <summary>
        /// The execution result
        /// </summary>
        public CaGenExecutionResult ExecutionResult;
        /// <summary>
        /// Initializes a new instance of the <see cref="CaGenException"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public CaGenException(CaGenExecutionResult result) : base(string.Format("Gen execution error. Code:{0} Message:{1} Type:{2}", result.ErrorCode, result.Message, result.MessageType.GetDescription()))
        {
            ExecutionResult = result;
        }
    }
}
