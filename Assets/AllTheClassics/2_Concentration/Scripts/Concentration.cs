using ContentContent.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace AllTheClassics.Concentration
{
	[RequireComponent(typeof(ConcentrationBoard))]
	public class Concentration : MonoBehaviour
	{
		public enum GameMode
		{
			Easy,
			Normal,
			Hard
		}

		const int kEasyModeCount = 6;
		const int kNormalModeCount = 10;
		const int kHardModeCount = 15;

		[SerializeField]
		private ConcentrationCard cardPrefab;

		[SerializeField, Tooltip("Include at least 15!")]
		private List<Sprite> sprites = new List<Sprite>();

		[SerializeField]
		private TextMeshProUGUI statusText;

		private List<ConcentrationCard> cards = new List<ConcentrationCard>();

		private int moveCounter;

		public GameMode Mode { get; set; }
		public ConcentrationCard FirstSeleceted { get; private set; }
		public ConcentrationCard SecondSeleceted { get; private set; }

		public UnityEvent Started;
		public UnityEvent Matched;
		public UnityEvent Missed;
		public UnityEvent GameOver;

		private ConcentrationBoard board;

		private void Awake()
		{
			board = GetComponent<ConcentrationBoard>();
		}

		//private IEnumerator Start()
		//{
		//	yield return new WaitForEndOfFrame();
		//	Restart();
		//	Initialized?.Invoke();
		//}

		public void StartGame()
		{
			board.Clean();
			FirstSeleceted = null;
			SecondSeleceted = null;
			moveCounter = 0;
			statusText.SetText($"Match 2 of the same card");

			switch (Mode)
			{
				case GameMode.Easy:
					board.InitializeBoard(kEasyModeCount * 2);
					InitializeCards(sprites.GetRange(0, kEasyModeCount));
					break;

				case GameMode.Normal:
					board.InitializeBoard(kNormalModeCount * 2);
					InitializeCards(sprites.GetRange(0, kNormalModeCount));
					break;

				case GameMode.Hard:
					board.InitializeBoard(kHardModeCount * 2);
					InitializeCards(sprites.GetRange(0, kHardModeCount));
					break;

				default:
					break;
			}
			Started?.Invoke();
		}

		private void InitializeCards(List<Sprite> sprites)
		{
			cards = new List<ConcentrationCard>();
			for (int i = 0; i < sprites.Count; i++)
			{
				// Instantiate a 2 new cards
				ConcentrationCard card1 = Instantiate(cardPrefab, transform);
				card1.gameObject.name = $"Card_{i}"; // name it
				card1.Flip(true); // flip it
				card1.SetCard(i, sprites[i]); // set the image
				card1.Clicked += Select; // register click

				ConcentrationCard card2 = Instantiate(cardPrefab, transform);
				card2.gameObject.name = $"Card_{i}";
				card2.Flip(true);
				card2.SetCard(i, sprites[i]);
				card2.Clicked += Select;

				cards.Add(card1);
				cards.Add(card2);
			}

			// Shuffle it
			if (cards.Count > 0)
			{
				cards.Shuffle();
			}

			// Position it in heiarchy
			for (int i = 0; i < cards.Count; i++)
			{
				cards[i].transform.SetSiblingIndex(i);
			}
		}

		public void Select(ConcentrationCard card)
		{
			// Card was already matched (removed from the array)
			//if (cards[index] == null)
			//{
			//	Debug.LogWarning($"Trying to select a card that was removed, select again");
			//	return;
			//}

			if (FirstSeleceted == null)
			{
				FirstSeleceted = card;// cards[index];
				card.Flip();
				Debug.Log($"Selected {FirstSeleceted.ID}");
			}
			else if (SecondSeleceted == null)
			{
				moveCounter++;
				SecondSeleceted = card;// cards[index];
				card.Flip(false, (flipped) =>
				{
					StartCoroutine(Wait(1f, () => Check()));
				});
				Debug.Log($"Selected {SecondSeleceted.ID}");
			}
		}

		private IEnumerator Wait(float seconds, Action then)
		{
			yield return new WaitForSeconds(seconds);
			then?.Invoke();
		}

		public void Check()
		{
			// Cannot check if no cards were selected
			if (FirstSeleceted == null || SecondSeleceted == null)
				return;

			// Matched
			if (FirstSeleceted.ID == SecondSeleceted.ID)
			{
				// Null the cards from the array
				cards[cards.IndexOf(FirstSeleceted)] = null;
				cards[cards.IndexOf(SecondSeleceted)] = null;

				// Destroy the cards
				FirstSeleceted.Hide();
				SecondSeleceted.Hide();

				// Raise event
				Matched?.Invoke();

				// Check for game over
				if (cards.All((card) => { return card == null; }))
				{
					statusText.SetText($"You won in {moveCounter} moves!");
					GameOver?.Invoke();
				}
			}
			else
			{
				FirstSeleceted.Flip();
				SecondSeleceted.Flip();
				Missed?.Invoke();
			}

			// Deselect
			FirstSeleceted = null;
			SecondSeleceted = null;
		}
	}
}