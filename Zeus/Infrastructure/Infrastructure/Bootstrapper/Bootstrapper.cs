using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Elmah;
using Employment.Web.Mvc.Infrastructure.Serialization;

namespace Employment.Web.Mvc.Infrastructure.Bootstrapper
{
    /// <summary>
    /// Bootstrapper that automates Application configuration in Global.asax.
    /// </summary>
    public abstract class Bootstrapper : IBootstrapper
    {
        private readonly object syncLock = new object();
        private static IContainerProvider container;

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        public IContainerProvider Container
        {
            [DebuggerStepThrough]
            get
            {
                if (container == null)
                {
                    lock (syncLock)
                    {
                        if (container == null)
                        {
                            container = CreateContainer();

                            DependencyResolver.SetResolver(container);
                        }
                    }
                }

                return container;
            }
        }

        /// <summary>
        /// Creates the container provider.
        /// </summary>
        /// <returns>The container provider.</returns>
        public abstract IContainerProvider CreateContainer();

        /// <summary>
        /// Bootstrapper start process to be called in Application_Start().
        /// </summary>
        public void Start()
        {
            // Resolve all Registrations, based on Order attributes position in sequence value, grouped so first in sequence are first
            var registrationGroups = Container.GetServices<IRegistration>().OrderBy(r => r.Order()).GroupBy(r => r.Group()).ToList();

            foreach (var registrationGroup in registrationGroups.OrderBy(r => r.Key))
            {
                // Perform each registration
                registrationGroup.ForEach(r => r.Register());
            }

            //// Resolve AutoMapper instance
            //var mapper = Container.GetService<IProfileExpression>();

            //// Resolve each Mapper and perform the mapping
            //Parallel.ForEach(Container.GetServices<IMapper>(), m => m.Map(mapper));

            //// Check mappings are valid
            //try
            //{
            //    Mapper.AssertConfigurationIsValid();
            //}
            //catch (AutoMapperConfigurationException ex)
            //{
            //    // Log invalid mapping
            //    ErrorLog.GetDefault(null).Log(new Error(ex));
            //}

            // Add the serialization surrogate to allow SelectListItem to be serialized
            var surrogateSelector = new SurrogateSelector();
            surrogateSelector.AddSurrogate(typeof(SelectListItem), new StreamingContext(StreamingContextStates.All), new SelectListItemSerializationSurrogate());
            SessionStateUtility.SerializationSurrogateSelector = surrogateSelector;


        }

        /// <summary>
        /// Bootstrapper end process to be called in Application_End().
        /// </summary>
        public void End()
        {
            
        }

        /// <summary>
        /// Dispose of Bootstrapper.
        /// </summary>
        public void Dispose()
        {
            if (Container != null)
            {
                Container.Dispose();
            }
        }
    }
}
