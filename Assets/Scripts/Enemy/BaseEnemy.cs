using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    private int _health = 1;
    
    protected Transform Target;
    protected Rigidbody Body;
    
    public virtual void Start()
    {
        //Find target
        Target = FindObjectOfType<DemonController>()?.transform;
        Body = GetComponent<Rigidbody>();
    }
    
    public void DoHit(int strength)
    {
        _health -= strength;

        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
