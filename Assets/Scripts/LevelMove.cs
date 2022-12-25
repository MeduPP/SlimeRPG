using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMove : MonoBehaviour
{
    [Header("Spawn parameters")]
    [SerializeField] private GameObject _groundPrefab;
    [SerializeField] private Transform LevelOrigin;
    [SerializeField] private float _tileLength;

    [Header("Move parameters")]
    [Tooltip("In tiles")]
    [SerializeField] private float _moveDistance;
    [SerializeField] private float _moveSpeed;
    [Tooltip("Point where ground ends")]
    [SerializeField] private float _groundBorder;

    private List<GameObject> GroundList = new List<GameObject>();

    public Action OnEndMove;

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            GroundList.Add(Instantiate(_groundPrefab, transform.position + (Vector3.forward * _tileLength * i), transform.rotation, transform));
        }
    }

    [ContextMenu("Move")]
    public void Move()
    {
        StartCoroutine(DoMove(_moveDistance));
    }

    IEnumerator DoMove(float distance)
    {
        float currentDitance = 0;
        while (currentDitance <= distance * _tileLength)
        {
            foreach (var item in GroundList)
            {
                item.transform.Translate(Vector3.forward * -_moveSpeed * Time.deltaTime);
            }

            GameObject tile = GroundList[0];

            if (tile.transform.position.z <= _groundBorder)
            {
                tile.transform.position = GroundList[GroundList.Count - 1].transform.position + (Vector3.forward * _tileLength);
                GroundList.Add(tile);
                GroundList.RemoveAt(0);
            }

            currentDitance += _moveSpeed * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        OnEndMove?.Invoke();
    }
}
