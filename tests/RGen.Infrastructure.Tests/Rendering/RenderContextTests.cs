using NUnit.Framework;
using RGen.Domain.Rendering;
using Shouldly;


namespace RGen.Infrastructure.Tests.Rendering;

public class RenderContextTests
{
	[Test]
	public void IsEmpty_should_return_true_if_both_properties_are_null() =>
		new RenderContext(null!, null!)
			.IsEmpty.ShouldBeTrue();

	[Test]
	public void Two_different_empty_instances_shall_be_equal()
	{
		var a = new RenderContext(null!, null!);
		var b = new RenderContext(null!, null!);

		(a == b).ShouldBeTrue();
	}
}