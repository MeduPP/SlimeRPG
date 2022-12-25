using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private Scrollbar HpBar;
    private float _maxHealPoints;
    private float _healthPoints;
    private float _moveSpeed;
    private float _damageDistance;
    private float _damageRate;
    [HideInInspector] public float Damage;

    public static Action<EnemyBehaviour> OnEnemyDeath;
    public static Action<float> OnEnemyDamage;

    public float HealthPoints {get { return _healthPoints; }}

    private bool _isMoving = false;
    private GameObject _target;
    private Coroutine _damageCoroutine;

    public void EnemyInit(
        GameObject target,
        float healPoints,
        float moveSpeed,
        float enemyDamage,
        float damageDistance,
        float damageRate)
    {
        _maxHealPoints = healPoints;
        _healthPoints = healPoints;
        _moveSpeed = moveSpeed;
        _target = target;
        Damage = enemyDamage;
        _damageDistance = damageDistance;
        _damageRate = damageRate;
        _isMoving = true;
    }

    void FixedUpdate()
    {
        Vector3 diraction = _target.transform.position - transform.position;

        if (diraction.magnitude <= _damageDistance && _damageCoroutine == null)
            _damageCoroutine = StartCoroutine(DoDamage());

        if (_isMoving)
        { 
            if (diraction.magnitude < 4f)//TODO: remove magic number
                transform.Translate(diraction.normalized * _moveSpeed * Time.deltaTime);
            else
                transform.Translate(Vector3.forward * -_moveSpeed * Time.deltaTime);
        }
    }

    private void Die()
    {
        if (_damageCoroutine != null)
            StopCoroutine(_damageCoroutine);

        OnEnemyDeath?.Invoke(this);
        Destroy(gameObject);//TODO: death effect
    }

    IEnumerator DoDamage()
    {
        while (true)
        {
            OnEnemyDamage?.Invoke(Damage);
            yield return new WaitForSeconds(_damageRate);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            _isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            float damage = other.gameObject.GetComponent<Bullet>().Damage;
            _healthPoints -= damage;
            Destroy(other.gameObject);//TODO: bullet destroy effect

            if (_healthPoints <= 0)
                Die();

            HpBar.size -= damage / _maxHealPoints;
        }
    }
}
