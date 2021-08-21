using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using NaughtyAttributes;

public class HoverEventListener : UIEventListener, IPointerEnterHandler, IPointerExitHandler
{
	// Enter
	[SerializeField] private bool _enter;
	[ShowIf("_enter")]
	private bool _entered;
	[ShowIf("_enter")]
	[Space(5)]
	public UnityEvent onEnter;

	// Exit
	[SerializeField] private bool _exit;
	[ShowIf("_exit")]
	[Space(5)]
	public UnityEvent onExit;

	// Hovering
	[SerializeField] private bool _hovering;
	private bool _hovered;
	[ShowIf("_hovering")]
	[SerializeField] private float _hoveringInvokeTime;
	private float _hoveringTime;
	[ShowIf("_hovering")]
	[Space(5)]
	public UnityEvent onEnterHovering;
	[ShowIf("_hovering")]
	public UnityEvent onHovering;

	private void Awake()
	{
		// Debug
		//if (_enter) onEnter.AddListener(() => Debug.Log("Enter"));
		//if (_exit) onExit.AddListener(() => Debug.Log("Exit"));
		//if (_hovering)
		//{
		//	onEnterHovering.AddListener(() => Debug.Log("Enter Hovering"));
		//	onHovering.AddListener(() => Debug.Log("Hovering"));
		//}
	}

	private void Update()
	{
		if (_entered)
		{
			if (_hovering)
			{
				if (_hoveringTime >= _hoveringInvokeTime)
				{
					if (!_hovered)
					{
						onEnterHovering?.Invoke();
						_hovered = true;
					}

					onHovering?.Invoke();
				}
				else
				{
					_hoveringTime += Time.deltaTime;
				}
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (_enter)
		{
			onEnter?.Invoke();

			_entered = true;
			_hoveringTime = 0f;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (_exit)
		{
			onExit?.Invoke();

			_entered = false;
			_hovered = false;
		}
	}
}
