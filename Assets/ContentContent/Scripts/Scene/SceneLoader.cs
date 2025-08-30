using ContentContent.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ContentContent.SceneManagement
{
	public class SceneLoader : SingletonBehaviour<SceneLoader>
	{
		[SerializeField]
		private SceneReference loadingScene;

		public static float Progress => Instance.loadAsyncOperation != null ? Instance.loadAsyncOperation.progress : 1f;

		private AsyncOperation loadAsyncOperation;
		private SceneReference sceneToLoad;

		public static void GoToScene(SceneReference scene)
		{
			if (Instance.sceneToLoad != null)
			{
				Debug.LogWarning($"{Instance.sceneToLoad} has not finished loading yet");
				return;
			}

			SceneManager.sceneLoaded += Instance.OnSceneLoaded;
			Instance.sceneToLoad = scene;

			if (Instance.loadingScene != null)
			{
				SceneManager.LoadScene(Instance.loadingScene);
			}
			else
			{
				SceneManager.LoadScene(scene);
			}
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (scene.path == Instance.loadingScene.ScenePath)
			{
				Instance.StartCoroutine(Instance.LoadSceneAsync());
			}
			else if (scene.path == Instance.sceneToLoad.ScenePath)
			{
				SceneManager.sceneLoaded -= OnSceneLoaded;
				Instance.sceneToLoad = null;
			}
		}

		private IEnumerator LoadSceneAsync()
		{
			yield return null;
			if (sceneToLoad != null)
			{
				loadAsyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
				while (!loadAsyncOperation.isDone)
				{
					yield return null;
				}
				loadAsyncOperation = null;
			}
		}
	}
}