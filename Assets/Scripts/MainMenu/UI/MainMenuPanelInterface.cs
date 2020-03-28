using NeonBlaze.Core.UI;
using UnityEngine.UIElements;

namespace NeonBlaze.MainMenu.UI
{
	public class MainMenuPanelInterface : PanelInterface
	{
		public Button StartButton { get; private set; }
		public Button OptionsButton { get; private set; }
		public Button ExitButton { get; private set; }

		protected override void Bind()
		{
			StartButton = mRoot.Q<Button>("start-button");
			OptionsButton = mRoot.Q<Button>("options-button");
			ExitButton = mRoot.Q<Button>("exit-button");
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			StartButton = null;
			OptionsButton = null;
			ExitButton = null;
		}
	}
}