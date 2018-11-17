//using System;
//using System.ComponentModel;
//using System.Globalization;


//namespace Employment.Web.Mvc.Infrastructure.TypeConverters
//{
//    /// <summary>
//    /// Defines a <see cref="TypeConverter" /> that converts to an instance of <see cref="AutoMapper.IProfileExpression" />.
//    /// </summary>
//    public class ProfileExpressionTypeConverter : TypeConverter
//    {
//        /// <summary>Converts the given object to the an instance of <see cref="AutoMapper.IProfileExpression" />.</summary>
//        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
//        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture. </param>
//        /// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
//        /// <returns>An instance of <see cref="AutoMapper.IProfileExpression" />.</returns>
//        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
//        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
//        {
//            return Mapper.Configuration;
//        }
//    }
//}
