﻿namespace Tycoon.Logic.Interfaces
{
	public interface IQueryHandler<TQuery, TResult>
		where TQuery : IQuery<TResult>
	{
		TResult Handle(TQuery query);
	}
}
