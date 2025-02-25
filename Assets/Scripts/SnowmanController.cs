using UnityEngine;
using UnityEngine.SceneManagement;

public class SnowmanController : MonoBehaviour
{
	[SerializeField] float moveSpeed = 2f;
	bool isGrounded = true;

	Rigidbody2D rb2d;
	[SerializeField] float loadDelay = 0.5f;  // ◄◄ "1/2 Second" Delay ◄◄
	[SerializeField] ParticleSystem crushEffect;
	[SerializeField] AudioClip crushSFX;
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{

			FindAnyObjectByType<PlayerController>().DisableControls();
			GetComponent<AudioSource>().PlayOneShot(crushSFX);
			Invoke("ReloadScene", loadDelay);

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
