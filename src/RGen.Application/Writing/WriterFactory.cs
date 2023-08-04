using System;
using System.Collections.Generic;
using RGen.Domain.Writing;


namespace RGen.Application.Writing;

public class WriterFactory : IWriterFactory
{
	private readonly Dictionary<Type, Func<IWriterOptions, IWriter>> _map = new();

	public IWriterFactory Register<TOption>(Func<TOption, IWriter> factory)
		where TOption : IWriterOptions
	{
		_map.Add(typeof(TOption), o => factory((TOption)o));
		return this;
	}

	public IWriter Create<TOptions>(TOptions options)
		where TOptions : IWriterOptions
	{
		if (_map.TryGetValue(options.GetType(), out var factory))
			return factory(options);

		throw new Exception($"Writer not found for type {options.GetType()}");
	}
}