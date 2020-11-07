using System.Linq;
using UnityEngine;

namespace NeonBlaze.PlayerMechanics.Input
{
	public sealed class CharacterInput : MonoBehaviour
	{
		private IInputProvider[] mProviders;
		private IInputProvider mLastProvider;

		public Vector2 MovementDirection { get; private set; }
		public Vector2 LookDirection { get; private set; }
		public bool Dash { get; private set; }
		public bool LightAttack { get; private set; }
		public bool HeavyAttackHeld { get; private set; }
		public bool HeavyAttackReleased { get; private set; }

		public void ManualUpdate()
		{
			if (mProviders.Length == 1)
			{
				var provider = mProviders[0];
				MovementDirection = provider.MovementDirection;
				LookDirection = provider.LookDirection;
				LightAttack = provider.LightAttack;
				HeavyAttackHeld = provider.HeavyAttackHeld;
				HeavyAttackReleased = provider.HeavyAttackReleased;
			}
			else
			{
				MovementDirection = mProviders
					.Select(p => p.MovementDirection)
					.FirstOrDefault(d => d != Vector2.zero);

				var provider = mProviders.FirstOrDefault(p => p.LookDirection != Vector2.zero
					&& p.LookDirectionChanged);
				if (provider == null) provider = mLastProvider;
				else mLastProvider = provider;

				LookDirection = provider?.LookDirection ?? Vector2.right;

				Dash = mProviders.Any(p => p.Dash);
				LightAttack = mProviders.Any(p => p.LightAttack);
				HeavyAttackHeld = mProviders.Any(p => p.HeavyAttackHeld);
				HeavyAttackReleased = mProviders.Any(p => p.HeavyAttackReleased);
			}
		}

		private void Awake()
		{
			mProviders = GetComponents<IInputProvider>();

			switch (mProviders.Length)
			{
				case 0:
					mProviders = new IInputProvider[] {new DefaultInputProvider()};
					break;
				case 1: break;
				default:
					mProviders = mProviders
						.OrderByDescending(p => p.Priority)
						.ToArray();
					break;
			}
		}

		private void OnDestroy()
		{
			mProviders = null;
			mLastProvider = null;
		}
	}

	public static class InputUtilities
	{
		public static Vector2 NormalizeInput(float x, float y)
		{
			if (x == 0 && y == 0) return Vector2.zero;
			else return new Vector2(Mathf.Abs(x) * x, Mathf.Abs(y) * y) / new Vector2(x, y).magnitude;
		}
	}
}