using System.Threading;
using System.Threading.Tasks;


namespace RGen.Application.Output;

public interface IOutput
{
	Task WriteAsync(string values, CancellationToken cancellationToken);
}