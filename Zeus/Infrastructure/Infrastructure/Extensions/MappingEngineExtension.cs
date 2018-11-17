//

//namespace Employment.Web.Mvc.Infrastructure.Extensions
//{
//    /// <summary>
//    /// Extensions for <see cref="IMappingEngine" />.
//    /// </summary>
//    public static class MappingEngineExtension
//    {
//        /// <summary>
//        /// Map many source objects to one destination object.
//        /// </summary>
//        /// <typeparam name="T">Type of the destination object to map to.</typeparam>
//        /// <param name="mappingEngine">The mapping engine instance.</param>
//        /// <param name="sources">The source objects to map to the destination object.</param>
//        /// <returns>The mapped object.</returns>
//        public static T Map<T>(this IMappingEngine mappingEngine, params object[] sources) where T : class
//        {
//            return mappingEngine.Map<T>(default(T), sources);
//        }

//        /// <summary>
//        /// Map many source objects to one destination object.
//        /// </summary>
//        /// <typeparam name="T">Type of the destination object to map to.</typeparam>
//        /// <param name="mappingEngine">The mapping engine instance.</param>
//        /// <param name="destination">The destination object instance.</param>
//        /// <param name="sources">The source objects to map to the destination object.</param>
//        /// <returns>The mapped object.</returns>
//        public static T Map<T>(this IMappingEngine mappingEngine, T destination, params object[] sources) where T : class
//        {
//            foreach (var source in sources)
//            {
//                destination = mappingEngine.Map(source, destination, source.GetType(), typeof(T)) as T;
//            }

//            return destination;
//        }
//    }
//}