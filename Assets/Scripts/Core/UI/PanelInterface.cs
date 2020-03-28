using System;
using System.Collections.Generic;
using Unity.UIElements.Runtime;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace NeonBlaze.Core.UI
{
	[RequireComponent(typeof(PanelRenderer))]
	public abstract class PanelInterface : MonoBehaviour
	{
		public event Action Initialized;

		[Range(byte.MinValue, byte.MaxValue)]
		[SerializeField] private byte m_Order = 127;

		private PanelRenderer mPanelRenderer;
		protected VisualElement mRoot;

		public PanelRenderer Renderer
		{
			get
			{
				if (mPanelRenderer == null) Awake();
				return mPanelRenderer;
			}
		}
		public bool IsInitialized { get; protected set; }
		public byte Order => m_Order;

		protected virtual void Awake()
		{
			mPanelRenderer = GetComponent<PanelRenderer>();
			IsInitialized = false;
		}

		protected virtual void OnEnable()
		{
			Renderer.postUxmlReload = BindInternal;
			if (Renderer.visualTree != null) Renderer.visualTree.visible = true;
		}

		protected virtual void OnDisable()
		{
			Renderer.visualTree.visible = false;
		}

		protected virtual void OnDestroy()
		{
			Initialized = null;
		}

		protected abstract void Bind();

		private IEnumerable<Object> BindInternal()
		{
			mRoot = Renderer.visualTree;
			Bind();
			IsInitialized = true;
			Initialized?.Invoke();
			return null;
		}
	}
}