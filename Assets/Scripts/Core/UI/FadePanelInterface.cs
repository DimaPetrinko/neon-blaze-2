using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace NeonBlaze.Core.UI
{
	public class FadePanelInterface : PanelInterface
	{
		public delegate void FadeDelegate(float target);

		public event FadeDelegate FadeStarted;
		public event FadeDelegate FadeFinished;

		[SerializeField] private float m_FadeSpeed = 1f;

		private VisualElement mFadePanel;
		private float mTargetOpacity;
		private bool mFading;

		[ContextMenu("Fade in")]
		public void FadeIn()
		{
			Fade(1f);
		}

		public void FadeIn(Action callback)
		{
			void OnFadeFinished(float t)
			{
				FadeFinished -= OnFadeFinished;
				callback();
			}

			FadeFinished += OnFadeFinished;
			FadeIn();
		}

		[ContextMenu("Fade out")]
		public void FadeOut()
		{
			Fade(0f);
		}

		public void FadeOut(Action callback)
		{
			void OnFadeFinished(float t)
			{
				FadeFinished -= OnFadeFinished;
				callback();
			}

			FadeFinished += OnFadeFinished;
			FadeOut();
		}

		public void Fade(float target)
		{
			mTargetOpacity = target;
			if (mFading) return;
			if (gameObject.activeInHierarchy) StartCoroutine(FadeCoroutine());
			else FadeImmediately(target);
		}

		public void Fade(float target, Action callback)
		{
			void OnFadeFinished(float t)
			{
				FadeFinished -= OnFadeFinished;
				callback();
			}

			FadeFinished += OnFadeFinished;
			Fade(target);
		}

		public void FadeImmediately(float target)
		{
			mTargetOpacity = target;
			if (!mFading) StartFade();
			ApplyOpacity(mTargetOpacity);
			FinishFade();
		}

		public void FadeImmediately(float target, Action callback)
		{
			void OnFadeFinished(float t)
			{
				FadeFinished -= OnFadeFinished;
				callback();
			}

			FadeFinished += OnFadeFinished;
			FadeImmediately(target);
		}

		protected override void Awake()
		{
			base.Awake();
			void OnInitialized()
			{
				Initialized -= OnInitialized;
				FadeImmediately(0);
			}

			Initialized += OnInitialized;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			mFading = false;
		}

		protected override void Bind()
		{
			mFadePanel = mRoot.Q("fade-plane");
		}

		private IEnumerator FadeCoroutine()
		{
			if (Renderer == null)
			{
				FinishFade();
				yield break;
			}
			StartFade();
			var currentOpacity = mFadePanel.style.opacity;
			while (!Mathf.Approximately(currentOpacity.value, mTargetOpacity))
			{
				if (mFadePanel != null)
				{
					var wasBelowTarget = currentOpacity.value < mTargetOpacity;
					var direction = wasBelowTarget ? 1 : -1;
					currentOpacity.value += direction * m_FadeSpeed;
					currentOpacity.value = wasBelowTarget
						? Mathf.Min(currentOpacity.value, mTargetOpacity)
						: Mathf.Max(currentOpacity.value, mTargetOpacity);
					ApplyOpacityUnsafe(currentOpacity);
				}

				yield return null;
			}
			FinishFade();
		}

		private void StartFade()
		{
			mFading = true;
			FadeStarted?.Invoke(mTargetOpacity);
		}

		private void FinishFade()
		{
			mFading = false;
			FadeFinished?.Invoke(mTargetOpacity);
		}

		private void ApplyOpacity(float opacity)
		{
			if (mFadePanel == null)
			{
				Debug.LogError("Fade panel is null");
				return;
			}

			var opacityStyle = mFadePanel.style.opacity;
			opacityStyle.value = opacity;
			ApplyOpacityUnsafe(opacityStyle);
		}

		private void ApplyOpacityUnsafe(StyleFloat style)
		{
			mFadePanel.style.opacity = style;
		}
	}
}