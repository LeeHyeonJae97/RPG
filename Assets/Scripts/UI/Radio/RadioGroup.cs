using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class RadioGroup : MonoBehaviour
{
    private enum ReferenceType { Children, Manually }

    [SerializeField] private IntEventChannelSO _channel;
    [SerializeField] private ReferenceType _type;
    [ShowIf("_type", ReferenceType.Manually)]
    [Indent(1)]
    [SerializeField] private RadioButton[] _buttons;
    private RadioButton _selected;

    private void Start()
    {
        if (_channel != null) _channel.onEventRaised += Select;

        if (_type == ReferenceType.Children) _buttons = GetComponentsInChildren<RadioButton>();

        for (int i = 0; i < _buttons.Length; i++)
        {
            int index = i;
            _buttons[i].onClick += () => Select(index);
            _buttons[i].OnStateChanged(false);
        }

        Select(0);
    }

    private void OnDestroy()
    {
        if (_channel != null) _channel.onEventRaised -= Select;
    }

    private void OnValidate()
    {
        if (_type != ReferenceType.Manually) _buttons = null;
    }

    public void Select(int index)
    {
        if (_buttons[index] == _selected) return;

        if (_selected != null) _selected.OnStateChanged(false);

        _selected = _buttons[index];
        _selected.OnStateChanged(true);
    }
}
