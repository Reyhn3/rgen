namespace RGen.Domain;

public sealed class Result : IResult
{
	public bool IsSuccessful { get; private init; }
	public ResultCode Code { get; private init; }
	public string? Message { get; private init; }

	public static IResult OK { get; } = new Result
		{
			IsSuccessful = true,
			Code = ResultCode.OK
		};

	public static IResult Failure(ResultCode code, string? message = null) =>
		new Result
			{
				IsSuccessful = false,
				Code = code,
				Message = message
			};
}