using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class Shoot : MonoBehaviour
{
    [Header("Shooting parameters")]
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private float _atackZone;
    [SerializeField] private float _fireRate;

    [Header("Bullet parameters")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _speed;
    [SerializeField] private float _curving;
    [SerializeField] private float _damage;

    public float FireRateBoost = 1;
    public float DamageBoost = 1;
    private void Start()
    {
        StartCoroutine(Shooting());
    }

    IEnumerator Shooting()
    {
        while (true)
        {
            if (_enemyManager.IsAliveEnemiesLeft())
            {
                float fireRate = _fireRate * FireRateBoost;//Apply fire rate boost
                float damage = DamageBoost * _damage;//Apply damage boost

                //TODO: shoot in the closest enemy  
                List<Transform> targets = _enemyManager.GetTargetEnemies();

                Transform nearestEnemy = FindNearestEnemy(targets);

                if (nearestEnemy == null)
                    yield return new WaitForFixedUpdate();

                Vector3 distance = nearestEnemy.position - transform.position;

                if (distance.magnitude <= _atackZone)
                {
                    GameObject bullet = Instantiate(_bulletPrefab);

                    if (nearestEnemy.GetComponent<EnemyBehaviour>().HealthPoints <= damage)

                    bullet.GetComponent<Bullet>().Init(nearestEnemy, transform, _speed, damage, _curving);

                    yield return new WaitForSeconds(fireRate);
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private Transform FindNearestEnemy(List<Transform> targets)
    {
        if (targets.Count == 0)
            return null;

        Transform nearestEnemy = targets[0];
        for (int i = 0; i < targets.Count - 1; i++)
        {
            if((targets[i + 1].position - transform.position).magnitude <
            (nearestEnemy.position - transform.position).magnitude)
                nearestEnemy = targets[i + 1];
        }
        return nearestEnemy;
    }
}
