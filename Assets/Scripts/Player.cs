using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] public float MaxHealPoint;
    [SerializeField] private Scrollbar HpBar;

    private float _healPoints;

    private void GetDamage(float damage)
    {
        HpBar.size -= damage / _healPoints;
        _healPoints -= damage;

        if (_healPoints <= 0)
            Die();
    }

    private void Die()
    {
        _healPoints = MaxHealPoint;
        HpBar.size= 1;
    }

    private void OnEnable()
    {
        EnemyBehaviour.OnEnemyDamage += GetDamage;    
    }

    private void OnDisable()
    {
        EnemyBehaviour.OnEnemyDamage += GetDamage;    
    }
}
