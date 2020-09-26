using UnityEngine;

namespace NeonBlaze.PlayerMechanics
{
	public interface ICharacterInput
	{
		Vector2 MovementDirection { get; }
		bool Dash { get; }
		bool LightAttack { get; }
		bool HeavyAttack { get; }

		void ManualUpdate();
	}
}