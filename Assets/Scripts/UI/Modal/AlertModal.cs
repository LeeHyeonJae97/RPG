using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlertModal : MonoBehaviour
{
	[SerializeField] private StringEventChannelSO _channel;
	[SerializeField] private TextMeshProUGUI _messageText;

	private void Start()
	{
		_channel.onEventRaised += Show;
	}

    private void OnDestroy()
    {
		_channel.onEventRaised -= Show;
    }

    public void Show(string message)
	{
		gameObject.SetActive(true);
		_messageText.text = message;
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}
}
