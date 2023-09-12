﻿using System.Numerics;


namespace RGen.Domain;


public static class MathUtils
{
	public static int CountNumberOfDecimalDigits(ulong n)
	{
		// Using mathematics is 100x faster than switching,
		// but unfortunately for Int64 with more than 14 digits,
		// the Log10 method is unreliable due to double precision.

		return n switch
			{
				< 10UL                   => 1,
				< 100UL                  => 2,
				< 1000UL                 => 3,
				< 10000UL                => 4,
				< 100000UL               => 5,
				< 1000000UL              => 6,
				< 10000000UL             => 7,
				< 100000000UL            => 8,
				< 1000000000UL           => 9,
				< 10000000000UL          => 10,
				< 100000000000UL         => 11,
				< 1000000000000UL        => 12,
				< 10000000000000UL       => 13,
				< 100000000000000UL      => 14,
				< 1000000000000000UL     => 15,
				< 10000000000000000UL    => 16,
				< 100000000000000000UL   => 17,
				< 1000000000000000000UL  => 18,
				< 10000000000000000000UL => 19,
				_                        => 20
			};
	}
}