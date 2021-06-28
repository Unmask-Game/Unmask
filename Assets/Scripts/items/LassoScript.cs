using System;
using System.Collections;
using UnityEngine;

public class LassoScript : Item
{
    [SerializeField] private GameObject ropeStartingPoint;
    private Transform _currentRopeEndPoint;
    private LineRenderer _lineRenderer;
    private bool _drawRope;
    private const float GeneralCooldownAfterHit = 3f;
    private const float AttackCooldownAfterHit = 8f;

    private void Start()
    {
        itemName = ItemName.Lasso;
        itemType = ItemType.Arrest;
        Damage = 0;
        Range = 1.4f;
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
        AudioManager playerAudio)
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Range))
        {
            var objectHit = hit.collider.gameObject;
            if (objectHit.CompareTag("TestVRPlayer"))
            {
                DrawRope(hit, playerAudio);
                objectHit.GetComponent<TestVRPlayer>().BeSlowedDown(GeneralCooldownAfterHit);
                yield return new WaitForSeconds(0);
                itemController.CooldownAllItems(AttackCooldownAfterHit, GeneralCooldownAfterHit);
                yield return new WaitForSeconds(GeneralCooldownAfterHit);
            }
        }

        // stop displaying rope
        _drawRope = false;
        _lineRenderer.gameObject.SetActive(false);
    }

    private void DrawRope(RaycastHit hit, AudioManager playerAudio)
    {
        // rope end is not the hit.point exactly, so the rope can easily move together with the player
        _currentRopeEndPoint = hit.transform;
        _lineRenderer.gameObject.SetActive(true);
        _drawRope = true;
        playerAudio.Play("Lasso");
    }

    private void UpdateRope()
    {
        _lineRenderer.SetPosition(0, ropeStartingPoint.transform.position);
        _lineRenderer.SetPosition(1, _currentRopeEndPoint.position);
    }
}