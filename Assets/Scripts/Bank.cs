using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    private ulong _coins;

    public static Action<ulong> OnCoinsValueChanged;

    private void Start()
    {
        OnCoinsValueChanged?.Invoke(_coins);
    }

    public void AddCoins(int value)
    {
        _coins += (ulong)value;
        OnCoinsValueChanged?.Invoke(_coins);
    }

    public bool SpendCoins(int value)
    {
        if((ulong)value > _coins)
            return false;
        else
        {
            _coins -= (ulong)value;
            OnCoinsValueChanged?.Invoke(_coins);
            return true;
        }
    }

    private void Earn(EnemyBehaviour enemyBehaviour)
    {
        _coins += 50;
        OnCoinsValueChanged?.Invoke(_coins);
    }

    private void OnEnable()
    {
        EnemyBehaviour.OnEnemyDeath += Earn;
    }

    private void OnDisable()
    {
        EnemyBehaviour.OnEnemyDeath -= Earn;
    }
}
