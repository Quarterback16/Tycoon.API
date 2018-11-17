//using System;
//using System.Linq.Expressions;

//using AutoMapper.Mappers;
//using Employment.Web.Mvc.Infrastructure.Mappers;
//using Employment.Web.Mvc.Infrastructure.Extensions;
//#if DEBUG
//using StackExchange.Profiling;
//#endif

//namespace Employment.Web.Mvc.Infrastructure.Wrappers
//{
//    /// <summary>
//    /// Defines a wrapper of <see cref="IMappingEngine" /> to allow profiling of the mapping engine.
//    /// </summary>
//    public class MappingEngineWrapper : IMappingEngine
//    {
//        private readonly IMappingEngine mappingEngine;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="MappingEngineWrapper" /> class.
//        /// </summary>
//        public MappingEngineWrapper()
//        {
//            // Order is important
//            MapperRegistry.AllMappers = () => new IObjectMapper[] {
//                new DataReaderMapper(), 
//                new TypeMapMapper(TypeMapObjectMapperRegistry.AllMappers()), 
//                new AutoMapper.Mappers.StringMapper(), 
//                new FlagsEnumMapper(), 
//                new EnumMapper(), 
//                new ArrayMapper(), 
//                new PageableMapper(), // Include Infrastructure ObjectMapper
//                new EnumerableToDictionaryMapper(), 
//                new NameValueCollectionMapper(), 
//                new DictionaryMapper(), 
//                new ListSourceMapper(), 
//                new ReadOnlyCollectionMapper(), 
//                new CollectionMapper(), 
//                new EnumerableMapper(), 
//                new AssignableMapper(), 
//                new TypeConverterMapper(), 
//                new NullableSourceMapper(), 
//                new NullableMapper(), 
//                new ImplicitConversionOperatorMapper(), 
//                new ExplicitConversionOperatorMapper()
//            };

//            mappingEngine = Mapper.Engine;
//        }

//        /// <summary>
//        /// Map from source to destination.
//        /// </summary>
//        /// <typeparam name="TDestination">Destination type.</typeparam>
//        /// <param name="source">Source object.</param>
//        /// <returns>Destination object mapped from source object.</returns>
//        public TDestination Map<TDestination>(object source)
//        {
//#if DEBUG
//            var step = MiniProfiler.Current.Step(string.Format("MappingEngine: Map to {0}", typeof(TDestination).GetUnderlyingType().Name));

//            try
//            {
//#endif
//                return mappingEngine.Map<TDestination>(source);
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }

//        /// <summary>
//        /// Map from source to destination.
//        /// </summary>
//        /// <typeparam name="TDestination">Destination type.</typeparam>
//        /// <param name="source">Source object.</param>
//        /// <param name="opts">Mapping options.</param>
//        /// <returns>Destination object mapped from source object.</returns>
//        public TDestination Map<TDestination>(object source, Action<IMappingOperationOptions> opts)
//        {
//#if DEBUG
//            var step = MiniProfiler.Current.Step(string.Format("MappingEngine: Map to {0}", typeof(TDestination).GetUnderlyingType().Name));

//            try
//            {
//#endif
//                return mappingEngine.Map<TDestination>(source, opts);
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }

//        /// <summary>
//        /// Map from source to destination.
//        /// </summary>
//        /// <typeparam name="TSource">Source type.</typeparam>
//        /// <typeparam name="TDestination">Destination type.</typeparam>
//        /// <param name="source">Source object.</param>
//        /// <returns>Destination object mapped from source object.</returns>
//        public TDestination Map<TSource, TDestination>(TSource source)
//        {
//#if DEBUG
//            var step = MiniProfiler.Current.Step(string.Format("MappingEngine: Map {0} to {1}", typeof(TSource).GetUnderlyingType().Name, typeof(TDestination).GetUnderlyingType().Name));

//            try
//            {
//#endif
//                return mappingEngine.Map<TSource, TDestination>(source);
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }

//        /// <summary>
//        /// Map from source to destination.
//        /// </summary>
//        /// <typeparam name="TSource">Source type.</typeparam>
//        /// <typeparam name="TDestination">Destination type.</typeparam>
//        /// <param name="source">Source object.</param>
//        /// <param name="opts">Mapping options.</param>
//        /// <returns>Destination object mapped from source object.</returns>
//        public TDestination Map<TSource, TDestination>(TSource source, Action<IMappingOperationOptions> opts)
//        {
//#if DEBUG
//            var step = MiniProfiler.Current.Step(string.Format("MappingEngine: Map {0} to {1}", typeof(TSource).GetUnderlyingType().Name, typeof(TDestination).GetUnderlyingType().Name));

