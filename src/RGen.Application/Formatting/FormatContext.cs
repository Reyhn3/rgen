namespace RGen.Application.Formatting;

public record FormatContext(string Raw, string Formatted)
{
	public static FormatContext Empty { get; } = new(null!, null!);
	public bool IsEmpty => Raw == null! && Formatted == null!;
}