using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private Transform winMenu;
    [SerializeField] private Transform gameOverMenu;

    private int score;

    private void OnEnable()
    {
        MelonManager.OnGameOver += GameOver;
        ScoreManager.OnScoreUpadate += UpdateScore;
        Melon.OnFinalMelon += WinGame;
    }

    private void OnDisable()
    {
        MelonManager.OnGameOver -= GameOver;
        ScoreManager.OnScoreUpadate -= UpdateScore;
        Melon.OnFinalMelon -= WinGame;
    }

    private void UpdateScore(int score) => this.score = score;

    private void WinGame(Melon planet)
    {
        if (!winMenu)
            return;

        GameManager.GetInstance().SetState(GameState.MenuPaused);
        winMenu.GetComponent<GameOverMenu>().ShowMenu(score);
    }

    private void GameOver()
    {
        if (!gameOverMenu)
            return;

        GameManager.GetInstance().SetState(GameState.MenuPaused);
        gameOverMenu.GetComponent<GameOverMenu>().ShowMenu(score);
    }

}
