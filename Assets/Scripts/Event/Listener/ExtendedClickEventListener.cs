using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using NaughtyAttributes;

public class ExtendedClickEventListener : UIEventListener, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] private bool _click;
	[Space(5)]
	[ShowIf("_click")] public UnityEvent<PointerEventData> onClick;

	[SerializeField] private bool _down;
	[Space(5)]
	[ShowIf("_down")] public UnityEvent<PointerEventData> onDown;

	[SerializeField] private bool _up;
	[Space(5)]
	[ShowIf("_up")] public UnityEvent<PointerEventData> onUp;

	[SerializeField] private bool _outsideClick;
	[Space(5)]
	[ShowIf("_outsideClick")] public UnityEvent onClickOutside;

	private void Awake()
	{
		// Debug		
		//onClick.AddListener((eventData) => Debug.Log("Click"));
		//onDown.AddListener((eventData) => Debug.Log("Down"));
		//onUp.AddListener((eventData) => Debug.Log("Up"));
		//onClickOutside.AddListener(() => Debug.Log("Click outside"));
	}

	private void Update()
	{
		if (_outsideClick)
		{
			if (Application.platform == RuntimePlatform.WindowsEditor && Input.GetMouseButtonDown(0))
			{
				if (!RectTransformUtility.RectangleContainsScreenPoint((RectTransform)transform, Input.mousePosition))
					onClickOutside?.Invoke();
			}
			else if (Application.platform == RuntimePlatform.Android && Input.touchCount > 0)
			{
				if (!RectTransformUtility.RectangleContainsScreenPoint((RectTransform)transform, Input.GetTouch(0).position))
					onClickOutside?.Invoke();
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (_click)
			onClick?.Invoke(eventData);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (_down)
			onDown?.Invoke(eventData);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (_up && eventData.pointerCurrentRaycast.gameObject == eventData.pointerClick)
			onUp?.Invoke(eventData);
	}
}
