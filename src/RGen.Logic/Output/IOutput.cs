using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace RGen.Logic.Output;

public interface IOutput
{
	Task WriteAsync<T>(IEnumerable<IEnumerable<T>> sets, CancellationToken cancellationToken);
}