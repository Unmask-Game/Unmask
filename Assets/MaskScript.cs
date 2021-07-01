using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MaskScript : MonoBehaviour
{
    [SerializeField]
    private NpcController _npcController;

    private Vector3 _position;
    private Quaternion _rotation;

    // Start is called before the first frame update
    void Start()
    {
        _position = gameObject.transform.localPosition;
        _rotation = gameObject.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Grabed a Maks!");
        _npcController.StopWalking();
    }

    public void OnRelease(SelectExitEventArgs args)
    {
        Debug.Log("Release those Maks!");
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.transform.localPosition = _position;
        gameObject.transform.localRotation = _rotation;
        _npcController.ResumeWalking();
    }
}
