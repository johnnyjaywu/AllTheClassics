using System;
using UnityEngine;
using UnityEngine.UI;

namespace AllTheClassics.TicTacToe
{
	[RequireComponent(typeof(Button))]
	public class TicTacToeCell : MonoBehaviour
	{

		[SerializeField]
		private GameObject markO;

		[SerializeField]
		private GameObject markX;

		public TicTacToe.Mark CurrentMark { get; private set; }
		public event Action<TicTacToeCell> ButtonClickEvent;

		private void Awake()
		{
			GetComponent<Button>().onClick.AddListener(() => { ButtonClickEvent?.Invoke(this); });
			ResetMark();
		}

		public void ResetMark()
		{
			CurrentMark = TicTacToe.Mark.None;
			markO.SetActive(false);
			markX.SetActive(false);
		}

		public void SetMark(TicTacToe.Mark mark)
		{
			markO.SetActive(mark == TicTacToe.Mark.O ? true : false);
			markX.SetActive(mark == TicTacToe.Mark.X ? true : false);
			CurrentMark = mark;
		}
	}
}