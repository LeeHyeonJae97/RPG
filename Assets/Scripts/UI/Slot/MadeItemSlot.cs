using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MadeItemSlot : MonoBehaviour
{
    [SerializeField] private Image _previewImage;

    public void Init(ItemSO info)
    {
        _previewImage.sprite = info.Preview;
        // star Ç¥½Ã

        gameObject.SetActive(true);
    }
}
