using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PhotonPlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject mask;
    [SerializeField] private MeshRenderer headRenderer;
    private PhotonView _view;
    
    private void Start()
    {
        _view = GetComponent<PhotonView>();
        if (!_view.IsMine)
        {
            // disable hud and camera for remote players
            cam.SetActive(false);
            hud.SetActive(false);
            GetComponent<PlayerInput>().enabled = false;
            GetComponent<PoliceSoundController>().enabled = false;
        }
        else
        {
            // Hide own player head & mask
            headRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            mask.SetActive(false);
        }
    }
}