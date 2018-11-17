using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.Csv;
using Employment.Web.Mvc.Infrastructure.Csv.Configuration;

namespace Employment.Web.Mvc.Infrastructure.Tests.Csv.Mocks
{
	public class ParserMock : ICsvParser
	{
		private readonly Queue<string[]> rows;

		public void Dispose()
		{
		}

		public CsvConfiguration Configuration { get; private set; }
		public int FieldCount { get; private set; }
		public long CharPosition { get; private set; }
		public long BytePosition { get; private set; }
		public int Row { get; private set; }

		public ParserMock( Queue<string[]> rows )
		{
			Configuration = new CsvConfiguration();
			this.rows = rows;
		}

		public string[] Read()
		{
			return rows.Dequeue();
		}
	}
}
