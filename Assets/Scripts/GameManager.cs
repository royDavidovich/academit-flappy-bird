using System;
using UnityEngine;

public enum GameState
{
    Ready,
    Playing,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.Ready;
    public int Score { get; private set; }

    public event Action<GameState> OnStateChanged;
    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        OnStateChanged?.Invoke(CurrentState);
    }

    public void StartGame()
    {
        if (CurrentState != GameState.Ready)
        {
            return;
        }

        Score = 0;
        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
        OnScoreChanged?.Invoke(Score);
        OnStateChanged?.Invoke(CurrentState);
    }

    public void AddScore(int amount = 1)
    {
        if (CurrentState != GameState.Playing)
        {
            return;
        }

        Score += amount;
        OnScoreChanged?.Invoke(Score);
    }

    public void GameOver()
    {
        if (CurrentState != GameState.Playing)
        {
            return;
        }

        CurrentState = GameState.GameOver;
        Time.timeScale = 0f;
        OnStateChanged?.Invoke(CurrentState);
    }
}
