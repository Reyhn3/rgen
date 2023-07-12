using System;
using System.Diagnostics;


namespace RGen.Application;

public static class TraceHelper
{
	public static void PrintException(Exception ex, string message, params object[] args)
	{
		Trace.WriteLine(string.Format(message, args) + Environment.NewLine + ex);
	}
}