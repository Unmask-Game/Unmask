using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField usernameText;

    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private Slider sensitivitySlider;

    public void OnEnable()
    {
        usernameText.text = SettingsManager.Instance.GetUsername();
        volumeSlider.value = SettingsManager.Instance.GetVolume();
        if (sensitivitySlider)
        {
            sensitivitySlider.value = SettingsManager.Instance.GetMouseSensitivity();
        }
        UpdateVolumeText();
    }

    public void SetUsername()
    {
        // We do not allow empty usernames
        if (string.IsNullOrWhiteSpace(usernameText.text))
        {
            // Reset to previous name
            usernameText.text = SettingsManager.Instance.GetUsername();
        }
        else
        {
            SettingsManager.Instance.SetUsername(usernameText.text);
        }
    }

    public void SetVolume()
    {
        SettingsManager.Instance.SetVolume(volumeSlider.value);
        UpdateVolumeText();
    }

    private void UpdateVolumeText()
    {
        TMP_Text text = volumeSlider.gameObject.GetComponentInChildren<TMP_Text>();
        text.text = "Volume (" + Mathf.RoundToInt(volumeSlider.value * 100) + "%)";
    }

    public void SetMouseSensitivity()
    {
        SettingsManager.Instance.SetMouseSensitivity(sensitivitySlider.value);
    }
}
