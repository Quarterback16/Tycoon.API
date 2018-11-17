using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosterLib
{
	public class PipeLine<T>
	{
		private readonly List<Action<T>> _actions = new List<Action<T>>();

		public void Execute( T input )
		{
			_actions.ForEach( ac => ac( input ) );
		}

		public PipeLine<T> Register( Action<T> action )
		{
			_actions.Add( action );
			return this;
		}
	}
}
