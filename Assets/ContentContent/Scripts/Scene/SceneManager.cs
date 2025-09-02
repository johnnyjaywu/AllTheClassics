using UnityEngine;
using UnityEngine.SceneManagement;
using ContentContent.Core;
using System.Collections;

namespace ContentContent.SceneManagement
{
	/// <summary>
	/// A singleton that contains scene management logic and can be added as a component. 
	/// Assumes scene index 0 is the root scene that does not get unloaded
	/// </summary>
	public class SceneManager : SingletonBehaviour<SceneManager>
	{
		[SerializeField]
		private int startScene = 1; // The start scene could be a custom splash screen

		[SerializeField]
		private bool autoLoadStartScene = true;

		private int currentSceneIndex;
		private Coroutine loadSceneRoutine;
		private Coroutine unloadSceneRoutine;

		private void Start()
		{
			currentSceneIndex = startScene;
			if (autoLoadStartScene)
			{
				LoadScene(startScene);
			}
		}

		public void LoadScene(int sceneIndex)
		{
			if (currentSceneIndex != sceneIndex)
			{
				if (unloadSceneRoutine != null)
				{
					StopCoroutine(unloadSceneRoutine);
				}
				unloadSceneRoutine = StartCoroutine(UnloadSceneAt(currentSceneIndex));
			}

			if (loadSceneRoutine != null)
			{
				StopCoroutine(loadSceneRoutine);
			}
			loadSceneRoutine = StartCoroutine(LoadSceneAt(sceneIndex));
		}

		private IEnumerator LoadSceneAt(int sceneIndex)
		{
			// The Application loads the Scene in the background as the current Scene runs.
			// This is particularly good for creating loading screens.

			AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

			// Wait until the asynchronous scene fully loads
			while (!asyncLoad.isDone)
			{
				yield return null;
			}

			currentSceneIndex = sceneIndex;
		}

		private IEnumerator UnloadSceneAt(int sceneIndex)
		{
			if (sceneIndex == 0) yield break;

			AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneIndex);

			// Wait until the asynchronous scene fully loads
			while (!asyncLoad.isDone)
			{
				yield return null;
			}
		}
	}
}