using System.Collections.Generic;


namespace RGen.Domain.Generators;

public interface IIntegerGenerator
{
	IEnumerable<int> Multiple(int n);
	IEnumerable<IEnumerable<int>> Set(int n, int o);
}