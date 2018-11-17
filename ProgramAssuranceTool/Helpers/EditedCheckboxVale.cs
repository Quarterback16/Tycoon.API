using System;
using System.Collections.Specialized;


namespace ProgramAssuranceTool.Helpers
{
	public struct EditedCheckboxValue
	{
		public bool Current { get; private set; }

		public bool Previous { get; private set; }

		public bool Changed { get; private set; }

		public EditedCheckboxValue( NameValueCollection collection, string checkboxId )
			: this()
		{
			var values = collection[ checkboxId ].Split( new[] { ',' } );
			switch (values.Length)
			{
				case 2:
					Current = bool.Parse( values[ 0 ] );
					Previous = bool.Parse( values[ 1 ] );
					Changed = ( Current != Previous );
					break;

				case 1:
					Current = bool.Parse( values[ 0 ] );
					Previous = Current;
					Changed = false;
					break;

				default:
					throw new FormatException( "invalid format for edited checkbox value in FormCollection" );
			}
		}
	}
}