//            try
//            {
//#endif
//                return mappingEngine.Map<TSource, TDestination>(source, opts);
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }

//        /// <summary>
//        /// Map from source to destination.
//        /// </summary>
//        /// <typeparam name="TSource">Source type.</typeparam>
//        /// <typeparam name="TDestination">Destination type.</typeparam>
//        /// <param name="source">Source object.</param>
//        /// <param name="destination">Destination object.</param>
//        /// <returns>Destination object mapped from source object.</returns>
//        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
//        {
//#if DEBUG
//            var step = MiniProfiler.Current.Step(string.Format("MappingEngine: Map {0} to {1}", typeof(TSource).GetUnderlyingType().Name, typeof(TDestination).GetUnderlyingType().Name));

//            try
//            {
//#endif
//                return mappingEngine.Map<TSource, TDestination>(source, destination);
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }

//        /// <summary>
//        /// Map from source to destination.
//        /// </summary>
//        /// <typeparam name="TSource">Source type.</typeparam>
//        /// <typeparam name="TDestination">Destination type.</typeparam>
//        /// <param name="source">Source object.</param>
//        /// <param name="destination">Destination object.</param>
//        /// <param name="opts">Mapping options.</param>
//        /// <returns>Destination object mapped from source object.</returns>
//        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IMappingOperationOptions> opts)
//        {
//#if DEBUG
//            var step = MiniProfiler.Current.Step(string.Format("MappingEngine: Map {0} to {1}", typeof(TSource).GetUnderlyingType().Name, typeof(TDestination).GetUnderlyingType().Name));

//            try
//            {
//#endif
//                return mappingEngine.Map<TSource, TDestination>(source, destination, opts);
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }

//        /// <summary>
//        /// Map from source to destination.
//        /// </summary>
//        /// <param name="sourceType">Source type.</param>
//        /// <param name="destinationType">Destination type.</param>
//        /// <param name="source">Source object.</param>
//        /// <returns>Destination object mapped from source object.</returns>
//        public object Map(object source, Type sourceType, Type destinationType)
//        {
//#if DEBUG
//            var step = MiniProfiler.Current.Step(string.Format("MappingEngine: Map {0} to {1}", sourceType.GetUnderlyingType().Name, destinationType.GetUnderlyingType().Name));

//            try
//            {
//#endif
//                return mappingEngine.Map(source, sourceType, destinationType);
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }

//        /// <summary>
//        /// Map from source to destination.
//        /// </summary>
//        /// <param name="sourceType">Source type.</param>
//        /// <param name="destinationType">Destination type.</param>
//        /// <param name="source">Source object.</param>
//        /// <param name="opts">Mapping options.</param>
//        /// <returns>Destination object mapped from source object.</returns>
//        public object Map(object source, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts)
//        {
//#if DEBUG
//            var step = MiniProfiler.Current.Step(string.Format("MappingEngine: Map {0} to {1}", sourceType.GetUnderlyingType().Name, destinationType.GetUnderlyingType().Name));

//            try
//            {
//#endif
//                return mappingEngine.Map(source, sourceType, destinationType, opts);
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }

//        /// <summary>
//        /// Map from source to destination.
//        /// </summary>
//        /// <param name="sourceType">Source type.</param>
//        /// <param name="destinationType">Destination type.</param>
//        /// <param name="source">Source object.</param>
//        /// <param name="destination">Destination object.</param>
//        /// <returns>Destination object mapped from source object.</returns>
//        public object Map(object source, object destination, Type sourceType, Type destinationType)
//        {
//#if DEBUG
//            var step = MiniProfiler.Current.Step(string.Format("MappingEngine: Map {0} to {1}", sourceType.GetUnderlyingType().Name, destinationType.GetUnderlyingType().Name));

//            try
//            {
//#endif
//                return mappingEngine.Map(source, destination, sourceType, destinationType);
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }

//        /// <summary>
//        /// Map from source to destination.
//        /// </summary>
//        /// <param name="sourceType">Source type.</param>
//        /// <param name="destinationType">Destination type.</param>
//        /// <param name="source">Source object.</param>
//        /// <param name="destination">Destination object.</param>
//        /// <param name="opts">Mapping options.</param>
//        /// <returns>Destination object mapped from source object.</returns>
//        public object Map(object source, object destination, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts)
//        {
//#if DEBUG
//            var step = MiniProfiler.Current.Step(string.Format("MappingEngine: Map {0} to {1}", sourceType.GetUnderlyingType().Name, destinationType.GetUnderlyingType().Name));

//            try
//            {
//#endif
//                return mappingEngine.Map(source, destination, sourceType, destinationType, opts);
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }

