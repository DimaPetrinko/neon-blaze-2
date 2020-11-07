using UnityEngine;

namespace NeonBlaze.PlayerMechanics.Input
{
	// TODO: implement this. currently it's a placeholder
	public sealed class GamepadInputProvider : PlayerInputProvider
	{
		public override Priority Priority => Priority.High;
		public override Vector2 LookDirection => Vector2.zero;
	}
}