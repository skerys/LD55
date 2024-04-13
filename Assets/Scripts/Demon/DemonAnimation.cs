using UnityEngine;

public class DemonAnimation : MonoBehaviour
{
    private DemonController _demon;
    private DashController _dash;

    private Animator _animator;
    private SpriteRenderer _sprite;

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _dash = GetComponent<DashController>();
        _demon = GetComponent<DemonController>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (_dash.DashInProgress)
        {
            _animator.Play("DemonDash");
            _sprite.flipX = _dash.dashStartPosition.x > _dash.dashTargetPosition.x;
        }
        else if (_demon.TargetVelocity.sqrMagnitude > 0.01f)
        {
            _animator.Play("DemonRun");
            _sprite.flipX = _demon.TargetVelocity.x < 0f;
        }
        else
        {
            _animator.Play("DemonIdle");
        }
    }
}
