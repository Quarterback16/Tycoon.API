using System;
using System.ComponentModel.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate a property should have a character countdown displayed.
    /// </summary>
    /// <remarks>
    /// This should only be used on <see cref="string" /> based properties in combination with <see cref="StringLengthAttribute" />.
    /// </remarks>
    /// <example>
    /// The <see cref="CountdownAttribute" /> should be used on a <see cref="string" /> based property in combination with <see cref="StringLengthAttribute" />.
    /// <code><![CDATA[
    /// 
    /// public class MyViewModel
    /// {
    ///     [Bindable]
    ///     [StringLength(10)]
    ///     [Countdown]
    ///     public string SinglelineText { get; set; }
    /// 
    ///     [Bindable]
    ///     [StringLength(1000)]
    ///     [Countdown]
    ///     [DataType(DataType.MultilineText)]
    ///     public string MultilineText { get; set; }
    /// }
    /// 
    /// ]]></code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CountdownAttribute : Attribute
    {

    }
}
