namespace RGen.Domain.Rendering;


public record RenderContext(string Raw, string Rendered)
{
	public static RenderContext Empty { get; } = new(null!, null!);
	public bool IsEmpty => Raw == null! && Rendered == null!;
}