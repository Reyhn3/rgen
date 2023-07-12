using System.Collections.Generic;


namespace RGen.Application.Formatting;

public interface IFormatter
{
	FormatContext Format<T>(IEnumerable<IEnumerable<T>> sets);
}