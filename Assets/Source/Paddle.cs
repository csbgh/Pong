using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour
{
    public float paddleHeight = 1.0f;
    public float direction = 1.0f;
    public Color color;

    private Vector3 initialPosition;
    private PongGameState gameState;

    private SpriteRenderer sprite;
    private Animator animator;

    void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        gameState = FindObjectOfType<PongGameState>();
        initialPosition = transform.position;
        sprite.color = color;
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "Ball")
        {
            animator.SetTrigger("OnPaddleHit");
        }
    }

    public void SetPosition(float y)
    {
        float halfHeight = paddleHeight / 2.0f;

        // ensure the new position is within the map area
        y = Mathf.Clamp(y, gameState.mapBorderBottom + halfHeight, gameState.mapBorderTop - halfHeight);

        transform.position = new Vector3(initialPosition.x, y, initialPosition.z);
    }
}
