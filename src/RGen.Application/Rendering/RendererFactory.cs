using System;
using System.Collections.Generic;
using RGen.Domain.Rendering;


namespace RGen.Application.Rendering;


public class RendererFactory : IRendererFactory
{
	private readonly Dictionary<Type, Func<IRendererOptions, IRenderer>> _map = new();

	public IRendererFactory Register<TOption>(Func<TOption, IRenderer> factory)
		where TOption : IRendererOptions
	{
		_map.Add(typeof(TOption), o => factory((TOption)o));
		return this;
	}

	public IRenderer Create<TOptions>(TOptions options)
		where TOptions : IRendererOptions
	{
		if (_map.TryGetValue(options.GetType(), out var factory))
			return factory(options);

		throw new Exception($"Renderer not found for type {options.GetType()}");
	}
}