//        /// <summary>
//        /// Dynamically map from source to destination.
//        /// </summary>
//        /// <typeparam name="TSource">Source type.</typeparam>
//        /// <typeparam name="TDestination">Destination type.</typeparam>
//        /// <param name="source">Source object.</param>
//        /// <returns>Destination object mapped from source object.</returns>
//        public TDestination DynamicMap<TSource, TDestination>(TSource source)
//        {
//#if DEBUG
//            var step = MiniProfiler.Current.Step(string.Format("MappingEngine: DynamicMap {0} to {1}", typeof(TSource).GetUnderlyingType().Name, typeof(TDestination).GetUnderlyingType().Name));

//            try
//            {
//#endif
//                return mappingEngine.DynamicMap<TSource, TDestination>(source);
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }

//        /// <summary>
//        /// Dynamically map from source to destination.
//        /// </summary>
//        /// <typeparam name="TDestination">Destination type.</typeparam>
//        /// <param name="source">Source object.</param>
//        /// <returns>Destination object mapped from source object.</returns>
//        public TDestination DynamicMap<TDestination>(object source)
//        {
//#if DEBUG
//            var step = MiniProfiler.Current.Step(string.Format("MappingEngine: DynamicMap to {0}", typeof(TDestination).GetUnderlyingType().Name));
            
//            try
//            {
//#endif
//                return mappingEngine.DynamicMap<TDestination>(source);
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }

//        /// <summary>
//        /// Dynamically map from source to destination.
//        /// </summary>
//        /// <param name="sourceType">Source type.</param>
//        /// <param name="destinationType">Destination type.</param>
//        /// <param name="source">Source object.</param>
//        /// <returns>Destination object mapped from source object.</returns>
//        public object DynamicMap(object source, Type sourceType, Type destinationType)
//        {
//#if DEBUG
//            var step = MiniProfiler.Current.Step(string.Format("MappingEngine: DynamicMap {0} to {1}", sourceType.GetUnderlyingType().Name, destinationType.GetUnderlyingType().Name));

//            try
//            {
//#endif
//                return mappingEngine.DynamicMap(source, sourceType, destinationType);
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }

//        /// <summary>
//        /// Dynamically map from source to destination.
//        /// </summary>
//        /// <typeparam name="TSource">Source type.</typeparam>
//        /// <typeparam name="TDestination">Destination type.</typeparam>
//        /// <param name="source">Source object.</param>
//        /// <param name="destination">Destination object.</param>
//        /// <returns>Destination object mapped from source object.</returns>
//        public void DynamicMap<TSource, TDestination>(TSource source, TDestination destination)
//        {
//#if DEBUG
//            var step = MiniProfiler.Current.Step(string.Format("MappingEngine: DynamicMap {0} to {1}", typeof(TSource).GetUnderlyingType().Name, typeof(TDestination).GetUnderlyingType().Name));

//            try
//            {
//#endif

//                mappingEngine.DynamicMap<TSource, TDestination>(source, destination);
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }

//        /// <summary>
//        /// Dynamically map from source to destination.
//        /// </summary>
//        /// <param name="sourceType">Source type.</param>
//        /// <param name="destinationType">Destination type.</param>
//        /// <param name="source">Source object.</param>
//        /// <param name="destination">Destination object.</param>
//        /// <returns>Destination object mapped from source object.</returns>
//        public void DynamicMap(object source, object destination, Type sourceType, Type destinationType)
//        {
//#if DEBUG
//            var step = MiniProfiler.Current.Step(string.Format("MappingEngine: DynamicMap {0} to {1}", sourceType.GetUnderlyingType().Name, destinationType.GetUnderlyingType().Name));

//            try
//            {
//#endif
//                mappingEngine.DynamicMap(source, destination, sourceType, destinationType);
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }

//        /// <summary>
//        /// Create map expression.
//        /// </summary>
//        /// <typeparam name="TSource">Source type.</typeparam>
//        /// <typeparam name="TDestination">Destination type.</typeparam>
//        /// <returns>Map expression.</returns>
//        public Expression<Func<TSource, TDestination>> CreateMapExpression<TSource, TDestination>()
//        {
//#if DEBUG
//            var step = MiniProfiler.Current.Step(string.Format("MappingEngine: CreateMapExpression {0} to {1}", typeof(TSource).GetUnderlyingType().Name, typeof(TDestination).GetUnderlyingType().Name));

//            try
//            {
//#endif
//                return mappingEngine.CreateMapExpression<TSource, TDestination>();
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }

//        /// <summary>
//        /// Dispose.
//        /// </summary>
//        public void Dispose()
//        {
//#if DEBUG
//            var step = MiniProfiler.Current.Step("MappingEngine: Dispose");

//            try
//            {
//#endif
//                mappingEngine.Dispose();
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }
//    }
//}
