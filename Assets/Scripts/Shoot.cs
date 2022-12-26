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

    IEnumerator Shooting()//TODO: Do not shoot at the enemy if the damage is fatal
    {
        //expected damage to target
        float sumDamage = 0;
        //current target
        Transform nearestEnemy = null;

        while (true)
        {
            if (_enemyManager.IsAliveEnemiesLeft())
            {
                float fireRate = _fireRate * FireRateBoost;//Apply fire rate boost
                float damage = DamageBoost * _damage;//Apply damage boost

                List<Transform> targets = _enemyManager.GetTargetEnemies();
                Transform newNearestEnemy = FindNearestEnemy(targets);

                //if get new target refresh damage and target
                if (nearestEnemy != newNearestEnemy)
                {
                    nearestEnemy = newNearestEnemy;
                    sumDamage = 0;
                }

                //if target is empty then wait
                if (nearestEnemy == null)
                {
                    yield return new WaitForFixedUpdate();
                    continue;
                }

                //calculate the distance to the nearest enemy calculate the distance to the nearest enemy
                float distance = (nearestEnemy.position - transform.position).magnitude;

                if (distance <= _atackZone)
                {
                    GameObject bullet = Instantiate(_bulletPrefab);

                    sumDamage += damage;

                    //If the enemy dies from the next shot, no need to shoot again.
                    if (nearestEnemy.GetComponent<EnemyBehaviour>().HealthPoints <= sumDamage)
                        _enemyManager.RemoveEnemyFromTargets(nearestEnemy);

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
            if ((targets[i + 1].position - transform.position).magnitude <
            (nearestEnemy.position - transform.position).magnitude)
                nearestEnemy = targets[i + 1];
        }
        return nearestEnemy;
    }
}
