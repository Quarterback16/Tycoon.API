//using Employment.Web.Mvc.Infrastructure.Extensions;
//using Microsoft.VisualStudio.TestTools.UnitTesting;


//namespace Employment.Web.Mvc.Infrastructure.Tests.Extensions
//{
//    /// <summary>
//    ///This is a test class for MappingEngineExtensionTest and is intended
//    ///to contain all MappingEngineExtensionTest Unit Tests
//    ///</summary>
//    [TestClass]
//    public class MappingEngineExtensionTest
//    {
//        public class Destination
//        {
//            public string PropertyFromSource1 { get; set; }

//            public string PropertyFromSource2 { get; set; }
//        }

//        public class Source1
//        {
//            public string Property { get; set; }
//        }

//        public class Source2
//        {
//            public string Property { get; set; }
//        }

//        private IMappingEngine mappingEngine;

//        protected IMappingEngine MappingEngine
//        {
//            get
//            {
//                if (mappingEngine == null)
//                {
//                    var source1ToDestinationMapper = Mapper.CreateMap<Source1, Destination>();
//                    source1ToDestinationMapper.ForAllMembers(m => m.Ignore());
//                    source1ToDestinationMapper.ForMember(dest => dest.PropertyFromSource1, options => options.MapFrom(src => src.Property));

//                    var source2ToDestinationMapper = Mapper.CreateMap<Source2, Destination>();
//                    source2ToDestinationMapper.ForAllMembers(m => m.Ignore());
//                    source2ToDestinationMapper.ForMember(dest => dest.PropertyFromSource2, options => options.MapFrom(src => src.Property));

//                    mappingEngine = Mapper.Engine;
//                }

//                return mappingEngine;
//            }
//        }

//        [TestMethod]
//        public void MappingEngineExtension_MapManySourceObjectsToOneDestinationObject_MappedSuccessfully()
//        {
//            var source1 = new Source1 {Property = "ValueFromSource1" };
//            var source2 = new Source2 { Property = "ValueFromSource2" };

//            var destination = MappingEngine.Map<Destination>(source1, source2);

//            Assert.IsNotNull(destination);
//            Assert.IsTrue(destination.PropertyFromSource1.Equals(source1.Property));
//            Assert.IsTrue(destination.PropertyFromSource2.Equals(source2.Property));
//        }
//    }
//}