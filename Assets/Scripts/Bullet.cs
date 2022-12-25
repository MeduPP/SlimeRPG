using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public float Damage;
    private Transform _target;
    private float _speed;
    private float _curving;

    private Vector3 _lastTargetPos;
    private float _pathLength;
    public void Init(Transform target, Transform shooterPos, float speed, float damage, float curving)
    {
        _target = target;
        _lastTargetPos = target.position;
        _speed = speed;
        Damage = damage;
        _curving = curving;
        _pathLength = (target.position + shooterPos.position).magnitude;
    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            _lastTargetPos = _target.position;
        }

        float t = 1 - ((transform.position - _lastTargetPos).magnitude / _pathLength);

        float y = _curving * Mathf.Cos(t * 2f);

        transform.position = Vector3.MoveTowards(transform.position, _lastTargetPos + Vector3.up * y, Time.deltaTime * _speed);

        if (_lastTargetPos + Vector3.up * y == transform.position)
        {
            Destroy(gameObject);
        }
    }
}
