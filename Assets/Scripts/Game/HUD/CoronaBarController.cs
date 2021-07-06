using System;
using UnityEngine;
using UnityEngine.UI;

public class CoronaBarController : MonoBehaviour
{
    private Animator _virusAnimator;
    private Image _coronaBar;
    private Text _percentage;
    
    private void Awake()
    {
        _coronaBar = GetChildComponentByName<Image>("Bar");
        _virusAnimator = GetChildComponentByName<Image>("Virus").GetComponent<Animator>();
        _percentage = GetChildComponentByName<Text>("Percentage");
    }

    private void Update()
    {
        // Update fill amount and increase animation speed linear to fill amount 
        _coronaBar.fillAmount = GameStateManager.Instance.GetCollectedMasksPercentile();
        _virusAnimator.speed = _coronaBar.fillAmount + 1;
        // Show percentage of fill amount
        _percentage.text = (int)Math.Round(_coronaBar.fillAmount * 100) + "%";
    }

    private T GetChildComponentByName<T>(string componentName) where T : Component
    {
        // Go through each child game object and check if names are matching
        foreach (var component in GetComponentsInChildren<T>(true))
        {
            if (component.gameObject.name == componentName)
                return component;
        }

        return null;
    }
}