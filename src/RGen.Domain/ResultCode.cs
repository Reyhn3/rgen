namespace RGen.Domain;

public enum ResultCode
{
	OK,

//IMPORTANT: These names and numbers must match ExitCode to provide correct user feedback

	NoDataGenerated = -100,
	WriteError = -101,

	OutputFilePathError = -200,
	OutputFileWriteError = -301
}