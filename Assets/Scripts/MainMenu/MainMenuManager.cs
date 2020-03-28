using NeonBlaze.Core;
using NeonBlaze.MainMenu.UI;
using UnityEditor;
using UnityEngine;

namespace NeonBlaze.MainMenu
{
	public class MainMenuManager : MonoBehaviour
	{
		[SerializeField] private MainMenuPanelInterface m_MainMenuPanelInterface;

		private void Start()
		{
			void OnUIInitialized()
			{
				m_MainMenuPanelInterface.Initialized -= OnUIInitialized;
				m_MainMenuPanelInterface.StartButton.clicked += OnStartButtonClicked;
				m_MainMenuPanelInterface.OptionsButton.clicked += OnOptionsButtonClicked;
				m_MainMenuPanelInterface.ExitButton.clicked += OnExitButtonClicked;
			}

			if (m_MainMenuPanelInterface.IsInitialized) OnUIInitialized();
			else m_MainMenuPanelInterface.Initialized += OnUIInitialized;
		}

		private void OnStartButtonClicked()
		{
			App.Instance.TransitionToScene("MainMenu", "Game");
		}

		private void OnOptionsButtonClicked() => Debug.Log("Options");

		private void OnExitButtonClicked()
		{
			void ExitAction()
			{
				#if UNITY_EDITOR
				EditorApplication.ExitPlaymode();
				#else
				Application.Quit();
				#endif
			}

			App.Instance.FadePanel.FadeIn(ExitAction);
		}

		private void OnDestroy()
		{
			if (m_MainMenuPanelInterface == null) return;
			m_MainMenuPanelInterface.StartButton.clicked -= OnStartButtonClicked;
			m_MainMenuPanelInterface.OptionsButton.clicked -= OnOptionsButtonClicked;
			m_MainMenuPanelInterface.ExitButton.clicked -= OnExitButtonClicked;
		}
	}
}