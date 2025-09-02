using UnityEngine;
using ContentContent.SceneManagement;

namespace ContentContent.SceneManagement
{
	/// <summary>
	/// This component will utilize <see cref="SimpleSceneManager"> to handle
	/// loading scenes at runtime
	/// </summary>
    public class SceneNavigator : MonoBehaviour
    {
		public void GoToScene(int index)
		{
			SimpleSceneManager.Instance.GoToScene(index);
		}
	}
}
