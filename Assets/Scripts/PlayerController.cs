using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
	Rigidbody2D rb2d;
	SurfaceEffector2D surfaceEffector2D;

	[SerializeField] float torqueAmount = 1f;
	[SerializeField] float airTorqueMultiplier = 2f; // Hệ số xoay trên không
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
	bool isGrounded = true;
	bool canMove = true;
	bool hasSpun = false;
	float totalSpin = 0f;
	float lastAngle = 0f;

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
		currentSpeed = baseSpeed;
	}

	void Update()
	{
		if (canMove)
		{
			RotatePlayer();
			UpdateSpeed();
		}

		if (canMove && isGrounded && Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
		}

		if (!isGrounded)
		{
			float currentAngle = transform.eulerAngles.z;
			float delta = Mathf.DeltaAngle(lastAngle, currentAngle);
			totalSpin += delta;
			lastAngle = currentAngle;

			if (Mathf.Abs(totalSpin) >= 360f && !hasSpun)
			{
				var x = Mathf.Abs(totalSpin) / 360f;
				gameManager.AddScore(100 * (int)x);
				hasSpun = true;
				currentSpeed = Mathf.Max(baseSpeed, currentSpeed - spinSpeedPenalty);
			}

		}
		else
		{
			totalSpin = 0f;
			lastAngle = transform.eulerAngles.z;
			hasSpun = false;
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

			// Nếu nhấn Shift thì override targetSpeed thành boostSpeed
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
