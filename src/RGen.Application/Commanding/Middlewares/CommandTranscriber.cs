using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace RGen.Application.Commanding.Middlewares;

public class CommandTranscriber
{
	public static InvocationMiddleware Instance =
		async (context, next) =>
			{
				var host = context.BindingContext.GetService(typeof(IHost)) as IHost;
				var logger = host?.Services.GetRequiredService<ILogger<CommandTranscriber>>();

				if (logger == null || !logger.IsEnabled(LogLevel.Debug))
				{
					await next(context);
					return;
				}

				var cmd = context.ParseResult.CommandResult;

				var parameters = cmd
					.Children
					.Select(c => new
						{
							Type = c.GetType(),
							Name = c.Symbol.Name.Trim('-'),
							Symbol = c
						})
					.OrderBy(c => c.Type, Comparer<Type>.Create((a, _) => a == typeof(ArgumentResult) ? 0 : 1))
					.ToArray();
				var nameLength = parameters.Max(p => p.Name.Length) + 1; // Add 1 for the colon

				logger.LogDebug("Command: {CommandName}", cmd.Command.Name);

				var sb = new StringBuilder();
				var fmt = new List<object>();

				sb.AppendLine("Parameters:");

				for (var i = 0; i < parameters.Length; i++)
				{
					object? value;
					int maximumNumberOfValues;
					var parameter = parameters[i];

					if (parameter.Symbol is ArgumentResult arg)
					{
						value = cmd.GetValueForArgument(arg.Argument);
						maximumNumberOfValues = arg.Argument.Arity.MaximumNumberOfValues;
					}
					else if (parameter.Symbol is OptionResult opt)
					{
						value = cmd.GetValueForOption(opt.Option);
						maximumNumberOfValues = opt.Option.Arity.MaximumNumberOfValues;
					}
					else
					{
						continue;
					}

					WriteSymbol(ref sb, ref fmt, i, parameter.Name, nameLength, maximumNumberOfValues, value);

					if (i + 1 < parameters.Length)
						sb.AppendLine();
				}

				var compiled = sb.ToString();
				logger.LogDebug(compiled, fmt.ToArray());

				await next(context);
			};

	private static void WriteSymbol(
		ref StringBuilder sb,
		ref List<object> fmt,
		int idx,
		string name,
		int nameLength,
		int maximumNumberOfValues,
		object? value)
	{
		var structuredValueName = "ParameterValue" + idx;

		sb.AppendFormat("    {0}:{1}", name, new string(' ', nameLength - name.Length));

		if (maximumNumberOfValues > 1)
		{
			if (value is not IEnumerable<Token> enumerable)
				// If maximumNumberOfValues > 1, the value IS an IEnumerable.
				// However, to please Polaris, explicitly check for this scenario.
				return;

			var tokens = enumerable.ToArray();

			if (tokens.Length == 0)
				WriteEmpty(ref sb, ref fmt, structuredValueName);
			else if (tokens.Length == 1)
				WriteSingle(ref sb, ref fmt, structuredValueName, tokens.Single().Value);
			else
				WriteArray(ref sb, ref fmt, structuredValueName, nameLength, tokens.Select(f => f.Value));
		}
		else
		{
			if (value == null)
				WriteNull(ref sb, ref fmt, structuredValueName);
			else
				WriteSingle(ref sb, ref fmt, structuredValueName, value);
		}
	}

	private static void WriteNull(ref StringBuilder sb, ref List<object> fmt, string valueName)
	{
		sb.AppendFormat("{{{0}}}", valueName);
		fmt.Add(null!);
	}

	private static void WriteEmpty(ref StringBuilder sb, ref List<object> fmt, string valueName)
	{
		sb.AppendFormat("{{{0}}}", valueName);
		fmt.Add(Array.Empty<object>());
	}

	private static void WriteSingle(ref StringBuilder sb, ref List<object> fmt, string valueName, object value)
	{
		sb.AppendFormat("{{{0}}}", valueName);
		fmt.Add(value.ToString());
	}

	private static void WriteArray(ref StringBuilder sb, ref List<object> fmt, string valueName, int nameLength, IEnumerable<object> values)
	{
		var array = values.ToArray();
		for (var i = 0; i < array.Length; i++)
		{
			var elementName = valueName + "A" + i;
			var elementValue = array[i];
			var paddingLength = i == 0 ? 0 : nameLength + 4 + 1;
			var padding = paddingLength == 0 ? string.Empty : new string(' ', paddingLength);
			sb.AppendFormat("{0}{{{1}}}", padding, elementName);
			fmt.Add(elementValue.ToString());

			if (i + 1 < array.Length)
				sb.AppendLine();
		}
	}
}