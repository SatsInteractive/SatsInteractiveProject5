using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    [SerializeField] public Vector2 moveInput;
    
    // Movement animation
    public Transform Face;
    public SpriteRenderer FaceSpriteRenderer;
    public float initialFaceScale = 0.08f;
    public float walkAnimationRate = 0.5f;
    public float scaleAmount = 0.01f;
    private bool isMoving;
    private Coroutine walkCoroutine;
    
    public bool isInMiniGame = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Face = transform.Find("Face").transform;
    }

    private void Update()
    {
        if (isInMiniGame)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (moveInput != Vector2.zero)
        {
            if (!isMoving)
            {
                isMoving = true;
                walkCoroutine = StartCoroutine(WalkAnimation());
            }
        }
        else
        {
            if (isMoving)
            {
                isMoving = false;
                StopCoroutine(walkCoroutine);
                ResetScale();
            }
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (isInMiniGame) return;
        rb.velocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
    }
    
    private IEnumerator WalkAnimation()
    {
        float timer = 0;
        while (true)
        {
            // Oscillate the scale for a walking effect
            float scale = initialFaceScale + Mathf.Sin(timer * Mathf.PI * 2) * scaleAmount;
            Face.localScale = new Vector3(scale, scale, 1);

            timer += Time.deltaTime * walkAnimationRate;
            yield return null;
        }
    }

    private void ResetScale()
    {
        Face.localScale = new Vector3(initialFaceScale, initialFaceScale, 1);
    }
    
    
}

