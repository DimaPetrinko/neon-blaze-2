using UnityEngine;

namespace NeonBlaze.PlayerMechanics.Input
{
	public sealed class KeyboardMouseInputProvider : PlayerInputProvider
	{
		[SerializeField] private Character m_Character;
		[SerializeField] private Camera m_Camera;

		public override Priority Priority => Priority.Medium;

		public override Vector2 LookDirection
		{
			get
			{
				Vector2 worldMousePosition = m_Camera. ScreenToWorldPoint(UnityEngine.Input.mousePosition);
				return (worldMousePosition - m_Character.Position).normalized;
			}
		}
	}
}