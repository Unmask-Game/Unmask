using System;
using UnityEngine;
using UnityEngine.UI;

public class CoronaBarController : MonoBehaviour
{
    private Animator _virusAnimator;
    private Image _coronaBar;
    private Text _percentage;
    private float _npcNum;
    private float _currentNumberOfStolenMasks;

    // Start is called before the first frame update
    private void Awake()
    {
        _coronaBar = GetChildComponentByName<Image>("Bar");
        _virusAnimator = GetChildComponentByName<Image>("Virus").GetComponent<Animator>();
        _percentage = GetChildComponentByName<Text>("Percentage");
    }

    private void Start()
    {
        // get number of NPCs etc... from another Controller
        _npcNum = 100;
    }

    // Update is called once per frame
    private void Update()
    {
        // get number of stolen masks etc... from another Controller
        //_currentNumberOfStolenMasks = 0;
        _currentNumberOfStolenMasks += 0.001f;
        
        _coronaBar.fillAmount = float.IsNaN(_currentNumberOfStolenMasks / _npcNum) ? 0 : _currentNumberOfStolenMasks / _npcNum;
        _virusAnimator.speed = _coronaBar.fillAmount + 1;
        _percentage.text = (int) Math.Round(_coronaBar.fillAmount * 100) + "%";
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
}