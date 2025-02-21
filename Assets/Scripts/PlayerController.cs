using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ▼ "Class/Member Variables" ▼
    Rigidbody2D rb2d;
    SurfaceEffector2D surfaceEffector2D;


    // ▼ "Serialize Field" 
    //      → to Make it "Visible" in the "Inspector" ▼
    [SerializeField] float torqueAmount = 1f;
    [SerializeField] float boostSpeed = 30f; 
    [SerializeField] float baseSpeed = 20f;
	[SerializeField] float jumpForce = 10f;
	bool isGrounded = true;
	bool canMove = true;




    // ▬ Start is called once before the first execution of Update after the MonoBehaviour is created  ▬
    void Start()
    {
        // ▼ Get the "Rigid Body 2D" Component of the "Player" Object 
        //      → and "Store it" in the "Local Variable" ▼
        rb2d = GetComponent<Rigidbody2D>();

        // ▼ Access the "FindObjectOfType()" Method ▼
        surfaceEffector2D = FindObjectOfType<SurfaceEffector2D>();
    }



    // ▬ Update is called once per frame ▬
    void Update()
    {
        // ▼ "If" the "Player" is "Can Move" ▼
        if (canMove)
        {
            // ▼ "Call" the "Methods" ▼
            RotatePlayer();
            RespondToBoost(); // ◄◄ "Player Improvement" ◄◄
        }
		if (canMove && isGrounded && Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
		}
	}
        


        
    



    // ▬ "DisableControls()" Method 
    //       → (with a "public" Access Modifier)
    //       → that "Disables" the "Input" of the "Player" ▬
    public void DisableControls()
    {
        // ▼ Set "Can Move" to "False" ▼
        canMove = false;
    }




	// ▬ "RotatePlayer()" Method ▬
	void Jump()
	{
		// Xóa vận tốc theo trục Y trước khi nhảy để đảm bảo nhảy đồng đều
		rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
		rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		isGrounded = false;
	}
	void RotatePlayer()
    {
        // ▼ "If" the "Left Arrow" Key is "Pressed" 
        //      → the "Player" will "Rotate Left" ▼
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // ▼ "Apply" a "Force" → to "Rotate Left" the "Player" 
            //      → using "AddTorque()" Method ▼
            rb2d.AddTorque(torqueAmount);
        }


        // ▼ "Else If" the "Right Arrow" Key is "Pressed" 
        //      → the "Player" will "Rotate Right" ▼
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            // ▼ "Apply" a "Force" → to "Rotate Right" the "Player" 
            //      → using "AddTorque()" Method ▼
            rb2d.AddTorque(-torqueAmount);
        }
    }



     // ▬ "RespondToBoost()" Method 
    //       → that "Increase" the "Player Speed" ▬
    void RespondToBoost()
    {
       // ▼ "If" the "Up Arrow" Key is "Pressed" 
        //      → the "Player" will "Boost/Increase Speed" ▼
        if (Input.GetKey(KeyCode.UpArrow))
        {
            // ▼ "Access" the "Surface Effector 2D" Component
            //      → and "Change" It's "Speed" Property Value 
            //      → to "Boost Speed" ▼
            surfaceEffector2D.speed = boostSpeed;
        }
 
        // ▼ Otherwise, "Stay" at "Normal Speed" ▼
        else
        {
            // ▼ "Access" the "Surface Effector 2D" Component
            //      → and "Change" It's "Speed" Property Value 
            //      → to "Base Speed" ▼
            surfaceEffector2D.speed = baseSpeed;
        }       
    }
}
