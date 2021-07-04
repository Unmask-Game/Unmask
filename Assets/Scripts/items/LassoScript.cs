using System.Collections;
using DefaultNamespace;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using static DefaultNamespace.Tags;

public class LassoScript : Item
{
    [SerializeField] private GameObject ropeStartingPoint;
    private Transform _currentRopeEndPoint;
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
        var ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit, Range))
        {
            var objectHit = hit.collider.gameObject;
            if (objectHit.CompareTag(VrPlayerTag))
            {
                PlayAnimation(playerAnimator, playerAudio);
                view.RPC("PlayItemAnimationRemote", RpcTarget.Others);
                objectHit.GetComponent<VRPlayerController>().OnLassoHit(Constants.LassoCooldown);
                yield return new WaitForSeconds(0);
                itemController.CooldownAllItems(Constants.AttackCooldownAfterHit, Constants.LassoCooldown);
                yield return new WaitForSeconds(Constants.LassoCooldown);
            }
        }

        // stop displaying rope
        _drawRope = false;
        _lineRenderer.gameObject.SetActive(false);
    }

    public override void PlayAnimation(Animator playerAnimator, AudioManager playerAudio)
    {
        playerAudio.Play("Lasso");
        _lineRenderer.gameObject.SetActive(true);
        StartCoroutine(StopAnimation(Constants.LassoCooldown));
        var vrPlayer = GameObject.FindWithTag(VrPlayerTag);
        DrawRope(vrPlayer.transform);
    }

    private IEnumerator StopAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        _lineRenderer.gameObject.SetActive(false);
    }

    private void DrawRope(Transform target)
    {
        // rope end is not the hit.point exactly, so the rope can easily move together with the player
        _currentRopeEndPoint = target.transform;
        _lineRenderer.gameObject.SetActive(true);
        _drawRope = true;
    }

    private void UpdateRope()
    {
        _lineRenderer.SetPosition(0, ropeStartingPoint.transform.position);
        _lineRenderer.SetPosition(1, _currentRopeEndPoint.GetComponent<Collider>().bounds.center + new Vector3(0, -0.3f, 0));
    }
}