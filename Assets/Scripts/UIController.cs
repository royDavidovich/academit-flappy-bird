using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject readyPanel;
    [SerializeField] GameObject playingHud;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text finalScoreText;
    [SerializeField] Button restartButton;

    void Start()
    {
        GameManager.Instance.OnStateChanged += HandleStateChanged;
        GameManager.Instance.OnScoreChanged += HandleScoreChanged;
        restartButton.onClick.AddListener(Restart);
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= HandleStateChanged;
            GameManager.Instance.OnScoreChanged -= HandleScoreChanged;
        }
    }

    void HandleStateChanged(GameState state)
    {
        readyPanel.SetActive(state == GameState.Ready);
        playingHud.SetActive(state == GameState.Playing);
        gameOverPanel.SetActive(state == GameState.GameOver);

        if (state == GameState.GameOver)
        {
            finalScoreText.text = GameManager.Instance.Score.ToString();
        }
    }

    void HandleScoreChanged(int score)
    {
        scoreText.text = score.ToString();
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
