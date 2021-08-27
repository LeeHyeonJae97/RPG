using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ConfirmModal : MonoBehaviour
{
	public static UnityAction<string, UnityAction> Do;

	[SerializeField] private TextMeshProUGUI _messageText;

	private Canvas _canvas;
	private UnityAction _onYes;

	private void Awake()
	{
		_canvas = GetComponentInParent<Canvas>();
	}

    private void Start()
    {
		Do += ShowUI;
    }

    private void OnDestroy()
	{
		Do -= ShowUI;
	}

    public void ShowUI(string message, UnityAction onYes)
	{
		_messageText.text = message;
		_onYes = onYes;
		_canvas.enabled = true;
	}

	public void Yes()
	{
		_onYes?.Invoke();
		_canvas.enabled = false;
	}

	public void No()
	{
		_canvas.enabled = false;
	}
}
