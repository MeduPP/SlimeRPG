using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _player;

    [Header("Enemy parameters")]
    [SerializeField] private float _healPoints;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _enemyDamage;
    [SerializeField] private float _enemyDamageDistance;
    [SerializeField] private float _enemyDamageRate;

    private List<EnemyBehaviour> aliveEnemies = new();
    private List<Transform> enemyIsTargets = new();

    public Action WaveEnd;

    public bool IsAliveEnemiesLeft()
    {
        return aliveEnemies.Count > 0;
    }

    public void RemoveEnemyFromTargets(Transform enemy)
    {
        enemyIsTargets.Remove(enemy);
    }

    public List<Transform> GetTargetEnemies()
    {
        List<Transform> list = new(aliveEnemies.Count);
        aliveEnemies.ForEach(item => list.Add(item.transform));
        return list;
    }

    public void CreateEnemy(int value)
    {
        for (int i = 0; i < value; i++)
        {
            GameObject enemy = Instantiate(_enemyPrefab, _spawnPoint.localPosition, _spawnPoint.rotation, transform);
            float randDistance = UnityEngine.Random.Range(-400, 400) / 100;
            float randDir = UnityEngine.Random.Range(0, 360);
            float posX = Mathf.Cos(randDir * Mathf.Deg2Rad) * randDistance;
            float posZ = Mathf.Sin(randDir * Mathf.Deg2Rad) * randDistance;
            enemy.transform.localPosition += new Vector3(posX, 0, posZ);

            aliveEnemies.Add(enemy.GetComponent<EnemyBehaviour>());
            aliveEnemies[aliveEnemies.Count - 1].EnemyInit(_player, _healPoints, _moveSpeed, _enemyDamage, _enemyDamageDistance, _enemyDamageRate);
        }
    }

    private void RemoveEnemy(EnemyBehaviour enemy)
    {
        aliveEnemies.Remove(enemy);

        if (aliveEnemies.Count == 0)
        {
            WaveEnd?.Invoke();
        }
    }

    private void OnEnable()
    {
        EnemyBehaviour.OnEnemyDeath += RemoveEnemy;
    }

    private void OnDisable()
    {
        EnemyBehaviour.OnEnemyDeath -= RemoveEnemy;
    }
}
