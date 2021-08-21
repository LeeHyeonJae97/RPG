using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tooltip : MonoBehaviour
{
    [SerializeField] private TooltipEventChannelSO _channel;
    [SerializeField] protected Transform _panel;
    [SerializeField] private Vector2 _offset;

    private void Start()
    {
        _channel.show += Show;
        _channel.hide += Hide;
        _channel.follow += Follow;
    }

    private void OnDestroy()
    {
        _channel.show -= Show;
        _channel.hide -= Hide;
        _channel.follow -= Follow;
    }

    public void Follow()
    {
        SetPos();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    protected void SetPos()
    {
#if UNITY_EDITOR
        _panel.position = (Vector2)Input.mousePosition + _offset;
#elif UNITY_ANDROID
        _panel.position = Input.GetTouch(0).position + _offset;
#endif
        gameObject.SetActive(true);
    }

    public abstract void Show(System.Object t);
}
