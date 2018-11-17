using Decorator.Implementation;
using Decorator.Interfaces;

namespace DecoratorPattern
{
	class Program
	{
		private static IExamine Examiner;
		static void Main( string[] args )
		{
			Examiner = new DecoratorExaminer( new ConcreteExaminer() );
			Examiner.Examine();
		}
	}

}
