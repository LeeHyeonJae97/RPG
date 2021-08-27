using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Waiting : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Animator _waitingIconAnim;

    [SerializeField] private WaitingEventChannelSO _channel;

    public static UnityAction<string> Begin;
    public static UnityAction End;

    private void Start()
    {
        Begin += ShowUI;
        End += HideUI;
    }

    private void OnDestroy()
    {
        Begin -= ShowUI;
        End -= HideUI;
    }

    private void ShowUI(string description)
    {
        _descriptionText.text = description;
        //_waitingIconAnim.Play("Waiting");
        gameObject.SetActive(true);
    }

    private void HideUI()
    {
        //_waitingIconAnim.Play("Idle");
        gameObject.SetActive(false);
    }
}
