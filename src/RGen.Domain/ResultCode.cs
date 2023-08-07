using System.ComponentModel;


namespace RGen.Domain;

public enum ResultCode
{
	OK,


	[Description("General: No data was generated")]
	NoDataGenerated = -100,

	[Description("Error when writing to output")]
	WriteError = -101,


	[Description("Output: There was a problem with the output file path")]
	OutputFilePathError = -200,

	[Description("Output: There was a problem writing to the output file path")]
	OutputFileWriteError = -301
}