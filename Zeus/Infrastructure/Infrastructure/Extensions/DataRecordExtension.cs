using System;
using System.Data;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IDataRecord" />.
    /// </summary>
    public static class DataRecordExtension
    {
        /// <summary>
        /// Determine whether the column name exists in the data record.
        /// </summary>
        /// <param name="dataRecord">The instance of the data record.</param>
        /// <param name="columnName">The column name to look for.</param>
        /// <returns><c>true</c> if the data record has the column; otherwise, <c>false</c>.</returns>
        public static bool HasColumn(this IDataRecord dataRecord, string columnName)
        {
            for (int i = 0; i < dataRecord.FieldCount; i++)
            {
                if (dataRecord.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
