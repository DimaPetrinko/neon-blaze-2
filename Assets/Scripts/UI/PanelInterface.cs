using System;
using System.Collections.Generic;
using Unity.UIElements.Runtime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NeonBlaze.UI
{
	[RequireComponent(typeof(PanelRenderer))]
	public abstract class PanelInterface : MonoBehaviour
	{
		protected PanelRenderer mPanelRenderer;

		protected virtual void Awake()
		{
			mPanelRenderer = GetComponent<PanelRenderer>();
		}

		protected virtual void OnEnable()
		{
			mPanelRenderer.postUxmlReload = Bind;
			mPanelRenderer.visualTree.visible = true;
		}

		private void OnDisable()
		{
			mPanelRenderer.visualTree.visible = false;
		}

		protected abstract IEnumerable<Object> Bind();

		protected abstract void OnInitialized();
	}
}