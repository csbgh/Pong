using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Paddle))]
public class PlayerController : MonoBehaviour
{
    private Paddle paddle;

    void Awake()
    {
        paddle = GetComponent<Paddle>();
    }

    void Update()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        paddle.SetPosition(worldPos.y);
    }
}
