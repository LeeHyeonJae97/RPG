using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CutoutUI : MonoBehaviour
{
    [SerializeField] private CutoutEventChannelSO _channel;
    [SerializeField] private RectTransform _cutoutTr;

    private void Start()
    {
        _channel.show += Show;
        _channel.hide += Hide;
    }

    private void OnDestroy()
    {
        _channel.show -= Show;
        _channel.hide -= Hide;
    }

    public void Show(RectTransform tr)
    {
        _cutoutTr.pivot = tr.pivot;
        _cutoutTr.position = tr.position;
        _cutoutTr.sizeDelta = new Vector2(tr.rect.width, tr.rect.height);

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        _channel.onHide?.Invoke();
        _channel.onHide = null;
        gameObject.SetActive(false);
    }
}
