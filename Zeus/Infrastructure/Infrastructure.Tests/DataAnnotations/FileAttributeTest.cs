using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Mappers;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Properties;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="FileAttribute" />.
    /// </summary>
    [TestClass]
    public class FileAttributeTest
    {
        //private IMappingEngine mappingEngine;

        //protected IMappingEngine MappingEngine
        //{
        //    get
        //    {
        //        if (mappingEngine == null)
        //        {
        //            var adwMapper = new AdwMapper();
        //            adwMapper.Map(Mapper.Configuration);

        //            var stringMapper = new StringMapper();
        //            stringMapper.Map(Mapper.Configuration);

        //            mappingEngine = Mapper.Engine;
        //        }

        //        return mappingEngine;
        //    }
        //}

        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IUserService> mockUserService;
        private Mock<IConfigurationManager> mockConfiguration;
        private Mock<ClaimsPrincipal> mockClaimsPrincipal;
        private Mock<ClaimsIdentity> mockClaimsIdentity;
        private Mock<IAdwService> mockAdwService;
        private Mock<HttpPostedFileBase> mockFile;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockContainerProvider = new Mock<IContainerProvider>();
            mockUserService = new Mock<IUserService>();
            mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            mockClaimsIdentity = new Mock<ClaimsIdentity>();
            mockConfiguration = new Mock<IConfigurationManager>();
            mockAdwService = new Mock<IAdwService>();
            mockFile = new Mock<HttpPostedFileBase>();

            // Setup principal to return identity
            mockClaimsPrincipal.Setup(m => m.Identity).Returns(mockClaimsIdentity.Object);

            // Setup identity to default as authenticated
            mockClaimsIdentity.Setup(m => m.Name).Returns("User");
            mockClaimsIdentity.Setup(m => m.IsAuthenticated).Returns(true);

            // Setup claims identity in User Service
            mockUserService.Setup(m => m.DateTime).Returns(DateTime.Now);
            mockUserService.Setup(m => m.Identity).Returns(mockClaimsIdentity.Object);

            // Setup ADW for blueword check on RTF
            mockAdwService.Setup(m => m.GetRelatedCodes("OFTW", "UNA")).Returns(new List<RelatedCodeModel> { new RelatedCodeModel { Dominant = true, DominantCode = "DominantCode", SubordinateDescription = "bing", RelatedCode = "OFTW", SubordinateCode = "UNA"}});
            mockAdwService.Setup(m => m.GetRelatedCodes("OFTW", "IDE")).Returns(new List<RelatedCodeModel> { new RelatedCodeModel { Dominant = true, DominantCode = "DominantCode", SubordinateDescription = "bingo", RelatedCode = "OFTW", SubordinateCode = "IDE" } });

            // Setup Dependency Resolver to use mocked Container Provider
            //mockContainerProvider.Setup(m => m.GetService<IMappingEngine>()).Returns(MappingEngine);
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            mockContainerProvider.Setup(m => m.GetService<IAdwService>()).Returns(mockAdwService.Object);
            mockContainerProvider.Setup(m => m.GetService<IConfigurationManager>()).Returns(mockConfiguration.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            var app = new NameValueCollection();
            app.Add("Environment", "PROD");
            mockConfiguration.Setup(a => a.AppSettings).Returns(app);
        }

        /// <summary>
        /// Test null reference exception in constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void File_ConstructorNull_ThrowsArgumentNullException()
        {
            new FileAttribute(null);
        }

        /// <summary>
        /// Test null reference exception in constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void File_ConstructorZeroLength_ThrowsArgumentNullException()
        {
            new FileAttribute(new FileType[0]);
        }

        /// <summary>
        /// Test default error.
        /// </summary>
        [TestMethod]
        public void File_DefaultError_ReturnsDefaultErrorMessage()
        {
            var name = "File";

            var sut = new FileAttribute(new[] { FileType.Doc });

            var errorMessage = sut.FormatErrorMessage(name);

            Assert.AreEqual(string.Format(DataAnnotationsResources.FileAttribute_Invalid, name), errorMessage);
        }

        /// <summary>
        /// Test error message.
        /// </summary>
        [TestMethod]
        public void File_ErrorMessage_ReturnsErrorMessage()
        {
            var errorMessageFormat = "My error";
            var sut = new FileAttribute(new[] { FileType.Doc }) { ErrorMessage = errorMessageFormat };

            var errorMessage = sut.FormatErrorMessage(string.Empty);

            Assert.AreEqual(errorMessageFormat, errorMessage);
        }

        /// <summary>
        /// Test valid.
        /// </summary>
        [TestMethod]
        public void File_SizePass_Validates()
        {
            var validationContext = new ValidationContext(this, null, null);

            validationContext.DisplayName = "File";

            var data = (byte[])new ImageConverter().ConvertTo(FileResources.PNG, typeof(byte[]));

            mockFile.Setup(m => m.FileName).Returns("file.png");
            mockFile.Setup(m => m.ContentLength).Returns(data.Length);
            mockFile.Setup(m => m.InputStream).Returns(new MemoryStream(data));

            var sut = new FileAttribute(new[] { FileType.Png });

            sut.Size = data.Length + 1;

            var valid = sut.GetValidationResult(mockFile.Object, validationContext);

            Assert.AreEqual(ValidationResult.Success, valid);
        }

        /// <summary>
        /// Test valid.
        /// </summary>
        [TestMethod]
        public void File_SizeFail_Fails()
        {
            var validationContext = new ValidationContext(this, null, null);

            validationContext.DisplayName = "File";

            var data = (byte[])new ImageConverter().ConvertTo(FileResources.PNG, typeof(byte[]));

            mockFile.Setup(m => m.FileName).Returns("file.png");
            mockFile.Setup(m => m.ContentLength).Returns(data.Length);
            mockFile.Setup(m => m.InputStream).Returns(new MemoryStream(data));

            var sut = new FileAttribute(new[] { FileType.Png });

            sut.Size = data.Length - 1;

            var valid = sut.GetValidationResult(mockFile.Object, validationContext);

            Assert.AreNotEqual(ValidationResult.Success, valid);
            Assert.IsNotNull(valid);
            Assert.IsTrue(!string.IsNullOrEmpty(valid.ErrorMessage));
        }

        /// <summary>
        /// Test valid.
        /// </summary>
        [TestMethod]
        public void File_CompressedSizePass_Validates()
        {
            var validationContext = new ValidationContext(this, null, null);

            validationContext.DisplayName = "File";

            var data = (byte[])new ImageConverter().ConvertTo(FileResources.PNG, typeof(byte[]));

            mockFile.Setup(m => m.FileName).Returns("file.png");
            mockFile.Setup(m => m.ContentLength).Returns(data.Length);
            mockFile.Setup(m => m.InputStream).Returns(new MemoryStream(data));

            var sut = new FileAttribute(new[] { FileType.Png });

            var compressedData = mockFile.Object.GetBytes().Compress();
            sut.CompressedSize = compressedData.Length + 1;

            var valid = sut.GetValidationResult(mockFile.Object, validationContext);

            Assert.AreEqual(ValidationResult.Success, valid);
        }

        /// <summary>
        /// Test valid.
        /// </summary>
        [TestMethod]
        public void File_CompressedSizeFail_Fails()
        {
            var validationContext = new ValidationContext(this, null, null);

            validationContext.DisplayName = "File";

            var data = (byte[])new ImageConverter().ConvertTo(FileResources.PNG, typeof(byte[]));

            mockFile.Setup(m => m.FileName).Returns("file.png");
            mockFile.Setup(m => m.ContentLength).Returns(data.Length);
            mockFile.Setup(m => m.InputStream).Returns(new MemoryStream(data));

            var sut = new FileAttribute(new[] { FileType.Png });

            var compressedData = mockFile.Object.GetBytes().Compress();
            sut.CompressedSize = compressedData.Length - 1;

            var valid = sut.GetValidationResult(mockFile.Object, validationContext);

            Assert.AreNotEqual(ValidationResult.Success, valid);
            Assert.IsNotNull(valid);
            Assert.IsTrue(!string.IsNullOrEmpty(valid.ErrorMessage));
        }

        /// <summary>
        /// Test error resource.
        /// </summary>
        [TestMethod]
        public void File_ErrorResource_ReturnsErrorResource()
        {
            var name = "File";
            var sut = new FileAttribute(new[] { FileType.Doc }) { ErrorMessageResourceName = "FileAttribute_InvalidFileModels", ErrorMessageResourceType = typeof(DataAnnotationsResources) };

            var errorMessage = sut.FormatErrorMessage(name);

            Assert.AreEqual(string.Format(DataAnnotationsResources.FileAttribute_InvalidFileModels, name), errorMessage);
        }

        /// <summary>
        /// Test valid.
        /// </summary>
        [TestMethod]
        public void File_GifWithWrongExtension_Fails()
        {
            var validationContext = new ValidationContext(this, null, null);

            validationContext.DisplayName = "File";

            var data = (byte[])new ImageConverter().ConvertTo(FileResources.GIF, typeof(byte[]));

            mockFile.Setup(m => m.FileName).Returns("file.wrong");
            mockFile.Setup(m => m.ContentLength).Returns(data.Length);
            mockFile.Setup(m => m.InputStream).Returns(new MemoryStream(data));

            var sut = new FileAttribute(new[] { FileType.Gif });

            var valid = sut.GetValidationResult(mockFile.Object, validationContext);

            Assert.AreNotEqual(ValidationResult.Success, valid);
            Assert.IsNotNull(valid);
            Assert.IsTrue(!string.IsNullOrEmpty(valid.ErrorMessage));
        }

        /// <summary>
        /// Test valid.
        /// </summary>
        [TestMethod]
        public void File_GifWithNoExtension_Fails()
        {
            var validationContext = new ValidationContext(this, null, null);

            validationContext.DisplayName = "File";

            var data = (byte[])new ImageConverter().ConvertTo(FileResources.GIF, typeof(byte[]));

            mockFile.Setup(m => m.FileName).Returns("file");
            mockFile.Setup(m => m.ContentLength).Returns(data.Length);
            mockFile.Setup(m => m.InputStream).Returns(new MemoryStream(data));

            var sut = new FileAttribute(new[] { FileType.Gif });

            var valid = sut.GetValidationResult(mockFile.Object, validationContext);

            Assert.AreNotEqual(ValidationResult.Success, valid);
            Assert.IsNotNull(valid);
            Assert.IsTrue(!string.IsNullOrEmpty(valid.ErrorMessage));
        }

        /// <summary>
        /// Test valid.
        /// </summary>
        [TestMethod]
        public void File_GifWithWrongFile_Fails()
        {
            var validationContext = new ValidationContext(this, null, null);

            validationContext.DisplayName = "File";

            var data = (byte[])new ImageConverter().ConvertTo(FileResources.JPG, typeof(byte[]));

            mockFile.Setup(m => m.FileName).Returns("file.gif");
            mockFile.Setup(m => m.ContentLength).Returns(data.Length);
            mockFile.Setup(m => m.InputStream).Returns(new MemoryStream(data));

            var sut = new FileAttribute(new[] { FileType.Gif });

            var valid = sut.GetValidationResult(mockFile.Object, validationContext);

            Assert.AreNotEqual(ValidationResult.Success, valid);
            Assert.IsNotNull(valid);
            Assert.IsTrue(!string.IsNullOrEmpty(valid.ErrorMessage));
        }

        /// <summary>
        /// Test valid.
        /// </summary>
        [TestMethod]
        public void File_ExpectingRtfButGetGif_Fails()
        {
            var validationContext = new ValidationContext(this, null, null);

            validationContext.DisplayName = "File";

            var data = (byte[])new ImageConverter().ConvertTo(FileResources.GIF, typeof(byte[]));

            mockFile.Setup(m => m.FileName).Returns("file.gif");
            mockFile.Setup(m => m.ContentLength).Returns(data.Length);
            mockFile.Setup(m => m.InputStream).Returns(new MemoryStream(data));

            var sut = new FileAttribute(new[] { FileType.Rtf });

            var valid = sut.GetValidationResult(mockFile.Object, validationContext);

            Assert.AreNotEqual(ValidationResult.Success, valid);
            Assert.IsNotNull(valid);
            Assert.IsTrue(!string.IsNullOrEmpty(valid.ErrorMessage));
        }

        /// <summary>
        /// Test valid.
        /// </summary>
        [TestMethod]
        public void File_Gif_Validates()
        {
            var validationContext = new ValidationContext(this, null, null);

            validationContext.DisplayName = "File";

            var data = (byte[])new ImageConverter().ConvertTo(FileResources.GIF, typeof(byte[]));

            mockFile.Setup(m => m.FileName).Returns("file.gif");
            mockFile.Setup(m => m.ContentLength).Returns(data.Length);
            mockFile.Setup(m => m.InputStream).Returns(new MemoryStream(data));

            var sut = new FileAttribute(new[] { FileType.Gif });

            var valid = sut.GetValidationResult(mockFile.Object, validationContext);

            Assert.AreEqual(ValidationResult.Success, valid);
        }

        /// <summary>
        /// Test valid.
        /// </summary>
        [TestMethod]
        public void File_Jpg_Validates()
        {
            var validationContext = new ValidationContext(this, null, null);

            validationContext.DisplayName = "File";

            var data = (byte[])new ImageConverter().ConvertTo(FileResources.JPG, typeof(byte[]));

            mockFile.Setup(m => m.FileName).Returns("file.jpg");
            mockFile.Setup(m => m.ContentLength).Returns(data.Length);
            mockFile.Setup(m => m.InputStream).Returns(new MemoryStream(data));

            var sut = new FileAttribute(new[] { FileType.Jpg });

            var valid = sut.GetValidationResult(mockFile.Object, validationContext);

            Assert.AreEqual(ValidationResult.Success, valid);
        }

        /// <summary>
        /// Test valid.
        /// </summary>
        [TestMethod]
        public void File_Png_Validates()
        {
            var validationContext = new ValidationContext(this, null, null);

            validationContext.DisplayName = "File";

            var data = (byte[])new ImageConverter().ConvertTo(FileResources.PNG, typeof(byte[]));

            mockFile.Setup(m => m.FileName).Returns("file.png");
            mockFile.Setup(m => m.ContentLength).Returns(data.Length);
            mockFile.Setup(m => m.InputStream).Returns(new MemoryStream(data));

            var sut = new FileAttribute(new[] { FileType.Png });

            var valid = sut.GetValidationResult(mockFile.Object, validationContext);

            Assert.AreEqual(ValidationResult.Success, valid);
        }

        #region RTF

        /// <summary>
        /// Test blue word.
        /// </summary>
        [TestMethod]
        public void File_RtfBlueword_Fails()
        {
            mockAdwService.Setup(m => m.GetRelatedCodes("OFTW", "UNA")).Returns(new List<RelatedCodeModel> { new RelatedCodeModel { Dominant = true, DominantCode = "DominantCode", SubordinateDescription = "foo", RelatedCode = "OFTW", SubordinateCode = "UNA" } });

            var validationContext = new ValidationContext(this, null, null);

            validationContext.DisplayName = "File";

            mockFile.Setup(m => m.FileName).Returns("file.rtf");
            mockFile.Setup(m => m.ContentLength).Returns(FileResources.RTF.Length);
            mockFile.Setup(m => m.InputStream).Returns(new MemoryStream(new UTF8Encoding().GetBytes(FileResources.RTF)));

            var sut = new FileAttribute(new[] { FileType.Rtf });

            var valid = sut.GetValidationResult(mockFile.Object, validationContext);

            Assert.AreEqual(string.Format(DataAnnotationsResources.BlueWordAttribute_InvalidWithBlueWords, validationContext.DisplayName, "foo"), valid.ErrorMessage);
        }

        /// <summary>
        /// Test identified blue word.
        /// </summary>
        [TestMethod]
        public void File_RtfIdentifiedBlueword_Validates()
        {
            mockAdwService.Setup(m => m.GetRelatedCodes("OFTW", "UNA")).Returns(new List<RelatedCodeModel> { new RelatedCodeModel { Dominant = true, DominantCode = "DominantCode", SubordinateDescription = "asd", RelatedCode = "OFTW", SubordinateCode = "UNA" } });
            mockAdwService.Setup(m => m.GetRelatedCodes("OFTW", "IDE")).Returns(new List<RelatedCodeModel> { new RelatedCodeModel { Dominant = true, DominantCode = "DominantCode", SubordinateDescription = "asdfooasd", RelatedCode = "OFTW", SubordinateCode = "IDE" } });

            var validationContext = new ValidationContext(this, null, null);

            validationContext.DisplayName = "File";

            mockFile.Setup(m => m.FileName).Returns("file.rtf");
            mockFile.Setup(m => m.ContentLength).Returns(FileResources.RTF.Length);
            mockFile.Setup(m => m.InputStream).Returns(new MemoryStream(new UTF8Encoding().GetBytes(FileResources.RTF)));

            var sut = new FileAttribute(new[] { FileType.Rtf });

            var valid = sut.GetValidationResult(mockFile.Object, validationContext);

            Assert.AreEqual(ValidationResult.Success, valid);
        }

        /// <summary>
        /// Test valid.
        /// </summary>
        [TestMethod]
        public void File_Rtf_Validates()
        {
            var validationContext = new ValidationContext(this, null, null);

            validationContext.DisplayName = "File";

            mockFile.Setup(m => m.FileName).Returns("file.rtf");
            mockFile.Setup(m => m.ContentLength).Returns(FileResources.RTF.Length);
            mockFile.Setup(m => m.InputStream).Returns(new MemoryStream(new UTF8Encoding().GetBytes(FileResources.RTF)));

            var sut = new FileAttribute(new[] { FileType.Rtf });

            var valid = sut.GetValidationResult(mockFile.Object, validationContext);

            Assert.AreEqual(ValidationResult.Success, valid);
        }

        /// <summary>
        /// Test valid.
        /// </summary>
        [TestMethod]
        public void File_RtfWithImage_Fails()
        {
            var validationContext = new ValidationContext(this, null, null);

            validationContext.DisplayName = "File";

            mockFile.Setup(m => m.FileName).Returns("file.rtf");
            mockFile.Setup(m => m.ContentLength).Returns(FileResources.RTF_Image.Length);
            mockFile.Setup(m => m.InputStream).Returns(new MemoryStream(new UTF8Encoding().GetBytes(FileResources.RTF_Image)));

            var sut = new FileAttribute(new[] { FileType.Rtf });

            var valid = sut.GetValidationResult(mockFile.Object, validationContext);

            Assert.IsNotNull(valid);
            Assert.AreEqual(string.Format(DataAnnotationsResources.FileAttribute_InvalidRtfImage, validationContext.DisplayName), valid.ErrorMessage);
        }

        /// <summary>
        /// Test valid.
        /// </summary>
        [TestMethod]
        public void File_RtfWithDrawing_Fails()
        {
            var validationContext = new ValidationContext(this, null, null);

            validationContext.DisplayName = "File";

            mockFile.Setup(m => m.FileName).Returns("file.rtf");
            mockFile.Setup(m => m.ContentLength).Returns(FileResources.RTF_Drawing.Length);
            mockFile.Setup(m => m.InputStream).Returns(new MemoryStream(new UTF8Encoding().GetBytes(FileResources.RTF_Drawing)));

            var sut = new FileAttribute(new[] { FileType.Rtf });

            var valid = sut.GetValidationResult(mockFile.Object, validationContext);

            Assert.IsNotNull(valid);
            Assert.AreEqual(string.Format(DataAnnotationsResources.FileAttribute_InvalidRtfImage, validationContext.DisplayName), valid.ErrorMessage);
        }

        #endregion
    }
}
