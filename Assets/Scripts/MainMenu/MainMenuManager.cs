using NeonBlaze.MainMenu.UI;
using NeonBlaze.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NeonBlaze.MainMenu
{
	public class MainMenuManager : MonoBehaviour
	{
		[SerializeField] private MainMenuPanelInterface m_MainMenuPanelInterface;
		[SerializeField] private FadePanelInterface m_FadePanelInterface;

		private void Start()
		{
			if (m_MainMenuPanelInterface.IsInitialized) OnUIInitialized();
			else m_MainMenuPanelInterface.Initialized += OnUIInitialized;
		}

		private void OnUIInitialized()
		{
			m_MainMenuPanelInterface.Initialized -= OnUIInitialized;
			m_MainMenuPanelInterface.StartButton.clicked += OnStartButtonClicked;
			m_MainMenuPanelInterface.OptionsButton.clicked += OnOptionsButtonClicked;
			m_MainMenuPanelInterface.ExitButton.clicked += OnExitButtonClicked;
		}

		private void OnStartButtonClicked()
		{
			void StartAction() => SceneManager.LoadSceneAsync("Game");

			m_FadePanelInterface.FadeIn(StartAction);
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

			m_FadePanelInterface.FadeIn(ExitAction);
		}

		private void OnDestroy()
		{
			m_MainMenuPanelInterface.StartButton.clicked -= OnStartButtonClicked;
			m_MainMenuPanelInterface.OptionsButton.clicked -= OnOptionsButtonClicked;
			m_MainMenuPanelInterface.ExitButton.clicked -= OnExitButtonClicked;
		}
	}
}