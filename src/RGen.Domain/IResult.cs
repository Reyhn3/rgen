namespace RGen.Domain;

public interface IResult
{
	bool IsSuccessful { get; }
	ResultCode Code { get; }
	string? Message { get; }
}