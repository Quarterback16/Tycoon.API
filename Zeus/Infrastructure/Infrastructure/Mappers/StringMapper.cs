//using System;
//using Employment.Web.Mvc.Infrastructure.TypeConverters;

//namespace Employment.Web.Mvc.Infrastructure.Mappers
//{
//    /// <summary>
//    /// The string mapper class
//    /// </summary>
//    internal static class StringMapper
//    {
//        /// <summary>
//        /// Converts the string.
//        /// </summary>
//        /// <param name="src">The source.</param>
//        /// <param name="sourceType">Type of the source.</param>
//        /// <param name="destType">Type of the dest.</param>
//        /// <returns></returns>
//        internal static object ConvertString(object src, Type sourceType, Type destType)
//        {
//            if (sourceType == typeof(string) && src!=null)
//            {
//                var source = src as string;
//                //TODO: Do we want null strings to be string.empty
//                //if (destType == typeof(string)) {  NullStringTypeConverter.Convert(context); return context.DestinationValue; }

//                if (destType == typeof(int)) { return IntTypeConverter.Convert(source);  }
//                if (destType == typeof(int?)) { return NullIntTypeConverter.Convert(source);  }
//                if (destType == typeof(long)) { return LongTypeConverter.Convert(source);  }
//                if (destType == typeof(long?)) { return NullLongTypeConverter.Convert(source);  }
//                if (destType == typeof(bool)) { return BoolTypeConverter.Convert(source);  }
//                if (destType == typeof(bool?)) { return NullBoolTypeConverter.Convert(source);  }
//                if (destType == typeof(decimal)) { return DecimalTypeConverter.Convert(source);  }
//                if (destType == typeof(decimal?)) { return NullDecimalTypeConverter.Convert(source);  }
//                if (destType == typeof(double)) { return DoubleTypeConverter.Convert(source);  }
//                if (destType == typeof(double?)) { return NullDoubleTypeConverter.Convert(source);  }
//                if (destType == typeof(float)) { return FloatTypeConverter.Convert(source);  }
//                if (destType == typeof(float?)) { return NullFloatTypeConverter.Convert(source);  }
//                if (destType == typeof(DateTime)) { return DateTimeTypeConverter.Convert(source);  }
//                if (destType == typeof(DateTime?)) { return NullDateTimeTypeConverter.Convert(source);  }
//            }
//            if (sourceType == destType)
//            {
//                return src;
//            }
//            return src;
//        }
//    }
//}
