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

	private GameManager gameManager;
	private AudioManager audioManager;
	private ScoreController scoreController;
	private CrushDetector crushDetector;

	bool isGrounded = true;
	bool canMove = true;
	bool hasSpun = false;
	float totalSpin = 0f;
	float lastAngle = 0f;

	// Âm thanh khi nhảy và lộn vòng
	[SerializeField] AudioClip jumpSound;
	[SerializeField] AudioClip flipSound;

	float currentSpeed;

	private void Awake()
	{
		gameManager = FindAnyObjectByType<GameManager>();
		scoreController = FindAnyObjectByType<ScoreController>();
		audioManager = FindAnyObjectByType<AudioManager>();
		crushDetector = FindAnyObjectByType<CrushDetector>();
	}

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		surfaceEffector2D = FindObjectOfType<SurfaceEffector2D>();
		audioSource = GetComponent<AudioSource>();  // Lấy AudioSource trên Player
		currentSpeed = baseSpeed;
		lastAngle = transform.eulerAngles.z;
	}

	void Update()
	{
		if (canMove)
		{
			RotatePlayer();
			UpdateSpeed();

			// Cập nhật logic quay để tính spin và cộng điểm
			if (!isGrounded)
			{
				float currentAngle = transform.eulerAngles.z;
				float delta = Mathf.DeltaAngle(lastAngle, currentAngle);
				totalSpin += delta;
				lastAngle = currentAngle;

				if (Mathf.Abs(totalSpin) >= 360f && !hasSpun)
				{
					int spins = (int)(Mathf.Abs(totalSpin) / 360f);

					// Phát âm flip (nếu có)
					if (flipSound != null && audioSource != null)
					{
						audioSource.PlayOneShot(flipSound);
					}
					// Phát âm coin sound và cộng điểm cho mỗi vòng quay
					audioManager.PlayCoinSound();
					crushDetector.SetScore(10 * spins);
					gameManager.AddScore(10 * spins);

					hasSpun = true;
					currentSpeed = Mathf.Max(baseSpeed, currentSpeed - spinSpeedPenalty);
				}
			}
			else
			{
				// Khi chạm đất thì reset lại các biến spin
				totalSpin = 0f;
				lastAngle = transform.eulerAngles.z;
				hasSpun = false;
			}

			if (isGrounded && Input.GetKeyDown(KeyCode.Space))
			{
				Jump();
			}
		}
	}

	public void DisableControls()
	{
		canMove = false;
	}

	void Jump()
	{
		// Reset vận tốc theo trục Y và áp dụng lực nhảy
		rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0f);
		rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		isGrounded = false;

		// Phát âm thanh khi nhảy
		if (jumpSound != null && audioSource != null)
		{
			audioSource.PlayOneShot(jumpSound);
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
			audioManager.PlayCoinSound();
			crushDetector.SetScore(10);
			gameManager.AddScore(10);
		}
		if (collision.gameObject.CompareTag("Coin"))
		{
			Destroy(collision.gameObject);
			audioManager.PlayCoinSound();
			crushDetector.SetScore(1);
			gameManager.AddScore(1);
		}
	}
}
