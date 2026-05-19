using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentTurn = 1;

    public int p1Score = 0;
    public int p2Score = 0;

    public Rigidbody[] allBalls;

    public float stopThreshold = 0.05f;

    public TextMeshProUGUI turnText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI resultText;

    bool gameEnd = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (gameEnd) return;

        // 4️⃣ 모든 공이 멈추면 턴 변경
        if (AllBallsStopped())
        {
            ChangeTurn();
        }
    }

    public bool AllBallsStopped()
    {
        foreach (var rb in allBalls)
        {
            if (rb.linearVelocity.magnitude > stopThreshold)
                return false;
        }
        return true;
    }

    public void ChangeTurn()
    {
        currentTurn = (currentTurn == 1) ? 2 : 1;
        UpdateUI();
    }

    public void AddScore(int player, int value)
    {
        if (player == 1)
            p1Score = Mathf.Max(0, p1Score + value);
        else
            p2Score = Mathf.Max(0, p2Score + value);

        CheckWin();
        UpdateUI();
    }

    void CheckWin()
    {
        if (p1Score >= 5)
            EndGame("Player 1 WIN!");
        else if (p2Score >= 5)
            EndGame("Player 2 WIN!");
    }

    void EndGame(string msg)
    {
        gameEnd = true;
        resultText.text = msg;
    }

    void UpdateUI()
    {
        turnText.text = $"Turn: Player {currentTurn}";
        scoreText.text = $"P1: {p1Score}  /  P2: {p2Score}";
    }
}