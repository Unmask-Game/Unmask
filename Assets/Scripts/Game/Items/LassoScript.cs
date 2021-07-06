using System.Collections;
using DefaultNamespace;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using static DefaultNamespace.Tags;

public class LassoScript : Item
{
    [SerializeField] private GameObject ropeStartingPoint;

    // Object where the line renderer is attached to
    private Transform _currentRopeEndPoint;

    // Line renderer to simulate a rope that is attached to an object
    private LineRenderer _lineRenderer;
    private bool _drawRope;

    private void Start()
    {
        itemName = ItemName.Lasso;
        itemType = ItemType.Arrest;
        Damage = 0;
        Range = Constants.LassoRange;
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        _lineRenderer.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (_drawRope)
        {
            UpdateRope();
        }
    }

    public override IEnumerator Attack(ItemController itemController, Camera cam, Animator playerAnimator,
        AudioManager playerAudio, PhotonView view)
    {
        // Check if hit is in range via raycast from the current mouse position
        var ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit, Range))
        {
            var objectHit = hit.collider.gameObject;
            if (objectHit.CompareTag(VrPlayerTag))
            {
                PlayAnimation(playerAnimator, playerAudio);
                // Send RPC to others to also play the item's animation ("PlayAnimation") for them 
                view.RPC("PlayItemAnimationRemote", RpcTarget.Others);
                objectHit.GetComponent<VRPlayerController>().OnLassoHit(Constants.LassoDuration);
                yield return new WaitForSeconds(0);
                itemController.CooldownAllItems(Constants.LassoCooldown, Constants.LassoDuration);
                // Display line renderer as long as the player is slowed down
                yield return new WaitForSeconds(Constants.LassoDuration);
            }
        }

        // Stop displaying rope 
        _drawRope = false;
        _lineRenderer.gameObject.SetActive(false);
    }

    public override void PlayAnimation(Animator playerAnimator, AudioManager playerAudio)
    {
        playerAudio.Play("Lasso");
        _lineRenderer.gameObject.SetActive(true);
        StartCoroutine(StopAnimation(Constants.LassoDuration));

        // Display rope (line renderer with texture) between item and mask thief (Vr player)
        var vrPlayer = GameObject.FindWithTag(VrPlayerTag);
        DrawRope(vrPlayer.transform);
    }

    // Stop animation after a set amount of time
    private IEnumerator StopAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        _lineRenderer.gameObject.SetActive(false);
    }

    private void DrawRope(Transform target)
    {
        // Rope end is not the hit.point exactly, so the rope can easily move together with the player
        _currentRopeEndPoint = target.transform;
        _lineRenderer.gameObject.SetActive(true);
        _drawRope = true;
    }

    private void UpdateRope()
    {
        // Rope starts at the item position and ends at the center of the mask thief's model (small offset so it will be displayed a little bit lower)
        _lineRenderer.SetPosition(0, ropeStartingPoint.transform.position);
        _lineRenderer.SetPosition(1,
            _currentRopeEndPoint.GetComponent<Collider>().bounds.center + new Vector3(0, -0.3f, 0));
    }
}