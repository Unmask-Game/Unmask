using DefaultNamespace;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{
    private Animator _shieldAnimator;
    private Image _healthbar;
    private Text _percentage;
    private VRPlayerController _vrController;

    private void Start()
    {
        _healthbar = GetChildComponentByName<Image>("Bar");
        _shieldAnimator = GetChildComponentByName<Image>("Shield").GetComponent<Animator>();
        _percentage = GetChildComponentByName<Text>("Percentage");
    }

    private void Update()
    {
        if (!_vrController)
        {
            // Try to get the vr player to update fill amount with percentages
            GetVrPlayer();
            return;
        }
        // Update fill amount and decrease animation speed linear to fill amount
        _healthbar.fillAmount = _vrController.GetResistancePointsPercentile();
        _shieldAnimator.speed = 2 - _healthbar.fillAmount;
        _percentage.text = (int)Math.Round(_healthbar.fillAmount * 100) + "%";
    }

    private T GetChildComponentByName<T>(string componentName) where T : Component
    {
        foreach (var component in GetComponentsInChildren<T>(true))
        {
            if (component.gameObject.name == componentName)
                return component;
        }

        return null;
    }
    
    private VRPlayerController GetVrPlayer()
    {
        if (_vrController == null)
        {
            GameObject vrPlayer = GameObject.FindGameObjectWithTag(Tags.VrPlayerTag);
            if (vrPlayer != null)
            {
                _vrController = vrPlayer.GetComponent<VRPlayerController>();
            }
        }
        return _vrController;
    }
}