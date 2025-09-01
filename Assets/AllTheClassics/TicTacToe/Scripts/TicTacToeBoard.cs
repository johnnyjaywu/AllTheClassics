using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static AllTheClassics.TicTacToe.TicTacToe;

namespace AllTheClassics.TicTacToe
{
	public class TicTacToe
	{
		public enum Mark
		{
			None,
			O,
			X
		}
	}

	public class TicTacToeBoard : MonoBehaviour
	{
		//[SerializeField]
		//private TicTacToeCell cellPrefab;

		[SerializeField]
		private List<TicTacToeCell> cells = new List<TicTacToeCell>();

		[SerializeField]
		private TextMeshProUGUI statusText;

		[SerializeField]
		private UnityEvent<Mark> OnGameOver;

		public Mark CurrentPlayerMark { get; private set; }

		private Mark[,] currentBoard = new Mark[3, 3];

		private void Awake()
		{
			Restart();

			foreach (var cell in cells)
			{
				cell.ButtonClickEvent += CellButtonClickEvent;
			}
		}

		private void OnDestroy()
		{
			foreach (var cell in cells)
			{
				cell.ButtonClickEvent -= CellButtonClickEvent;
			}
		}

		private void CellButtonClickEvent(TicTacToeCell cell)
		{
			if (cell.CurrentMark != Mark.None) return;

			// get index of the currentBoard in order to mark the currentBoard
			int index = cells.IndexOf(cell);
			currentBoard[index / 3, index % 3] = CurrentPlayerMark;

			// Mark the cell
			cell.SetMark(CurrentPlayerMark);
			CurrentPlayerMark = CurrentPlayerMark == Mark.O ? Mark.X : Mark.O;
			statusText.text = $"{CurrentPlayerMark}'s turn!";

			Mark winner = CheckGame();

			// Check for draw
			if (winner == Mark.None)
			{
				for (int row = 0; row < 3; row++)
				{
					for (int col = 0; col < 3; col++)
					{
						if (currentBoard[row, col] == Mark.None)
						{
							return; // Game still going
						}
					}
				}
				// Draw
				GameOver(Mark.None, true);
			}
			else
			{
				GameOver(winner);
			}
		}

		private Mark CheckGame()
		{
			// Check horizontals
			for (int row = 0; row < 3; row++)
			{
				if (currentBoard[row, 0] != Mark.None && currentBoard[row, 0] == currentBoard[row, 1] && currentBoard[row, 1] == currentBoard[row, 2])
				{
					return currentBoard[row, 0]; // Return the winner ('X' or 'O')
				}
			}

			// Check verticals
			for (int col = 0; col < 3; col++)
			{
				if (currentBoard[0, col] != Mark.None && currentBoard[0, col] == currentBoard[1, col] && currentBoard[1, col] == currentBoard[2, col])
				{
					return currentBoard[0, col]; // Return the winner
				}
			}

			// Check for diagonals
			if (currentBoard[0, 0] != Mark.None && currentBoard[0, 0] == currentBoard[1, 1] && currentBoard[1, 1] == currentBoard[2, 2])
			{
				return currentBoard[0, 0]; // Return the winner
			}
			if (currentBoard[0, 2] != Mark.None && currentBoard[0, 2] == currentBoard[1, 1] && currentBoard[1, 1] == currentBoard[2, 0])
			{
				return currentBoard[0, 2]; // Return the winner
			}

			return Mark.None;
		}

		private void GameOver(Mark winner, bool isDraw = false)
		{
			foreach (var cell in cells)
			{
				cell.GetComponent<Button>().enabled = false;
				foreach (var graphic in cell.GetComponentsInChildren<Graphic>())
				{
					graphic.raycastTarget = false;
				}
			}

			if (!isDraw)
			{
				statusText.text = $"{winner} won!";
			}
			else
			{
				statusText.text = $"Draw!";
			}

			OnGameOver?.Invoke(winner);
		}

		public void Restart()
		{
			// Initialize board
			currentBoard = new Mark[3, 3];

			// Initialize first player mark
			CurrentPlayerMark = Mark.O;
			statusText.text = $"{CurrentPlayerMark}'s turn!";

			// Reset cells
			foreach (var cell in cells)
			{
				cell.GetComponent<Button>().enabled = true;
				cell.GetComponent<Graphic>().raycastTarget = true;
				foreach (var graphic in cell.GetComponentsInChildren<Graphic>())
				{
					graphic.raycastTarget = true;
				}
				cell.ResetMark();
			}

			// Create cells
			//for (int i = 0; i < 9; i++)
			//{
			//	var cell = Instantiate(cellPrefab);
			//	cell.name = $"Cell_{i}";
			//	cell.transform.SetParent(transform, false);
			//	cells.Add(cell);
			//	cell.ButtonClickEvent += CellButtonClickEvent;
			//}
		}
	}
}