using UnityEngine;

namespace NeonBlaze.PlayerMechanics.Input
{
	public interface IInputProvider
	{
		Priority Priority { get; }
		Vector2 MovementDirection { get; }
		Vector2 LookDirection { get; }
		bool Dash { get; }
		bool LightAttack { get; }
		bool HeavyAttackHeld { get; }
		bool HeavyAttackReleased { get; }

		bool LookDirectionChanged { get; }
	}

	public enum Priority
	{
		Low = 0,
		Medium = 1,
		High = 2
	}

	public sealed class DefaultInputProvider : IInputProvider
	{
		public Priority Priority => Priority.Low;
		public Vector2 MovementDirection => Vector2.zero;
		public Vector2 LookDirection => Vector2.right;
		public bool Dash => false;
		public bool LightAttack => false;
		public bool HeavyAttackHeld => false;
		public bool HeavyAttackReleased => false;

		public bool LookDirectionChanged => false;
	}
}