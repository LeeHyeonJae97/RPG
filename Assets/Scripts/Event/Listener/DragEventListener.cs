using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragEventListener : UIEventListener, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[Space(10)]
	public UnityEvent onBeginDrag;
	public UnityEvent<PointerEventData> onDrag;
	public UnityEvent onEndDrag;

	private void Awake()
	{
		// Debug	
		//onBeginDrag.AddListener(() => Debug.Log("Begin Drag"));
		//onDrag.AddListener((dir) => Debug.Log("Drag"));
		//onEndDrag.AddListener(() => Debug.Log("End drag"));
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		onBeginDrag?.Invoke();
	}

	public void OnDrag(PointerEventData eventData)
	{
		onDrag?.Invoke(eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		onEndDrag?.Invoke();
	}
}
