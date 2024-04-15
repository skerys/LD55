using UnityEngine;

public class BarbarianEnemy : BaseEnemy
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject helmetEffect;
    [SerializeField] private Sprite helmetlessSprite;

    public override void Start()
    {
        base.Start();
        _health = 2;
    }

    public override bool DoHit(int strength)
    {
        bool value = base.DoHit(strength);
        if (_health == 1)
        {
            Instantiate(helmetEffect, transform.position, Quaternion.identity);
            Sprite.sprite = helmetlessSprite;
        }

        return value;
    }

    private void Update()
    {
        if (Target)
        {
            Vector3 dirToTarget = (Target.position - transform.position).normalized;
            Body.velocity = dirToTarget * speed;

            Sprite.flipX = Body.velocity.x < 0f;
        }
        
    }
}
