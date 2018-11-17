using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Properties;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to validate an uploaded file.
    /// </summary>
    /// <remarks>
    /// File signature validation can be skipped by adding a "SkipFileSignatureCheck" key to the Web.config <appSettings /> section with a value of <c>true</c>.
    /// File signatures are sourced from: http://www.garykessler.net/library/file_sigs.html
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FileAttribute : ValidationAttribute
    {
        private IConfigurationManager ConfigurationManager
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IConfigurationManager>() : null;
            }
        }

        #region Document file models

        private static readonly FileModel Doc = new FileModel(FileType.Doc, new List<string> { "doc", "docx" }, new List<FileSignatureModel> { new FileSignatureModel(new byte[] { 0xEC, 0xA5, 0xC1, 0x00 }, 512), new FileSignatureModel(new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00 }) });
        private static readonly FileModel Pdf = new FileModel(FileType.Pdf, "pdf", new byte[] { 0x25, 0x50, 0x44, 0x46 });
        private static readonly FileModel Rtf = new FileModel(FileType.Rtf, "rtf", new byte[] { 0x7B, 0x5C, 0x72, 0x74, 0x66, 0x31 });
        private static readonly FileModel Xls = new FileModel(FileType.Xls, new List<string> { "xls", "xlsx" }, new List<FileSignatureModel> { new FileSignatureModel(new byte[] { 0x09, 0x08, 0x10, 0x00, 0x00, 0x06, 0x05, 0x00 }, 512), new FileSignatureModel(new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00 }) });
        private static readonly FileModel Xml = new FileModel(FileType.Xml, "xml", new List<FileSignatureModel> { new FileSignatureModel(new byte[] { 0xEF, 0xBB, 0xBF, 0x3C, 0x3F, 0x78, 0x6D, 0x6C }), new FileSignatureModel(new byte[] { 0x3C, 0x3F, 0x78, 0x6D, 0x6C }) });
        private static readonly FileModel Txt = new FileModel(FileType.Txt, "txt", new byte[]{} );
        #endregion

        #region Image file models

        private static readonly FileModel Gif = new FileModel(FileType.Gif, "gif", new List<FileSignatureModel> { new FileSignatureModel(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }), new FileSignatureModel(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }) });
        private static readonly FileModel Jpg = new FileModel(FileType.Jpg, new List<string> { "jpg", "jpe", "jpeg" }, new byte[] { 0xFF, 0xD8, 0xFF });
        private static readonly FileModel Png = new FileModel(FileType.Png, "png", new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A });

        #endregion

        private static readonly List<FileModel> FileModels = new List<FileModel> { Doc, Pdf, Rtf, Xls, Xml, Gif, Jpg, Png, Txt };

        private const string RtfImageTag = "\\pict";
        private const string RtfDrawingTag = "\\do";
        private const string RtfObjectTag = "\\object";
        private const string RtfMacObjectTag = "\\bkmkpub";

        /// <summary>
        /// Default error message format string.
        /// </summary>
        private string DefaultErrorMessageFormatString
        {
            get { return DataAnnotationsResources.FileAttribute_Invalid; }
        }

        /// <summary>
        /// Allowed file types.
        /// </summary>
        internal FileType[] FileTypes { get; set; }

        /// <summary>
        /// Maximum allowed file size.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Maximum allowed compressed file size.
        /// </summary>
        public int CompressedSize { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.FileAttribute" /> class.
        /// </summary>
        /// <param name="fileTypes">Allowed file types.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="fileTypes" /> is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="fileTypes" /> does not have a matching <see cref="FileModel" /> configured in <see cref="FileModels" />.</exception>
        public FileAttribute(FileType[] fileTypes)
        {
            if (fileTypes == null || fileTypes.Length == 0)
            {
                throw new ArgumentNullException("fileTypes");
            }

            // Check there is a configured file model for each file type
            foreach (var fileType in fileTypes)
            {
                if (FileModels.FirstOrDefault(m => m.FileType == fileType) == null)
                {
                    throw new InvalidOperationException(string.Format(DataAnnotationsResources.FileAttribute_InvalidFileModels, fileType));
                }
            }

            FileTypes = fileTypes;
        }

        /// <summary>
        /// Formats the error message that is displayed when validation fails.
        /// </summary>
        /// <param name="name">The name of the field that caused the validation failure.</param>
        /// <returns>The formatted error message.</returns>
        public override string FormatErrorMessage(string name)
        {
            if (string.IsNullOrEmpty(ErrorMessage) && string.IsNullOrEmpty(ErrorMessageResourceName))
            {
                ErrorMessage = DefaultErrorMessageFormatString;
            }

            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
        }

        /// <summary>Validates the specified object.</summary>
        /// <param name="value">The object to validate.</param>
        /// <param name="validationContext">The <see cref="T:System.ComponentModel.DataAnnotations.ValidationContext" /> object that describes the context where the validation checks are performed. This parameter cannot be null.</param>
        /// <exception cref="T:System.ComponentModel.DataAnnotations.ValidationException">Validation failed.</exception>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var postedFile = value as HttpPostedFileBase;

            if (postedFile == null)
            {
                // No posted file to validate
                return ValidationResult.Success;
            }

            // Validate posted file size
            if (Size > 0 && postedFile.ContentLength > Size)
            {
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, DataAnnotationsResources.FileAttribute_InvalidSize, validationContext.DisplayName, GetAllowedFileSizeDescription(Size)));
            }

            byte[] postedBytes = null;
            
            // Get file model of posted file by matching against posted file extension
            var postedExtension = postedFile.FileName.Split('.').LastOrDefault();

            if (postedExtension == null)
            {
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, DataAnnotationsResources.FileAttribute_InvalidExtension, validationContext.DisplayName, GetAllowedFileExtensionsDescription()));
            }
            
            // Get file model of posted file by matching against posted file extension
            var postedFileModel = FileModels.FirstOrDefault(m => m.Extensions.Contains(postedExtension.ToLower()));

            // Invalid posted file if there is no matching file model or it is not an allowed file type
            if (postedFileModel == null || !FileTypes.Contains(postedFileModel.FileType))
            {
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, DataAnnotationsResources.FileAttribute_InvalidType, validationContext.DisplayName, GetAllowedFileExtensionsDescription()));
            }
             
   
            // Check config to see if file signature check should be skipped
            bool skip;
            if ( (postedFileModel.FileType != FileType.Txt) && !(ConfigurationManager.AppSettings["SkipFileSignatureCheck"] != null && bool.TryParse(ConfigurationManager.AppSettings.Get("SkipFileSignatureCheck"), out skip) && skip))
            {
                // Get copy of bytes
                postedBytes = postedFile.GetBytes();

                // Check posted file data has a valid signature for its file type
                if (!postedFileModel.ValidSignature(postedBytes))
                {
                    return new ValidationResult(string.Format(CultureInfo.CurrentCulture, DataAnnotationsResources.FileAttribute_InvalidSignature, validationContext.DisplayName, GetAllowedFileExtensionsDescription()));
                }
            }

            // RTF specific validation
            if (postedFileModel.FileType == FileType.Rtf)
            {
                postedBytes = postedBytes ?? postedFile.GetBytes();

                var text = new UTF8Encoding().GetString(postedBytes, 0, postedBytes.Length);

                if (!string.IsNullOrEmpty(text))
                {
                    text = text.ToLower();

                    if (text.Contains(string.Format("{0}{{", RtfImageTag)) || text.Contains(string.Format("{0}\\", RtfImageTag)) || text.Contains(string.Format("{0}{{", RtfDrawingTag)) || text.Contains(string.Format("{0}\\", RtfDrawingTag)))
                    {
                        return new ValidationResult(string.Format(DataAnnotationsResources.FileAttribute_InvalidRtfImage, validationContext.DisplayName));
                    }

                    if (text.Contains(string.Format("{0}{{", RtfObjectTag)) || text.Contains(string.Format("{0}\\", RtfObjectTag)) || text.Contains(string.Format("{0}{{", RtfMacObjectTag)) || text.Contains(string.Format("{0}\\", RtfMacObjectTag)))
                    {
                        return new ValidationResult(string.Format(DataAnnotationsResources.FileAttribute_InvalidRtfObject, validationContext.DisplayName));
                    }

                    var blueWordResult = new BlueWordAttribute().ValidateBlueWord(validationContext.DisplayName, text);
                    
                    if (blueWordResult != ValidationResult.Success)
                    {
                        return blueWordResult;
                    }
                }
            }

            // Validate compressed file size
            if (CompressedSize > 0)
            {
                var compressedBytes = (postedBytes ?? postedFile.GetBytes()).Compress();

                if (compressedBytes != null && compressedBytes.Length > CompressedSize)
                {
                    return new ValidationResult(string.Format(CultureInfo.CurrentCulture, DataAnnotationsResources.FileAttribute_InvalidCompressedSize, validationContext.DisplayName, GetAllowedFileSizeDescription(CompressedSize)));
                }
            }

            return ValidationResult.Success;
        }

        internal string GetAllowedFileSizeDescription(int fileSize)
        {
            var suffix = "B";
            int kiloByte = 1024;
            int megaByte = 1024 * kiloByte;
            int gigaByte = 1024 * megaByte;

            decimal size = fileSize;

            if (size >= gigaByte)
            {
                size /= gigaByte;
                suffix = "GB";
            }
            else if (size >= megaByte)
            {
                size /= megaByte;
                suffix = "MB";
            }
            else if (size >= kiloByte)
            {
                size /= kiloByte;
                suffix = "KB";
            }

            return string.Format("{0:0.##} {1}", size, suffix);
        }

        /// <summary>
        /// Accepted file extensions
        /// </summary>
        /// <returns></returns>
        public string GetAcceptedFileExtensions()
        {
            var files = new StringBuilder();

            for (int i = 0; i < FileTypes.Length; i++)
            {
                if (files.Length > 0)
                {
                    files.Append(",");
                }

                files.Append(".");
                files.Append(string.Join(",.", FileModels.First(m => m.FileType == FileTypes[i]).Extensions).ToLower());
            }

            return files.ToString();
        }

        private string GetAllowedFileExtensionsDescription()
        {
            var files = new StringBuilder();

            for (int i = 0; i < FileTypes.Length; i++)
            {
                if (files.Length > 0)
                {
                    files.Append(i == FileTypes.Length - 1 ? " or " : ", ");
                }

                files.Append(string.Join("/", FileModels.First(m => m.FileType == FileTypes[i]).Extensions).ToUpper());
            }

            return files.ToString();
        }
    }
}
