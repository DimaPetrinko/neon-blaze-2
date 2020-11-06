using UnityEngine;

namespace NeonBlaze.Utils
{
	public static class VectorExtensions
	{
		public static bool ShorterThan(this Vector3 vector, float value)
		{
			return vector.sqrMagnitude < value * value;
		}

		public static bool LongerThan(this Vector3 vector, float value)
		{
			return vector.sqrMagnitude > value * value;
		}

		public static bool Equal(this Vector3 vector, float value)
		{
			return !vector.ShorterThan(value) && !vector.LongerThan(value);
		}

		public static bool ShorterThan(this Vector2 vector, float value)
		{
			return vector.sqrMagnitude < value * value;
		}

		public static bool LongerThan(this Vector2 vector, float value)
		{
			return vector.sqrMagnitude > value * value;
		}

		public static bool Equal(this Vector2 vector, float value)
		{
			return !vector.ShorterThan(value) && !vector.LongerThan(value);
		}

		public static bool IsCloseTo(this Vector3 vector, Vector3 other, float threshold = 0.001f)
		{
			return (vector - other).ShorterThan(threshold);
		}

		public static bool IsCloseTo(this Vector2 vector, Vector2 other, float threshold = 0.001f)
		{
			return (vector - other).ShorterThan(threshold);
		}
	}
}