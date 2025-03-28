﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class SnowmanController : MonoBehaviour
{
	[SerializeField] float moveSpeed = 2f;
	bool isGrounded = true;
	CrushDetector crush;
	Rigidbody2D rb2d;
	[SerializeField] float loadDelay = 0.5f;  // ◄◄ "1/2 Second" Delay ◄◄
	[SerializeField] ParticleSystem crushEffect;
	[SerializeField] AudioClip crushSFX;
    private bool scored = false;

    void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		rb2d.linearVelocity = new Vector2(-moveSpeed, rb2d.linearVelocity.y);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{

			FindAnyObjectByType<PlayerController>().DisableControls();
			GetComponent<AudioSource>().PlayOneShot(crushSFX);
            var crush = collision.gameObject.GetComponent<CrushDetector>();

            PlayerPrefs.SetInt("FinalScore", crush.GetScore()); 


            Invoke("ReloadScene", loadDelay);
            crush.GetEffect();
        }
       
    }
	void ReloadScene()
	{
		// ▼ "LoadScene()" Built-In Method
		//      → from the "SceneManager" Class 
		//      → which will "Load" our "Level 1" Scene, 
		//      → with "Index 0" ▼
		SceneManager.LoadScene(0);
	}
}
