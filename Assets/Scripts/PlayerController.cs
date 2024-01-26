using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    [SerializeField] private float moveInputX;
    [SerializeField] private float moveInputY;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleMovementInput();
    }

    void FixedUpdate()
    {
        Move();
    }
    
    private void HandleMovementInput()
    {
        moveInputX = Input.GetAxis("Horizontal");
        moveInputY = Input.GetAxis("Vertical");
    }

    void Move()
    {
        rb.velocity = new Vector2(moveInputX * moveSpeed, moveInputY * moveSpeed);
    }
}

