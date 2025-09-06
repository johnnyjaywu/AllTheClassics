using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace AllTheClassics.Concentration
{
	[RequireComponent(typeof(Button), typeof(CanvasGroup))]
	public class ConcentrationCard : MonoBehaviour
	{
		[SerializeField]
		private RectTransform visual;

		[SerializeField]
		private Image frontImage;

		[SerializeField]
		float flipTime = 0.25f;

		public int ID { get; private set; }

		public event Action<ConcentrationCard> Clicked;
		public event Action<ConcentrationCard, bool> Flipped;

		private Button button;
		private CanvasGroup canvasGroup;
		private bool isFlipped; // default = 0 rotation

		private void Awake()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(() => Clicked?.Invoke(this));
			canvasGroup = GetComponent<CanvasGroup>();
		}

		public void SetCard(int id, Sprite sprite)
		{
			ID = id;
			frontImage.sprite = sprite;
		}

		public void Flip(bool immediate = false, Action<bool> onFlipped = null)
		{
			isFlipped = !isFlipped;

			var tween = visual.transform.DORotate(new(0, isFlipped ? 180 : 0, 0), immediate ? 0 : flipTime);
			if (immediate)
			{
				onFlipped?.Invoke(isFlipped);
			}
			else
			{
				tween.onComplete = () => onFlipped?.Invoke(isFlipped);
			}
		}

		public void Hide()
		{
			canvasGroup.alpha = 0;
			canvasGroup.interactable = false;
		}
	}
}