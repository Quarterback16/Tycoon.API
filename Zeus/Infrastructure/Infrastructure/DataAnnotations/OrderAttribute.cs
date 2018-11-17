using System;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the order.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class OrderAttribute : Attribute
    {
        /// <summary>
        /// Whether the item should come first in sequence.
        /// </summary>
        public bool FirstInSequence { get; set; }

        /// <summary>
        /// The items position in sequence.
        /// </summary>
        public int PositionInSequence { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.OrderAttribute" /> class.
        /// </summary>
        public OrderAttribute() : this(int.MaxValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.OrderAttribute" /> class.
        /// </summary>
        /// <param name="positionInSequence">The position in sequence.</param>
        public OrderAttribute(int positionInSequence)
        {
            PositionInSequence = positionInSequence;
        }
    }
}
