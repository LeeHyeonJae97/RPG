using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class AlertModal : MonoBehaviour
{
	public static UnityAction<string> Do;

	[SerializeField] private TextMeshProUGUI _messageText;

	private Canvas _canvas;

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

    public void ShowUI(string message)
	{
		_canvas.enabled = true;
		_messageText.text = message;
	}

	public void HideUI()
	{
		_canvas.enabled = false;
	}
}
