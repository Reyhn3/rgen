using System.Threading;
using System.Threading.Tasks;


namespace RGen.Application.Writing;

public interface IWriter
{
	Task WriteAsync(string values, CancellationToken cancellationToken);
}