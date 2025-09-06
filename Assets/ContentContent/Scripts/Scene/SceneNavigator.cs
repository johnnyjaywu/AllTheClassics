using UnityEngine;
using ContentContent.SceneManagement;

namespace ContentContent.SceneManagement
{
	/// <summary>
	/// This component will utilize <see cref="SceneController"> to handle
	/// loading scenes at runtime
	/// </summary>
    public class SceneNavigator : MonoBehaviour
    {
		public void GoToScene(int index)
		{
			SceneController.Instance.GoToScene(index);
		}
	}
}
