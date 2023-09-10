﻿using System;
using System.CommandLine;
using RGen.Domain.Generating.Generators;


namespace RGen.Application.Commanding.Integer;

public class GenerateIntegerCommand : Command
{
	public GenerateIntegerCommand()
		: base("int", "Generate integer value(s)")
	{
		AddArgument(
			new Argument<int>(
					"n",
					() => 1,
					"The number of values to generate")
				.InValidRangeOnly(1, 1_000_000));

		AddOption(
			new Option<int>(
					"--set",
					() => 1,
					"The number of sets to generate")
				.InValidRangeOnly(1, ushort.MaxValue));

		AddOption(
			new Option<int?>(
					"--length",
					() => null,
					"The length, in number of digits, of the generated number")
				.InValidRangeOnly(1, (int)Math.Log10(int.MaxValue)));

		AddOption(
			new Option<int?>(
					"--min",
					() => null,
					"The minimum value to allow"));

		AddOption(
			new Option<int?>(
					"--max",
					() => null,
					"The maximum value to allow"));

//TODO: Maybe this is not "format" but "base" (as in 2, 8, 10, 16), and format should be e.g. prefixed/capitals etc.? 
		AddOption(
			new Option<IntegerFormat>(
					"--format",
					() => IntegerFormat.Decimal,
					"The representation to use"));
	}
}