using System;
using System.Linq;

namespace ContentContent.Core
{
	public static class RandomUtilities
	{
		public static string RandomAlphanumeric(int length)
		{
			System.Diagnostics.Debug.Assert(length > 0);

			Random random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

			return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
		}
	} 
}