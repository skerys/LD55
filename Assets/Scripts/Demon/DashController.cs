using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Serialization;

public class DashController : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private LayerMask mouseClickLayer = default;
    [SerializeField] private LayerMask wallLayer = default;
    [SerializeField] private float maxDashLength = 5f;
    [SerializeField] private float dashDuration = 2f;
    [SerializeField] private float dashCooldown = 0.5f;
    [SerializeField] private MirrorImageDestruct mirrorImageSprite = default;
    [SerializeField] private int mirrorImageCount = 3;
    [SerializeField] private SpriteRenderer sprite;
    
    private SphereCollider _collider;
    private Rigidbody _body;
    private DemonController _demon;

    public Vector3 dashStartPosition;
    public Vector3 dashTargetPosition;
    private float _dashProgress = 0f;
    private float _dashCooldownTimer;

    private int _mirrorImagesSpawned = 0;
    [HideInInspector] public int killsThisDash = 0;

    private bool _dashInProgress = false;

    public bool DashInProgress
    {
        get { return _dashInProgress; }
        
    }

    private void Start()
    {
        _collider = GetComponent<SphereCollider>();
        _body = GetComponent<Rigidbody>();
        _demon = GetComponent<DemonController>();

        _dashCooldownTimer = dashCooldown;
    }

    private void Update()
    {
        if (!_dashInProgress && _dashCooldownTimer < dashCooldown)
        {
            _dashCooldownTimer += Time.deltaTime;
        }
        
        if (!_dashInProgress && _dashCooldownTimer >= dashCooldown && Input.GetMouseButtonDown(0))
        {
            SetupDash();
        }

        if (_dashInProgress)
        {
            transform.position = Vector3.Lerp(dashStartPosition, dashTargetPosition, _dashProgress);

            _dashProgress += (1f / dashDuration) * Time.deltaTime;

            if (_dashProgress > _mirrorImagesSpawned / (float)mirrorImageCount)
            {
                _mirrorImagesSpawned++;
                var selfDestruct = Instantiate(mirrorImageSprite, transform.position + Vector3.up * 0.4f, transform.rotation);
                selfDestruct.timeToDestruct = dashDuration * (1 - _dashProgress);
                selfDestruct.GetComponent<SpriteRenderer>().flipX = dashStartPosition.x > dashTargetPosition.x;
            }
            
            if (_dashProgress > 1f)
            {
                EndDash();
            }
        }
        
        Debug.DrawLine(transform.position, dashTargetPosition, Color.cyan);
    }

    private void SetupDash()
    {
        Ray mouseRay = mainCam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (!Physics.Raycast(mouseRay, out hitInfo, 20f, mouseClickLayer))
        {
            return;
        }

        Vector3 dashDirection = (hitInfo.point - transform.position).normalized;
        float dashDistance = Mathf.Min(maxDashLength, (hitInfo.point - transform.position).magnitude);
        Vector3 spherePoint = transform.TransformPoint(_collider.center);

        if(Physics.SphereCast(spherePoint, _collider.radius, dashDirection, out hitInfo, dashDistance, wallLayer))
        {
            spherePoint += hitInfo.distance * dashDirection;
        }
        else
        {
            spherePoint += dashDistance * dashDirection;
        }

        dashStartPosition = transform.position;    
        
        dashTargetPosition = spherePoint;
        dashTargetPosition.y = 0f;
        
        _body.isKinematic = true;
        _collider.isTrigger = true;
        _demon.enabled = false;
        _dashInProgress = true;
        killsThisDash = 0;
    }

    private void EndDash()
    {
        _body.isKinematic = false;
        _collider.isTrigger = false;
        _demon.enabled = true;
        _dashInProgress = false;
        _dashProgress = 0f;
        _mirrorImagesSpawned = 0;

        if (killsThisDash <= 1)
        {
            _dashCooldownTimer = 0f;
        }
    }

    public void ModifyDashLength(float value)
    {
        maxDashLength *= value;
    }

    public void ModifyDashCooldown(float value)
    {
        dashCooldown *= value;
    }
}
