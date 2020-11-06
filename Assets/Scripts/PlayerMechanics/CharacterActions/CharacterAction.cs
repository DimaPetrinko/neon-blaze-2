using System;
using System.Collections;
using UnityEngine;

namespace NeonBlaze.PlayerMechanics.CharacterActions
{
	public abstract class CharacterAction : ScriptableObject
	{
		[SerializeField] [Range(0, 200)] protected float m_StaminaCost = 20;
		[SerializeField] [Range(0, 5)] protected float m_PrepareDuration = 0.5f;
		[SerializeField] [Range(0, 5)] protected float m_PerformDuration = 0.1f;
		[SerializeField] [Range(0, 10)] protected float m_RecoveryDuration = 0.3f;

		public abstract CharacterActionExecutor GetExecutor();
	}

	public abstract class CharacterActionExecutor
	{
		public event Action BeforePrepareCalled;
		public event Action AfterPrepareCalled;
		public event Action BeforePerformCalled;
		public event Action AfterPerformCalled;
		public event Action BeforeRecoveryCalled;
		public event Action AfterRecoveryCalled;
		public event Action Executed;

		protected readonly float mStaminaCost;
		protected readonly float mPrepareDuration;
		protected readonly float mPerformDuration;
		protected readonly float mRecoveryDuration;

		protected CharacterActionExecutor(float staminaCost, float prepareDuration,
			float performDuration, float recoveryDuration)
		{
			mStaminaCost = staminaCost;
			mPrepareDuration = prepareDuration;
			mPerformDuration = performDuration;
			mRecoveryDuration = recoveryDuration;
		}

		public IEnumerator Execute(Character character)
		{
			BeforePrepareCalled?.Invoke();
			Prepare(character);
			AfterPrepareCalled?.Invoke();

			yield return new WaitForSeconds(mPrepareDuration);

			BeforePerformCalled?.Invoke();
			Perform(character);
			AfterPerformCalled?.Invoke();

			yield return new WaitForSeconds(mPerformDuration);

			BeforeRecoveryCalled?.Invoke();
			Recover(character);
			AfterRecoveryCalled?.Invoke();

			yield return new WaitForSeconds(mRecoveryDuration);

			Finish(character);
			Executed?.Invoke();
		}

		protected abstract void Prepare(Character character);
		protected abstract void Perform(Character character);
		protected abstract void Recover(Character character);
		protected abstract void Finish(Character character);
	}
}