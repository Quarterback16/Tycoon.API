namespace ProgramAssuranceTool.Models
{
	public class PatColumn
	{
		public string ColumnName { get; set; }
		public string DataType { get; set; }
		public string ListCode { get; set; }

		public override string ToString()
		{
			return string.Format( "{0}:{1}:{2}", ColumnName, DataType, ListCode );
		}
	}
}