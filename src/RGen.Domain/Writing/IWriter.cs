using System.Threading;
using System.Threading.Tasks;


namespace RGen.Domain.Writing;

public interface IWriter
{
	Task<IResult> WriteAsync(string values, CancellationToken cancellationToken);
}