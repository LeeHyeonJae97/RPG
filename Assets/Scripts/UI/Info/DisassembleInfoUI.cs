using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisassembleInfoUI : MonoBehaviour
{
    [SerializeField] private UIEventChannelSO _channel;
    [SerializeField] private Image _previewImage;

    private void Start()
    {
        _channel.updateDisassembleInfoUI += UpdateUI;
    }

    private void OnDestroy()
    {
        _channel.updateDisassembleInfoUI -= UpdateUI;
    }

    public void UpdateUI(ItemSO info)
    {
        if(info != null)
        {
            _previewImage.sprite = info.Preview;
            _previewImage.gameObject.SetActive(true);
            
            // 해체 결과물 정보
        }
        else
        {
            _previewImage.gameObject.SetActive(false);
        }        
    }
}
