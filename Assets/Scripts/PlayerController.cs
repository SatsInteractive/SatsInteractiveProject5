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
    public ParticleSystem playerParticles;
    
    public bool isInMiniGame = false;
    
    [Header("Step Sounds")]
    public AudioClip[] stepSounds;
    private AudioSource audioSource;
    public float stepSoundInterval = 0.4f;
    private float nextStepSoundTime = 0f;
    public float walkingSpeedMagForStepSound = 1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Face = transform.Find("Face").transform;
        audioSource = GetComponent<AudioSource>();
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
        if (rb.velocity.magnitude > walkingSpeedMagForStepSound)
        {
            if (Time.time > nextStepSoundTime)
            {
                nextStepSoundTime = Time.time + stepSoundInterval;
                PlayStepSound();
            }
        }
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
    
    public void PlayParticles()
    {
        if (playerParticles != null)
        {
            playerParticles.Play();
        }
        else
        {
            Debug.LogError("Player particles are not assigned!");
        }
    }

    private void ResetScale()
    {
        Face.localScale = new Vector3(initialFaceScale, initialFaceScale, 1);
    }
    
    public void PlayStepSound()
    {
        audioSource.PlayOneShot(stepSounds[UnityEngine.Random.Range(0, stepSounds.Length)]);
    }
    
    public void ChangeVolumeLevel(float value)
    {
        audioSource.volume = value * 0.5f;
    }
    
    
}

