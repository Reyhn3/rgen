﻿namespace RGen.Domain.Generating;


public interface IGenerator
{
//TODO: #42: Refactor to use parameter-object instead of list of parameters
	IRandomValues Generate(
		int numberOfElements,
		int numberOfSets,
		int? lengthOfElement,
		ulong? min,
		ulong? max);
}