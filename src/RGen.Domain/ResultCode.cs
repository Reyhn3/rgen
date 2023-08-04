namespace RGen.Domain;

public enum ResultCode
{
	OK,

	NoDataGenerated = -100,
	WriteError = -101,

	OutputFilePathError = -200,
	OutputFileWriteError = -301
}