using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private GameObject deathEffect;
    
    protected int _health = 1;
    
    protected Transform Target;
    protected Rigidbody Body;
    protected SpriteRenderer Sprite;
    
    public virtual void Start()
    {
        //Find target
        Target = FindObjectOfType<DemonController>()?.transform;
        Body = GetComponent<Rigidbody>();
        Sprite = GetComponentInChildren<SpriteRenderer>();
    }
    
    public virtual bool DoHit(int strength)
    {
        _health -= strength;

        if (_health <= 0)
        {
            Die();
            ScenarioManager.RemoveEnemyCount(1);
            return true;
        }
        return false;
    }

    private void Die()
    {
        SoundManager.Instance.PlaySound(OneShotSoundTypes.EnemyDeath);
        var go = Instantiate(deathEffect, transform.position, Quaternion.identity);
        foreach (var sr in go.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.flipX = Sprite.flipX;
        }
        
        Destroy(this.gameObject);
    }
}
