using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private List<Image> hearts = new List<Image>();
    [SerializeField] private Sprite fullSprite;
    [SerializeField] private Sprite emptySprite;
    
    private int _knownMaxHealth = 99;
    private int _knownCurrentHealth = 99;
    
    public static HealthUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    public void UpdateUI(int maxHealth, int currentHealth)
    {
        if (maxHealth != _knownMaxHealth)
        {
            for (int i = 0; i < hearts.Count; ++i)
            {
                hearts[i].gameObject.SetActive(i < maxHealth);
            }

            _knownMaxHealth = maxHealth;
        }

        if (currentHealth != _knownCurrentHealth)
        {
            for (int i = 0; i < hearts.Count; ++i)
            {
                hearts[i].sprite = i < currentHealth ? fullSprite : emptySprite;
            }

            _knownCurrentHealth = currentHealth;
        }
    }

    public void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
