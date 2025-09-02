using UnityEngine;

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

		public Vector2[] WinningLine { get; private set; } = new Vector2[3];

		public Mark CurrentPlayerMark { get; private set; } = Mark.O;

		#endregion Properties

		private Mark[,] board = new Mark[3, 3];

		public void Reset()
		{
			WinningLine = new Vector2[3];
			CurrentPlayerMark = Mark.O;
			board = new Mark[3, 3];
		}

		public void MarkBoard(int row, int col)
		{
			board[row, col] = CurrentPlayerMark;
			CurrentPlayerMark = CurrentPlayerMark == Mark.O ? Mark.X : Mark.O;
		}

		public GameState CheckGame()
		{
			// Check horizontals
			for (int row = 0; row < 3; row++)
			{
				if (board[row, 0] != Mark.None && board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2])
				{
					WinningLine[0] = new Vector2(row, 0);
					WinningLine[1] = new Vector2(row, 1);
					WinningLine[2] = new Vector2(row, 2);
					return board[row, 0] == Mark.O ? GameState.OWon : GameState.XWon; // Return the winner ('X' or 'O')
				}
			}

			// Check verticals
			for (int col = 0; col < 3; col++)
			{
				if (board[0, col] != Mark.None && board[0, col] == board[1, col] && board[1, col] == board[2, col])
				{
					WinningLine[0] = new Vector2(0, col);
					WinningLine[1] = new Vector2(1, col);
					WinningLine[2] = new Vector2(2, col);
					return board[col, 0] == Mark.O ? GameState.OWon : GameState.XWon;
				}
			}

			// Check for diagonals
			if (board[0, 0] != Mark.None && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
			{
				WinningLine[0] = new Vector2(0, 0);
				WinningLine[1] = new Vector2(1, 1);
				WinningLine[2] = new Vector2(2, 2);
				return board[0, 0] == Mark.O ? GameState.OWon : GameState.XWon;
			}
			if (board[0, 2] != Mark.None && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
			{
				WinningLine[0] = new Vector2(0, 2);
				WinningLine[1] = new Vector2(1, 1);
				WinningLine[2] = new Vector2(2, 0);
				return board[0, 2] == Mark.O ? GameState.OWon : GameState.XWon;
			}

			// No winner found, check if the board is full
			for (int row = 0; row < 3; row++)
			{
				for (int col = 0; col < 3; col++)
				{
					if (board[row, col] == Mark.None)
					{
						return GameState.Ongoing; // Game still going
					}
				}
			}

			// No winner found and the board is full
			return GameState.Draw;
		}
	}
}