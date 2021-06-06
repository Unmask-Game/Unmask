using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Controller : MonoBehaviour
{

    private List<InputDevice> devices = new List<InputDevice>();
    public XRNode xrNode = XRNode.RightHand;


    // Start is called before the first frame update
    void Start()
    {
        InputDevices.GetDevicesAtXRNode(xrNode, devices);
    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log("controller update");
        foreach (InputDevice device in devices)
        {
            Debug.Log(device.name);
        }

    }
}
