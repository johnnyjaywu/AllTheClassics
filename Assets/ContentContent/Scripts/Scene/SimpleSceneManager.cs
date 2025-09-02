using UnityEngine;
using UnityEngine.SceneManagement;
using ContentContent.Core;
using System.Collections;

namespace ContentContent.SceneManagement
{
	/// <summary>
	/// A singleton that contains scene management logic and can be added as a component.
	/// </summary>
	public class SimpleSceneManager : SingletonBehaviour<SimpleSceneManager>
	{
		[SerializeField, Min(1)]
		private uint startScene; // The start scene could be a custom splash screen

		[SerializeField]
		private bool autoLoadStartScene = false;

		[SerializeField, Tooltip("If true, will load scenes additively (keeping the root scene alive)")]
		private bool useRootScene = false;

		private Coroutine loadSceneRoutine;
		private Coroutine unloadSceneRoutine;

		private void Start()
		{
			if (autoLoadStartScene && startScene > 0)
			{
				GoToScene((int)startScene);
			}
		}

		public void GoToScene(int sceneIndex)
		{
			// Check my current loaded scenes
			Scene activeScene = SceneManager.GetActiveScene();
			if (activeScene.buildIndex == sceneIndex)
			{
				// Scene already loaded, don't do anything
				Debug.LogWarning($"Scene[{sceneIndex}] is already loaded.");
				return;
			}
			else
			{
				// Unload current scene
				if (unloadSceneRoutine != null)
				{
					StopCoroutine(unloadSceneRoutine);
				}
				unloadSceneRoutine = StartCoroutine(UnloadSceneAt(activeScene.buildIndex));

				// Load new scene
				if (loadSceneRoutine != null)
				{
					StopCoroutine(loadSceneRoutine);
				}
				loadSceneRoutine = StartCoroutine(LoadSceneAt(sceneIndex));
			}
		}

		private IEnumerator LoadSceneAt(int sceneIndex)
		{
			LoadSceneMode mode = useRootScene ? LoadSceneMode.Additive : LoadSceneMode.Single;

			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, mode);

			// Wait until the asynchronous scene fully loads
			while (!asyncLoad.isDone)
			{
				yield return null;
			}
		}

		private IEnumerator UnloadSceneAt(int sceneIndex)
		{
			// Don't unload the root scene or skip if not using root scene
			if (sceneIndex == 0 || !useRootScene) yield break;

			AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneIndex);

			// Wait until the asynchronous scene fully unloaded
			while (!asyncUnload.isDone)
			{
				yield return null;
			}
		}
	}
}