using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;
public class VRManager : MonoBehaviour
{
    private static VRManager _instance;
    public static VRManager Instance { get { return _instance; } }

    public bool isVR;
    
    void Awake()
    {
        if (_instance != null && _instance != this)
            gameObject.SetActive(false);
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    void OnApplicationQuit()
    {
        StopXR();
    }

    public void StartXR()
    {
        isVR = true;
        StartCoroutine(StartXRCoroutine());
    }
    public void StartXR(int scene)
    {
        isVR = true;
        StartCoroutine(StartXRCoroutine(scene));
    }

    private IEnumerator StartXRCoroutine(int scene)
    {
        yield return StartXRCoroutine();
        SceneManager.LoadScene(scene);
    }

    private IEnumerator StartXRCoroutine()
    {
        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
        }
        else
        {
            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
    }
    void StopXR()
    {
        Debug.Log("Stopping XR...");

        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Debug.Log("XR stopped completely.");
    }
}
