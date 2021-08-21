using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldListUI : MonoBehaviour
{
    [SerializeField] private UIEventChannelSO _channel;

    [SerializeField] private WorldSlot _slotPrefab;
    [SerializeField] private Transform _slotHolder;

    private void Start()
    {
        _channel.openWorldListUI += Open;   
        _channel.closeWorldListUI += Close;
    }

    private void OnDestroy()
    {
        _channel.openWorldListUI -= Open;
        _channel.closeWorldListUI -= Close;
    }

    private void Open(WorldSO[] infos = null, UnityAction<string> selectWorld = null)
    {
        if (infos != null && selectWorld != null)
        {
            for (int i = 0; i < infos.Length; i++)
            {
                // NOTE :
                // WorldSO���� �޸𸮿��� ������ Ŭ������ �� StageAddressablesLabel���� ����� �о���� ������ ������

                int index = i;
                Instantiate(_slotPrefab, _slotHolder).Init(infos[index], () => selectWorld(infos[index].Name));
            }
        }

        gameObject.SetActive(true);
    }

    private void Close()
    {
        gameObject.SetActive(false);            
    }
}
