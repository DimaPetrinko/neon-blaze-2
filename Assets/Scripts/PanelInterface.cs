using System.Collections.Generic;
using Unity.UIElements.Runtime;
using UnityEngine;

namespace NeonBlaze
{
	[RequireComponent(typeof(PanelRenderer))]
	public abstract class PanelInterface : MonoBehaviour
	{
		protected PanelRenderer mPanelRenderer;

		protected virtual void Awake() => mPanelRenderer = GetComponent<PanelRenderer>();

		protected virtual void OnEnable() => mPanelRenderer.postUxmlReload = Bind;

		protected abstract IEnumerable<Object> Bind();
	}
}