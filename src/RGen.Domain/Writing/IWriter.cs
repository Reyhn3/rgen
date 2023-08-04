using System.Threading;
using System.Threading.Tasks;
using RGen.Domain.Formatting;


namespace RGen.Domain.Writing;

public interface IWriter
{
	Task<IResult> WriteAsync(FormatContext context, CancellationToken cancellationToken);
}