using UnityEngine;

namespace NeonBlaze.PlayerMechanics
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
			MovementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
			Dash = Input.GetButtonDown("Dash");
			LightAttack = Input.GetButtonDown("LightAttack");
			HeavyAttackHeld = Input.GetButton("HeavyAttack");
			HeavyAttackReleased = Input.GetButtonUp("HeavyAttack");
		}
	}
}