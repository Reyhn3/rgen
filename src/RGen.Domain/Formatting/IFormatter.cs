using System.Collections.Generic;


namespace RGen.Domain.Formatting;

public interface IFormatter
{
	FormatContext Format<T>(IEnumerable<IEnumerable<T>> sets);
}