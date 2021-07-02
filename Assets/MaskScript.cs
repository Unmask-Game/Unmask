using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MaskScript : MonoBehaviour
{
    [SerializeField]
    private NpcController _npcController;

    [SerializeField] private GameObject _parentMask;

    [SerializeField]
    private AudioSource _audioSource;

    private Vector3 _worldPosition;
    private Vector3 _position;
    private Quaternion _rotation;

    private bool _attached;

    // Start is called before the first frame update
    void Start()
    {
        _position = gameObject.transform.localPosition;
        _rotation = gameObject.transform.localRotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_attached)
        {
            Debug.Log("Rip: " + Vector3.Distance(gameObject.transform.position, _worldPosition));
        }

        if (_attached && Vector3.Distance(gameObject.transform.position, _worldPosition) > 0.5)
        {

            _parentMask.SetActive(false);
            _audioSource.Play();
            _attached = false;
        }
    }

    public void OnGrab(SelectEnterEventArgs args)
    {
        _attached = true;
        Debug.Log("Grabed a Maks!");
        _npcController.StopWalking();
        _worldPosition = transform.position;
    }

    public void OnRelease(SelectExitEventArgs args)
    {
        _attached = false;
        Debug.Log("Release those Maks!");
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.transform.localPosition = _position;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gameObject.transform.localRotation = _rotation;
        _npcController.ResumeWalking();
    }
}
