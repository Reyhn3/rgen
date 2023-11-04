namespace RGen.Domain.Rendering;


public record RenderContext(string Formatted, string Rendered)
{
	public static RenderContext Empty { get; } = new(null!, null!);
	public bool IsEmpty => Formatted == null! && Rendered == null!;
}