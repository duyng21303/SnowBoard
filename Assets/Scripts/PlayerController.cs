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
    bool hasSpun = false;     
    float totalSpin = 0f;    
    float lastAngle = 0f;
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

        // Kiểm tra nhảy
        if (canMove && isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Đo độ xoay khi đang bay
        if (!isGrounded)
        {
            float currentAngle = transform.eulerAngles.z;
            // DeltaAngle tính chênh lệch góc (có tính đến vòng xoay 0-360)
            float delta = Mathf.DeltaAngle(lastAngle, currentAngle);
            totalSpin += delta;
            lastAngle = currentAngle;

            // Nếu tổng tuyệt đối >= 360 và chưa cộng điểm 
            if (Mathf.Abs(totalSpin) >= 360f && !hasSpun)
            {
                // Cộng 100 điểm
                ScoreController.instance.AddScore(100);
                hasSpun = true;
            }
        }
        else
        {
            // Nếu Player đang ở trên mặt đất, reset biến
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
		// Xóa vận tốc theo trục Y để đảm bảo nhảy đồng đều
		rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0f);
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
