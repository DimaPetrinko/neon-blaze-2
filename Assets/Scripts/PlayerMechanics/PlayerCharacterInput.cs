using System.Collections.Generic;
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

		private readonly Queue<InputActionType> mActionQueue = new Queue<InputActionType>();

		public void ManualUpdate()
		{
			MovementDirection = InputUtilities.NormalizeInput(
				Input.GetAxisRaw("Horizontal"),
				Input.GetAxisRaw("Vertical"));

			if (Input.GetButtonDown("Dash")) mActionQueue.Enqueue(InputActionType.Dash);
			if (Input.GetButtonDown("LightAttack")) mActionQueue.Enqueue(InputActionType.LightAttack);
			HeavyAttackHeld = Input.GetButton("HeavyAttack");
			if (Input.GetButtonUp("HeavyAttack")) mActionQueue.Enqueue(InputActionType.HeavyAttack);
		}

		public InputActionType GetNextAction(out bool value)
		{
			if (mActionQueue.Count > 0)
			{
				value = true;
				return mActionQueue.Dequeue();
			}
			else
			{
				value = false;
				return InputActionType.None;
			}
		}
	}

	public enum InputActionType
	{
		None, Dash, LightAttack, HeavyAttack
	}
}