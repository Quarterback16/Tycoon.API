using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Mappers;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Properties;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="AdwSelectionAttribute" />.
    /// </summary>
    [TestClass]
    public class AdwSelectionAttributeTest
    {
        private class Model : ContingentValidationModel<AdwSelectionAttribute>
        {
            [AdwSelection(SelectionType.None, AdwType.ListCode, "FOO", ErrorMessageResourceName = "AdwSelectionAttribute_Invalid", ErrorMessageResourceType = typeof(DataAnnotationsResources))]
            public string UsingErrorResource { get; set; }

            [AdwSelection(SelectionType.Single, AdwType.ListCode, "FOO", ErrorMessage = "My error.")]
            public string UsingErrorMessage { get; set; }

            [AdwSelection(SelectionType.Single, AdwType.ListCode, "FOO")]
            public string SingleListCode { get; set; }

            [AdwSelection(SelectionType.Multiple, AdwType.ListCode, "FOO")]
            public IEnumerable<string> MultipleListCode { get; set; }

            [AdwSelection(SelectionType.Single, AdwType.RelatedCode, "FOOO")]
            public string RelatedCodeNoDependentPropertyOrDependentValue { get; set; }

            [AdwSelection(SelectionType.Single, AdwType.RelatedCode, "FOOO", Dominant = true, DependentProperty = "SingleListCode")]
            public string SingleRelatedCodeDependentProperty { get; set; }

            [AdwSelection(SelectionType.Multiple, AdwType.RelatedCode, "FOOO", Dominant = true, DependentProperty = "SingleListCode")]
            public IEnumerable<string> MultipleRelatedCodeDependentProperty { get; set; }

            [AdwSelection(SelectionType.Single, AdwType.RelatedCode, "FOOO", Dominant = true, DependentValue = "Code")]
            public string SingleRelatedCodeDependentValue { get; set; }

            [AdwSelection(SelectionType.Multiple, AdwType.RelatedCode, "FOOO", Dominant = true, DependentValue = "Code")]
            public IEnumerable<string> MultipleRelatedCodeDependentValue { get; set; }

            [AdwSelection(SelectionType.None, AdwType.ListCode, "FOO", DisplayType = AdwDisplayType.Code)]
            public string ListCodeDisplayCode { get; set; }

            [AdwSelection(SelectionType.None, AdwType.ListCode, "FOO", DisplayType = AdwDisplayType.CodeAndDescription)]
            public string ListCodeDisplayCodeAndDescription { get; set; }

            [AdwSelection(SelectionType.None, AdwType.ListCode, "FOO", DisplayType = AdwDisplayType.CodeAndShortDescription)]
            public string ListCodeDisplayCodeAndShortDescription { get; set; }

            [AdwSelection(SelectionType.None, AdwType.ListCode, "FOO", DisplayType = AdwDisplayType.Description)]
            public string ListCodeDisplayDescription { get; set; }

            [AdwSelection(SelectionType.None, AdwType.ListCode, "FOO", DisplayType = AdwDisplayType.ShortDescription)]
            public string ListCodeDisplayShortDescription { get; set; }

            [AdwSelection(SelectionType.None, AdwType.RelatedCode, "FOOO", DependentValue = "Code", DisplayType = AdwDisplayType.Code)]
            public string RelatedCodeDisplayCode { get; set; }

            [AdwSelection(SelectionType.None, AdwType.RelatedCode, "FOOO", DependentValue = "Code", DisplayType = AdwDisplayType.CodeAndDescription)]
            public string RelatedCodeDisplayCodeAndDescription { get; set; }

            [AdwSelection(SelectionType.None, AdwType.RelatedCode, "FOOO", DependentValue = "Code", DisplayType = AdwDisplayType.CodeAndShortDescription)]
            public string RelatedCodeDisplayCodeAndShortDescription { get; set; }

            [AdwSelection(SelectionType.None, AdwType.RelatedCode, "FOOO", DependentValue = "Code", DisplayType = AdwDisplayType.Description)]
            public string RelatedCodeDisplayDescription { get; set; }

            [AdwSelection(SelectionType.None, AdwType.RelatedCode, "FOOO", DependentValue = "Code", DisplayType = AdwDisplayType.ShortDescription)]
            public string RelatedCodeDisplayShortDescription { get; set; }

            [AdwSelection(SelectionType.None, AdwType.RelatedCode, "FOOO", DependentProperty = "SingleListCode", DisplayType = AdwDisplayType.Code)]
            public string RelatedCodeDisplayCodeWithDependentProperty { get; set; }

            [AdwSelection(SelectionType.Single, AdwType.ListCode, "FOO", ExcludeValues = new[] { "Foo", "Bar" })]
            public string SingleListCodeExcludeValues { get; set; }

            [AdwSelection(SelectionType.Single, AdwType.RelatedCode, "FOOO", DependentValue = "Code", ExcludeValues = new[] { "FooSubordinateCode", "BarSubordinateCode" })]
            public string SingleRelatedCodeExcludeValues { get; set; }

            [AdwSelection(SelectionType.Multiple, AdwType.ListCode, "FOO", ExcludeValues = new[] { "Foo", "Bar" })]
            public IEnumerable<string> MultipleListCodeExcludeValues { get; set; }

            [AdwSelection(SelectionType.Multiple, AdwType.RelatedCode, "FOOO", DependentValue = "Code", ExcludeValues = new[] { "FooSubordinateCode", "BarSubordinateCode" })]
            public IEnumerable<string> MultipleRelatedCodeExcludeValues { get; set; }
        }

        //private IMappingEngine mappingEngine;

        //protected IMappingEngine MappingEngine
        //{
        //    get
        //    {
        //        if (mappingEngine == null)
        //        {
        //            var mapper = new AdwMapper();
        //            mapper.Map(Mapper.Configuration);
        //            mappingEngine = Mapper.Engine;
        //        }

        //        return mappingEngine;
        //    }
        //}

        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IAdwService> mockAdwService;
        private Mock<IAdwAdminService> mockAdwAdminService;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockContainerProvider = new Mock<IContainerProvider>();
            mockAdwService = new Mock<IAdwService>();
            mockAdwAdminService = new Mock<IAdwAdminService>();

            // Setup Dependency Resolver to use mocked Container Provider
            mockContainerProvider.Setup(m => m.GetService<IAdwService>()).Returns(mockAdwService.Object);
            mockContainerProvider.Setup(m => m.GetService<IAdwAdminService>()).Returns(mockAdwAdminService.Object);
            //mockContainerProvider.Setup(m => m.GetService<IMappingEngine>()).Returns(MappingEngine);
            DependencyResolver.SetResolver(mockContainerProvider.Object);
        }

        /// <summary>
        /// Test property null reference exception when Dependency Resolver does not have the IAdwService registered.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AdwSelection_NoAdwServiceInDependencyResolver_ThrowsNullReferenceException()
        {
            mockContainerProvider = new Mock<IContainerProvider>();
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            var model = new Model() { SingleListCode = "Code" };

            model.IsValid("SingleListCode");
        }

        /// <summary>
        /// Test property null reference exception when Dependency Resolver is not a Container Provider.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AdwSelection_NoContainerProviderInDependencyResolver_ThrowsNullReferenceException()
        {
            var mockDependencyResolver = new Mock<IDependencyResolver>();
            DependencyResolver.SetResolver(mockDependencyResolver.Object);

            var model = new Model() { SingleListCode = "Code" };

            model.IsValid("SingleListCode");
        }

        /// <summary>
        /// Test property fails and uses the specified error resource.
        /// </summary>
        [TestMethod]
        public void AdwSelection_UsingErrorResource_UsesErrorResource()
        {
            var codes = new[]
                            {
                                new CodeModel
                                    {
                                        Code = "NotExist",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { UsingErrorResource = "Code" };

            var attribute = model.GetAttribute("UsingErrorResource");

            var result = attribute.GetValidationResult(model.GetType().GetProperty("UsingErrorResource").GetValue(model, null), new ValidationContext(this, null, null));

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test property fails and uses the specified error message.
        /// </summary>
        [TestMethod]
        public void AdwSelection_UsingErrorMessage_UsesErrorMessage()
        {
            var codes = new[]
                            {
                                new CodeModel
                                    {
                                        Code = "NotExist",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { UsingErrorMessage = "Code" };

            var attribute = model.GetAttribute("UsingErrorMessage");

            var result = attribute.GetValidationResult(model.GetType().GetProperty("UsingErrorMessage").GetValue(model, null), new ValidationContext(this, null, null));

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ErrorMessage == "My error.");
        }

        #region Single selection List Code

        /// <summary>
        /// Test property validates if single selected list code value exists in the Adw List Code table and is not an excluded value.
        /// </summary>
        [TestMethod]
        public void AdwSelection_SingleSelectionListCodeExistsWithExcludeValuesButNotSelected_Validates()
        {
            var codes = new[]
                            {
                                new CodeModel
                                    {
                                        Code = "Code",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    },
                                new CodeModel
                                    {
                                        Code = "Foo",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    },
                                new CodeModel
                                    {
                                        Code = "Bar",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCodeExcludeValues = "Code" };

            Assert.IsTrue(model.IsValid("SingleListCodeExcludeValues"));
        }

        /// <summary>
        /// Test property fails if single selected list code value exists in the Adw List Code table but is an excluded value.
        /// </summary>
        [TestMethod]
        public void AdwSelection_SingleSelectionListCodeExistsWithExcludeValueSelected_Fails()
        {
            var codes = new[]
                            {
                                new CodeModel
                                    {
                                        Code = "Code",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    },
                                new CodeModel
                                    {
                                        Code = "Foo",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    },
                                new CodeModel
                                    {
                                        Code = "Bar",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCodeExcludeValues = "Foo" };

            Assert.IsFalse(model.IsValid("SingleListCodeExcludeValues"));
        }

        /// <summary>
        /// Test property validates if single selected list code value exists in the Adw List Code table.
        /// </summary>
        [TestMethod]
        public void AdwSelection_SingleSelectionListCodeExists_Validates()
        {
            var codes = new[]
                            {
                                new CodeModel
                                    {
                                        Code = "Code",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = "Code" };

            Assert.IsTrue(model.IsValid("SingleListCode"));
        }

        /// <summary>
        /// Test property validates if single selected list code value is not selected.
        /// </summary>
        [TestMethod]
        public void AdwSelection_SingleSelectionListCodeNoSelection_Validates()
        {
            var codes = new[]
                            {
                                new CodeModel
                                    {
                                        Code = "Code",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = string.Empty };

            Assert.IsTrue(model.IsValid("SingleListCode"));
        }

        /// <summary>
        /// Test property fails if single selected list code value does not exist in the Adw List Code table.
        /// </summary>
        [TestMethod]
        public void AdwSelection_SingleSelectionListCodeDoesNotExist_Fails()
        {
            var codes = new[]
                            {
                                new CodeModel
                                    {
                                        Code = "NotExist",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = "Code" };

            Assert.IsFalse(model.IsValid("SingleListCode"));
        }

        #endregion

        #region Multiple selection List Code

        /// <summary>
        /// Test property validates if multiple selected list code value exists in the Adw List Code table and is not an excluded value.
        /// </summary>
        [TestMethod]
        public void AdwSelection_MultipleSelectionListCodeExistsWithExcludeValuesButNotSelected_Validates()
        {
            var codes = new[]
                            {
                                new CodeModel
                                    {
                                        Code = "Code",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    },
                                new CodeModel
                                    {
                                        Code = "Foo",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    },
                                new CodeModel
                                    {
                                        Code = "Bar",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { MultipleListCodeExcludeValues = new[] { "Code" } };

            Assert.IsTrue(model.IsValid("MultipleListCodeExcludeValues"));
        }

        /// <summary>
        /// Test property fails if multiple selected list code value exists in the Adw List Code table but is an excluded value.
        /// </summary>
        [TestMethod]
        public void AdwSelection_MultipleSelectionListCodeExistsWithExcludeValueSelected_Fails()
        {
            var codes = new[]
                            {
                                new CodeModel
                                    {
                                        Code = "Code",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    },
                                new CodeModel
                                    {
                                        Code = "Foo",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    },
                                new CodeModel
                                    {
                                        Code = "Bar",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { MultipleListCodeExcludeValues = new [] { "Foo" } };

            Assert.IsFalse(model.IsValid("MultipleListCodeExcludeValues"));
        }

        /// <summary>
        /// Test property validates if multiple selected list code value exists in the Adw List Code table.
        /// </summary>
        [TestMethod]
        public void AdwSelection_MultipleSelectionListCodeExists_Validates()
        {
            var codes = new[]
                            {
                                new CodeModel
                                    {
                                        Code = "Code",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { MultipleListCode = new [] { "Code" } };

            Assert.IsTrue(model.IsValid("MultipleListCode"));
        }

        /// <summary>
        /// Test property validates if multiple selected list code value is not selected.
        /// </summary>
        [TestMethod]
        public void AdwSelection_MultipleSelectionListCodeNoSelection_Validates()
        {
            var codes = new[]
                            {
                                new CodeModel
                                    {
                                        Code = "Code",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { MultipleListCode = new [] { string.Empty }};

            Assert.IsTrue(model.IsValid("MultipleListCode"));
        }

        /// <summary>
        /// Test property fails if multiple selected list code value does not exist in the Adw List Code table.
        /// </summary>
        [TestMethod]
        public void AdwSelection_MultipleSelectionListCodeDoesNotExist_Fails()
        {
            var codes = new[]
                            {
                                new CodeModel
                                    {
                                        Code = "NotExist",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { MultipleListCode = new[] { "Code" } };

            Assert.IsFalse(model.IsValid("MultipleListCode"));
        }

        #endregion

        #region Related Code with no dependent property or value

        /// <summary>
        /// Test property validates if single selected related code value exists in the Adw Related Code table and is not an excluded value.
        /// </summary>
        [TestMethod]
        public void AdwSelection_SingleSelectionRelatedCodeExistsWithExcludeValuesButNotSelected_Validates()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    },
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "FooDominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "FooSubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    },
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "BarDominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "BarSubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleRelatedCodeExcludeValues = "SubordinateCode" };

            Assert.IsTrue(model.IsValid("SingleRelatedCodeExcludeValues"));
        }

        /// <summary>
        /// Test property fails if single selected related code value exists in the Adw Related Code table but is an excluded value.
        /// </summary>
        [TestMethod]
        public void AdwSelection_SingleSelectionRelatedCodeExistsWithExcludeValueSelected_Fails()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    },
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "FooDominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "FooSubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    },
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "BarDominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "BarSubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleRelatedCodeExcludeValues = "FooSubordinateCode" };

            Assert.IsFalse(model.IsValid("SingleRelatedCodeExcludeValues"));
        }

        /// <summary>
        /// Test property validates if no dependent property or value is supplied and a match is found.
        /// </summary>
        [TestMethod]
        public void AdwSelection_RelatedCodeWithNoDependentPropertyOrValue_Validates()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { RelatedCodeNoDependentPropertyOrDependentValue = "SubordinateCode" };

            Assert.IsTrue(model.IsValid("RelatedCodeNoDependentPropertyOrDependentValue"));
        }

        #endregion

        #region Single selection Related Code with dependent property

        /// <summary>
        /// Test property validates if single selected related code value with dependent property exists in the Adw Related Code table.
        /// </summary>
        [TestMethod]
        public void AdwSelection_SingleSelectionRelatedCodeWithDependentPropertyExists_Validates()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = "SubordinateCode", SingleRelatedCodeDependentProperty = "SubordinateCode" };

            Assert.IsTrue(model.IsValid("SingleRelatedCodeDependentProperty"));
        }

        /// <summary>
        /// Test property validates if single selected related code value with dependent property is not selected.
        /// </summary>
        [TestMethod]
        public void AdwSelection_SingleSelectionRelatedCodeWithDependentPropertyNoSelection_Validates()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = "SubordinateCode", SingleRelatedCodeDependentProperty = string.Empty };

            Assert.IsTrue(model.IsValid("SingleRelatedCodeDependentProperty"));
        }

        /// <summary>
        /// Test property validates if single selected related code value with dependent property not selected.
        /// </summary>
        [TestMethod]
        public void AdwSelection_SingleSelectionRelatedCodeWithDependentPropertyEmpty_Validates()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = string.Empty };

            Assert.IsTrue(model.IsValid("SingleRelatedCodeDependentProperty"));
        }

        /// <summary>
        /// Test property validates if single selected related code value with dependent property not selected.
        /// </summary>
        [TestMethod]
        public void AdwSelection_SingleSelectionRelatedCodeWithDependentPropertyNull_Validates()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = null };

            Assert.IsTrue(model.IsValid("SingleRelatedCodeDependentProperty"));
        }

        /// <summary>
        /// Test property fails if single selected related code value with dependent property does not exist in the Adw Related Code table.
        /// </summary>
        [TestMethod]
        public void AdwSelection_SingleSelectionRelatedCodeWithDependentPropertyDoesNotExist_Fails()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "NotExists",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "NotExists",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = "SubordinateCode", SingleRelatedCodeDependentProperty = "SubordinateCode" };

            Assert.IsFalse(model.IsValid("SingleRelatedCodeDependentProperty"));
        }

        #endregion

        #region Multiple selection Related Code with dependent property

        /// <summary>
        /// Test property validates if multiple selected related code value exists in the Adw Related Code table and is not an excluded value.
        /// </summary>
        [TestMethod]
        public void AdwSelection_MultipleSelectionRelatedCodeExistsWithExcludeValuesButNotSelected_Validates()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    },
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "FooDominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "FooSubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    },
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "BarDominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "BarSubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { MultipleRelatedCodeExcludeValues = new[] { "SubordinateCode" } };

            Assert.IsTrue(model.IsValid("MultipleRelatedCodeExcludeValues"));
        }

        /// <summary>
        /// Test property fails if multiple selected related code value exists in the Adw Related Code table but is an excluded value.
        /// </summary>
        [TestMethod]
        public void AdwSelection_MultipleSelectionRelatedCodeExistsWithExcludeValueSelected_Fails()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    },
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "FooDominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "FooSubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    },
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "BarDominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "BarSubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { MultipleRelatedCodeExcludeValues = new [] { "FooSubordinateCode" } };

            Assert.IsFalse(model.IsValid("MultipleRelatedCodeExcludeValues"));
        }

        /// <summary>
        /// Test property validates if multiple selected related code value with dependent property exists in the Adw Related Code table.
        /// </summary>
        [TestMethod]
        public void AdwSelection_MultipleSelectionRelatedCodeWithDependentPropertyExists_Validates()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = "SubordinateCode", MultipleRelatedCodeDependentProperty = new[] { "SubordinateCode" } };

            Assert.IsTrue(model.IsValid("MultipleRelatedCodeDependentProperty"));
        }

        /// <summary>
        /// Test property validates if multiple selected related code value with dependent property is not selected.
        /// </summary>
        [TestMethod]
        public void AdwSelection_MultipleSelectionRelatedCodeWithDependentPropertyNoSelection_Validates()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = "SubordinateCode", MultipleRelatedCodeDependentProperty = new[] { string.Empty } };

            Assert.IsTrue(model.IsValid("MultipleRelatedCodeDependentProperty"));
        }

        /// <summary>
        /// Test property validates if multiple selected related code value with dependent property not selected.
        /// </summary>
        [TestMethod]
        public void AdwSelection_MultipleSelectionRelatedCodeWithDependentPropertyEmpty_Validates()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = string.Empty };

            Assert.IsTrue(model.IsValid("MultipleRelatedCodeDependentProperty"));
        }

        /// <summary>
        /// Test property validates if multiple selected related code value with dependent property not selected.
        /// </summary>
        [TestMethod]
        public void AdwSelection_MultipleSelectionRelatedCodeWithDependentPropertyNull_Validates()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = null };

            Assert.IsTrue(model.IsValid("MultipleRelatedCodeDependentProperty"));
        }

        /// <summary>
        /// Test property fails if multiple selected related code value with dependent property does not exist in the Adw Related Code table.
        /// </summary>
        [TestMethod]
        public void AdwSelection_MultipleSelectionRelatedCodeWithDependentPropertyDoesNotExist_Fails()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "NotExists",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "NotExists",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = "SubordinateCode", MultipleRelatedCodeDependentProperty = new[] { "SubordinateCode" } };

            Assert.IsFalse(model.IsValid("MultipleRelatedCodeDependentProperty"));
        }

        #endregion

        #region Single selection Related Code with dependent value

        /// <summary>
        /// Test property validates if single selected related code value with dependent value exists in the Adw Related Code table.
        /// </summary>
        [TestMethod]
        public void AdwSelection_SingleSelectionRelatedCodeWithDependentValueExists_Validates()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = "SubordinateCode", SingleRelatedCodeDependentValue = "SubordinateCode" };

            Assert.IsTrue(model.IsValid("SingleRelatedCodeDependentValue"));
        }

        /// <summary>
        /// Test property validates if single selected related code value with dependent value is not selected.
        /// </summary>
        [TestMethod]
        public void AdwSelection_SingleSelectionRelatedCodeWithDependentValueNoSelection_Validates()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = "SubordinateCode", SingleRelatedCodeDependentValue = string.Empty };

            Assert.IsTrue(model.IsValid("SingleRelatedCodeDependentValue"));
        }

        /// <summary>
        /// Test property validates if single selected related code value with dependent value not selected.
        /// </summary>
        [TestMethod]
        public void AdwSelection_SingleSelectionRelatedCodeWithDependentValueEmpty_Validates()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = string.Empty };

            Assert.IsTrue(model.IsValid("SingleRelatedCodeDependentValue"));
        }
        
        /// <summary>
        /// Test property fails if single selected related code value with dependent value does not exist in the Adw Related Code table.
        /// </summary>
        [TestMethod]
        public void AdwSelection_SingleSelectionRelatedCodeWithDependentValueDoesNotExist_Fails()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "NotExists",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "NotExists",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = "SubordinateCode", SingleRelatedCodeDependentValue = "SubordinateCode" };

            Assert.IsFalse(model.IsValid("SingleRelatedCodeDependentValue"));
        }

        #endregion

        #region Multiple selection Related Code with dependent property

        /// <summary>
        /// Test property validates if multiple selected related code value with dependent value exists in the Adw Related Code table.
        /// </summary>
        [TestMethod]
        public void AdwSelection_MultipleSelectionRelatedCodeWithDependentValueExists_Validates()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = "SubordinateCode", MultipleRelatedCodeDependentValue = new[] { "SubordinateCode" } };

            Assert.IsTrue(model.IsValid("MultipleRelatedCodeDependentValue"));
        }

        /// <summary>
        /// Test property validates if multiple selected related code value with dependent value is not selected.
        /// </summary>
        [TestMethod]
        public void AdwSelection_MultipleSelectionRelatedCodeWithDependentValueNoSelection_Validates()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = "SubordinateCode", MultipleRelatedCodeDependentValue = new[] { string.Empty } };

            Assert.IsTrue(model.IsValid("MultipleRelatedCodeDependentValue"));
        }

        /// <summary>
        /// Test property validates if multiple selected related code value with dependent value not selected.
        /// </summary>
        [TestMethod]
        public void AdwSelection_MultipleSelectionRelatedCodeWithDependentValueEmpty_Validates()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = string.Empty };

            Assert.IsTrue(model.IsValid("MultipleRelatedCodeDependentValue"));
        }

        /// <summary>
        /// Test property validates if multiple selected related code value with dependent value not selected.
        /// </summary>
        [TestMethod]
        public void AdwSelection_MultipleSelectionRelatedCodeWithDependentValueNull_Validates()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = null };

            Assert.IsTrue(model.IsValid("MultipleRelatedCodeDependentValue"));
        }

        /// <summary>
        /// Test property fails if multiple selected related code value with dependent value does not exist in the Adw Related Code table.
        /// </summary>
        [TestMethod]
        public void AdwSelection_MultipleSelectionRelatedCodeWithDependentValueDoesNotExist_Fails()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "NotExists",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "NotExists",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = "SubordinateCode", MultipleRelatedCodeDependentValue = new[] { "SubordinateCode" } };

            Assert.IsFalse(model.IsValid("MultipleRelatedCodeDependentValue"));
        }

        #endregion

        #region Get select list for List Code using display type

        /// <summary>
        /// Test property returns a select list with the display code for the value text.
        /// </summary>
        [TestMethod]
        public void AdwSelection_ListCodeSelectListDisplayCode_UsesDisplayType()
        {
            var codes = new[]
                            {
                                new CodeModel
                                    {
                                        Code = "Code",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model();

            var selectedItems = model.GetAttribute("ListCodeDisplayCode").GetSelectListItems(model);

            Assert.IsTrue(selectedItems.Any(i => i.Text.Contains(codes.First().Code)));
        }

        /// <summary>
        /// Test property returns a select list with the display code and description for the value text.
        /// </summary>
        [TestMethod]
        public void AdwSelection_ListCodeSelectListDisplayCodeAndDescription_UsesDisplayType()
        {
            var codes = new[]
                            {
                                new CodeModel
                                    {
                                        Code = "Code",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model();

            var selectedItems = model.GetAttribute("ListCodeDisplayCodeAndDescription").GetSelectListItems(model);

            Assert.IsTrue(selectedItems.Any(i => i.Text.Contains(codes.First().Code) && i.Text.Contains(codes.First().Description)));
        }

        /// <summary>
        /// Test property returns a select list with the display code and short description for the value text.
        /// </summary>
        [TestMethod]
        public void AdwSelection_ListCodeSelectListDisplayCodeAndShortDescription_UsesDisplayType()
        {
            var codes = new[]
                            {
                                new CodeModel
                                    {
                                        Code = "Code",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model();

            var selectedItems = model.GetAttribute("ListCodeDisplayCodeAndShortDescription").GetSelectListItems(model);

            Assert.IsTrue(selectedItems.Any(i => i.Text.Contains(codes.First().Code) && i.Text.Contains(codes.First().ShortDescription)));
        }

        /// <summary>
        /// Test property returns a select list with the display description for the value text.
        /// </summary>
        [TestMethod]
        public void AdwSelection_ListCodeSelectListDisplayDescription_UsesDisplayType()
        {
            var codes = new[]
                            {
                                new CodeModel
                                    {
                                        Code = "Code",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model();

            var selectedItems = model.GetAttribute("ListCodeDisplayDescription").GetSelectListItems(model);

            Assert.IsTrue(selectedItems.Any(i => i.Text.Contains(codes.First().Description)));
        }

        /// <summary>
        /// Test property returns a select list with the display short description for the value text.
        /// </summary>
        [TestMethod]
        public void AdwSelection_ListCodeSelectListDisplayShortDescription_UsesDisplayType()
        {
            var codes = new[]
                            {
                                new CodeModel
                                    {
                                        Code = "Code",
                                        Description = "Description",
                                        ShortDescription = "ShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model();

            var selectedItems = model.GetAttribute("ListCodeDisplayShortDescription").GetSelectListItems(model);

            Assert.IsTrue(selectedItems.Any(i => i.Text.Contains(codes.First().ShortDescription)));
        }

        #endregion

        #region Get select list for Related Code using display type

        /// <summary>
        /// Test property returns a select list with the display code for the value text.
        /// </summary>
        [TestMethod]
        public void AdwSelection_RelatedCodeSelectListDisplayCode_UsesDisplayType()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model();

            var selectedItems = model.GetAttribute("RelatedCodeDisplayCode").GetSelectListItems(model);

            Assert.IsTrue(selectedItems.Any(i => i.Text.Contains(codes.First().ToCodeModel().Code)));
        }

        /// <summary>
        /// Test property returns a select list with the display code and description for the value text.
        /// </summary>
        [TestMethod]
        public void AdwSelection_RelatedCodeSelectListDisplayCodeAndDescription_UsesDisplayType()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model();

            var selectedItems = model.GetAttribute("RelatedCodeDisplayCodeAndDescription").GetSelectListItems(model);

            Assert.IsTrue(selectedItems.Any(i => i.Text.Contains(codes.First().ToCodeModel().Code) && i.Text.Contains(codes.First().ToCodeModel().Description)));
        }

        /// <summary>
        /// Test property returns a select list with the display code and short description for the value text.
        /// </summary>
        [TestMethod]
        public void AdwSelection_RelatedCodeSelectListDisplayCodeAndShortDescription_UsesDisplayType()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model();

            var selectedItems = model.GetAttribute("RelatedCodeDisplayCodeAndShortDescription").GetSelectListItems(model);

            Assert.IsTrue(selectedItems.Any(i => i.Text.Contains(codes.First().ToCodeModel().Code) && i.Text.Contains(codes.First().ToCodeModel().ShortDescription)));
        }

        /// <summary>
        /// Test property returns a select list with the display description for the value text.
        /// </summary>
        [TestMethod]
        public void AdwSelection_RelatedCodeSelectListDisplayDescription_UsesDisplayType()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model();

            var selectedItems = model.GetAttribute("RelatedCodeDisplayDescription").GetSelectListItems(model);

            Assert.IsTrue(selectedItems.Any(i => i.Text.Contains(codes.First().ToCodeModel().Description)));
        }

        /// <summary>
        /// Test property returns a select list with the display short description for the value text.
        /// </summary>
        [TestMethod]
        public void AdwSelection_RelatedCodeSelectListDisplayShortDescription_UsesDisplayType()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model();

            var selectedItems = model.GetAttribute("RelatedCodeDisplayShortDescription").GetSelectListItems(model);

            Assert.IsTrue(selectedItems.Any(i => i.Text.Contains(codes.First().ToCodeModel().ShortDescription)));
        }

        /// <summary>
        /// Test property returns a select list with the display code with dependent property for the value text.
        /// </summary>
        [TestMethod]
        public void AdwSelection_RelatedCodeSelectListDisplayCodeWithDependentProperty_UsesDisplayType()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = "Code" };

            var selectedItems = model.GetAttribute("RelatedCodeDisplayCodeWithDependentProperty").GetSelectListItems(model);

            Assert.IsTrue(selectedItems.Any(i => i.Text.Contains(codes.First().ToCodeModel().Code)));
        }

        /// <summary>
        /// Test property returns a select list with the display code with dependent property empty for the value text.
        /// </summary>
        [TestMethod]
        public void AdwSelection_RelatedCodeSelectListDisplayCodeWithDependentPropertyEmpty_NoSelectListItems()
        {
            var codes = new[]
                            {
                                new RelatedCodeModel
                                    {
                                        Dominant = true,
                                        DominantCode = "DominantCode",
                                        DominantDescription = "DominantDescription",
                                        DominantShortDescription = "DominantShortDescription",
                                        SubordinateCode = "SubordinateCode",
                                        SubordinateDescription = "SubordinateDescription",
                                        SubordinateShortDescription = "SubordinateShortDescription",
                                        StartDate = DateTime.MinValue,
                                        EndDate = null
                                    }
                            };

            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(codes);

            var model = new Model() { SingleListCode = string.Empty };

            var selectedItems = model.GetAttribute("RelatedCodeDisplayCodeWithDependentProperty").GetSelectListItems(model);

            Assert.IsFalse(selectedItems.Any());
        }

        #endregion
    }
}
