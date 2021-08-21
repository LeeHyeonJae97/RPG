using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ConfirmModal : MonoBehaviour
{
	[SerializeField] private ConfirmModalEventChannelSO _channel;
	[SerializeField] private TextMeshProUGUI _messageText;

	private UnityAction _onYes;

    private void Start()
    {
		_channel.show += Show;
    }

    private void OnDestroy()
	{
		_channel.show -= Show;
	}

    public void Show(string message, UnityAction onYes)
	{
		gameObject.SetActive(true);
		_messageText.text = message;
		_onYes = onYes;
	}

	public void Yes()
	{
		_onYes?.Invoke();
		gameObject.SetActive(false);
	}

	public void No()
	{
		gameObject.SetActive(false);
	}
}
