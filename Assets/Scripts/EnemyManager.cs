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
    private List<Transform> enemesIsTarget = new();

    public Action WaveEnd;

    public bool IsAliveEnemiesLeft()
    {
        return aliveEnemies.Count > 0;
    }

    public void RemoveEnemyFromTargets(Transform enemy)
    {
        enemesIsTarget.Remove(enemy);
    }

    public List<Transform> GetTargetEnemies()
    {
        //List<Transform> targets = new();
        //aliveEnemies.ForEach(item => targets.Add(item.transform));
        //return targets;
        return enemesIsTarget;
    }

    public void CreateEnemy(int value)
    {
        for (int i = 0; i < value; i++)
        {
            GameObject enemy = Instantiate(_enemyPrefab, _spawnPoint.localPosition, _spawnPoint.rotation, transform);

            //Randomise enemy spawn position
            float randDistance = UnityEngine.Random.Range(-400, 400) / 100;
            float randDir = UnityEngine.Random.Range(0, 360);
            float posX = Mathf.Cos(randDir * Mathf.Deg2Rad) * randDistance;
            float posZ = Mathf.Sin(randDir * Mathf.Deg2Rad) * randDistance;
            enemy.transform.localPosition += new Vector3(posX, 0, posZ);

            aliveEnemies.Add(enemy.GetComponent<EnemyBehaviour>());
            aliveEnemies[aliveEnemies.Count - 1].EnemyInit(_player, _healPoints, _moveSpeed, _enemyDamage, _enemyDamageDistance, _enemyDamageRate);
        }
        aliveEnemies.ForEach(item => enemesIsTarget.Add(item.transform));
    }

    private void RemoveEnemy(EnemyBehaviour enemy)
    {
        aliveEnemies.Remove(enemy);
        enemesIsTarget.Remove(enemy?.transform);
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
