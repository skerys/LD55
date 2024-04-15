using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private BaseEnemy meleeEnemy;
    [SerializeField] private RangerEnemy rangedEnemy;

    [SerializeField] private float spawnCircleRadius;
    [SerializeField] private float minCooldown;
    [SerializeField] private float maxCooldown;

    private float _currentCooldown;
    
    public event Action OnEmptyReserves = delegate {};

    private enum EnemyType
    {
        Melee = 0,
        Ranged = 1,
        Barbarian = 2,
        Length = 3
    };

    public List<int> reinforcements = new List<int>(new int[(int)EnemyType.Length]);

    public void FillReinforcements(int meleeCount, int rangedCount, int barbarianCount)
    {
        reinforcements[(int)EnemyType.Melee] = meleeCount;
        reinforcements[(int)EnemyType.Ranged] = rangedCount;
        reinforcements[(int)EnemyType.Barbarian] = barbarianCount;
    }
    
    

    void Start()
    {
        _currentCooldown = Random.Range(minCooldown, maxCooldown);
    }

    void Update()
    {
        if (_currentCooldown > 0f)
        {
            _currentCooldown -= Time.deltaTime;
            if (_currentCooldown <= 0f)
            {
                DoSpawn();
                _currentCooldown = Random.Range(minCooldown, maxCooldown);
            }
        }
    }

    void DoSpawn()
    {
        List<int> nonZeroReinforcementIndices = new List<int>();

        for (int i = 0; i < (int)EnemyType.Length; i++)
        {
            if (reinforcements[i] > 0) nonZeroReinforcementIndices.Add(i);
        }

        if (nonZeroReinforcementIndices.Count == 0)
        {
            OnEmptyReserves();
            return;
        }

        int randomNonZeroIndex = nonZeroReinforcementIndices[Random.Range(0, nonZeroReinforcementIndices.Count)];
        reinforcements[randomNonZeroIndex]--;
        
        EnemyType enemyType = (EnemyType)randomNonZeroIndex;

        Vector2 randomPos = Random.insideUnitCircle.normalized * spawnCircleRadius;
        Vector3 worldPos = new Vector3(randomPos.x, 0f, randomPos.y);

        ScenarioManager.AddEnemyCount(1);
        
        switch (enemyType)
        {
            case EnemyType.Melee:
                Instantiate(meleeEnemy, worldPos, Quaternion.identity);
                break;
            case EnemyType.Ranged:
                RangerEnemy ranger = Instantiate(rangedEnemy, worldPos, Quaternion.identity);
                ranger.initialWander = true;

                Vector2 randomWanderPos = Random.insideUnitCircle * 5f;
                Vector3 wanderWorldPos = new Vector3(randomWanderPos.x, 0f, randomWanderPos.y);

                ranger.initialWanderPosition = wanderWorldPos;
                break;
            case EnemyType.Barbarian:
                break;
        }
    }
}
