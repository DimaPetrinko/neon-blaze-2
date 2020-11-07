using UnityEngine;

namespace NeonBlaze.PlayerMechanics.Input
{
	public sealed class KeyboardMouseInputProvider : PlayerInputProvider
	{
		[SerializeField] private Character m_Character;
		[SerializeField] private Camera m_Camera;

		private Vector3 mLastMousePosition;

		public override Priority Priority => Priority.Medium;

		public override Vector2 LookDirection
		{
			get
			{
				var mousePosition = UnityEngine.Input.mousePosition;
				LookDirectionChanged = mousePosition != mLastMousePosition;
				mLastMousePosition = mousePosition;
				Vector2 worldMousePosition = m_Camera. ScreenToWorldPoint(mousePosition);
				return (worldMousePosition - m_Character.Position).normalized;
			}
		}

		public override bool LookDirectionChanged { get; protected set; }
	}
}