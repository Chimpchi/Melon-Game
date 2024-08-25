using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{

    [SerializeField] private TMP_Text scoreText;

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.GetInstance().SetState(GameState.Gameplay);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        GameManager.GetInstance().SetState(GameState.MenuUnPaused);
    }

    public void ShowMenu(int score)
    {
        if (!scoreText)
            return;

        scoreText.text = score.ToString();
        gameObject.SetActive(true);
    }

}
