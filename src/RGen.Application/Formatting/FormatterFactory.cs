﻿using System;
using System.Collections.Generic;
using RGen.Application.Rendering;
using RGen.Domain.Formatting;
using RGen.Domain.Rendering;


namespace RGen.Application.Formatting;


public class FormatterFactory : IFormatterFactory
{
	private readonly Dictionary<Type, Func<IFormatterOptions, IFormatter>> _map = new();

	public IFormatterFactory Register<TOption>(Func<TOption, IFormatter> factory)
		where TOption : IFormatterOptions
	{
		_map.Add(typeof(TOption), o => factory((TOption)o));
		return this;
	}

	public IFormatter Create<TOptions>(TOptions options)
		where TOptions : IFormatterOptions
	{
		if (_map.TryGetValue(options.GetType(), out var factory))
			return factory(options);

		throw new Exception($"Formatter not found for type {options.GetType()}");
	}
}