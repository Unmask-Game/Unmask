using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MaskScript : MonoBehaviour
{
    [SerializeField]
    private NpcController _npcController;

    [SerializeField]
    private AudioSource _audioSource;

    /* Set to initial local position */
    private Vector3 _localOriginPosition;
    private Quaternion _localOriginRotation;

    /* To be set when attached to controller */
    private bool _attached;
    private Transform _attachTransform;
    private Vector3 _attachStartPosition;
    private Vector3 _attachOffset;

    // Start is called before the first frame update
    void Start()
    {
        _localOriginPosition = gameObject.transform.localPosition;
        _localOriginRotation = gameObject.transform.localRotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_attached)
        {
            // Debug.Log("attach position: " + _attachTransform.position + "; attachOffset: " + _attachOffset);
            gameObject.transform.transform.position = _attachTransform.position + _attachOffset;
        }

        if (_attached && Vector3.Distance(_attachTransform.position, _attachStartPosition) > 0.5)
        {
            _npcController.RemoveMask();
            _audioSource.Play();
            Detach();
        }
    }

    public void OnGrab(SelectEnterEventArgs args)
    {
        _attached = true;
        _npcController.StopWalking();

        // Assign attachment point
        _attachStartPosition = transform.position;
        _attachTransform = args.interactor.transform;
        _attachOffset = gameObject.transform.transform.position - _attachTransform.position;

        Debug.Log("Grabbed a Mask!");
    }

    public void OnRelease(SelectExitEventArgs args)
    {
        Detach();
        Debug.Log("Released a Mask!");
    }

    private void Detach()
    {
        _attached = false;

        // Reset Velocity
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        // Reset Position
        gameObject.transform.localPosition = _localOriginPosition;
        gameObject.transform.localRotation = _localOriginRotation;

        // Resume NPC Pathfinding
        _npcController.ResumeWalking();
    }
}
