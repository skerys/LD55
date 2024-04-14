using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DemonController : MonoBehaviour
{
    [SerializeField] private float MaxSpeed = 1f;
    [SerializeField] private float MaxAcceleration = 10f;
    [SerializeField] private float damagedBounceVelocity = 5f;

    [HideInInspector] public bool allowInput = true;
    
    private Rigidbody _body;
    private Vector2 _input;

    private Vector3 _targetVelocity;
    
    public Vector3 TargetVelocity
    {
        get { return _targetVelocity; }
        set
        {
            if (!allowInput)
                _targetVelocity = value;
        }
    }

    void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (allowInput)
        {
            _input.x = Input.GetAxisRaw("Horizontal");
            _input.y = Input.GetAxisRaw("Vertical");
            
            _targetVelocity = new Vector3(_input.x, 0.0f, _input.y);
            _targetVelocity = Vector3.ClampMagnitude(_targetVelocity, 1.0f);
        }
        

        _targetVelocity *= MaxSpeed;
    }

    void FixedUpdate()
    {
        Vector3 oldVelocity = _body.velocity;
        Vector3 newVelocity = Vector3.zero;
        
        float maxSpeedChange = MaxAcceleration * Time.deltaTime;

        newVelocity.x = Mathf.MoveTowards(oldVelocity.x, _targetVelocity.x, maxSpeedChange);
        newVelocity.z = Mathf.MoveTowards(oldVelocity.z, _targetVelocity.z, maxSpeedChange);
        
        _body.velocity = newVelocity;
    }

    public void Bounce(Vector3 direction)
    {
        if (_body.isKinematic) return;
        
        _body.velocity += direction.normalized * damagedBounceVelocity;
    }

    public void ModifyMoveSpeed(float value)
    {
        MaxSpeed *= value;
    }
}
