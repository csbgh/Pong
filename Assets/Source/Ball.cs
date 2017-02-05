using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    // Modifiable object properties
    public float maxBounceAngle = 35.0f; // Direction of paddle bounce in degrees, from 0 at paddle center to maxBounceAngle at edge
    public float initialSpeed = 5.0f;
    public float maxSpeed = 12.0f;
    public AudioClip bounceSound;

    // Current movement properties of the ball
    private Vector3 direction = new Vector3(1, 1, 0);
    private float speed = 5.0f;

    // Values indicating if a ball is waiting to be served
    // and when it should be served respectively
    private bool ballWaiting = true;
    private float ballServeTime = 0.0f;

    private float lastBounceSound = 0.0f;

    // Cached component references
    private Rigidbody2D body;
    private TrailRenderer trail;
    private PongGameState gameState;

    void Awake()
    {
        // Get all required components once
        trail = GetComponent<TrailRenderer>();
        body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        gameState = FindObjectOfType<PongGameState>();
        direction = direction.normalized;
        speed = initialSpeed;

        ballWaiting = true;
        ballServeTime = Time.time + 1.5f;
    }

    void Update()
    {
        // check if the ball is waiting to be served
        // serve it if it has been waiting long enough
        if (ballWaiting && Time.time > ballServeTime)
        {
            speed = initialSpeed;
            body.velocity = direction * speed;
            ballWaiting = false;
        }
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        // Check which trigger we have hit and call the appropriate event
        string otherTag = Other.gameObject.tag;
        if (otherTag == "Paddle")
        {
            Paddle hitPaddle = Other.GetComponent<Paddle>();
            OnHitPaddle(hitPaddle);
        }
        if (otherTag == "GoalLeft")
        {
            OnHitGoal(0);
        }
        else if (otherTag == "GoalRight")
        {
            OnHitGoal(1);
        }
    }

    void OnCollisionEnter2D(Collision2D Col)
    {
        if (Col.gameObject.tag == "World")
        {
            OnHitWorld(Col.contacts[0].normal);
        }
    }

    void OnCollisionStay2D(Collision2D Col)
    {
        if (Col.gameObject.tag == "World")
        {
            OnHitWorld(Col.contacts[0].normal);
        }
    }

    private void OnHitPaddle(Paddle hitPaddle)
    {
        if (hitPaddle == null)
            return;

        // The distance from the center of the paddle, normalized to its size
        float distance = (transform.position.y - hitPaddle.transform.position.y) / (hitPaddle.paddleHeight / 2.0f);

        // Determine the angle the ball will bounce at
        float bounceAngle = distance * (maxBounceAngle * Mathf.Deg2Rad);

        // Increase the speed of the ball on each hit, ensure it does not exceed maxSpeed
        speed = Mathf.Min(speed + 0.25f, maxSpeed);

        // Get and apply the new direction of the balls movement
        // Multiply by the paddle direction to flip the direction for the right paddle
        direction.x = Mathf.Cos(bounceAngle) * hitPaddle.direction;
        direction.y = Mathf.Sin(bounceAngle);
        body.velocity = direction * speed;

        SetColor(hitPaddle.color); // Set the effects color to the last player to hit the ball
        PlayBounceSound();
    }

    private void OnHitGoal(int PlayerIndex)
    {
        gameState.OnPlayerScored(PlayerIndex);
        Reset();
    }

    private void OnHitWorld(Vector3 normal)
    {
        float absDirectionY = Mathf.Abs(direction.y);
        if (normal.y < 0.0)
        {
            direction.y = -absDirectionY;
        }
        else
        {
            direction.y = absDirectionY;
        }

        body.velocity = direction * speed;

        PlayBounceSound();
    }

    private void Reset()
    {
        speed = initialSpeed;
        transform.position = new Vector3(0, 0, 0);
        direction.y = -direction.y;
        body.velocity = new Vector3(0, 0, 0);

        ballWaiting = true;
        ballServeTime = Time.time + 1.5f;

        trail.Clear();
    }

    private void PlayBounceSound()
    {
        // Prevent the bounce sound from playing more than once every 0.05 seconds
        if (Time.time - lastBounceSound > 0.05f)
        {
            AudioSource.PlayClipAtPoint(bounceSound, transform.position);
            lastBounceSound = Time.time;
        }
    }

    private void SetColor(Color Col)
    {
        trail.material.SetColor("_TintColor", Col);
    }
}
