using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cutout : MonoBehaviour
{
    public static UnityAction<RectTransform> Do;
    public static UnityAction Cancel;
    public static UnityAction OnCanceled;

    [SerializeField] private RectTransform _cutoutTr;

    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        Do += ShowUI;
        Cancel += HideUI;
    }

    private void OnDestroy()
    {
        Do -= ShowUI;
        Cancel -= HideUI;
        OnCanceled = null;
    }

    private void ShowUI(RectTransform tr)
    {
        Canvas targetCanvas = tr.GetComponentInParent<Canvas>();
        OnCanceled += () => targetCanvas.enabled = false;

        _cutoutTr.pivot = tr.pivot;
        _cutoutTr.position = tr.position;
        _cutoutTr.sizeDelta = new Vector2(tr.rect.width, tr.rect.height);

        targetCanvas.enabled = true;
        _canvas.enabled = true;
    }

    public void HideUI()
    {
        OnCanceled?.Invoke();
        OnCanceled = null;

        _canvas.enabled = false;
    }
}
