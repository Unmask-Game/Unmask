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

    // Start is called before the first frame update
    private void Start()
    {
        _healthbar = GetChildComponentByName<Image>("Bar");
        _shieldAnimator = GetChildComponentByName<Image>("Shield").GetComponent<Animator>();
        _percentage = GetChildComponentByName<Text>("Percentage");
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_vrController)
        {
            GetVrPlayer();
            Debug.Log("The VR Controller: " + _vrController);
            return;
        }
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