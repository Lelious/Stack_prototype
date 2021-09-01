using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{    
    [SerializeField] private TextMeshProUGUI playerScore;
    [SerializeField] private TextMeshProUGUI playerBestScore;

    public int bestScore;
    private int score;
    private void Awake()
    {
        LoadBestScore();
    }
    private void Start()
    {
        playerBestScore.text = $"BEST SCORE: {bestScore}";

        GameManager.OnPlatformSpawned += GameManager_OnPlatformSpawned;
        GameManager.OnGameEnded += GameManager_OnGameEnded;
    }
    public void SaveBestScore()
    {
        SaveLoadSystem.SaveScore(this);
    }

    public void LoadBestScore()
    {
        PlayerScore playerScore = SaveLoadSystem.LoadScore();
        if (playerScore == null)
        {
            bestScore = 0;
        }
        else
        {
            bestScore = playerScore.bestScore;
        }
    }

    private void OnDestroy()
    {
        GameManager.OnGameEnded -= GameManager_OnGameEnded;
        GameManager.OnPlatformSpawned -= GameManager_OnPlatformSpawned;
    }

    private void GameManager_OnPlatformSpawned()
    {
        playerScore.text = $"SCORE: {score}";       

        if (score > bestScore)
        {
            bestScore = score;
        }
        score++;
    }

    private void GameManager_OnGameEnded()
    {
        SaveBestScore();
    }
}
