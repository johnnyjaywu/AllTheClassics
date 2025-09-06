using System;
using System.Collections.Generic;

namespace AllTheClassics.TicTacToe
{
	/// <summary>
	/// TicTacToe model that handles all game data and logic
	/// </summary>
	public class TicTacToe
	{
		#region Enums

		public enum Mark
		{
			None,
			O,
			X
		}

		public enum GameState
		{
			Ongoing,
			Draw,
			OWon,
			XWon
		}

		#endregion Enums

		#region Properties

		public int[] WinningLine { get; private set; } = new int[3];

		public Mark CurrentPlayerMark { get; private set; } = Mark.O;

		public bool IsEndless { get; set; }

		public event Action<int> OnMarkReset;

		#endregion Properties

		private Mark[] board = new Mark[9];
		private Queue<int> moves = new Queue<int>();

		public void Reset()
		{
			board = new Mark[9];
			WinningLine = new int[3];
			CurrentPlayerMark = Mark.O;
			moves = new Queue<int>();
		}

		public GameState MarkBoard(int position)
		{
			board[position] = CurrentPlayerMark;
			moves.Enqueue(position);

			// Check the game
			GameState gameState = CheckGame();

			// If game is still ongoing
			if (gameState == GameState.Ongoing)
			{
				// Remove the last move if necessary
				if (IsEndless && moves.Count > 6)
				{
					int removePosition = moves.Dequeue();
					board[removePosition] = Mark.None;
					OnMarkReset?.Invoke(removePosition);
				}

			}

			CurrentPlayerMark = CurrentPlayerMark == Mark.O ? Mark.X : Mark.O;
			return gameState;
		}

		private GameState CheckGame()
		{
			// Check horizontals
			for (int row = 0; row < 3; row++)
			{
				if (board[row * 3] != Mark.None && board[row * 3] == board[row * 3 + 1] && board[row * 3 + 1] == board[row * 3 + 2])
				{
					WinningLine[0] = row * 3;
					WinningLine[1] = row * 3 + 1;
					WinningLine[2] = row * 3 + 2;
					return board[row * 3] == Mark.O ? GameState.OWon : GameState.XWon; // Return the winner ('X' or 'O')
				}
			}

			// Check verticals
			for (int col = 0; col < 3; col++)
			{
				if (board[col] != Mark.None && board[col] == board[col + 3] && board[col + 3] == board[col + 6])
				{
					WinningLine[0] = col;
					WinningLine[1] = col + 3;
					WinningLine[2] = col + 6;
					return board[col] == Mark.O ? GameState.OWon : GameState.XWon;
				}
			}

			// Check for diagonals
			if (board[0] != Mark.None && board[0] == board[4] && board[4] == board[8])
			{
				WinningLine[0] = 0;
				WinningLine[1] = 4;
				WinningLine[2] = 8;
				return board[0] == Mark.O ? GameState.OWon : GameState.XWon;
			}
			if (board[2] != Mark.None && board[2] == board[4] && board[4] == board[6])
			{
				WinningLine[0] = 2;
				WinningLine[1] = 4;
				WinningLine[2] = 6;
				return board[2] == Mark.O ? GameState.OWon : GameState.XWon;
			}

			// No winner found, check if the board is full
			for (int i = 0; i < 9; i++)
			{
				if (board[i] == Mark.None)
				{
					return GameState.Ongoing; // Game still going
				}
			}

			// No winner found and the board is full
			return GameState.Draw;
		}
	}
}