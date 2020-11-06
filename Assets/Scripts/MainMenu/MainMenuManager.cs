using System;
using NeonBlaze.Core;
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

	public class MainMenuPanelInterface
	{
		public event Action Initialized;
		public MainMenuPanelInterface.Button StartButton { get; set; }
		public MainMenuPanelInterface.Button OptionsButton { get; set; }
		public MainMenuPanelInterface.Button ExitButton { get; set; }
		public bool IsInitialized { get; set; }

		public class Button
		{
			public event Action clicked;
		}
	}
}