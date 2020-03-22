using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace NeonBlaze.UI
{
	public class MenuPanelInterface : PanelInterface
	{
		public Button StartButton { get; private set; }
		public Button OptionsButton { get; private set; }
		public Button ExitButton { get; private set; }

		protected override IEnumerable<Object> Bind()
		{
			var root = mPanelRenderer.visualTree;
			StartButton = root.Q<Button>("start-button");
			OptionsButton = root.Q<Button>("options-button");
			ExitButton = root.Q<Button>("exit-button");
			OnInitialized();
			return null;
		}

		protected override void OnInitialized()
		{
			if (StartButton != null) StartButton.clicked += () => Debug.Log("Start!");
			else Debug.LogError($"{nameof(StartButton)} is null");
			if (OptionsButton != null) OptionsButton.clicked += () => Debug.Log("Options");
			else Debug.LogError($"{nameof(OptionsButton)} is null");
			if (ExitButton != null) ExitButton.clicked += () => Debug.Log("Exiting");
			else Debug.LogError($"{nameof(ExitButton)} is null");
		}
	}
}