using UnityEngine;

public class RangerEnemy : BaseEnemy
{
    [SerializeField] private float shotCooldown;
    [SerializeField] private float aimTime;
    [SerializeField] private Arrow arrowPrefab;
    [SerializeField] private Transform targeting;
    [SerializeField] private float wanderSpeed;
    
    private bool _isAiming = false;

    private float _aimingTimer = 0f;
    private float _cooldownTimer = 0f;

    private Vector3 _wanderDirection;

    private Camera mainCamera;

    public override void Start()
    {
        base.Start();
        mainCamera = Camera.main;
        RandomizeWanderDir();
    }

    void Update()
    {
        if (!_isAiming)
        {
            Body.velocity = _wanderDirection * wanderSpeed;
            
            _cooldownTimer -= Time.deltaTime;
            
            if (_cooldownTimer < 0f)
                StartAiming();
        }
        else
        {
            Body.velocity = Vector3.zero;
            
            _aimingTimer += Time.deltaTime;

            UpdateTargeting();
            
            if (_aimingTimer > aimTime)
            {
                DoShot();
            }
        }
            
    }

    void StartAiming()
    {
        _isAiming = true;
        _aimingTimer = 0f;
        
        targeting.gameObject.SetActive(true);
    }

    void UpdateTargeting()
    {
        Vector3 a = Vector3.ProjectOnPlane(Vector3.forward, mainCamera.transform.forward);
        Vector3 b = Vector3.ProjectOnPlane(Target.position - transform.position, mainCamera.transform.forward);
        
        float zAngle = Vector3.SignedAngle(a, b, -mainCamera.transform.forward);
        
        targeting.rotation = Quaternion.Euler(30f, 0f, -zAngle);
    }

    void DoShot()
    {
        Arrow newArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);

        newArrow.transform.forward = Target.position - transform.position;

        _cooldownTimer = shotCooldown;
        targeting.gameObject.SetActive(false);
        
        _isAiming = false;

        RandomizeWanderDir();
    }

    private void RandomizeWanderDir()
    {
        Vector2 randomDir = Random.insideUnitCircle;
        _wanderDirection = new Vector3(randomDir.x, 0f, randomDir.y);
    }
}
