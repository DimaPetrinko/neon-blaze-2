using System.Linq;
using Unity.UIElements.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NeonBlaze.Core.UI
{
	public class UIOrderManager : MonoBehaviour
	{

		private void Awake()
		{
			void OnSceneLoaded(Scene s, LoadSceneMode m) => ApplyOrder();

			SceneManager.sceneLoaded += OnSceneLoaded;
			ApplyOrder();
		}

		private void ApplyOrder()
		{
			var panels = FindObjectsOfType<PanelInterface>();
			panels = panels.OrderBy(i => i.Order).ToArray();

			foreach (var p in panels)
			{
				InternalBridge.UnregisterPanel(p.gameObject.GetInstanceID());
				InternalBridge.RegisterPanel(p.gameObject.GetInstanceID(), p.Renderer.panel);
			}
		}
	}
}