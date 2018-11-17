using System;
using Decorator.Interfaces;

namespace Decorator.Implementation
{
	public class DecoratorExaminer : IExamine
	{
		private readonly IExamine DecoratedExaminer;
		public DecoratorExaminer( IExamine decoratedExaminer )
		{
			DecoratedExaminer = decoratedExaminer;
		}

		public void Examine()
		{
			DoSomethingExtra();  //  extension Point!!!!
			DecoratedExaminer.Examine();   //  but still do the original stuff
		}

		private void DoSomethingExtra()
		{
			//  Extra functionality here
		}
	}

}
