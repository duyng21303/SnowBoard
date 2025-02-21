using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	Rigidbody2D rb2d;
	SurfaceEffector2D surfaceEffector2D;

	[SerializeField] float torqueAmount = 1f;
	[SerializeField] float boostSpeed = 30f;
	[SerializeField] float baseSpeed = 20f;
	[SerializeField] float jumpForce = 10f;
	bool isGrounded = true;
	bool canMove = true;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		surfaceEffector2D = FindObjectOfType<SurfaceEffector2D>();
	}

	void Update()
	{
		if (canMove)
		{
			RotatePlayer();
			RespondToBoost();
		}

		// Chỉ cho phép nhảy khi đang chạm đất
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
		// Xóa vận tốc theo trục Y để đảm bảo nhảy đồng đều
		rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
		rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		isGrounded = false;
	}

	void RotatePlayer()
	{
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			rb2d.AddTorque(torqueAmount);
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			rb2d.AddTorque(-torqueAmount);
		}
	}

	void RespondToBoost()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			surfaceEffector2D.speed = boostSpeed;
		}
		else
		{
			surfaceEffector2D.speed = baseSpeed;
		}
	}

	// Phát hiện va chạm với đất để reset trạng thái "isGrounded"
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = true;
		}
	}
}
