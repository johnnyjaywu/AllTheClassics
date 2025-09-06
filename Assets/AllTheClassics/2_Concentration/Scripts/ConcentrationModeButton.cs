using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AllTheClassics.Concentration
{
	[RequireComponent(typeof(Button))]
	public class ConcentrationModeButton : MonoBehaviour
	{
		[SerializeField]
		private Concentration game;

		[SerializeField]
		private Concentration.GameMode mode;

		private Button button;

		private void Awake()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(() => { 
				game.Mode = mode;
				game.StartGame();
			});
		}
	}
}