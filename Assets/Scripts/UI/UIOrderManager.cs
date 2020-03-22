using System.Linq;
using UnityEngine;

namespace NeonBlaze.UI
{
	public class UIOrderManager : MonoBehaviour
	{
		private PanelInterface[] mPanels;

		private void Awake()
		{
			mPanels = FindObjectsOfType<PanelInterface>();
			mPanels = mPanels.OrderBy(i => i.Order).ToArray();
			// sort depending on the order
			foreach (var p in mPanels)
			{
				p.gameObject.SetActive(false);
				p.gameObject.SetActive(true);
			}
		}
	}
}