using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;


public class ControllerScript : MonoBehaviour
{
    private LineRenderer _line;
    private XRRayInteractor _interactor;

    // Start is called before the first frame update
    void Start()
    {
        this._line = GetComponent<LineRenderer>();
        this._interactor = GetComponent<XRRayInteractor>();
    }

    // Update is called once per frame
    void Update()
    {

        bool hit = _interactor.TryGetCurrent3DRaycastHit(out RaycastHit raycastHit);
        if (hit)
        {
            GameObject obj = raycastHit.transform.gameObject;
            if (obj.CompareTag("VR_UI"))
            {
                this._line.forceRenderingOff = false;
            }
            else
            {
                this._line.forceRenderingOff = true;
            }
            Debug.Log("hit " + raycastHit.GetType().Name + " <-> " + raycastHit.transform.gameObject.tag);

        }
        else
        {
            Debug.Log("Miss");
            this._line.forceRenderingOff = true;
        }

    }
}
