using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Extension
{
	public class ButtonExtension : Button
	{
		public UnityEvent onDown;
		public UnityEvent onUp;

		// Long press
		[SerializeField] private bool _longPress;
		private bool _pressed;
		private bool _longPressed;
		[SerializeField] private float _pressedTimeForLongPress;
		private float _pressedTime;
		public UnityEvent onEnterLongPress;
		public UnityEvent onLongPress;
		public UnityEvent onExitLongPress;

		// Double click
		[SerializeField] private bool _doubleClick;
		[SerializeField] private float _clickIntervalForDoubleClick;
		private float _clickInterval;
		public UnityEvent onDoubleClick;

		protected override void Awake()
		{
            //onDown.AddListener(() => Debug.Log("Down"));
            //onUp.AddListener(() => Debug.Log("Up"));
            //onClick.AddListener(() => Debug.Log("Click"));
            //if (_longPress)
            //{
            //    onEnterLongPress.AddListener(() => Debug.Log("Enter long press"));
            //    onLongPress.AddListener(() => Debug.Log("Long press"));
            //    onExitLongPress.AddListener(() => Debug.Log("Exit long press"));
            //}
            //if (_doubleClick)
            //    onDoubleClick.AddListener(() => Debug.Log("Double click"));

            if (_doubleClick)
				_clickInterval = _clickIntervalForDoubleClick;
		}

		private void Update()
		{
			if (_longPress && _pressed)
			{
				if (_pressedTime >= _pressedTimeForLongPress)
				{
					if (!_longPressed)
					{
						onEnterLongPress?.Invoke();
						_longPressed = true;
					}

					onLongPress?.Invoke();
				}
				else
				{
					_pressedTime += Time.deltaTime;
				}
			}

			if (_doubleClick && _clickInterval >= 0)
				_clickInterval -= Time.deltaTime;
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			onDown?.Invoke();

			if (_longPress)
			{
				_pressed = true;
				_pressedTime = 0;
			}
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			base.OnPointerUp(eventData);

			if (eventData.hovered.Contains(eventData.pointerClick))
			{
				if (_longPress && _longPressed)
					onExitLongPress?.Invoke();
				else
					onUp?.Invoke();
			}

			if (_longPress)
			{
				_pressed = false;
				_longPressed = false;
			}
		}

		public override void OnPointerClick(PointerEventData eventData)
		{
			if (_doubleClick)
			{
				if (_clickInterval < 0)
				{
					base.OnPointerClick(eventData);
					_clickInterval = _clickIntervalForDoubleClick;
				}
				else
				{
					onDoubleClick?.Invoke();
				}
			}
			else
			{
				base.OnPointerClick(eventData);
			}
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);

			if (_longPress && _pressed)
				_pressed = false;
		}
	}
}
