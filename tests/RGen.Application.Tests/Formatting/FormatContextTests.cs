using NUnit.Framework;
using RGen.Application.Formatting;
using Shouldly;


namespace RGen.Application.Tests.Formatting;

public class FormatContextTests
{
	[Test]
	public void IsEmpty_should_return_true_if_both_properties_are_null() =>
		new FormatContext(null!, null!)
			.IsEmpty.ShouldBeTrue();

	[Test]
	public void Two_different_empty_instances_shall_be_equal()
	{
		var a = new FormatContext(null!, null!);
		var b = new FormatContext(null!, null!);

		(a == b).ShouldBeTrue();
	}
}