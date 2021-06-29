using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRController : MonoBehaviour
{
    [SerializeField] private GameObject interactionManager;

    void Start()
    {
        if (!VRManager.Instance.isVR)
        {
            interactionManager.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    
    void Update()
    {
        
    }
}
