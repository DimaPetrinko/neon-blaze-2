using UnityEngine;

namespace NeonBlaze.PlayerMechanics.Input
{
	public sealed class GamepadInputProvider : PlayerInputProvider
	{
		private Vector2 mLastLookDirection;

		public override Priority Priority => Priority.High;
		public override Vector2 LookDirection
		{
			get
			{
				var newDirection = new Vector2(UnityEngine.Input.GetAxisRaw("Mouse X"),
					UnityEngine.Input.GetAxisRaw("Mouse Y")).normalized;
				if (newDirection == Vector2.zero) newDirection = mLastLookDirection;
				LookDirectionChanged = newDirection != mLastLookDirection;
				mLastLookDirection = newDirection;

				return newDirection;
			}
		}
		public override bool LookDirectionChanged { get; protected set; }
	}
}