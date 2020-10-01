namespace NeonBlaze.PlayerMechanics
{
	public static class CharacterStatesExtension
	{
		public static bool Is(this CharacterState state, CharacterState other)
		{
			return state == other;
		}
		public static bool IsNormal(this CharacterState state)
		{
			return state == CharacterState.Normal;
		}
		public static bool IsNormalOrDashCooldown(this CharacterState state)
		{
			return state == CharacterState.Normal || state == CharacterState.DashCooldown;
		}
		public static bool IsAttack(this CharacterState state)
		{
			return state == CharacterState.AttackWindUp || state == CharacterState.AttackHit
				|| state == CharacterState.AttackRecovery;
		}
	}
}