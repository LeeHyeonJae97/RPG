using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class CustomToggle : MonoBehaviour
{
    [System.Serializable]
    public class ToggleEvent : UnityEvent<bool> { }

    public enum Transition { None, ColorTint, SpriteSwap }

    [SerializeField] private bool initOnStart;

    [Tooltip("On right when on")]
    [SerializeField] private bool isOn;

    [SerializeField] private Transition transition;

    [SerializeField] private Image bkgImage;
    [SerializeField] private Color onBkgColor = Color.white, offBkgColor = Color.white;
    [SerializeField] private Sprite onBkgSprite, offBkgSprite;

    [SerializeField] private ToggleEvent onClick = new ToggleEvent();

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Toggle);
    }

    private void Start()
    {
        if(initOnStart)
        {
            switch (transition)
            {
                case Transition.ColorTint:
                    bkgImage.color = isOn ? onBkgColor : offBkgColor;
                    break;
                case Transition.SpriteSwap:
                    bkgImage.sprite = isOn ? onBkgSprite : offBkgSprite;
                    break;
            }

            // Additional events
            onClick?.Invoke(isOn);
        }
    }

    public void Toggle()
    {
        Toggle(!isOn, true);
    }

    public void Toggle(bool isOn, bool invokeEvent = false)
    {
        this.isOn = isOn;

        switch (transition)
        {
            case Transition.ColorTint:
                bkgImage.color = isOn ? onBkgColor : offBkgColor;
                break;
            case Transition.SpriteSwap:
                bkgImage.sprite = isOn ? onBkgSprite : offBkgSprite;
                break;
        }

        // Additional events
        if (invokeEvent) onClick?.Invoke(isOn);
    }

    public void AddListener(UnityAction<bool> onClick)
    {
        this.onClick?.AddListener(onClick);
    }
}
