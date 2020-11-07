using UnityEngine;

namespace NeonBlaze.PlayerMechanics.Input
{
	public abstract class PlayerInputProvider : MonoBehaviour, IInputProvider
	{
		public abstract Priority Priority { get; }
		public abstract Vector2 LookDirection { get; }
		public abstract bool LookDirectionChanged { get; protected set; }


		public Vector2 MovementDirection => InputUtilities.NormalizeInput(
			UnityEngine.Input.GetAxisRaw("Horizontal"),
			UnityEngine.Input.GetAxisRaw("Vertical"));
		public bool Dash => UnityEngine.Input.GetButtonDown("Dash");
		public bool LightAttack => UnityEngine.Input.GetButtonDown("LightAttack");
		public bool HeavyAttackHeld => UnityEngine.Input.GetButton("HeavyAttack");
		public bool HeavyAttackReleased => UnityEngine.Input.GetButtonUp("HeavyAttack");
	}
}