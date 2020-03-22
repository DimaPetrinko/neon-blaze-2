using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace NeonBlaze.UI
{
	public class FadePanelInterface : PanelInterface
	{
		[SerializeField] private float m_FadeSpeed = 1f;

		private VisualElement mFadePanel;
		private float mTargetOpacity;
		private bool mFading;

		#if UNITY_EDITOR
		[ContextMenu("Fade in")]
		public void FadeIn()
		{
			Fade(1f);
		}

		[ContextMenu("Fade out")]
		public void FadeOut()
		{
			Fade(0f);
		}
		#endif

		public void Fade(float target)
		{
			mTargetOpacity = target;
			if (!mFading)
			{
				if (gameObject.activeInHierarchy) StartCoroutine(FadeCoroutine());
				else ApplyOpacity(target);
			}
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

		protected override void OnInitialized()
		{
			ApplyOpacity(0f);
		}

		private IEnumerator FadeCoroutine()
		{
			mFading = true;
			if (mPanelRenderer == null) yield break;
			var currentOpacity = mFadePanel.style.opacity;
			while (currentOpacity.value != mTargetOpacity)
			{
				if (mFadePanel != null)
				{
					var direction = currentOpacity.value < mTargetOpacity ? 1 : -1;
					currentOpacity.value += direction * m_FadeSpeed;
					if (direction > 0 && currentOpacity.value > mTargetOpacity
						|| direction < 0 && currentOpacity.value < mTargetOpacity)
						currentOpacity.value = mTargetOpacity;
					ApplyOpacityUnsafe(currentOpacity);
				}

				yield return null;
			}
			mFading = false;
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