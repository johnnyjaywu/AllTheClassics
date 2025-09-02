using UnityEngine;
using UnityEngine.UI;

namespace ContentContent.UI
{
	[RequireComponent(typeof(Toggle))]
	public class SimpleToggleColor : MonoBehaviour
	{
		[SerializeField]
		private Graphic[] targets;

		[SerializeField]
		private Color onColor;

		[SerializeField]
		private Color offColor;

		private Toggle toggle;

		private ColorBlock startingColors;
		private void Awake()
		{
			toggle = GetComponent<Toggle>();
			toggle.onValueChanged.AddListener(SetColor);
			startingColors = toggle.colors;
		}

		public void SetColor(bool on)
		{
			toggle.colors = new ColorBlock
			{ 
				normalColor = on ? onColor : offColor,
				highlightedColor = on ? onColor : offColor,
				disabledColor = startingColors.disabledColor,
				pressedColor = startingColors.pressedColor,
				selectedColor = startingColors.selectedColor,
				colorMultiplier = startingColors.colorMultiplier,
				fadeDuration = startingColors.fadeDuration
			};
			
			if (targets != null && targets.Length > 0)
			{
				foreach (Graphic target in targets)
				{
					target.color = on ? onColor : offColor;
				}
			}
		}
	}
}