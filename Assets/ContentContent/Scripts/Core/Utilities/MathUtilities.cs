using UnityEngine;

namespace ContentContent.Core
{
	public static class MathUtilities
	{
		public static Vector2 GetPointInCircle(float degree, Vector2 center, float radius = 1, bool clockwise = false)
		{
			degree = Wrap360(degree);
			degree = clockwise ? -degree : degree;
			return DegreeToVector2(degree) * radius + center;
		}

		public static float Vector2ToDegree(Vector2 direction)
		{
			return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		}

		public static Vector2 RadianToVector2(float radian)
		{
			return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
		}

		public static Vector2 DegreeToVector2(float degree)
		{
			return RadianToVector2(degree * Mathf.Deg2Rad);
		}

		public static float Wrap360(float degree)
		{
			return degree >= 0 ? degree % 360 : degree % 360 + 360;
		}

		public static float DistanceSquared(Vector3 a, Vector3 b)
		{
			return (b - a).sqrMagnitude;
		}

		public static Vector3 GetNearestPoint(Vector3 from, Vector3[] targets)
		{
			if (targets == null || targets.Length == 0)
				return Vector3.zero;

			int nearestIndex = 0;
			float nearestDistanceSquared = float.MaxValue;
			for (int i = 0; i < targets.Length; ++i)
			{
				Vector3 target = targets[i];
				float distSqr = (target - from).sqrMagnitude;
				if (nearestDistanceSquared >= distSqr)
				{
					nearestDistanceSquared = distSqr;
					nearestIndex = i;
				}
			}
			return targets[nearestIndex];
		}

		public static uint Fibonacci(uint n)
		{
			if (n == 0) return 0;
			if (n == 1) return 1;
			uint count = n - 1;
			uint[] fib = new uint[count + 1];
			fib[0] = 0;
			fib[1] = 1;
			for (int i = 2; i <= count; ++i)
			{
				fib[i] = fib[i - 2] + fib[i - 1];
			}
			return fib[count];
		}
	} 
}