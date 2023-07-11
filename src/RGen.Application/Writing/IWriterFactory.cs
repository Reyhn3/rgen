using System;


namespace RGen.Application.Writing;

public interface IWriterFactory
{
	public IWriterFactory Register<TOption>(Func<TOption, IWriter> factory)
		where TOption : IWriterOptions;

	IWriter Create<TOptions>(TOptions options)
		where TOptions : IWriterOptions;
}