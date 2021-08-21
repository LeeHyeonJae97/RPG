using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Animator _waitingIconAnim;

    [SerializeField] private WaitingEventChannelSO _channel;

    private void Start()
    {
        _channel.begin += Begin;
        _channel.end += End;
    }

    private void OnDestroy()
    {
        _channel.begin -= Begin;
        _channel.end -= End;
    }

    private void Begin(string description)
    {
        _descriptionText.text = description;
        //_waitingIconAnim.Play("Waiting");
        gameObject.SetActive(true);
    }

    private void End()
    {
        //_waitingIconAnim.Play("Idle");
        gameObject.SetActive(false);
    }
}
