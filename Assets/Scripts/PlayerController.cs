using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    SurfaceEffector2D surfaceEffector2D;
    AudioSource audioSource;  // AudioSource để phát âm thanh

    [SerializeField] float torqueAmount = 1f;
    [SerializeField] float airTorqueMultiplier = 1f; // Hệ số xoay trên không
    [SerializeField] float baseSpeed = 20f;
    [SerializeField] float boostSpeed = 30f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float accelerationRate = 2f;
    [SerializeField] float maxSlopeAngle = 45f;
    [SerializeField] float spinSpeedPenalty = 5f;
    [SerializeField] LayerMask groundLayer;

    // Âm thanh khi nhảy và lộn vòng
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip flipSound;

    private GameManager gameManager;
    private AudioManager audioManager;
    private ScoreController scoreController;

    bool isGrounded = true;
    bool canMove = true;
    bool hasSpun = false;
    float totalSpin = 0f;
    float lastAngle = 0f;

    float totalRotation = 0f;        // Tổng số độ đã xoay
    float previousRotation = 0f;     // Lưu lại góc xoay trước đó
    float currentSpeed;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        scoreController = FindAnyObjectByType<ScoreController>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        surfaceEffector2D = FindObjectOfType<SurfaceEffector2D>();
        audioSource = GetComponent<AudioSource>();  // Lấy AudioSource trên Player
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        if (canMove)
        {
            RotatePlayer();
            UpdateSpeed();
            CheckFlip();  // Kiểm tra lộn vòng
        }

        if (canMove && isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    public void DisableControls()
    {
        canMove = false;
    }

    void Jump()
    {
        rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0f);
        rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;

        // Phát âm thanh khi nhảy
        if (jumpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    // Hàm kiểm tra và phát âm thanh khi lộn vòng
    void CheckFlip()
    {
        float currentRotation = transform.eulerAngles.z;
        float deltaRotation = Mathf.DeltaAngle(previousRotation, currentRotation);
        totalRotation += deltaRotation;
        previousRotation = currentRotation;

        // Kiểm tra nếu đã lộn đủ 360 độ
        if (Mathf.Abs(totalRotation) >= 360f)
        {
            totalRotation = 0f; // Reset lại tổng số độ đã xoay

            // Phát âm thanh khi lộn vòng
            if (flipSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(flipSound);
            }
        }
    }

    void RotatePlayer()
    {
        float appliedTorque = torqueAmount;
        if (!isGrounded)
        {
            appliedTorque *= airTorqueMultiplier;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb2d.AddTorque(appliedTorque);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rb2d.AddTorque(-appliedTorque);
        }
    }

    void UpdateSpeed()
    {
        if (isGrounded)
        {
            float targetSpeed = baseSpeed;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 5f, groundLayer);
            if (hit)
            {
                float angle = Vector2.Angle(hit.normal, Vector2.up);
                if (angle > 5f)
                {
                    float t = Mathf.InverseLerp(5f, maxSlopeAngle, angle);
                    targetSpeed = Mathf.Lerp(baseSpeed, boostSpeed, t);
                }
            }

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                targetSpeed = boostSpeed;
            }

            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accelerationRate * Time.deltaTime);
            surfaceEffector2D.speed = currentSpeed;
        }
        else
        {
            surfaceEffector2D.speed = currentSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            gameManager.AddScore(10);
        }
        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            audioManager.PlayCoinSound();
            gameManager.AddScore(1);
        }
    }
}
