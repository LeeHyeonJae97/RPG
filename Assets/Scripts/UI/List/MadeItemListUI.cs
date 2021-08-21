using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MadeItemListUI : MonoBehaviour
{
    [SerializeField] private UIEventChannelSO _channel;
    [SerializeField] private MadeItemSlot[] _slots;

    private void Start()
    {
        _channel.openMadeItemListUI += Open;
        _channel.closeMadeItemListUI += Close;
    }

    private void OnDestroy()
    {
        _channel.openMadeItemListUI -= Open;
        _channel.closeMadeItemListUI -= Close;
    }

    public void Open(ItemSO[] infos)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (i < infos.Length)
            {
                int index = i;
                _slots[i].Init(infos[index]);
            }
            else
            {
                _slots[i].gameObject.SetActive(false);
            }
        }

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
