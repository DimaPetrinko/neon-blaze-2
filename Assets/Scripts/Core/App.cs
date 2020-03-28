using System;
using System.Collections;
using System.Collections.Generic;
using NeonBlaze.Core.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NeonBlaze.Core
{
	public class App : MonoBehaviour
	{
		[SerializeField] private FadePanelInterface m_FadePanel;

		public static App Instance { get; private set; }

		public FadePanelInterface FadePanel => m_FadePanel;

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

			var queue = new Queue<Action>();

			void NextAction()
			{
				if (queue.Count > 0) queue.Dequeue()?.Invoke();
				else FinishSequence();
			}

			void FinishSequence()
			{
				queue.Clear();
				callback?.Invoke();
			}

			if (fromPresent && FadePanel != null) queue.Enqueue(() => FadePanel.FadeIn(NextAction));
			if (fromPresent) queue.Enqueue(() => StartCoroutine(TrackAsyncOperation(UnloadScene(fromScene), NextAction)));
			queue.Enqueue(() => StartCoroutine(TrackAsyncOperation(LoadScene(toScene), NextAction)));
			if (FadePanel != null) queue.Enqueue(() => FadePanel.FadeOut(NextAction));

			NextAction();

			// fade
			// unload
			// wait
			// load
			// fade
		}

		private IEnumerator TrackAsyncOperation(AsyncOperation op, Action callback)
		{
			while (!op.isDone)
			{
				Debug.Log($"Progress {op.progress}");
				yield return null;
			}
			callback?.Invoke();

			// while not done
			// skip frame
			// then callback
		}

		private void Awake()
		{
			if (Instance == null) Instance = this;
			else Destroy(gameObject);
		}

		private void Start()
		{
			void OnUIInitialized()
			{
				m_FadePanel.Initialized -= OnUIInitialized;

				FadePanel.FadeImmediately(1);
				TransitionToScene("", "MainMenu");
			}

			if (m_FadePanel.IsInitialized) OnUIInitialized();
			else m_FadePanel.Initialized += OnUIInitialized;
		}
	}
}