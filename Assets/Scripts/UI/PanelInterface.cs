using System;
using System.Collections.Generic;
using Unity.UIElements.Runtime;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace NeonBlaze.UI
{
	[RequireComponent(typeof(PanelRenderer))]
	public abstract class PanelInterface : MonoBehaviour
	{
		[Range(byte.MinValue, byte.MaxValue)]
		[SerializeField] private byte m_Order = 127;

		protected PanelRenderer mPanelRenderer;
		protected VisualElement mRoot;

		public byte Order => m_Order;

		protected virtual void Awake()
		{
			mPanelRenderer = GetComponent<PanelRenderer>();
		}

		protected virtual void OnEnable()
		{
			mPanelRenderer.postUxmlReload = BindInternal;
			if (mPanelRenderer.visualTree != null) mPanelRenderer.visualTree.visible = true;
		}

		protected virtual void OnDisable()
		{
			mPanelRenderer.visualTree.visible = false;
		}

		protected abstract void Bind();

		protected abstract void OnInitialized();

		private IEnumerable<Object> BindInternal()
		{
			mRoot = mPanelRenderer.visualTree;
			Bind();
			OnInitialized();
			return null;
		}
	}
}