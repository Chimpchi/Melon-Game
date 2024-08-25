using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MenuPaused, MenuUnPaused, Gameplay
}

public class GameManager : MonoBehaviour
{

    private static GameManager Instance;

    public GameState currentState;
    private float pausedTimeScale;

    public delegate void StateChanged(GameState currentState, GameState previousState);
    public static event StateChanged OnStateChange;

    public static GameManager GetInstance()
    {
        if(Instance)
            return Instance;

        return new GameObject("Game Manager").AddComponent<GameManager>();
    }

    private void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        pausedTimeScale = Time.timeScale;
        DontDestroyOnLoad(gameObject);
    }

    public void SetState(GameState state)
    {
        if (currentState == state)
            return;

        (state, currentState) = (currentState, state);

        if (currentState == GameState.MenuPaused)
        {
            pausedTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            AudioListener.pause = true;
            OnStateChange?.Invoke(currentState, state);
            return;
        }

        Time.timeScale = pausedTimeScale;
        AudioListener.pause = false;
        OnStateChange?.Invoke(currentState, state);
    }

}
