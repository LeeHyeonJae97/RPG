using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _progressText;
    [SerializeField] private Image _loadingBarFillImage;
    [SerializeField] private LoadingEventChannelSO _channel;

    private void Start()
    {
        _channel.updateDescriptionUI += UpdateDescriptionUI;
        _channel.updateProgressUI += UpdateProgressUI;
    }

    private void OnDestroy()
    {
        _channel.updateDescriptionUI -= UpdateDescriptionUI;
        _channel.updateProgressUI -= UpdateProgressUI;
    }

    private void UpdateDescriptionUI(string description)
    {
        _descriptionText.text = description;
    }

    private void UpdateProgressUI(int progress)
    {
        _progressText.text = $"{progress}%";
        _loadingBarFillImage.fillAmount = progress / 100;
    }
}
