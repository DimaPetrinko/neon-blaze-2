using UnityEngine;

namespace NeonBlaze.PlayerMechanics.CharacterActions
{
	[CreateAssetMenu(menuName = "Create DashAction", fileName = "DashAction", order = 0)]
	public sealed class DashAction : CharacterAction
	{
		[SerializeField] private float m_DashSpeed;

		public override CharacterActionExecutor GetExecutor() => new DashActionExecutor(m_DashSpeed,
			m_StaminaCost, m_PrepareDuration, m_PerformDuration, m_RecoveryDuration);
	}

	public sealed class DashActionExecutor : CharacterActionExecutor
	{
		private readonly float mDashSpeed;

		public DashActionExecutor(float dashSpeed, float staminaCost,
			float prepareDuration, float performDuration, float recoveryDuration)
			: base(staminaCost, prepareDuration, performDuration, recoveryDuration)
		{
			mDashSpeed = dashSpeed;
		}

		protected override void Prepare(Character character)
		{
			character.OverrideMovementDirection(character.MovementDirection * mDashSpeed);
		}

		protected override void Perform(Character character)
		{
			character.Stats.Stamina -= mStaminaCost;
		}

		protected override void Recover(Character character)
		{
		}

		protected override void Finish(Character character)
		{
			character.MovementOverrideEnabled = false;
		}
	}

}