using UnityEngine;
using System.Collections;

public enum AIDifficulty
{
    Easy = 0,
    Medium = 1,
    Hard = 2,
    Impossible = 3
}

[System.Serializable]
public struct AISettings
{
    public float movementSpeed;
    public float angledShotBias;
}

[RequireComponent(typeof(Paddle))]
public class AIController : MonoBehaviour
{
    public AIDifficulty difficulty;
    public AISettings[] difficultySettings;

    private float targetOffset = 0.0f;
    private float velocity = 0.0f;

    // values that control how well the AI plays
    private float movementSpeed = 1.0f;
    private float angledShotBias = 1.0f;

    private Paddle paddle;
    private Ball ball;
    private PongGameState gameState;

    void Awake()
    {
        gameState = FindObjectOfType<PongGameState>();
        paddle = GetComponent<Paddle>();
    }

    void Start()
    {
        // try and get the ball game object
        ball = FindObjectOfType<Ball>();

        SetDifficulty(gameState.currentDifficulty);
        ChooseNextHit();
    }

    void Update()
    {
        // Determine target to move to
        float target = ball.transform.position.y + targetOffset;

        float distance = Mathf.Abs(target - transform.position.y);
        float position = Mathf.SmoothDamp(transform.position.y, target, ref velocity, distance / movementSpeed);
        paddle.SetPosition(position);
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        ChooseNextHit();
    }

    public void SetDifficulty(AIDifficulty newDifficulty)
    {
        // Get the array index of the difficulty and check it has been set
        int difficultyIndex = (int)newDifficulty;
        if (difficultyIndex >= difficultySettings.Length)
            return;

        AISettings newSettings = difficultySettings[difficultyIndex];

        movementSpeed = newSettings.movementSpeed;
        angledShotBias = newSettings.angledShotBias;

        difficulty = newDifficulty;
    }

    private void ChooseNextHit()
    {
        // Determine the part of the paddle the AI will use to hit
        float randVal = ((Random.value * 2.0f) - 1.0f) * angledShotBias;
        float paddleHitPos = Mathf.Clamp(randVal, -0.95f, 0.95f);
        targetOffset = (paddle.paddleHeight / 2.0f) * paddleHitPos;
    }
}
