using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DemonHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invulnerableTime = 0.3f;
    [SerializeField] private SpriteRenderer _demonSprite;
    
    private DemonController _demon;
    private int _currentHealth;
    private float _invulnerabiltyTimer = 0.1f;
    private static readonly int WhiteAmount = Shader.PropertyToID("_WhiteAmount");

    [SerializeField] private CinemachineVirtualCamera deathCamera;
    
    public event Action OnKill = delegate {  };
    
    void Start()
    {
        _demon = GetComponent<DemonController>();
        _currentHealth = maxHealth;
        
        HealthUI.Instance?.UpdateUI(maxHealth, _currentHealth);
    }

    void Update()
    {
        if (_invulnerabiltyTimer > 0f)
        {
            _invulnerabiltyTimer -= Time.deltaTime;
            var t = _invulnerabiltyTimer / invulnerableTime;
            _demonSprite.material.SetFloat(WhiteAmount, Mathf.Clamp01(t));
        }
    }
    
    public void TakeDamage(Vector3 damagerPos)
    {
        if (_invulnerabiltyTimer > 0f)
            return;
        
        _currentHealth--;
        _demon.Bounce(transform.position - damagerPos);
        _invulnerabiltyTimer = invulnerableTime;

        if (_currentHealth <= 0)
        {
            deathCamera.transform.parent = null;
            deathCamera.gameObject.SetActive(true);
            OnKill();
        }

        HealthUI.Instance?.UpdateUI(maxHealth, _currentHealth);

    }

    public void AddMaxHealth(int h)
    {
        maxHealth += h;
        _currentHealth = maxHealth;

        HealthUI.Instance?.UpdateUI(maxHealth, _currentHealth);
    }
}
