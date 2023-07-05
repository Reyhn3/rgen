using System.Collections.Generic;


namespace RGen.Logic.Formatting;

public interface IFormatter
{
	string Format<T>(IEnumerable<IEnumerable<T>> sets);
}