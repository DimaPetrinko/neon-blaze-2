using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NeonBlaze.Core
{
	public class App : MonoBehaviour
	{
		[SerializeField] private FadePanelView m_FadePanel;

		public static App Instance { get; private set; }

		public FadePanelView FadePanel => m_FadePanel;

		public AsyncOperation LoadScene(string sceneName, bool additive = true)
		{
			return SceneManager.LoadSceneAsync(sceneName, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
		}

		public AsyncOperation UnloadScene(string sceneName)
		{
			return SceneManager.UnloadSceneAsync(sceneName);
		}

		public void TransitionToScene(string fromScene, string toScene, Action callback = null)
		{
			var fromPresent = !string.IsNullOrWhiteSpace(fromScene);
			var toPresent = !string.IsNullOrWhiteSpace(toScene);

			if (!toPresent)
			{
				callback?.Invoke();
				Debug.LogError("No to scene was specified. Aborting!");
				return;
			}

			var sequence  = new Sequence<Action>();
			if (fromPresent && FadePanel != null) sequence.Add(() => FadePanel.FadeIn(() => sequence.Next()));
			if (fromPresent) sequence.Add(() => StartCoroutine(TrackAsyncOperation(UnloadScene(fromScene), () => sequence.Next())));
			sequence.Add(() => StartCoroutine(TrackAsyncOperation(LoadScene(toScene), () => sequence.Next())));
			if (FadePanel != null) sequence.Add(() => FadePanel.FadeOut(() => sequence.Next()));

			sequence.Finished += callback;
			sequence.Start();
		}

		private IEnumerator TrackAsyncOperation(AsyncOperation op, Action callback)
		{
			while (!op.isDone)
			{
				Debug.Log($"Progress {op.progress}");
				yield return null;
			}
			callback?.Invoke();
		}

		private void Awake()
		{
			if (Instance == null) Instance = this;
			else Destroy(gameObject);
		}

		private void Start()
		{
			// void OnUIInitialized()
			// {
			// 	m_FadePanel.Initialized -= OnUIInitialized;
			//
			// 	FadePanel.FadeImmediately(1);
			// 	TransitionToScene("", "MainMenu");
			// }
			//
			// if (m_FadePanel.IsInitialized) OnUIInitialized();
			// else m_FadePanel.Initialized += OnUIInitialized;

			TransitionToScene("", "MainMenu");
		}
	}
}