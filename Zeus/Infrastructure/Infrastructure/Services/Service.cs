using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Wrappers;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Employment.Esc.Shared.Contracts.Execution;

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// Defines a service.
    /// </summary>
    public abstract class Service
    {
        /// <summary>
        /// Client <see cref="IClient"/>
        /// </summary>
        protected readonly IClient Client;


        private readonly ICacheService cacheService;

        private CacheServiceWrapper wrapper;

        /// <summary>
        /// Cache service <see cref="ICacheService" />
        /// </summary>
        /// <remarks>
        /// Uses <see cref="CacheServiceWrapper" /> to obey cache namespace per service enforcement.
        /// </remarks>
        public ICacheService CacheService
        {
            get
            {
                if (wrapper == null)
                {
                    // Namespace to service name
                    string @namespace = GetType().FullName;

                    wrapper = new CacheServiceWrapper(cacheService, @namespace);
                }

                return wrapper;
            }
        }

        /// <summary>
        /// User service <see cref="IUserService" />
        /// </summary>
        public IUserService UserService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IUserService>() : null;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Service" /> class.
        /// </summary>
        /// <param name="client">Client for interacting with WCF services.</param>
        /// <param name="cacheService">Cache service for interacting with cached data.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="client"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheService"/> is <c>null</c>.</exception>
        protected Service(IClient client,  ICacheService cacheService)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }


            if (cacheService == null)
            {
                throw new ArgumentNullException("cacheService");
            }

            Client = client;
            this.cacheService = cacheService; // Private
        }
        
        /// <summary>
        /// Validate the request using Enterprise library validation block
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="request"></param>
        protected void ValidateRequest<TRequest>(TRequest request)
        {
            ValidationResults results = Validation.Validate(request);

            if (!results.IsValid)
            {
                var errors = new Dictionary<string, string>();

                foreach (var result in results)
                {
                    var key = result.Key ?? string.Empty;
                    
                    if (!errors.ContainsKey(key))
                    {
                        errors.Add(key, result.Message);
                    }
                    else
                    {
                        errors[key] += string.Format("{0}{1}", Environment.NewLine, result.Message);
                    }
                }

                throw new ServiceValidationException(errors);
            }
        }

        /// <summary>
        /// Validate response
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="response"></param>
        protected void ValidateResponse<TResponse>(TResponse response) where TResponse : IResponseWithExecutionResult
        {
            response.Validate();
        }
    }
}
