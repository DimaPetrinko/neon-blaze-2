using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NeonBlaze.UI
{
	public abstract class BaseUIPanel : MonoBehaviour
	{
		public RectTransform RectTransform
		{
			get
			{
				if (mRectTransform == null) mRectTransform = (RectTransform)transform;
				return mRectTransform;
			}
		}

		[SerializeField] private bool m_RemoveLayouts = true;

		private RectTransform mRectTransform;

		protected virtual void Start()
		{
			StartCoroutine(RemoveLayouts());
		}

		private IEnumerator RemoveLayouts()
		{
			yield return null;

			foreach (var c in GetComponentsInChildren<LayoutGroup>()) Destroy(c);
			foreach (var c in GetComponentsInChildren<ContentSizeFitter>()) Destroy(c);
		}
	}
}