using UnityEngine;

namespace NeonBlaze.PlayerMechanics.Input
{
	public interface ICharacterInput
	{
		Vector2 MovementDirection { get; }
		bool Dash { get; }
		bool LightAttack { get; }
		bool HeavyAttackHeld { get; }
		bool HeavyAttackReleased { get; }

		void ManualUpdate();
	}
}