using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        StartCoroutine(Shoting());
    }
                        
    IEnumerator Shoting()
    {
        while (true)
        {
            if (_enemyManager.IsAliveEnemiesLeft())
            {

                float fireRate = _fireRate * FireRateBoost;//Apply fire rate boost
                float damage = DamageBoost * _damage;//Apply damage boost

                //TODO: shoot in the closest enemy  
                foreach (var item in _enemyManager.GetAliveEnemies())
                {
                    Vector3 distance = item.transform.position - transform.position;
                    if (distance.magnitude <= _atackZone)
                    {
                        GameObject bullet = Instantiate(_bulletPrefab);
                        bullet.GetComponent<Bullet>().Init(item.transform, transform, _speed, damage, _curving);

                        yield return new WaitForSeconds(fireRate);
                        break;
                    }
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
