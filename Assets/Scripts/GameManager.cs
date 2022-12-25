using System.Collections;
using UnityEngine;
enum GameStage
    {
        Wave,
        WaitForWave,
        Move,
        WaitForMove
    }
public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private LevelMove _levelMove;

    private GameStage gameStage = GameStage.Wave;

    private void Start()
    {
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        while(true)
        {
            switch (gameStage)
            {
                case GameStage.Wave:
                    _enemyManager.CreateEnemy(3); //TODO: different count of enemies in wave
                    gameStage = GameStage.WaitForWave;
                    break;
                case GameStage.WaitForWave:
                    yield return new WaitForFixedUpdate();
                    break;
                case GameStage.Move:
                    yield return new WaitForSeconds(2);//magic number
                    _levelMove.Move();
                    gameStage = GameStage.WaitForMove;
                    break;
                case GameStage.WaitForMove:
                    yield return new WaitForFixedUpdate();
                    break;
            }
        }
    }

    private void SetWaveState()
    {
        gameStage = GameStage.Wave;
    }
    
    private void SetMoveState()
    {
        gameStage = GameStage.Move;
    }
    private void OnEnable()
    {
        _enemyManager.WaveEnd += SetMoveState;
        _levelMove.OnEndMove += SetWaveState;
    }

    private void OnDisable()
    {
        _enemyManager.WaveEnd -= SetMoveState;
        _levelMove.OnEndMove -= SetWaveState;
    }
}
