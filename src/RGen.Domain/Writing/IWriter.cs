using System.Threading;
using System.Threading.Tasks;
using RGen.Domain.Rendering;


namespace RGen.Domain.Writing;

public interface IWriter
{
	Task<IResult> WriteAsync(RenderContext context, CancellationToken cancellationToken);
}