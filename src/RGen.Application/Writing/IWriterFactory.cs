namespace RGen.Application.Writing;

public interface IWriterFactory
{
	IWriter Create<TOptions>(TOptions options)
		where TOptions : IWriterOptions;
}