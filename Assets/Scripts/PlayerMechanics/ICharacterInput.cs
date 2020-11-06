using UnityEngine;

namespace NeonBlaze.PlayerMechanics
{
	public interface ICharacterInput
	{
		Vector2 MovementDirection { get; }
		bool Dash { get; }
		bool LightAttack { get; }
		bool HeavyAttackHeld { get; }
		bool HeavyAttackReleased { get; }

		void ManualUpdate();

		InputActionType GetNextAction(out bool value);
	}

	public static class InputUtilities
	{
		public static Vector2 NormalizeInput(float x, float y)
		{
			if (x == 0 && y == 0) return Vector2.zero;
			else return new Vector2(Mathf.Abs(x) * x, Mathf.Abs(y) * y) / new Vector2(x, y).magnitude;
		}
	}
}