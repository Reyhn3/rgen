using System.Collections.Generic;


namespace RGen.Application.Formatting;

public interface IFormatter
{
	string Format<T>(IEnumerable<IEnumerable<T>> sets, bool isColoringDisabled);
}