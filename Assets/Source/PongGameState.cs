using UnityEngine;
using System.Collections;

public enum PongGameType
{
    PlayerAndAI,
    AIOnly
}

public class PongGameState : MonoBehaviour
{
    public AIDifficulty currentDifficulty;
    public Color leftColor;
    public Color rightColor;
    public float mapBorderTop = 3.84f;
    public float mapBorderBottom = -3.84f;
    public ParticleSystem[] GoalFX;
    public PongUI pongUI;

    public AudioClip PlayerScoreSound;

    // Constants
    public const int MAX_PLAYERS = 2;

    private int[] PlayerScore;

    // Objects of current game
    private Ball ball;
    private Paddle paddleLeft;
    private Paddle paddleRight;

    // Game Object prefabs
    private GameObject ballPfb;
    private GameObject playerPfb;
    private GameObject aiPfb;

    void Awake()
    {
        PlayerScore = new int[MAX_PLAYERS];

        // Load all necessary prefabs
        ballPfb = Resources.Load<GameObject>("Prefabs/Ball");
        playerPfb = Resources.Load<GameObject>("Prefabs/PlayerPaddle");
        aiPfb = Resources.Load<GameObject>("Prefabs/AIPaddle");
    }

    void Start()
    {
        StartGame(PongGameType.AIOnly);
    }

    public void StartGame(PongGameType newGameType)
    {
        ResetGame();

        // All game modes require a ball and 1 ai
        ball = CreateGameObject<Ball>(ballPfb, new Vector3(0, 0, 0));
        paddleRight = CreateGameObject<Paddle>(aiPfb, new Vector3(6, 0, 0));
        pongUI.SetPlayerNameText(1, "AI (" + currentDifficulty.ToString() + ")");

        switch (newGameType)
        {
            case PongGameType.PlayerAndAI:
                // create the left paddle with a player controller
                paddleLeft = CreateGameObject<Paddle>(playerPfb, new Vector3(-6, 0, 0));
                pongUI.SetPlayerNameText(0, "Player 1");
                break;
            case PongGameType.AIOnly:
                // create the left paddle with an AI controller
                paddleLeft = CreateGameObject<Paddle>(aiPfb, new Vector3(-6, 0, 0));
                pongUI.SetPlayerNameText(0, "AI (" + currentDifficulty.ToString() + ")");
                break;
        }

        paddleLeft.color = leftColor;
        paddleLeft.direction = 1;
        paddleRight.color = rightColor;
        paddleRight.direction = -1;
    }

    public void OnPlayerScored(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex > MAX_PLAYERS)
            return;

        PlayerScore[playerIndex]++;
        AudioSource.PlayClipAtPoint(PlayerScoreSound, new Vector3(0, 0, 0));
        GoalFX[playerIndex].Emit(200);
    }

    public int GetPlayerScore(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex > MAX_PLAYERS)
            return 0;

        return PlayerScore[playerIndex];
    }

    private void ResetGame()
    {
        // Reset scores
        for (int p = 0; p < PlayerScore.Length; p++)
        {
            PlayerScore[p] = 0;
        }

        // Clear all dynamic game objects from map
        if (ball != null)
            Destroy(ball.gameObject);

        if (paddleLeft != null)
            Destroy(paddleLeft.gameObject);

        if (paddleRight != null)
            Destroy(paddleRight.gameObject);
    }

    private T CreateGameObject<T>(GameObject prefab, Vector3 position)
    {
        T newComponent = default(T);
        GameObject newObj = (GameObject)Instantiate(prefab, position, Quaternion.identity);
        if (newObj != null)
        {
            newComponent = newObj.GetComponent<T>();
        }

        return newComponent;
    }
}
