using System.ComponentModel.DataAnnotations;

namespace ProgramAssuranceTool.Infrastructure.DataAnnotations
{
	public class GenericDataTypeAttribute : DataTypeAttribute
	{
		public GenericDataTypeAttribute( DataType dataType ) : base( dataType )
		{
			if (dataType.Equals( DataType.Date ))
				ErrorMessage = "Date format must be dd/mm/yyyy";
		}

	}
}