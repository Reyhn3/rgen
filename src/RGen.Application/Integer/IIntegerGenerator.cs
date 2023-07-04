using System.Collections.Generic;


namespace RGen.Application.Integer;

public interface IIntegerGenerator
{
	IEnumerable<int> Multiple(int n);
	IEnumerable<IEnumerable<int>> Set(int n, int o);
}