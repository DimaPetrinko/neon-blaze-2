using UnityEngine;

namespace NeonBlaze.PlayerMechanics.Input
{
	public sealed class PlayerCharacterInput : MonoBehaviour, ICharacterInput
	{
		public Vector2 MovementDirection { get; private set; }
		public bool Dash { get; private set; }
		public bool LightAttack { get; private set; }
		public bool HeavyAttackHeld { get; private set; }
		public bool HeavyAttackReleased { get; private set; }

		public void ManualUpdate()
		{
			MovementDirection = new Vector2(UnityEngine.Input.GetAxisRaw("Horizontal"), UnityEngine.Input.GetAxisRaw("Vertical")).normalized;
			Dash = UnityEngine.Input.GetButtonDown("Dash");
			LightAttack = UnityEngine.Input.GetButtonDown("LightAttack");
			HeavyAttackHeld = UnityEngine.Input.GetButton("HeavyAttack");
			HeavyAttackReleased = UnityEngine.Input.GetButtonUp("HeavyAttack");
		}
	}
}