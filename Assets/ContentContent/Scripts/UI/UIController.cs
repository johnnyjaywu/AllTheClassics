using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ContentContent.UI
{
    public class UIController : MonoBehaviour
    {
		[SerializeField]
		private UIPanel firstActiveMenu;

		[SerializeField]
		private List<UIPanel> menus = new List<UIPanel>();

		private void Awake()
		{
			// Find all menus
			menus = FindObjectsByType<UIPanel>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
		}

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

		public void GoToMenu(UIPanel menu)
		{
			if (menus.Contains(menu))
			{
				GoToMenu(menus.IndexOf(menu));
			}
		}

		public void GoToMenu(int index)
		{
			// Deactivate all menus except the target
			foreach (UIPanel menu in menus)
			{
				if (menu != null)
				{
					menu.gameObject.SetActive(false);
				}
			}

			menus[index].gameObject.SetActive(true);
		}
	}
}
