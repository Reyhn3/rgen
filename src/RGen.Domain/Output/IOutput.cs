using System.Threading;
using System.Threading.Tasks;


namespace RGen.Domain.Output;

public interface IOutput
{
	Task WriteAsync(string values, CancellationToken cancellationToken);
}