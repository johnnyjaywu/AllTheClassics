using ContentContent.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ContentContent.UI
{
	/// <summary>
	/// Place this component on the root element of a menu to handle UI selection events in a more unified way across all input devices
	/// </summary>
	public class UISelectionHandler : MonoBehaviour
	{
		[Tooltip("The first selected element for this menu, default to first item in the selectable list")]
		[SerializeField] protected Selectable firstSelected;

		[Tooltip("The input action we are listening to (i.e. UI/Navigate)")]
		[SerializeField] protected InputActionReference inputActionReference;

		[SerializeField, ContextMenuItem("Find selectables in children", "FindAllSelectablesInChildren")]
		protected List<Selectable> selectables = new List<Selectable>();

		protected Selectable lastSelected;

		public virtual void Awake()
		{
			FindAllSelectablesInChildren();
		}

		public virtual void OnEnable()
		{
			inputActionReference.action.performed += InputAction_Performed;
			SelectPrevious();
		}

		public virtual void OnDisable()
		{
			inputActionReference.action.performed -= InputAction_Performed;
		}

		public void FindAllSelectablesInChildren()
		{
			selectables = GetComponentsInChildren<Selectable>(true).ToList();
			foreach (var selectable in selectables)
			{
				AddSelectionListeners(selectable);
			}
			firstSelected = selectables.Count > 0 ? selectables[0] : null;
			lastSelected = firstSelected;
			SelectDefault();
		}

		public void SelectDefault()
		{
			if (firstSelected == null)
				return;

			SetSelectedDelayed(firstSelected.gameObject);
		}

		public void SelectPrevious()
		{
			if (lastSelected == null)
				SelectDefault();

			SetSelectedDelayed(lastSelected.gameObject);
		}

		public void SetSelectedDelayed(GameObject target)
		{
			StartCoroutine(SelectAfterDelay(target));
		}

		protected virtual IEnumerator SelectAfterDelay(GameObject target)
		{
			yield return null;
			if (target != null)
				EventSystem.current.SetSelectedGameObject(target);
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

		protected virtual void InputAction_Performed(InputAction.CallbackContext context)
		{
			if (EventSystem.current.currentSelectedGameObject == null && lastSelected != null)
			{
				EventSystem.current.SetSelectedGameObject(lastSelected.gameObject);
			}
		}
	}
}