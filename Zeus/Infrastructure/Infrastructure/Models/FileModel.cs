using System;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.Types;
using System.Collections.Generic;

namespace Employment.Web.Mvc.Infrastructure.Models
{
    /// <summary>
    /// Defines a model containing a list of signatures for a file type.
    /// </summary>
    internal class FileModel
    {
        internal FileType FileType { get; set; }
        internal List<FileSignatureModel> Signatures { get; set; }
        internal List<string> Extensions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileModel" /> class.
        /// </summary>
        /// <param name="fileType">The type of file.</param>
        /// <param name="extension">The valid file extension of the file.</param>
        /// <param name="signature">The signature of the file.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="extension" /> is null.</exception>
        internal FileModel(FileType fileType, string extension, byte[] signature)
        {
            if (string.IsNullOrEmpty(extension))
            {
                throw new ArgumentNullException("extension");
            }

            FileType = fileType;
            Extensions = new List<string> { extension };
            Signatures = new List<FileSignatureModel> { new FileSignatureModel(signature) };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileModel" /> class.
        /// </summary>
        /// <param name="fileType">The type of file.</param>
        /// <param name="extension">The valid file extension of the file.</param>
        /// <param name="signature">The signature of the file.</param>
        /// <param name="signatureOffset">The offset of the signature.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="extension" /> is null.</exception>
        internal FileModel(FileType fileType, string extension, byte[] signature, int signatureOffset)
        {
            if (string.IsNullOrEmpty(extension))
            {
                throw new ArgumentNullException("extension");
            }

            FileType = fileType;
            Extensions = new List<string> { extension };
            Signatures = new List<FileSignatureModel> { new FileSignatureModel(signature, signatureOffset) };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileModel" /> class.
        /// </summary>
        /// <param name="fileType">The type of file.</param>
        /// <param name="extension">The valid file extension of the file.</param>
        /// <param name="signatures">The signatures.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="signatures" /> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="extension" /> is null.</exception>
        internal FileModel(FileType fileType, string extension, List<FileSignatureModel> signatures)
        {
            if (string.IsNullOrEmpty(extension))
            {
                throw new ArgumentNullException("extension");
            }

            if (signatures == null)
            {
                throw new ArgumentNullException("signatures");
            }

            FileType = fileType;
            Extensions = new List<string> { extension };
            Signatures = signatures;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileModel" /> class.
        /// </summary>
        /// <param name="fileType">The type of file.</param>
        /// <param name="extensions">The valid file extensions of the file.</param>
        /// <param name="signature">The signature of the file.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="extensions" /> is null.</exception>
        internal FileModel(FileType fileType, List<string> extensions, byte[] signature)
        {
            if (extensions == null)
            {
                throw new ArgumentNullException("extensions");
            }

            FileType = fileType;
            Extensions = extensions;
            Signatures = new List<FileSignatureModel> {new FileSignatureModel(signature)};
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileModel" /> class.
        /// </summary>
        /// <param name="fileType">The type of file.</param>
        /// <param name="extensions">The valid file extensions of the file.</param>
        /// <param name="signature">The signature of the file.</param>
        /// <param name="signatureOffset">The offset of the signature.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="extensions" /> is null.</exception>
        internal FileModel(FileType fileType, List<string> extensions, byte[] signature, int signatureOffset)
        {
            if (extensions == null)
            {
                throw new ArgumentNullException("extensions");
            }

            FileType = fileType;
            Extensions = extensions;
            Signatures = new List<FileSignatureModel> { new FileSignatureModel(signature, signatureOffset) };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileModel" /> class.
        /// </summary>
        /// <param name="fileType">The type of file.</param>
        /// <param name="extensions">The valid file extensions of the file.</param>
        /// <param name="signatures">The signatures.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="extensions" /> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="signatures" /> is null.</exception>
        internal FileModel(FileType fileType, List<string> extensions, List<FileSignatureModel> signatures)
        {
            if (extensions == null)
            {
                throw new ArgumentNullException("extensions");
            }

            if (signatures == null)
            {
                throw new ArgumentNullException("signatures");
            }

            FileType = fileType;
            Extensions = extensions;
            Signatures = signatures;
        }

        /// <summary>
        /// Checks if the file byte data has a matching signature.
        /// </summary>
        /// <param name="fileBytes">The file bytes.</param>
        /// <returns><c>true</c> if the signature of the file byte data matches a signature for the file type; otherwise, <c>false</c>.</returns>
        internal bool ValidSignature(byte[] fileBytes)
        {
            var validSignature = false;

            foreach (var signature in Signatures)
            {
                if (fileBytes.Skip(signature.SignatureOffset).Take(signature.Signature.Length).SequenceEqual(signature.Signature))
                {
                    validSignature = true;
                    break;
                }
            }

            return validSignature;
        }
    }
}
