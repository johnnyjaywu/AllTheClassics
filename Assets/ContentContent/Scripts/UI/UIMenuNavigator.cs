using System.Collections.Generic;
using UnityEngine;

namespace ContentContent.UI
{
	public class UIMenuNavigator : MonoBehaviour
	{
		[SerializeField]
		private GameObject firstActiveMenu;

		[SerializeField]
		private List<GameObject> menus = new List<GameObject>();

		private void Start()
		{
			GoToMenu(menus[0]);
		}

		private void OnValidate()
		{
			// Puts the first active menu in the front of the list
			if (firstActiveMenu != null)
			{
				if (!menus.Contains(firstActiveMenu))
				{
					menus.Insert(0, firstActiveMenu);
				}
			}
		}

		public void GoToMenu(GameObject menu)
		{
			if (menus.Contains(menu))
			{
				GoToMenu(menus.IndexOf(menu));
			}
		}

		public void GoToMenu(int index)
		{
			// Deactivate all menus except the target
			foreach (GameObject menu in menus)
			{
				if (menu != null)
				{
					menu.SetActive(false);
				}
			}

			menus[index].SetActive(true);
		}
	}
}