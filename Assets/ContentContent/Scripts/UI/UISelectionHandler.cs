using ContentContent.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
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
		[Header("References")]

		[ContextMenuItem("Find Selectables in children", "FindAllSelectablesInChildren")]
		[Tooltip("Reference all <Selectable> elements that you want to be able to navigate to")]
		[SerializeField] protected List<Selectable> m_selectables = new List<Selectable>();

		[Tooltip("The first selected element")]
		[SerializeField] protected Selectable m_firstSelected;

		[Tooltip("The input action we are listening to (i.e. UI/Navigate)")]
		[SerializeField] protected InputActionReference m_inputActionReference;

		protected Selectable _lastSelected;

		public virtual void Awake()
		{
			_lastSelected = m_firstSelected;
			foreach (var selectable in m_selectables)
			{
				AddSelectionListeners(selectable);
			}
		}

		public virtual void OnEnable()
		{
			m_inputActionReference.action.performed += InputAction_Performed;
			StartCoroutine(SelectAfterDelay());
		}

		protected virtual IEnumerator SelectAfterDelay()
		{
			yield return null;
			EventSystem.current.SetSelectedGameObject(m_firstSelected.gameObject);
		}

		public virtual void OnDisable()
		{
			m_inputActionReference.action.performed -= InputAction_Performed;
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
			_lastSelected = eventData.selectedObject.GetComponent<Selectable>();
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
			if (EventSystem.current.currentSelectedGameObject == null && _lastSelected != null)
			{
				EventSystem.current.SetSelectedGameObject(_lastSelected.gameObject);
			}
		}

		private void FindAllSelectablesInChildren()
		{
			m_selectables = GetComponentsInChildren<Selectable>().ToList();
		}
	}
}