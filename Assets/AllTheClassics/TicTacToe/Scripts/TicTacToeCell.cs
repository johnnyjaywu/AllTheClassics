using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AllTheClassics.TicTacToe
{
	/// <summary>
	/// A view-view-model class handling UI visual and click event of a Tic Tac Toe cell
	/// </summary>
	[RequireComponent(typeof(Button))]
	public class TicTacToeCell : MonoBehaviour
	{

		[SerializeField]
		private GameObject markO;

		[SerializeField]
		private GameObject markX;

		public UnityEvent OnResetRequested;
		public UnityEvent OnHighlightRequested;

		public TicTacToe.Mark CurrentMark { get; private set; }
		public event Action<TicTacToeCell> OnCellClicked;

		private void Awake()
		{
			GetComponent<Button>().onClick.AddListener(() => { OnCellClicked?.Invoke(this); });
			ResetMark();
		}

		public void ResetMark()
		{
			CurrentMark = TicTacToe.Mark.None;
			markO.SetActive(false);
			markX.SetActive(false);
			OnResetRequested?.Invoke();
		}

		public void SetMark(TicTacToe.Mark mark)
		{
			markO.SetActive(mark == TicTacToe.Mark.O ? true : false);
			markX.SetActive(mark == TicTacToe.Mark.X ? true : false);
			CurrentMark = mark;
		}

		public void Highlight()
		{
			OnHighlightRequested?.Invoke();
		}
	}
}