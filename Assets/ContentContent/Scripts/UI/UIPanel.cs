using ContentContent.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace ContentContent.UI
{
	public class UIPanel : MonoBehaviour
	{
		[Tooltip("The first selected element for this menu, default to first item in the selectable list")]
		[SerializeField] protected Selectable firstSelected;

		private InputSystemUIInputModule inputModule;
		private List<Selectable> selectables = new List<Selectable>();
		private Selectable lastSelected;

		private void Awake()
		{
			// Get all selectables in children
			selectables = GetComponentsInChildren<Selectable>(true).ToList();
			foreach (var selectable in selectables)
			{
				AddSelectionListeners(selectable);
			}
			firstSelected = selectables.Count > 0 ? selectables[0] : null;
			lastSelected = firstSelected;
			SelectDefault();

			// Get input module
			inputModule = FindFirstObjectByType<InputSystemUIInputModule>();
			if (inputModule == null)
			{
				Debug.LogWarning($"Missing object with component of type {typeof(InputSystemUIInputModule)}");
			}
		}

		private void OnEnable()
		{
			inputModule.move.action.performed += OnMoveInputPerformed;
			SelectPrevious();
		}

		private void OnDisable()
		{
			inputModule.move.action.performed -= OnMoveInputPerformed;
		}

		public void SelectDefault()
		{
			if (firstSelected == null)
				return;

			SetSelectedDelayed(firstSelected);
		}

		public void SelectPrevious()
		{
			if (lastSelected == null)
				SelectDefault();

			SetSelectedDelayed(lastSelected);
		}

		public void SetSelectedDelayed(Selectable selected)
		{
			StartCoroutine(SelectAfterDelay(selected.gameObject));
		}

		protected virtual IEnumerator SelectAfterDelay(GameObject targetSelectable)
		{
			yield return null;
			if (targetSelectable != null)
				EventSystem.current.SetSelectedGameObject(targetSelectable);
		}

		protected virtual void AddSelectionListeners(Selectable selectable)
		{
			// add listener
			EventTrigger trigger = selectable.gameObject.GetAddComponent<EventTrigger>();
			
			// add SELECT event
			EventTrigger.Entry selectEntry = new EventTrigger.Entry
			{
				eventID = EventTriggerType.Select
			};
			selectEntry.callback.AddListener(OnSelect);
			trigger.triggers.Add(selectEntry);

			// add DESELECT event
			EventTrigger.Entry deselectEntry = new EventTrigger.Entry
			{
				eventID = EventTriggerType.Deselect
			};
			deselectEntry.callback.AddListener(OnDeselect);
			trigger.triggers.Add(deselectEntry);

			// add POINTER ENTER event
			EventTrigger.Entry pointerEnter = new EventTrigger.Entry
			{
				eventID = EventTriggerType.PointerEnter
			};
			pointerEnter.callback.AddListener(OnPointerEnter);
			trigger.triggers.Add(pointerEnter);

			// add POINTER EXIT event
			EventTrigger.Entry pointerExit = new EventTrigger.Entry
			{
				eventID = EventTriggerType.PointerExit
			};
			pointerExit.callback.AddListener(OnPointerExit);
			trigger.triggers.Add(pointerExit);
		}

		private void OnSelect(BaseEventData eventData)
		{
			lastSelected = eventData.selectedObject.GetComponent<Selectable>();
		}

		private void OnDeselect(BaseEventData eventData)
		{
		}

		private void OnPointerEnter(BaseEventData eventData)
		{
			PointerEventData pointerEventData = eventData as PointerEventData;
			if (pointerEventData != null)
			{
				Selectable selectable = pointerEventData.pointerEnter.GetComponentInParent<Selectable>();
				if (selectable == null)
				{
					selectable = pointerEventData.pointerEnter.GetComponentInChildren<Selectable>();
				}

				// If there was already a selected object, deselect it first
				if (EventSystem.current.currentSelectedGameObject != null)
					EventSystem.current.SetSelectedGameObject(null);

				// Select the new object
				pointerEventData.selectedObject = selectable.gameObject;
			}
		}

		private void OnPointerExit(BaseEventData eventData)
		{
			PointerEventData pointerEventData = eventData as PointerEventData;
			if (pointerEventData != null)
			{
				pointerEventData.selectedObject = null;
			}
		}

		protected virtual void OnMoveInputPerformed(InputAction.CallbackContext context)
		{
			if (EventSystem.current.currentSelectedGameObject == null && lastSelected != null)
			{
				EventSystem.current.SetSelectedGameObject(lastSelected.gameObject);
			}
		}
	}
}