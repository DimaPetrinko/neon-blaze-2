using System;
using System.Collections.Generic;
using UnityEngine;

namespace NeonBlaze.Core
{
	[RequireComponent(typeof(Animator))]
	public sealed class FadePanelView : MonoBehaviour
	{
		public event Action<float> AnimationCompleted;

		private Animator mAnimator;
		private byte mCurrentIndex;

		private Dictionary<byte, Action<float>> mCallbacks;

		private void Awake()
		{
			mAnimator = GetComponent<Animator>();
		}

		public void FadeImmediately(int i) {}
		public void FadeIn(Action action) {}
		public void FadeOut(Action action) {}


		[ContextMenu("Show")]
		public void Show()
		{
			ToggleShow(true, () => Debug.Log("Shown"));
		}

		[ContextMenu("Hide")]
		public void Hide()
		{
			ToggleShow(false, () => Debug.Log("Hidden"));
		}

		public void ToggleShow(bool show, Action callback)
		{
			void OnAnimationCompleted(byte i, Action c)
			{
				AnimationCompleted -= mCallbacks[i];
				mCallbacks.Remove(i);
				if (i == mCurrentIndex) c?.Invoke();
			}

			mCurrentIndex++;
			Action<float> action = target => OnAnimationCompleted(mCurrentIndex, callback);
			mCallbacks.Add(mCurrentIndex, action);
			AnimationCompleted += action;

			mAnimator.SetBool("Show", show);
		}

		public void OnAnimationCompleted(float target)
		{
			AnimationCompleted?.Invoke(target);
		}
	}
}