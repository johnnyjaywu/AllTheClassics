using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static AllTheClassics.TicTacToe.TicTacToe;

namespace AllTheClassics.TicTacToe
{
	[RequireComponent(typeof(CanvasGroup))]
	public class TicTacToeBoard : MonoBehaviour
	{
		[SerializeField]
		private List<TicTacToeCell> cells = new List<TicTacToeCell>();

		[SerializeField]
		private TextMeshProUGUI statusText;

		[SerializeField]
		private UnityEvent OnGameOver;

		private CanvasGroup canvasGroup;

		private TicTacToe game;

		private void Awake()
		{
			canvasGroup = GetComponent<CanvasGroup>();
			game = new TicTacToe();
			Restart();

			// Initialize Cells
			foreach (var cell in cells)
			{
				cell.OnCellClicked += CellButtonClickEvent;
			}
		}

		private void OnDestroy()
		{
			foreach (var cell in cells)
			{
				cell.OnCellClicked -= CellButtonClickEvent;
			}
		}

		private void CellButtonClickEvent(TicTacToeCell cell)
		{
			if (cell.CurrentMark != Mark.None) return;

			// Mark the cell
			cell.SetMark(game.CurrentPlayerMark);

			// get index of the cell clicked to mark the board
			// *ASSUMES the cell array is ordered top left to bottom right
			int index = cells.IndexOf(cell);
			game.MarkBoard(index / 3, index % 3);

			// Check game
			GameState gameState = game.CheckGame();

			// Game still going
			if (gameState == GameState.Ongoing)
			{
				// Set status text
				statusText.text = $"{game.CurrentPlayerMark}'s turn!";
				return;
			}

			// Game over
			HandleGameOver(gameState);
		}

		private void HandleGameOver(GameState gameState)
		{
			// Make the board not interactable
			canvasGroup.interactable = false;

			if (gameState == GameState.Draw)
			{
				statusText.text = $"Draw!";
			}
			else
			{
				statusText.text = gameState == GameState.OWon ? "O Won!": "X Won!";
				
				// Highlight winning positions
				int cell0 = (int)(game.WinningLine[0].x * 3 + game.WinningLine[0].y);
				int cell1 = (int)(game.WinningLine[1].x * 3 + game.WinningLine[1].y);
				int cell2 = (int)(game.WinningLine[2].x * 3 + game.WinningLine[2].y);
				cells[cell0].Highlight();
				cells[cell1].Highlight();
				cells[cell2].Highlight();
			}

			OnGameOver?.Invoke();
		}

		public void Restart()
		{
			// Reset game
			game.Reset();

			// Set status text
			statusText.text = $"{game.CurrentPlayerMark}'s turn!";

			// Reset cells
			foreach (var cell in cells)
			{
				cell.ResetMark();
			}

			// Set board to interactable
			canvasGroup.interactable = true;
		}
	}
}