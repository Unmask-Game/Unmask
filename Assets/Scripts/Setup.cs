using System.Collections;
using System.Collections.Generic;
using ParrelSync;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;

public class Setup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (ClonesManager.IsClone())
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
        else
            VRManager.Instance.StartXR(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    private static string GetArg(string name)
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }
        return null;
    }
}
