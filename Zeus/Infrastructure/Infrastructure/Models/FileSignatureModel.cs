using System;

namespace Employment.Web.Mvc.Infrastructure.Models
{

    /// <summary>
    /// Defines a model containing file signature details.
    /// </summary>
    internal class FileSignatureModel
    {
        /// <summary>
        /// The bytes that make up the signature.
        /// </summary>
        internal byte[] Signature { get; private set; }

        /// <summary>
        /// The offset where the signature starts.
        /// </summary>
        internal int SignatureOffset { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSignatureModel" /> class.
        /// </summary>
        /// <param name="signature">The signature of the file.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="signature" /> is null.</exception>
        internal FileSignatureModel(byte[] signature)
        {
            if (signature == null)
            {
                throw new ArgumentNullException("signature");
            }

            Signature = signature;
            SignatureOffset = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSignatureModel" /> class.
        /// </summary>
        /// <param name="signature">The signature of the file.</param>
        /// <param name="signatureOffset">The offset of the signature.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="signature" /> is null.</exception>
        internal FileSignatureModel(byte[] signature, int signatureOffset)
        {
            if (signature == null)
            {
                throw new ArgumentNullException("signature");
            }

            Signature = signature;
            SignatureOffset = signatureOffset;
        }
    }
}
