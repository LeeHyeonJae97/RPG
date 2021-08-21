using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using NaughtyAttributes;

public class ClickEventListener : UIEventListener, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] private bool _click;
	[Space(5)]
	[ShowIf("_click")] public UnityEvent onClick;

	[SerializeField] private bool _down;
	[Space(5)]
	[ShowIf("_down")] public UnityEvent onDown;

	[SerializeField] private bool _up;
	[Space(5)]
	[ShowIf("_up")] public UnityEvent onUp;

	private void Awake()
	{
		// Debug
		//onClick.AddListener(() => Debug.Log("Click"));
		//onDown.AddListener(() => Debug.Log("Down"));
		//onUp.AddListener(() => Debug.Log("Up"));
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (_click)
			onClick?.Invoke();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (_down)
			onDown?.Invoke();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (_up && eventData.pointerCurrentRaycast.gameObject == eventData.pointerClick)
			onUp?.Invoke();
	}
}
