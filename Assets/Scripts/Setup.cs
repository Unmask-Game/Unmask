using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;
#if (UNITY_EDITOR) 
using ParrelSync;
#endif

public class Setup : MonoBehaviour
{
    [SerializeField] private bool preferVR;
    
    // Runs on game launch and enables XR depending on launch arg
    void Start()
    {
        bool useVr = GetArg("-hmd");
        #if (UNITY_EDITOR)
        useVr = ClonesManager.GetArgument().Equals("vr") || preferVR;
        #endif
        if (useVr)
            VRManager.Instance.StartXR(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);

    }
    
    // Check for launch arg
    // Source: https://forum.unity.com/threads/pass-custom-parameters-to-standalone-on-launch.429144/
    private static bool GetArg(string name)
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name)
            {
                return true;
            }
        }
        return false;
    }
}
