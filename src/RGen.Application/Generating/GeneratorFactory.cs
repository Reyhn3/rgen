using System;
using System.Collections.Generic;
using RGen.Domain.Generating;


namespace RGen.Application.Generating;


public class GeneratorFactory : IGeneratorFactory
{
	private readonly Dictionary<Type, Func<IGeneratorOptions, IGenerator>> _map = new();

	public IGeneratorFactory Register<TOption>(Func<TOption, IGenerator> factory)
		where TOption : IGeneratorOptions
	{
		_map.Add(typeof(TOption), o => factory((TOption)o));
		return this;
	}

	public IGenerator Create<TOptions>(TOptions options)
		where TOptions : IGeneratorOptions
	{
		if (_map.TryGetValue(options.GetType(), out var factory))
			return factory(options);

		throw new Exception($"Generator not found for type {options.GetType()}");
	}
}