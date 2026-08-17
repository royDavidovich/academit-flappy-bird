using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject readyPanel;
    [SerializeField] private GameObject playingHud;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private Button restartButton;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += HandleStateChanged;
        GameManager.Instance.OnScoreChanged += HandleScoreChanged;
        restartButton.onClick.AddListener(Restart);

        HandleStateChanged(GameManager.Instance.CurrentState);
        HandleScoreChanged(GameManager.Instance.Score);
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= HandleStateChanged;
            GameManager.Instance.OnScoreChanged -= HandleScoreChanged;
        }
    }

    private void HandleStateChanged(GameState state)
    {
        readyPanel.SetActive(state == GameState.Ready);
        playingHud.SetActive(state == GameState.Playing);
        gameOverPanel.SetActive(state == GameState.GameOver);

        if (state == GameState.GameOver)
        {
            finalScoreText.text = GameManager.Instance.Score.ToString();
        }
    }

    private void HandleScoreChanged(int score)
    {
        scoreText.text = score.ToString();
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
