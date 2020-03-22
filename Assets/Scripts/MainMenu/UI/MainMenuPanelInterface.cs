using UnityEngine.UIElements;
using NeonBlaze.UI;

namespace NeonBlaze.MainMenu.UI
{
	public class MainMenuPanelInterface : PanelInterface
	{
		public Button StartButton { get; private set; }
		public Button OptionsButton { get; private set; }
		public Button ExitButton { get; private set; }

		protected override void Bind()
		{
			var root = mPanelRenderer.visualTree;
			StartButton = root.Q<Button>("start-button");
			OptionsButton = root.Q<Button>("options-button");
			ExitButton = root.Q<Button>("exit-button");
		}

		protected override void Bound() {}
	}
}