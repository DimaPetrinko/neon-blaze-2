using UnityEngine;

namespace NeonBlaze.PlayerMechanics.Input
{
	// TODO: here will be the enemy's brain
	public sealed class EnemyInputProvider : MonoBehaviour, IInputProvider
	{
		public Priority Priority => Priority.High;
		public Vector2 MovementDirection { get; private set; }
		public Vector2 LookDirection { get; private set; }
		public bool Dash { get; private set; }
		public bool LightAttack { get; private set; }
		public bool HeavyAttackHeld { get; private set; }
		public bool HeavyAttackReleased { get; private set; }
		public bool LookDirectionChanged { get; private set; }
	}
}