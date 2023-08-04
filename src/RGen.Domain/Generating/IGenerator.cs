namespace RGen.Domain.Generating;

public interface IGenerator
{
	IRandomValues Generate(int n, int o);
}