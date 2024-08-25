using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] private TMP_Text score;

    public delegate void ScoreUpdate(int score);
    public static ScoreUpdate OnScoreUpadate;

    public int Score { get; private set; }

    private void OnEnable()
    {
        Melon.OnMerge += UpdateScore;
    }

    private void OnDisable()
    {
        Melon.OnMerge -= UpdateScore;
    }

    public void UpdateScore(int points)
    {
        Score += points;

        if(!score)
        {
            Debug.LogError("Score display is null. Did you remember to reference it?");
            return;
        }

        score.text = Score.ToString();
        OnScoreUpadate?.Invoke(Score);
    }

}
