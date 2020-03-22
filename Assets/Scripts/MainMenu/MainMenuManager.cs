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
			m_MainMenuPanelInterface.Initialized += OnUIInitialized;
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
			void OnFadeFinished(float target)
			{
				m_FadePanelInterface.FadeFinished -= OnFadeFinished;
				SceneManager.LoadSceneAsync("Game");
			}

			m_FadePanelInterface.FadeFinished += OnFadeFinished;
			m_FadePanelInterface.FadeIn();
		}

		private void OnOptionsButtonClicked() => Debug.Log("Options");

		private void OnExitButtonClicked()
		{
			void OnFadeFinished(float target)
			{
				m_FadePanelInterface.FadeFinished -= OnFadeFinished;
				#if UNITY_EDITOR
				EditorApplication.ExitPlaymode();
				#else
				Application.Quit();
				#endif
			}

			m_FadePanelInterface.FadeFinished += OnFadeFinished;
			m_FadePanelInterface.FadeIn();
		}

		private void OnDestroy()
		{
			m_MainMenuPanelInterface.StartButton.clicked -= OnStartButtonClicked;
			m_MainMenuPanelInterface.OptionsButton.clicked -= OnOptionsButtonClicked;
			m_MainMenuPanelInterface.ExitButton.clicked -= OnExitButtonClicked;
		}
	}
}